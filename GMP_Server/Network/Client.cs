using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RakNet;
using GUC.Enumeration;
using GUC.Server.WorldObjects;
using GUC.Server.Network.Messages;
using GUC.Network;

namespace GUC.Server.Network
{
    class Client : IDisposable
    {
        //Networking
        public RakNetGUID guid;
        public SystemAddress systemAddress;
        public String DriveString = "";
        public String MacString = "";
        public bool isValid = false;

        public bool instanceNPCNeeded;
        public bool instanceItemNeeded;

        //Account
        public int accountID = -1;
        public bool isLoggedIn { get { return accountID != -1; } set { } }        

        //Ingame
        public NPC mainChar = null; //Main
        public NPC character = null; //current npc
        public bool isControlling { get { return character != null; } set { } } 

        public List<AbstractCtrlVob> VobControlledList = new List<AbstractCtrlVob>();
        
        public Client(RakNetGUID guid, SystemAddress systemAddress)
        {
            this.guid = new RakNetGUID(guid.g);
            this.systemAddress = systemAddress;
        }

        public void SetControl(NPC npc)
        {
            if (character != null)
            {
                sWorld.PlayerList.Remove(character);
                sWorld.NPCList.Add(character);
                if (character.World != null)
                {
                    character.World.PlayerDict.Remove(npc.ID);
                    character.World.NPCDict.Add(character.ID, character);
                }
                if (character.cell != null)
                {
                    character.cell.PlayerList.Remove(npc);
                    character.cell.NPCList.Add(character);
                }
                character.client = null;
            }

            if (npc.Spawned)
            {
                npc.World.NPCDict.Remove(npc.ID);
                npc.World.PlayerDict.Add(npc.ID, npc);
                if (npc.cell != null)
                {
                    npc.cell.NPCList.Remove(npc);
                    npc.cell.PlayerList.Add(npc);
                }

                if (npc.VobController != null)
                    npc.VobController.RemoveControlledVob(npc);
            }

            sWorld.NPCList.Remove(npc);
            sWorld.PlayerList.Add(npc);

            npc.client = this;
            character = npc;
            Player.WriteControl(this, character);
        }

        public void CheckValidity(String driveString, String macString, byte[] npcTableHash, byte[] itemTableHash)
        {
            //FIXME: Check for banned strings
            this.DriveString = driveString;
            this.MacString = macString;

            instanceNPCNeeded = !npcTableHash.SequenceEqual(NPCInstance.hash);
            instanceItemNeeded = !itemTableHash.SequenceEqual(ItemInstance.hash);

            isValid = true;
        }

        public void Disconnect()
        {
            Disconnect(false);
        }

        public void Disconnect(bool connectionLost)
        {
            if (mainChar != null)
            {
                mainChar.Despawn();
                sWorld.RemoveVob(mainChar);
                mainChar = null;
                //Messages.Connection.DisconnectMessage.Write(mainChar, connectionLost);
            }

            if (character != null)
            { //client was controlling someone else
                character.client = null;
            }

            for (int i = 0; i < VobControlledList.Count; i++)
            {
                VobControlledList[i].VobController = null;
                VobControlledList[i].FindNewController();
            }
        }

        public void AddControlledVob(AbstractCtrlVob vob)
        {
            VobControlledList.Add(vob);
            vob.VobController = this;
            BitStream stream = Program.server.SetupStream(NetworkID.ControlAddVobMessage); stream.mWrite(vob.ID);
            Program.server.ServerInterface.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'G', guid, false);
            Log.Logger.log("AddCtrl: " + character.ID + " " + vob.ID + ": " + vob.GetType().Name);
        }

        public void RemoveControlledVob(AbstractCtrlVob vob)
        {
            VobControlledList.Remove(vob);
            vob.VobController = null;
            BitStream stream = Program.server.SetupStream(NetworkID.ControlRemoveVobMessage); stream.mWrite(vob.ID);
            Program.server.ServerInterface.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'G', guid, false);
            Log.Logger.log("RemoveCtrl: " + character.ID + " " + vob.ID + ": " + vob.GetType().Name);
        }

        private bool disposed = false;
        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                guid.Dispose();
                disposed = true;
            }
        }
    }
}
