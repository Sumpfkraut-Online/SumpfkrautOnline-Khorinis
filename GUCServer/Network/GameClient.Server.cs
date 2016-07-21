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

        bool fakeClient = false;

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

            if (this.character != null)
            {
                this.character.client = null;
                if (this.character.IsSpawned)
                {
                    this.character.World.RemoveClient(this);
                    this.Character.Cell.RemoveClient(this);
                }
            }
            else if (this.isSpectating)
            {
                this.specWorld.RemoveClient(this);
                this.SpecCell.RemoveClient(this);

                this.specWorld.CheckCellRemove(this.SpecCell);
                this.specWorld = null;
                this.SpecCell = null;
            }

            this.isCreated = false;

            idColl.Remove(this);
            clients.Remove(ref this.collID);

            this.ScriptObject.OnDisconnection();

            this.character = null;
        }

        /// <summary>
        /// return FALSE to break the loop.
        /// </summary>
        public static void ForEach(Predicate<GameClient> predicate)
        {
            clients.ForEachPredicate(predicate);
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

        internal GameClient()
        {
            this.fakeClient = true;
        }

        #endregion

        #region Vob visibility

        List<BaseVob> visibleVobs = new List<BaseVob>();

        internal void AddVisibleVob(BaseVob vob)
        {
            visibleVobs.Add(vob);
        }

        internal void RemoveVisibleVob(BaseVob vob)
        {
            visibleVobs.Remove(vob);
        }

        void CleanVobList()
        {
            //FIXME: Send a single stream!

            Vec3f pos = this.GetPosition();
            for (int i = visibleVobs.Count - 1; i >= 0; i--)
            {
                if (visibleVobs[i].GetPosition().GetDistance(pos) > World.SpawnRemoveRange)
                {
                    PacketWriter stream = GameServer.SetupStream(NetworkIDs.WorldDespawnMessage);
                    stream.Write((ushort)visibleVobs[i].ID);
                    this.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W');

                    visibleVobs[i].RemoveVisibleClient(this);
                    visibleVobs.RemoveAt(i);
                }
            }
        }

        void UpdateVobList()
        {
            //FIXME: Send a single stream!

            World world = this.GetWorld();
            Vec3f pos = this.GetPosition();

            if (visibleVobs.Count > 0)
            {
                world.ForEachDynVobRougher(pos, World.SpawnInsertRange, vob =>
                {
                    if (!visibleVobs.Contains(vob))
                    {
                        if (pos.GetDistance(vob.GetPosition()) < World.SpawnInsertRange)
                        {
                            visibleVobs.Add(vob);
                            vob.AddVisibleClient(this);

                            PacketWriter stream = GameServer.SetupStream(NetworkIDs.WorldSpawnMessage);
                            stream.Write((byte)vob.VobType);
                            vob.WriteStream(stream);
                            this.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W');
                        }
                    }
                });
            }
            else
            {
                world.ForEachDynVobRougher(pos, World.SpawnInsertRange, vob =>
                {
                    if (pos.GetDistance(vob.GetPosition()) < World.SpawnInsertRange)
                    {
                        visibleVobs.Add(vob);
                        vob.AddVisibleClient(this);

                        PacketWriter stream = GameServer.SetupStream(NetworkIDs.WorldSpawnMessage);
                        stream.Write((byte)vob.VobType);
                        vob.WriteStream(stream);
                        this.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W');
                    }
                });
            }
        }

        #endregion

        internal void ConfirmLoadWorldMessage()
        {
            if (character != null)
            {
                JoinWorld(character.World, character.GetPosition());
                character.SpawnPlayer();
            }
            else if (specWorld != null)
            {
                this.isSpectating = true;
                JoinWorld(specWorld, specPos);
            }
            else
            {
                throw new Exception("Unallowed LoadWorldMessage");
            }
        }

        #region Spectating

        internal BigCell SpecCell;

        public void SetPosition(Vec3f pos)
        {
            this.SetPosition(pos, true);
        }

        internal void SetPosition(Vec3f pos, bool sendToClient)
        {
            this.specPos = pos.CorrectPosition();
            this.specWorld.UpdateClientCell(this, this.specPos);
            CleanVobList();
            UpdateVobList();

            if (!this.fakeClient)
            {
                ForEach(client =>
                {
                    if (client.fakeClient)
                        client.SetPosition(Randomizer.GetVec3fRad(client.specPos, 50), false);
                });
            }
        }

        partial void pSetToSpectate(World world, Vec3f pos, Vec3f dir)
        {
            if (this.isSpectating)
            {
                return;
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
                    this.character.World.RemoveClient(this);
                    WorldMessage.WriteLoadMessage(this, world);
                }
                else // just switch cells
                {
                    this.isSpectating = true;
                    JoinWorld(world, pos);
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

        #endregion

        void JoinWorld(World world, Vec3f pos)
        {
            if (!this.fakeClient)
                ForEach(client =>
                {
                    if (client.fakeClient)
                    {
                        client.specPos = Randomizer.GetVec3fRad(pos, 20000);
                        client.specWorld = world;
                        client.isSpectating = true;
                        client.JoinWorld(world, client.specPos);
                    }
                });

            world.AddClientToCells(this);

            world.ForEachDynVobRougher(pos, World.SpawnInsertRange, vob =>
            {
                if (vob.GetPosition().GetDistance(pos) < World.SpawnInsertRange)
                {
                    visibleVobs.Add(vob);
                    vob.AddVisibleClient(this);
                }
            });

            PacketWriter stream = GameServer.SetupStream(NetworkIDs.WorldJoinMessage);
            stream.Write((ushort)visibleVobs.Count);
            for (int i = 0; i < visibleVobs.Count; i++)
            {
                stream.Write((byte)visibleVobs[i].VobType);
                visibleVobs[i].WriteStream(stream);
            }
            this.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W');
        }

        void LeaveWorld(World world)
        {
            world.RemoveClientFromCells(this);

            PacketWriter stream = GameServer.SetupStream(NetworkIDs.WorldLeaveMessage);
            this.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W');

            visibleVobs.Clear();
        }

        #region Player control

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
                        this.character.World.RemoveClient(this);
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
                            this.specWorld.RemoveClient(this);
                            WorldMessage.WriteLoadMessage(this, npc.World);
                        }
                        else
                        {
                            PacketWriter stream = GameServer.SetupStream(NetworkIDs.PlayerControlMessage);
                            stream.Write((ushort)npc.ID);
                            npc.WriteTakeControl(stream);
                            this.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, '\0');
                        }

                        this.SpecCell.RemoveClient(this);
                        this.specWorld.CheckCellRemove(this.SpecCell);

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
                            this.character.World.RemoveClient(this);
                            WorldMessage.WriteLoadMessage(this, npc.World);
                        }
                        else
                        {
                            JoinWorld(npc.World, npc.GetPosition());

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
                    if (this.character != null)
                        LeaveWorld(this.character.World);
                    else if (this.isSpectating)
                        LeaveWorld(this.specWorld);
                }

                npc.client = this;
            }
            this.character = npc;
        }

        #endregion

        #region Networking

        internal void Send(PacketWriter stream, PacketPriority pp, PacketReliability pr, char orderingChannel)
        {
            if (!fakeClient)
                GameServer.ServerInterface.Send(stream.GetData(), stream.GetLength(), pp, pr, '\0'/*orderingChannel*/, this.guid, false);
        }

        public int GetLastPing()
        {
            return GameServer.ServerInterface.GetLastPing(this.guid);
        }

        public void Disconnect()
        {
            GameServer.DisconnectClient(this);
        }

        public void Kick(string message = "")
        {
            throw new NotImplementedException();
        }

        public void Ban(string message = "")
        {
            Kick(message);
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

        #endregion
    }
}
