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

namespace GUC.Server.Network
{
    public class Client
    {
        #region Properties

        //Networking
        internal RakNetGUID guid;
        internal SystemAddress systemAddress;
        public String SystemAddress { get { return systemAddress.ToString(); } }


        //Ingame
        public NPC Character { get; internal set; }

        internal List<Vob> VobControlledList = new List<Vob>();
        
        public int AccountID = -1; //FIXME ?

        #endregion

        #region Rank

        int rank = 0;
        public int Rank { get { return rank; } }

        public void SetRank(int rank)
        {
            if (rank < 0 || rank > byte.MaxValue)
                throw new ArgumentOutOfRangeException("Rank value is out of range! Must be 0...255");

            PacketWriter strm = GameServer.SetupStream(NetworkIDs.RankMessage);
            strm.Write((byte)rank);
            Send(strm, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE, '\0');

            this.rank = rank;
        }

        #endregion

        #region Validation check

        internal bool isValid = false;

        byte[] driveHash;
        public byte[] DriveHash { get { return driveHash; } }

        byte[] macHash;
        public byte[] MacHash { get { return macHash; } }

        internal bool CheckValidation(byte[] driveHash, byte[] macHash)
        {
            this.driveHash = driveHash;
            this.macHash = macHash;

            this.isValid = ScriptManager.Interface.OnClientValidation(this);
            if (!isValid)
            {
                this.Kick();
            }
            return this.isValid;
        }

        #endregion

        #region Constructors

        internal Client(RakNetGUID guid, SystemAddress systemAddress)
        {
            this.guid = new RakNetGUID(guid.g);
            this.systemAddress = systemAddress;
        }

        #endregion

        #region Player control

        public void SetControl(NPC npc)
        {
            if (npc == null)
                throw new ArgumentNullException("NPC is null!");

            if (npc.IsPlayer)
            {
                Logger.LogWarning("Rejected SetControl from client {0}: NPC {1} is a Player!", 0, npc.ID);
                return;
            }

            //npc is already in the world, set to player
            if (npc.IsSpawned)
            {
                //npc.World.Vobs.Players.Add( npc);
                if (npc.Cell != null)
                {
                 //   npc.Cell.Vobs.Players.Add(npc);
                }

                //if (npc.VobController != null)
                //    npc.VobController.RemoveControlledVob(npc);
            }
            
            //Vob.AllVobs.Players.Add(npc);
            
            npc.Client = this;
            Character = npc;
            //NPC.WriteControl(this, Character);
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
