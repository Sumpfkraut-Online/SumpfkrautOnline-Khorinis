using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RakNet;
using GUC.Enumeration;
using GUC.Server.WorldObjects;
using GUC.Network;

namespace GUC.Server.Network
{
    public class Client
    {
        public int AccountID = -1; //FIXME ?
        public byte Rank = 0;

        //Networking
        internal RakNetGUID guid;
        internal SystemAddress systemAddress;
        public String SystemAddress { get { return systemAddress.ToString(); } }
        public String DriveString { get; internal set; }
        public String MacString { get; internal set; }

        internal bool instanceNeeded;
        internal bool isValid = false;

        //Ingame
        public NPC MainChar { get; internal set; }
        public NPC Character { get; internal set; }

        internal List<Vob> VobControlledList = new List<Vob>();

        internal Client(RakNetGUID guid, SystemAddress systemAddress)
        {
            this.guid = new RakNetGUID(guid.g);
            this.systemAddress = systemAddress;
        }

        public void SetControl(NPC npc)
        {
            SetControl(npc, npc.World);
        }

        public void SetControl(NPC npc, World world)
        {
            if (npc == null || world == null)
            {
                return;
            }

            if (npc.isPlayer)
            {
                Log.Logger.logWarning("Client.SetControl rejected: NPC is a Player!");
                return;
            }

            //set old character to NPC
            if (Character != null)
            {
                Server.sVobs.Players.Remove(Character);
                if (Character.World != null)
                {
                    Character.World.Vobs.Players.Remove(npc);
                }
                if (Character.cell != null)
                {
                    Character.cell.Vobs.Players.Remove(npc);
                }
                Character.client = null;
            }

            //npc is already in the world, set to player
            if (npc.IsSpawned)
            {
                npc.World.Vobs.Players.Add( npc);
                if (npc.cell != null)
                {
                    npc.cell.Vobs.Players.Add(npc);
                }

                //if (npc.VobController != null)
                //    npc.VobController.RemoveControlledVob(npc);
            }
            
            Server.sVobs.Players.Add(npc);

            npc.World = world;
            npc.client = this;
            Character = npc;
            NPC.WriteControl(this, Character);
        }


        public static Predicate<Client> IsAllowedToConnect;
        internal void CheckValidity(String driveString, String macString, byte[] instanceTableHash)
        {
            this.DriveString = driveString;
            this.MacString = macString;

            if (IsAllowedToConnect != null)
            {
                if (!IsAllowedToConnect(this))
                {
                    isValid = false;
                    Delete();
                    return;
                }
            }
            isValid = true;

            instanceNeeded = !instanceTableHash.SequenceEqual(Server.sInstances.Hash);
        }

        public void Disconnect()
        {
            Program.server.KickClient(this);
        }

        internal void Delete()
        {
            if (MainChar != null)
            {
                MainChar.Delete();
                MainChar = null;
            }

            if (Character != null)
            { //client was controlling someone else
                Character.client = null;
            }

            /*for (int i = 0; i < VobControlledList.Count; i++)
            {
                VobControlledList[i].VobController = null;
                VobControlledList[i].FindNewController();
            }*/
        }

        internal void AddControlledVob(Vob vob)
        {
           /* VobControlledList.Add(vob);
            vob.VobController = this;
            PacketWriter stream = Program.server.SetupStream(NetworkID.ControlAddVobMessage); stream.Write(vob.ID);
            Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W');
            Log.Logger.log("AddCtrl: " + Character.ID + " " + vob.ID + ": " + vob.GetType().Name);

            if (vob is NPC)
            {
                ((NPC)vob).GoTo(this.Character, 500);
            }*/
        }

        internal void RemoveControlledVob(Vob vob)
        {
            /*VobControlledList.Remove(vob);
            vob.VobController = null;
            PacketWriter stream = Program.server.SetupStream(NetworkID.ControlRemoveVobMessage); stream.Write(vob.ID);
            Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W');
            Log.Logger.log("RemoveCtrl: " + Character.ID + " " + vob.ID + ": " + vob.GetType().Name);*/
        }

        public void SendErrorMsg(string msg)
        {

        }

        public void SendLoginMsg(PacketWriter stream)
        {
            Program.server.ServerInterface.Send(stream.GetData(), stream.GetLength(), PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE, 'M', this.guid, false);
        }

        internal void Send(PacketWriter stream, PacketPriority pp, PacketReliability pr, char orderingChannel)
        {
            Program.server.ServerInterface.Send(stream.GetData(), stream.GetLength(), pp, pr, orderingChannel, guid, false);
        }

        public int GetLastPing()
        {
            return Program.server.ServerInterface.GetLastPing(guid);
        }
    }
}
