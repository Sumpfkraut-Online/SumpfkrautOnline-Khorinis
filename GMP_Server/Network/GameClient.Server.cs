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
            void OnDisconnection();
        }

        #endregion

        #region Properties

        //Networking
        internal RakNetGUID guid;
        internal SystemAddress systemAddress;
        public String SystemAddress { get { return systemAddress.ToString(); } }

        internal List<Vob> VobControlledList = new List<Vob>();

        byte[] driveHash;
        public byte[] DriveHash { get { return driveHash; } }

        byte[] macHash;
        public byte[] MacHash { get { return macHash; } }

        #endregion

        #region Constructors

        internal GameClient(RakNetGUID guid, SystemAddress systemAddress, byte[] macHash, byte[] signHash)
        {
            this.macHash = macHash;
            this.driveHash = signHash;
            this.guid = new RakNetGUID(guid.g);
            this.systemAddress = new SystemAddress(systemAddress.ToString(), systemAddress.GetPort());
        }

        #endregion

        #region Player control

        public void SetControl(NPC npc)
        {
            if (npc == null)
                throw new ArgumentNullException("NPC is null!");

            if (npc.IsPlayer)
            {
                Logger.LogWarning("Rejected SetControl: NPC {0} is a Player!", npc.ID);
                return;
            }

            // set old character to npc
            if (this.Character != null)
            {
                this.Character.Client = null;
                if (this.Character.IsSpawned)
                {
                    this.Character.World.Vobs.players.Remove(this.Character.ID);
                    this.Character.Cell.Vobs.players.Remove(this.Character.ID);
                }
            }

            // npc is already in the world, set to player
            if (npc.IsSpawned)
            {
                npc.World.Vobs.players.Add(npc.ID, npc);
                npc.Cell.Vobs.players.Add(npc.ID, npc);

                //if (npc.VobController != null)
                //    npc.VobController.RemoveControlledVob(npc);
            }

            if (npc.IsSpawned)
            {
                if (character != null && character.IsSpawned && character.World != npc.World)
                {
                    World.SendWorldMessage(this, npc.World);
                }

                PacketWriter stream = GameServer.SetupStream(NetworkIDs.PlayerControlMessage);
                stream.Write((ushort)npc.ID);
                npc.WriteTakeControl(stream);
                Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE, '\0');
            }

            npc.Client = this;
            character = npc;
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
