using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RakNet;
using GUC.WorldObjects;
using GUC.Log;
using GUC.Enumeration;
using GUC.WorldObjects.Cells;
using GUC.Network.Messages;
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
            void OnConnection();
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

            this.ScriptObject.OnConnection();

            this.isCreated = true;
        }

        internal void Delete()
        {
            if (!this.isCreated)
                throw new Exception("Client is not in the collection!");

            this.isCreated = false;

            this.ScriptObject.OnDisconnection();

            idColl.Remove(this);
            clients.Remove(ref this.collID);
        }

        /// <summary>
        /// return FALSE to break the loop.
        /// </summary>
        public static void ForEach(Predicate<GameClient> predicate)
        {
            clients.ForEach(predicate);
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

        #region Vob Guiding

        internal List<GuidableVob> GuidedVobs = new List<GuidableVob>(100);
        internal List<GuidableVob> NewGuidedVobs = new List<GuidableVob>(20);
        internal int ProbableVobs = 0;

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

        internal void ConfirmLoadWorldMessage()
        {
            if (character != null)
            {
                character.InsertInWorld();
            }
            else if (specWorld != null)
            {
                this.isSpectating = true;
                // Write surrounding vobs to this client
                int[] coords = BigCell.GetCoords(specPos);
                this.SpecCell = specWorld.GetCellFromCoords(coords[0], coords[1]);
                BigCell[] arr = new BigCell[BigCell.NumSurroundingCells]; int i = 0;
                SpecCell.ForEachSurroundingCell(cell =>
                {
                    if (cell.DynVobs.GetCount() > 0)
                        arr[i++] = cell; // save for cell message
                });
                WorldMessage.WriteCellMessage(arr, new BigCell[0], 0, this);
                this.SpecCell.AddClient(this);
                this.specWorld.AddToPlayers(this);
            }
            else
            {
                throw new Exception("Unallowed LoadWorldMessage");
            }
        }

        internal BigCell SpecCell;

        partial void pSetToSpectate(World world, Vec3f pos, Vec3f dir)
        {
            if (this.isSpectating)
            {
                return;
                throw new NotImplementedException();
            }
            else
            {
                // set old character to npc
                if (this.character != null)
                {
                    this.character.client = null;
                    if (this.character.IsSpawned)
                    {
                        this.character.Cell.RemoveClient(this);
                    }
                }

                if (this.character == null)
                {
                    WorldMessage.WriteLoadMessage(this, world);
                }
                else if (this.character.IsSpawned && this.character.World != world)
                {
                    this.character.World.RemoveFromPlayers(this);
                    WorldMessage.WriteLoadMessage(this, world);
                }
                else // just switch cells
                {
                    int[] coords = BigCell.GetCoords(pos);
                    this.SpecCell = world.GetCellFromCoords(coords[0], coords[1]);
                    ChangeCells(character.Cell, this.SpecCell);
                    this.SpecCell.AddClient(this);
                    this.isSpectating = true;
                }
                this.character = null;

                var stream = GameServer.SetupStream(NetworkIDs.SpectatorMessage);
                stream.Write(pos);
                stream.Write(dir);
                this.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, '\0');

                this.specPos = pos;
                this.specDir = dir;
                this.specWorld = world;
            }
        }

        #region Player control

        internal int worldID = -1;
        internal int cellID = -1;

        partial void pSetControl(NPC npc)
        {
            if (npc == null)
            {
                // set old character to npc
                if (this.character != null)
                {
                    this.character.client = null;
                    if (this.character.IsSpawned)
                    {
                        this.character.Cell.RemoveClient(this);
                    }
                }
            }
            else
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
                        this.character.Cell.RemoveClient(this);
                    }
                }

                // npc is already in the world, set to player
                if (npc.IsSpawned)
                {
                    if (this.isSpectating)
                    {
                        if (this.specWorld != npc.World)
                        {
                            this.specWorld.RemoveFromPlayers(this);
                            WorldMessage.WriteLoadMessage(this, npc.World);
                        }
                        else
                        {
                            this.ChangeCells(this.SpecCell, npc.Cell);

                            PacketWriter stream = GameServer.SetupStream(NetworkIDs.PlayerControlMessage);
                            stream.Write((ushort)npc.ID);
                            npc.WriteTakeControl(stream);
                            this.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, '\0');
                        }
                        this.SpecCell.RemoveClient(this);
                        if (this.SpecCell.DynVobs.GetCount() <= 0 && this.SpecCell.ClientCount <= 0)
                            this.specWorld.netCells.Remove(this.SpecCell.Coord);
                        this.SpecCell = null;
                        this.specWorld = null;
                        this.isSpectating = false;
                    }
                    else
                    {
                        if (this.character == null)
                        {
                            WorldMessage.WriteLoadMessage(this, npc.World);
                        }
                        else if (this.character.World != npc.World)
                        {
                            this.character.World.RemoveFromPlayers(this);
                            WorldMessage.WriteLoadMessage(this, npc.World);
                        }
                        else
                        {
                            this.ChangeCells(this.character.Cell, npc.Cell);

                            PacketWriter stream = GameServer.SetupStream(NetworkIDs.PlayerControlMessage);
                            stream.Write((ushort)npc.ID);
                            npc.WriteTakeControl(stream);
                            this.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, '\0');
                        }
                    }
                    
                    npc.Cell.AddClient(this);
                }
                else // npc is not spawned remove all old vobs
                {
                    BigCell[] oldCells = new BigCell[BigCell.NumSurroundingCells];
                    int oldVobCount = 0; int i = 0;
                    this.character.Cell.ForEachSurroundingCell(cell =>
                    {
                        int count = cell.DynVobs.GetCount();
                        if (count > 0)
                        {
                            oldCells[i++] = cell;
                            oldVobCount += count;
                        }
                    });
                    WorldMessage.WriteCellMessage(new BigCell[0], oldCells, oldVobCount, this);
                }

                npc.client = this;
                this.character = npc;
            }
        }

        internal void ChangeCells(BigCell from, BigCell to)
        {
            int i = 0;
            BigCell[] oldCells = new BigCell[BigCell.NumSurroundingCells];
            int oldVobCount = 0;
            from.ForEachSurroundingCell(cell =>
            {
                if (cell.X > to.X + 1 || cell.X < to.X - 1 || cell.Y > to.Y + 1 || cell.Y < to.Y - 1)
                {
                    int count = cell.DynVobs.GetCount();
                    if (count > 0)
                    {
                        oldCells[i++] = cell;
                        oldVobCount += count;
                    }
                }
            });

            // new cells
            i = 0;
            BigCell[] newCells = new BigCell[BigCell.NumSurroundingCells];
            to.ForEachSurroundingCell(cell =>
            {
                if (cell.X > from.X + 1 || cell.X < from.X - 1 || cell.Y > from.Y + 1 || cell.Y < from.Y - 1)
                {
                    if (cell.DynVobs.GetCount() > 0)
                    {
                        newCells[i++] = cell;
                    }
                }
            });

            WorldMessage.WriteCellMessage(newCells, oldCells, oldVobCount, this);
        }

        internal void ChangeCells(BigCell from, int x, int y)
        {
            int i = 0;
            BigCell[] oldCells = new BigCell[BigCell.NumSurroundingCells];
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
            BigCell[] newCells = new BigCell[BigCell.NumSurroundingCells];
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
            GameServer.ServerInterface.Send(stream.GetData(), stream.GetLength(), pp, pr, '\0'/*orderingChannel*/, this.guid, false);
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

        public static PacketWriter GetMenuMsgStream()
        {
            return GameServer.SetupStream(NetworkIDs.ScriptMessage);
        }

        public void SendMenuMsg(PacketWriter stream, PktPriority pr, PktReliability rl)
        {
            this.Send(stream, (PacketPriority)pr, (PacketReliability)rl, 'M');
        }
    }
}
