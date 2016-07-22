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
                this.specWorld.RemoveClientFromCells(this);
                this.specWorld = null;
            }
            
            foreach (BaseVob vob in visibleVobs.Values)
            {
                vob.RemoveVisibleClient(this);
            }
            visibleVobs.Clear();

            this.isCreated = false;

            idColl.Remove(this);
            clients.Remove(ref this.collID);

            this.ScriptObject.OnDisconnection();

            this.character = null;

            this.systemAddress.Dispose();
            this.guid.Dispose();
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

        internal int cellID = -1;

        internal int PacketCount = 0;
        internal long LastCheck = 0;

        //Networking
        RakNetGUID guid;
        public RakNetGUID Guid { get { return this.guid; } }
        SystemAddress systemAddress;
        public String SystemAddress { get { return systemAddress.ToString(); } }

        byte[] driveHash;
        public byte[] GetDriveHash()
        {
            byte[] arr = new byte[driveHash.Length];
            Array.Copy(driveHash, arr, driveHash.Length);
            return arr;
        }

        byte[] macHash;
        public byte[] GetMacHash()
        {
            byte[] arr = new byte[macHash.Length];
            Array.Copy(macHash, arr, macHash.Length);
            return arr;
        }

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

        #region Vob visibility

        Dictionary<int, BaseVob> visibleVobs = new Dictionary<int, BaseVob>();

        internal void AddVisibleVob(BaseVob vob)
        {
            visibleVobs.Add(vob.ID, vob);
        }

        internal void RemoveVisibleVob(BaseVob vob)
        {
            visibleVobs.Remove(vob.ID);
        }

        static BaseVob[] vobArr = new BaseVob[250];
        void UpdateVobList(World world, Vec3f pos)
        {
            int removeCount = 0, addCount = 0;
            PacketWriter stream = GameServer.SetupStream(NetworkIDs.WorldCellMessage);

            // first, clean all vobs which are out of range
            if (visibleVobs.Count > 0)
            {
                // save the position where the count is written
                int removeCountBytePos = stream.CurrentByte;
                stream.Write(ushort.MinValue);

                // get an array, because we can't remove elements from the dictionary in its foreach loop (it's also faster)
                if (vobArr.Length < visibleVobs.Count)
                {
                    vobArr = visibleVobs.Values.ToArray();
                }
                else
                {
                    visibleVobs.Values.CopyTo(vobArr, 0);
                }

                // check whether the known vobs are out of range
                for (int i = visibleVobs.Count-1; i >= 0; i--)
                {
                    if (vobArr[i].GetPosition().GetDistancePlanar(pos) > World.SpawnRemoveRange)
                    {
                        stream.Write((ushort)vobArr[i].ID);

                        vobArr[i].RemoveVisibleClient(this);
                        visibleVobs.Remove(vobArr[i].ID);
                        removeCount++;
                    }
                }

                // vobs were removed, write the count at the start
                if (removeCount > 0)
                {
                    int currentByte = stream.CurrentByte;
                    stream.CurrentByte = removeCountBytePos;
                    stream.Write((ushort)removeCount);
                    stream.CurrentByte = currentByte;
                }
            }
            else
            {
                stream.Write(ushort.MinValue);
            }

            // save the position where we wrote the count of new vobs
            int countBytePos = stream.CurrentByte;
            stream.Write(ushort.MinValue);

            // then look for new vobs
            if (visibleVobs.Count > 0) // we have to check whether we know the vob already
            {                
                world.ForEachDynVobRougher(pos, World.SpawnInsertRange, vob =>
                {
                    if (!visibleVobs.ContainsKey(vob.ID))
                    {
                        if (pos.GetDistancePlanar(vob.GetPosition()) < World.SpawnInsertRange)
                        {
                            AddVisibleVob(vob);
                            vob.AddVisibleClient(this);
                            
                            stream.Write((byte)vob.VobType);
                            vob.WriteStream(stream);
                            addCount++;
                        }
                    }
                });
            }
            else // just add everything
            {
                world.ForEachDynVobRougher(pos, World.SpawnInsertRange, vob =>
                {
                    if (pos.GetDistancePlanar(vob.GetPosition()) < World.SpawnInsertRange)
                    {
                        AddVisibleVob(vob);
                        vob.AddVisibleClient(this);
                        
                        stream.Write((byte)vob.VobType);
                        vob.WriteStream(stream);
                        addCount++;
                    }
                });
            }

            // vobs were added, write the correct count at the start
            if (addCount > 0)
            {
                int currentByte = stream.CurrentByte;
                stream.CurrentByte = countBytePos;
                stream.Write((ushort)addCount);
                stream.CurrentByte = currentByte;
            }
            else if (removeCount == 0) // nothing changed
            {
                return;
            }
            
            this.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W');
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
                SpectatorMessage.WriteSpectatorMessage(this, specPos, specDir);
                specWorld.AddClientToCells(this);
                JoinWorld(specWorld, specPos);
            }
            else
            {
                throw new Exception("Unallowed LoadWorldMessage");
            }
        }

        #region Spectating

        internal BigCell SpecCell;

        partial void pSpecSetPos()
        {
            this.SetPosition(this.specPos, true);
        }

        Vec3f lastPos = Vec3f.Null;
        const int UpdateDistance = 100;
        internal void SetPosition(Vec3f pos, bool sendToClient)
        {
            this.specPos = pos.CorrectPosition();
            this.specWorld.UpdateClientCell(this, this.specPos);

            if (specPos.GetDistancePlanar(lastPos) > UpdateDistance)
            {
                lastPos = specPos;
                UpdateVobList(this.specWorld, specPos);
            }
        }

        partial void pSetToSpectate(World world, Vec3f pos, Vec3f dir)
        {
            this.isSpectating = false;
            if (this.character == null)
            {
                WorldMessage.WriteLoadMessage(this, world);
            }
            else
            {
                // set old character to npc
                this.character.client = null;
                if (this.character.IsSpawned)
                {
                    this.character.World.RemoveClient(this);
                    this.character.Cell.RemoveClient(this);

                    if (this.character.World != world)
                    {
                        WorldMessage.WriteLoadMessage(this, world);
                    }
                    else
                    {
                        // same world, just update
                        this.isSpectating = true;
                        SpectatorMessage.WriteSpectatorMessage(this, pos, dir);
                        UpdateVobList(world, pos);
                    }
                }
                else
                {
                    WorldMessage.WriteLoadMessage(this, world);
                }
                this.character = null;
            }

            this.specPos = pos;
            this.specDir = dir;
            this.specWorld = world;
        }

        #endregion

        void JoinWorld(World world, Vec3f pos)
        {
            world.AddClient(this);
            
            PacketWriter stream = GameServer.SetupStream(NetworkIDs.WorldJoinMessage);

            // save vob count position for later
            int countBytePos = stream.CurrentByte;
            stream.Write(ushort.MinValue);

            // check for vobs
            world.ForEachDynVobRougher(pos, World.SpawnInsertRange, vob =>
            {
                if (vob.GetPosition().GetDistance(pos) < World.SpawnInsertRange)
                {
                    AddVisibleVob(vob);
                    vob.AddVisibleClient(this);

                    stream.Write((byte)vob.VobType);
                    vob.WriteStream(stream);
                }
            });

            if (visibleVobs.Count > 0)
            {
                // write correct vob count to the front
                int currentByte = stream.CurrentByte;
                stream.CurrentByte = countBytePos;
                stream.Write((ushort)visibleVobs.Count);
                stream.CurrentByte = currentByte;


                this.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W');
            }
        }

        void LeaveWorld(World world)
        {
            world.RemoveClient(this);

            PacketWriter stream = GameServer.SetupStream(NetworkIDs.WorldLeaveMessage);
            this.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W');

            foreach (BaseVob vob in visibleVobs.Values)
            {
                vob.RemoveVisibleClient(this);
            }

            visibleVobs.Clear();
        }

        #region Player control

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
                    this.LeaveWorld(this.character.World);
                }
            }
            else
            {
                if (npc.IsPlayer)
                {
                    Logger.LogWarning("Rejected SetControl of Player {0} by Client {1}!", npc.ID, this.ID);
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
                        this.specWorld.RemoveClient(this);
                        this.specWorld.RemoveClientFromCells(this);
                        this.isSpectating = false;

                        if (this.specWorld != npc.World)
                        {
                            WorldMessage.WriteLoadMessage(this, npc.World);
                        }
                        else
                        {
                            UpdateVobList(npc.World, npc.GetPosition());

                            PacketWriter stream = GameServer.SetupStream(NetworkIDs.PlayerControlMessage);
                            stream.Write((ushort)npc.ID);
                            npc.WriteTakeControl(stream);
                            this.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, '\0');
                        }

                        this.specWorld = null;
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
            Logger.Log("Client kicked: {0} IP:{1}", this.ID, this.SystemAddress);
            GameServer.DisconnectClient(this);
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
