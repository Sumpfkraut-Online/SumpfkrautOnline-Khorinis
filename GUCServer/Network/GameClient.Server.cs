using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RakNet;
using GUC.WorldObjects;
using GUC.Log;
using GUC.Enumeration;
using GUC.Server.Network;
using GUC.Server.WorldObjects.Cells;
using GUC.Server.Network.Messages;
using GUC.WorldObjects.Collections;
using GUC.Types;

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
            void OnReadMenuMsg(PacketReader stream);
            void OnReadIngameMsg(PacketReader stream);
            void OnDisconnection();
        }

        #endregion

        #region Collection

        static StaticCollection<GameClient> idColl = new StaticCollection<GameClient>(200); // slots
        static DynamicCollection<GameClient> clients = new DynamicCollection<GameClient>();

        internal void Create()
        {
            if (this.isCreated)
                throw new Exception("Client is already in the collection!");

            idColl.Add(this);
            clients.Add(this, ref this.collID);

            this.isCreated = true;
        }

        internal void Delete()
        {
            if (!this.isCreated)
                throw new Exception("Client is not in the collection!");

            this.isCreated = false;

            idColl.Remove(this);
            clients.Remove(ref this.collID);
        }

        public static void ForEach(Action<GameClient> action)
        {
            clients.ForEach(action);
        }

        public static int GetCount() { return clients.Count; }

        public static bool TryGetClient(int id, out GameClient client)
        {
            return idColl.TryGet(id, out client);
        }

        #endregion

        public override void Update()
        {
            throw new NotImplementedException();
        }

        #region Properties

        internal int PacketCount = 0;
        internal long nextCheck = 0;

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

        internal NetCell SpecCell;

        partial void pSetToSpectate(World world, Vec3f pos, Vec3f dir)
        {
            if (this.isSpectating)
            {

            }
            else
            {
                if (this.character == null || (this.character.IsSpawned && this.character.World != world))
                {
                    WorldMessage.WriteLoadMessage(this, world);
                }
                else // character is already in this world
                {
                    int[] coords = NetCell.GetCoords(pos);
                    ChangeCells(character.Cell, coords[0], coords[1]);
                }

                // set old character to npc
                if (this.character != null)
                {
                    this.character.client = null;
                    if (this.character.IsSpawned)
                    {
                        this.character.World.RemoveFromPlayers(this);
                        this.character.Cell.Clients.Remove(ref this.cellID);
                    }
                    this.character = null;
                }
            }
            
        }

        #region Player control

        internal int worldID = -1;
        internal int cellID = -1;

        partial void pSetControl(NPC npc)
        {
            if (npc.IsPlayer)
            {
                Logger.LogWarning("Rejected SetControl: NPC {0} is a Player!", npc.ID);
                return;
            }

            // set old character to npc
            if (this.character != null)
            {
                this.character.client = null;
                if (this.character.IsSpawned)
                {
                    this.character.World.RemoveFromPlayers(this);
                    this.character.Cell.Clients.Remove(ref this.cellID);
                }
            }

            // npc is already in the world, set to player
            if (npc.IsSpawned)
            {
                //if (npc.VobController != null)
                //    npc.VobController.RemoveControlledVob(npc);

                if (character == null || (character.IsSpawned && character.World != npc.World))
                {
                    WorldMessage.WriteLoadMessage(this, npc.World);
                }
                else
                {
                    ChangeCells(character.Cell, npc.Cell);

                    PacketWriter stream = GameServer.SetupStream(NetworkIDs.PlayerControlMessage);
                    stream.Write((ushort)npc.ID);
                    npc.WriteTakeControl(stream);
                    Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, '\0');
                }

                npc.World.AddToPlayers(this);
                npc.Cell.Clients.Add(this, ref this.cellID);
            }

            npc.client = this;
            character = npc;
        }

        void ChangeCells(NetCell from, NetCell to)
        {
            int i = 0;
            NetCell[] oldCells = new NetCell[NetCell.NumSurroundingCells];
            int oldVobCount = 0;
            from.ForEachSurroundingCell(cell =>
            {
                if (!(cell.X <= to.X + 1 && cell.X >= to.X - 1 && cell.Y <= to.Y + 1 && cell.Y >= to.Y - 1))
                {
                    if (cell.DynVobs.GetCount() > 0)
                    {
                        oldCells[i++] = cell;
                        oldVobCount += cell.DynVobs.GetCount();
                    }
                }
            });

            // new cells
            i = 0;
            NetCell[] newCells = new NetCell[NetCell.NumSurroundingCells];
            to.ForEachSurroundingCell(cell =>
            {
                if (!(cell.X <= from.X + 1 && cell.X >= from.X - 1 && cell.Y <= from.Y + 1 && cell.Y >= from.Y - 1))
                {
                    if (cell.DynVobs.GetCount() > 0)
                        newCells[i++] = cell;
                }
            });

            WorldMessage.WriteCellMessage(newCells, oldCells, oldVobCount, this);
        }

        void ChangeCells(NetCell from, int x, int y)
        {
            int i = 0;
            NetCell[] oldCells = new NetCell[NetCell.NumSurroundingCells];
            int oldVobCount = 0;
            from.ForEachSurroundingCell(cell =>
            {
                if (!(cell.X <= x + 1 && cell.X >= x - 1 && cell.Y <= y + 1 && cell.Y >= y - 1))
                {
                    if (cell.DynVobs.GetCount() > 0)
                    {
                        oldCells[i++] = cell;
                        oldVobCount += cell.DynVobs.GetCount();
                    }
                }
            });

            // new cells
            i = 0;
            NetCell[] newCells = new NetCell[NetCell.NumSurroundingCells];
            from.World.ForEachSurroundingCell(x, y, cell =>
            {
                if (!(cell.X <= from.X + 1 && cell.X >= from.X - 1 && cell.Y <= from.Y + 1 && cell.Y >= from.Y - 1))
                {
                    if (cell.DynVobs.GetCount() > 0)
                        newCells[i++] = cell;
                }
            });

            WorldMessage.WriteCellMessage(newCells, oldCells, oldVobCount, this);
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

        public static PacketWriter GetMenuMsgStream()
        {
            return GameServer.SetupStream(NetworkIDs.ScriptMessage);
        }

        public void SendMenuMsg(PacketWriter stream)
        {
            this.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'M');
        }

    }
}
