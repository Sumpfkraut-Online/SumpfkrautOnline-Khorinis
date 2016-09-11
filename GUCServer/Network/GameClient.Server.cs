using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RakNet;
using GUC.WorldObjects;
using GUC.Log;
using GUC.GameObjects.Collections;
using GUC.WorldObjects.Cells;
using GUC.Scripting;
using GUC.Types;
using GUC.WorldObjects.VobGuiding;
using GUC.Models;
using GUC.WorldObjects.Instances;

namespace GUC.Network
{
    public partial class GameClient
    {
        #region Network Messages

        internal static class Messages
        {
            public static bool ReadConnection(PacketReader stream, RakNetGUID guid, SystemAddress ip, out GameClient client)
            {
                client = ScriptManager.Interface.CreateClient();
                client.driveHash = stream.ReadBytes(16);
                client.macHash = stream.ReadBytes(16);

                if (client.ScriptObject.IsAllowedToConnect())
                {
                    client.guid = new RakNetGUID(guid.g);
                    client.systemAddress = new SystemAddress(ip.ToString(), ip.GetPort());
                    return true;
                }
                else
                {
                    client = null;
                    return false;
                }
            }

            public static void WriteDynamics(GameClient client)
            {
                if (BaseVobInstance.GetCountDynamics() > 0 && ModelInstance.CountDynamics > 0)
                {
                    PacketWriter strm = GameServer.SetupStream(ServerMessages.DynamicsMessage);

                    // MODELS
                    if (ModelInstance.CountDynamics > 0)
                    {
                        strm.Write(true);
                        strm.Write((ushort)ModelInstance.CountDynamics);
                        ModelInstance.ForEachDynamic(model =>
                        {
                            model.WriteStream(strm);
                        });
                    }
                    else
                    {
                        strm.Write(false);
                    }

                    // INSTANCES
                    if (BaseVobInstance.GetCountDynamics() > 0)
                    {
                        strm.Write(true);
                        for (int i = 0; i < (int)VobTypes.Maximum; i++)
                        {
                            strm.Write((ushort)BaseVobInstance.GetCountDynamicsOfType((VobTypes)i));
                            BaseVobInstance.ForEachDynamicOfType((VobTypes)i, inst =>
                            {
                                inst.WriteStream(strm);
                            });
                        }
                    }
                    else
                    {
                        strm.Write(false);
                    }

                    client.Send(strm, PktPriority.Low, PktReliability.Reliable, '\0');
                }
            }

            #region Spectator

            public static void ReadSpectatorPosition(PacketReader stream, GameClient client)
            {
                client.SetPosition(stream.ReadCompressedPosition(), false);
            }

            public static void WriteSpectatorMessage(GameClient client, Vec3f pos, Vec3f dir)
            {
                var stream = GameServer.SetupStream(ServerMessages.SpectatorMessage);
                stream.Write(pos);
                stream.Write(dir);
                client.Send(stream, PktPriority.Low, PktReliability.Reliable, '\0');
            }

            #endregion

            #region NPC Control

            public static void WritePlayerControl(GameClient client, NPC npc)
            {
                PacketWriter stream = GameServer.SetupStream(ServerMessages.PlayerControlMessage);
                stream.Write((ushort)npc.ID);
                npc.WriteTakeControl(stream);
                client.Send(stream, PktPriority.Low, PktReliability.ReliableOrdered, '\0');
            }

            #endregion
            
            public static void ReadLoadWorldMessage(PacketReader stream, GameClient client)
            {
                if (client.character != null)
                {
                    client.JoinWorld(client.character.World, client.character.GetPosition());
                    client.character.SpawnPlayer();
                }
                else if (client.specWorld != null)
                {
                    client.isSpectating = true;
                    WriteSpectatorMessage(client, client.specPos, client.specDir); // tell the client that he's spectating
                    client.specWorld.AddClient(client);
                    client.specWorld.AddSpectatorToCells(client);
                    client.JoinWorld(client.specWorld, client.specPos);
                }
                else
                {
                    throw new Exception("Unallowed LoadWorldMessage");
                }
            }

            public static void ReadScriptCommandMessage(PacketReader stream, GameClient client, World world, bool hero)
            {
                GuidedVob vob;
                if (hero)
                {
                    vob = client.character;
                }
                else
                {
                    world.TryGetVob(stream.ReadUShort(), out vob);
                }

                if (vob == null)
                    return;

                client.ScriptObject.ReadScriptCommandMessage(stream, vob);
            }
        }

        #endregion

        #region ScriptObject

        /// <summary>
        /// The ScriptObject interface
        /// </summary>
        public partial interface IScriptClient : IScriptGameObject
        {
            bool IsAllowedToConnect();

            void ReadScriptMessage(PacketReader stream);
            void ReadScriptCommandMessage(PacketReader stream, GuidedVob vob);
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

            this.ScriptObject.OnConnection();

            Messages.WriteDynamics(this);
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
                this.specWorld.RemoveSpectatorFromCells(this);
                this.specWorld = null;
            }

            visibleVobs.ForEach(vob => vob.RemoveVisibleClient(this));
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
        public static void ForEachPredicate(Predicate<GameClient> predicate)
        {
            clients.ForEachPredicate(predicate);
        }

        public static void ForEach(Action<GameClient> action)
        {
            clients.ForEach(action);
        }

        public static int Count { get { return clients.Count; } }

        public static bool TryGetClient(int id, out GameClient client)
        {
            return idColl.TryGet(id, out client);
        }

        #endregion

        #region Vob guiding

        internal GODictionary<GuidedVob> GuidedVobs = new GODictionary<GuidedVob>(20);

        class IntBox { public int Count = 1; }
        Dictionary<int, IntBox> guideTargets = new Dictionary<int, IntBox>(5);
        internal void AddGuideTarget(BaseVob vob)
        {
            IntBox box;
            if (guideTargets.TryGetValue(vob.ID, out box))
            {
                box.Count++;
            }
            else
            {
                guideTargets.Add(vob.ID, new IntBox());
                if (!visibleVobs.Contains(vob.ID))
                {
                    vob.targetOf.Add(this);
                }
            }
        }

        internal void RemoveGuideTarget(BaseVob vob)
        {
            IntBox box;
            if (guideTargets.TryGetValue(vob.ID, out box))
            {
                box.Count--;
                if (box.Count == 0)
                {
                    guideTargets.Remove(vob.ID);
                    vob.targetOf.Remove(this);
                }
            }
        }

        #endregion
        
        #region Properties

        internal int cellID = -1;
        
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
        
        #region Vob visibility

        GODictionary<BaseVob> visibleVobs = new GODictionary<BaseVob>();

        internal void AddVisibleVob(BaseVob vob)
        {
            visibleVobs.Add(vob);
            vob.targetOf.Remove(this);
        }

        internal void RemoveVisibleVob(BaseVob vob)
        {
            visibleVobs.Remove(vob.ID);
            if (guideTargets.ContainsKey(vob.ID))
            {
                vob.targetOf.Add(this);
            }
        }

        internal void UpdateVobList(World world, Vec3f pos)
        {
            int removeCount = 0, addCount = 0;
            PacketWriter stream = GameServer.SetupStream(ServerMessages.WorldCellMessage);

            // first, clean all vobs which are out of range
            if (visibleVobs.Count > 0)
            {
                // save the position where the count is written
                int removeCountBytePos = stream.CurrentByte;
                stream.Write(ushort.MinValue);

                visibleVobs.ForEach(vob =>
                {
                    if (vob.GetPosition().GetDistancePlanar(pos) > World.SpawnRemoveRange)
                    {
                        stream.Write((ushort)vob.ID);

                        vob.RemoveVisibleClient(this);
                        visibleVobs.Remove(vob.ID);
                        removeCount++;
                    }
                });

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
                    if (!visibleVobs.Contains(vob.ID))
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

            this.Send(stream, PktPriority.Low, PktReliability.ReliableOrdered, 'W');
        }

        void JoinWorld(World world, Vec3f pos)
        {
            PacketWriter stream = GameServer.SetupStream(ServerMessages.WorldJoinMessage);

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


                this.Send(stream, PktPriority.Low, PktReliability.ReliableOrdered, 'W');
            }
        }

        void LeaveWorld()
        {
            World.Messages.WriteLeaveWorld(this);

            visibleVobs.ForEach(vob => vob.RemoveVisibleClient(this));
            visibleVobs.Clear();
        }

        #endregion
        
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
            this.specWorld.UpdateSpectatorCell(this, this.specPos);

            if (specPos.GetDistancePlanar(lastPos) > UpdateDistance)
            {
                lastPos = specPos;
                UpdateVobList(this.specWorld, specPos);
            }
        }

        partial void pSetToSpectate(World world, Vec3f pos, Vec3f dir)
        {
            if (this.isSpectating) // is spectating, but in a different world
            {
                this.isSpectating = false;
                this.specWorld.RemoveClient(this);
                this.specWorld.RemoveSpectatorFromCells(this);
                World.Messages.WriteLoadWorld(this, world);
            }
            else
            {
                if (this.character == null)
                {
                    World.Messages.WriteLoadWorld(this, world);
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
                            World.Messages.WriteLoadWorld(this, world);
                        }
                        else
                        {
                            // same world, just update
                            this.isSpectating = true;
                            Messages.WriteSpectatorMessage(this, pos, dir);
                            world.AddClient(this);
                            world.AddSpectatorToCells(this);
                            UpdateVobList(world, pos);
                        }
                    }
                    else
                    {
                        World.Messages.WriteLoadWorld(this, world);
                    }
                    this.character = null;
                }
            }

            this.specPos = pos;
            this.specDir = dir;
            this.specWorld = world;
        }

        #endregion

        #region Player control

        partial void pSetControl(NPC npc)
        {
            if (npc == null)
            {
                if (this.isSpectating)
                {
                    this.specWorld.RemoveClient(this);
                    this.specWorld.RemoveSpectatorFromCells(this);
                }
                else if (this.character != null)
                {
                    if (this.character.IsSpawned)
                    {
                        this.character.World.RemoveClient(this);
                        this.character.Cell.RemoveClient(this);
                    }
                    this.character.client = null;
                }
                this.LeaveWorld();
            }
            else
            {
                if (npc.IsPlayer)
                {
                    Logger.LogWarning("Rejected SetControl of Player {0} by Client {1}!", npc.ID, this.ID);
                    return;
                }

                // npc is already in the world, set to player
                if (npc.IsSpawned)
                {
                    if (this.isSpectating)
                    {
                        if (this.specWorld != npc.World)
                        {
                            this.specWorld.RemoveClient(this);
                            this.specWorld.RemoveSpectatorFromCells(this);

                            this.visibleVobs.ForEach(v => v.RemoveVisibleClient(this));
                            this.visibleVobs.Clear();

                            World.Messages.WriteLoadWorld(this, npc.World);
                        }
                        else // same world
                        {
                            if (npc.Cell != this.SpecCell)
                            {
                                this.specWorld.RemoveSpectatorFromCells(this);
                                npc.Cell.AddClient(this);
                            }

                            npc.client = this;
                            UpdateVobList(npc.World, npc.GetPosition());

                            Messages.WritePlayerControl(this, npc);
                        }

                        this.specWorld = null; 
                        this.isSpectating = false;
                    }
                    else
                    {
                        if (this.character == null) // has been in the main menu probably
                        {
                            World.Messages.WriteLoadWorld(this, npc.World);
                        }
                        else if (this.character.World != npc.World) // different world
                        {
                            if (this.character.IsSpawned) // just to be sure
                            {
                                this.character.World.RemoveClient(this);
                                this.character.Cell.RemoveClient(this);

                                this.visibleVobs.ForEach(v => v.RemoveVisibleClient(this));
                                this.visibleVobs.Clear();
                            }
                            this.character.client = null;
                            npc.client = this;
                            World.Messages.WriteLoadWorld(this, npc.World);
                        }
                        else // same world
                        {
                            if (this.character.Cell != npc.Cell)
                            {
                                this.character.Cell.RemoveClient(this);
                                npc.Cell.AddClient(this);
                            }

                            this.character.client = null;
                            npc.client = this;
                            UpdateVobList(npc.World, npc.GetPosition());

                            Messages.WritePlayerControl(this, npc);
                        }
                    }
                }
                else // npc is not spawned remove all old vobs
                {
                    if (this.isSpectating)
                    {
                        this.specWorld.RemoveClient(this);
                        this.specWorld.RemoveSpectatorFromCells(this);
                        this.specWorld = null;
                        this.isSpectating = false;
                        LeaveWorld();
                    }
                    else if (this.character != null)
                    {
                        this.character.Cell.RemoveClient(this);
                        this.character.World.RemoveClient(this);
                        this.character.client = null;
                        LeaveWorld();
                    }
                    npc.client = this;
                }
            }
            this.character = npc;
        }

        #endregion

        #region Networking

        internal void Send(PacketWriter stream, PktPriority pp, PktReliability pr, char orderingChannel)
        {
            GameServer.ServerInterface.Send(stream.GetData(), stream.GetLength(), (PacketPriority)pp, (PacketReliability)pr, '\0'/*orderingChannel*/, this.guid, false);
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

        public static PacketWriter GetScriptMessageStream()
        {
            return GameServer.SetupStream(ServerMessages.ScriptMessage);
        }

        public void SendScriptMessage(PacketWriter stream, PktPriority pr, PktReliability rl)
        {
            this.Send(stream, pr, rl, 'M');
        }

        #endregion
    }
}
