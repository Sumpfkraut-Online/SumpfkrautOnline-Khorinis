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
        public int AccountID = -1; //FIXME

        //Networking
        internal RakNetGUID guid;
        internal SystemAddress systemAddress;
        public String SystemAddress { get { return systemAddress.ToString(); } }
        public String DriveString { get; internal set; }
        public String MacString { get; internal set; }

        internal bool instanceNPCNeeded;
        internal bool instanceItemNeeded;
        internal bool instanceMobNeeded;
        internal bool isValid = false;

        //Ingame
        public NPC MainChar { get; internal set; }
        public NPC Character { get; internal set; }

        internal List<AbstractCtrlVob> VobControlledList = new List<AbstractCtrlVob>();

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
                Network.Server.sPlayerDict.Remove(Character.ID);
                Network.Server.sNpcDict.Add(Character.ID, Character);
                if (Character.World != null)
                {
                    Character.World.playerDict.Remove(npc.ID);
                    Character.World.npcDict.Add(Character.ID, Character);
                }
                if (Character.cell != null)
                {
                    Character.cell.PlayerList.Remove(npc);
                    Character.cell.NPCList.Add(Character);
                }
                Character.client = null;
            }

            //npc is already in the world, set to player
            if (npc.Spawned)
            {
                npc.World.npcDict.Remove(npc.ID);
                npc.World.playerDict.Add(npc.ID, npc);
                if (npc.cell != null)
                {
                    npc.cell.NPCList.Remove(npc);
                    npc.cell.PlayerList.Add(npc);
                }

                if (npc.VobController != null)
                    npc.VobController.RemoveControlledVob(npc);
            }

            Network.Server.sNpcDict.Remove(npc.ID);
            Network.Server.sPlayerDict.Add(npc.ID, npc);

            npc.client = this;
            Character = npc;
            NPC.WriteControl(this, Character);
        }


        public static Predicate<Client> IsAllowedToConnect;
        internal void CheckValidity(String driveString, String macString, byte[] npcTableHash, byte[] itemTableHash, byte[] mobTableHash)
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

            instanceNPCNeeded = !npcTableHash.SequenceEqual(NPCInstance.Table.hash);
            instanceItemNeeded = !itemTableHash.SequenceEqual(ItemInstance.Table.hash);
            instanceMobNeeded = !mobTableHash.SequenceEqual(MobInstance.Table.hash);
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

            for (int i = 0; i < VobControlledList.Count; i++)
            {
                VobControlledList[i].VobController = null;
                VobControlledList[i].FindNewController();
            }
        }

        internal void AddControlledVob(AbstractCtrlVob vob)
        {
            VobControlledList.Add(vob);
            vob.VobController = this;
            BitStream stream = Program.server.SetupStream(NetworkID.ControlAddVobMessage); stream.mWrite(vob.ID);
            Program.server.ServerInterface.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W', guid, false);
            Log.Logger.log("AddCtrl: " + Character.ID + " " + vob.ID + ": " + vob.GetType().Name);

            if (vob is NPC)
            {
                ((NPC)vob).GoTo(this.Character, 500);
            }
        }

        internal void RemoveControlledVob(AbstractCtrlVob vob)
        {
            VobControlledList.Remove(vob);
            vob.VobController = null;
            BitStream stream = Program.server.SetupStream(NetworkID.ControlRemoveVobMessage); stream.mWrite(vob.ID);
            Program.server.ServerInterface.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W', guid, false);
            Log.Logger.log("RemoveCtrl: " + Character.ID + " " + vob.ID + ": " + vob.GetType().Name);
        }

        public void SendErrorMsg(string msg)
        {

        }

        public void SendLoginMsg(PacketWriter stream)
        {

        }
    }
}
