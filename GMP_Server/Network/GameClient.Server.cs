using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RakNet;
using GUC.WorldObjects;
using GUC.Network;
using GUC.Log;
using GUC.Enumeration;
using GUC.Scripting;
using GUC.Server.Network;

namespace GUC.Network
{
    public partial class GameClient
    {
        #region ScriptObject

        /// <summary>
        /// The ScriptObject interface
        /// </summary>
        public partial interface IScriptClient : IScriptGameObject
        {
            bool OnValidation();
        }

        #endregion

        #region Properties

        //Networking
        internal RakNetGUID guid;
        internal SystemAddress systemAddress;
        public String SystemAddress { get { return systemAddress.ToString(); } }

        internal List<Vob> VobControlledList = new List<Vob>();

        #endregion

        #region Validation check

        internal bool isValid = false;
        public bool IsValid { get { return this.isValid; } }

        byte[] driveHash;
        public byte[] DriveHash { get { return driveHash; } }

        byte[] macHash;
        public byte[] MacHash { get { return macHash; } }

        internal bool CheckValidation(byte[] driveHash, byte[] macHash)
        {
            this.driveHash = driveHash;
            this.macHash = macHash;

            this.isValid = this.ScriptObject == null ? true : this.ScriptObject.OnValidation();
            if (!this.isValid)
            {
                this.Kick();
            }
            return this.isValid;
        }

        #endregion

        #region Constructors

        internal GameClient(RakNetGUID guid, SystemAddress systemAddress)
        {
            this.guid = new RakNetGUID(guid.g);
            this.systemAddress = systemAddress;
            ScriptManager.Interface.OnClientConnection(this);
        }

        #endregion

        #region Player control

        public void SetControl(NPC npc)
        {
            if (npc == null)
                throw new ArgumentNullException("NPC is null!");

            if (npc.IsPlayer)
            {
                Logger.LogWarning("Rejected SetControl: NPC {1} is a Player!", npc.ID);
                return;
            }

            // set old character to npc
            if (this.Character != null)
            {
                if (this.Character.IsSpawned)
                {
                    npc.World.Vobs.players.Remove(this.Character.ID);
                }
            }

            // npc is already in the world, set to player
            if (npc.IsSpawned)
            {
                npc.World.Vobs.players.Add(npc.ID, npc);
                if (npc.Cell != null)
                {
                 //   npc.Cell.Vobs.Players.Add(npc);
                }

                //if (npc.VobController != null)
                //    npc.VobController.RemoveControlledVob(npc);
            }
            
            npc.Client = this;
            character = npc;
            
            PacketWriter stream = GameServer.SetupStream(NetworkIDs.PlayerControlMessage);
            stream.Write((ushort)npc.ID);
            npc.WriteTakeControl(stream);
            Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE, '\0');
        }

        #endregion

        internal void Send(PacketWriter stream, PacketPriority pp, PacketReliability pr, char orderingChannel)
        {
            GameServer.ServerInterface.Send(stream.GetData(), stream.GetLength(), pp, pr, orderingChannel, this.guid, false);
        }

        public int GetLastPing()
        {
            return GameServer.ServerInterface.GetLastPing(this.guid);
        }

        public void Kick()
        {
            GameServer.DisconnectClient(this);
        }

        public void Ban()
        {
            GameServer.AddToBanList(this.SystemAddress);
        }



        internal void AddControlledVob(Vob vob)
        {
           /* VobControlledList.Add(vob);
            vob.VobController = this;
            PacketWriter stream = Network.Server.SetupStream(NetworkID.ControlAddVobMessage); stream.Write(vob.ID);
            Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W');
            Log.Logger.Log("AddCtrl: " + Character.ID + " " + vob.ID + ": " + vob.GetType().Name);

            if (vob is NPC)
            {
                ((NPC)vob).GoTo(this.Character, 500);
            }*/
        }

        internal void RemoveControlledVob(Vob vob)
        {
            /*VobControlledList.Remove(vob);
            vob.VobController = null;
            PacketWriter stream = Network.Server.SetupStream(NetworkID.ControlRemoveVobMessage); stream.Write(vob.ID);
            Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W');
            Log.Logger.Log("RemoveCtrl: " + Character.ID + " " + vob.ID + ": " + vob.GetType().Name);*/
        }
    }
}
