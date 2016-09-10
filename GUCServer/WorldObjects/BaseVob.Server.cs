using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Types;
using GUC.WorldObjects.Cells;
using GUC.Network;
using GUC.GameObjects.Collections;

namespace GUC.WorldObjects
{
    public partial class BaseVob
    {
        #region Network Messages

        internal static class Messages
        {
            public static void WriteSetPosDir(BaseVob vob, GameClient exclude)
            {
                PacketWriter stream = GameServer.SetupStream(ServerMessages.VobPosDirMessage);
                stream.Write((ushort)vob.ID);
                stream.WriteCompressedPosition(vob.pos);
                stream.WriteCompressedDirection(vob.dir);
                
                if (exclude == null)
                {
                    vob.visibleClients.ForEach(client => client.Send(stream, PktPriority.Low, PktReliability.Unreliable, 'W'));
                }
                else
                {
                    vob.visibleClients.ForEach(client =>
                    {
                        if (client != exclude)
                            client.Send(stream, PktPriority.Low, PktReliability.Unreliable, 'W');
                    });
                }

                for (int i = 0; i < vob.targetOf.Count; i++)
                    vob.targetOf[i].Send(stream, PktPriority.Low, PktReliability.Unreliable, 'W');
            }
        }

        #endregion

        #region Vob guiding

        internal List<GameClient> targetOf = new List<GameClient>();

        #endregion

        #region Position & Direction

        partial void pSetPosition()
        {
            throw new NotImplementedException();

            if (this.isCreated && !this.IsStatic)
            {
                this.world.UpdateVobCell(this, this.pos);

                /*if (visibleClients.Count > 0)
                {
                    PacketWriter stream = GameServer.SetupStream(NetworkIDs.VobPosMessage);
                    stream.Write(this.pos);

                    for (int i = 0; i < visibleClients.Count; i++)
                    {
                        visibleClients[i].Send(stream, PktPriority.Low, PktReliability.ReliableOrdered, 'W');
                    }
                }*/

                UpdateClientList();
            }
        }

        partial void pSetDirection()
        {
            throw new NotImplementedException();

            if (this.isCreated && !this.IsStatic)
            {
                /*if (visibleClients.Count > 0)
                {
                    PacketWriter stream = GameServer.SetupStream(NetworkIDs.VobDirMessage);
                    stream.Write(this.dir);

                    for (int i = 0; i < visibleClients.Count; i++)
                    {
                        visibleClients[i].Send(stream, PktPriority.Low, PktReliability.ReliableOrdered, 'W');
                    }
                }*/
            }
        }

        /// <summary>
        /// Set the position & direction of this vob
        /// </summary>
        public void SetPosDir(Vec3f position, Vec3f direction)
        {
            SetPosDir(position, direction, null);
        }

        Vec3f lastPos;
        internal void SetPosDir(Vec3f position, Vec3f direction, GameClient exclude)
        {
            this.pos = position.CorrectPosition();
            this.dir = direction.CorrectDirection();

            if (this.isCreated && !this.IsStatic)
            {
                this.world.UpdateVobCell(this, pos);
                if (this is NPC)
                    this.world.UpdateNPCCell((NPC)this, pos);

                bool updateVis;
                if (lastPos.GetDistancePlanar(this.pos) > 100)
                {
                    updateVis = true;
                    lastPos = this.pos;
                    CleanClientList();
                }
                else
                {
                    updateVis = false;
                }

                if (visibleClients.Count > 0 || targetOf.Count > 0)
                {
                    Messages.WriteSetPosDir(this, exclude);
                }

                if (updateVis)
                {
                    UpdateClientList();
                }
            }
        }

        #endregion

        #region Cells

        internal int CellID = -1;
        BigCell cell;
        internal BigCell Cell { get { return this.cell; } }

        internal virtual void AddToCell(BigCell cell)
        {
            this.cell = cell;
            this.cell.AddDynVob(this);
        }

        internal virtual void RemoveFromCell()
        {
            this.cell.RemoveDynVob(this);
            this.world.CheckCellRemove(this.cell);
            this.cell = null;
        }

        #endregion

        #region Spawn & Despawn

        partial void pAfterSpawn(World world, Vec3f position, Vec3f direction)
        {
            UpdateClientList();
        }

        partial void pBeforeDespawn()
        {
            if (visibleClients.Count > 0)
            {
                PacketWriter stream = GameServer.SetupStream(ServerMessages.VobDespawnMessage);
                stream.Write((ushort)this.ID);
                visibleClients.ForEach(client =>
                {
                    client.Send(stream, PktPriority.Low, PktReliability.ReliableOrdered, 'W');
                    client.RemoveVisibleVob(this);
                });
                visibleClients.Clear();
            }
        }

        #endregion

        #region Client visibility

        internal void ForEachVisibleClient(Action<GameClient> action)
        {
            visibleClients.ForEach(action);
        }

        protected GODictionary<GameClient> visibleClients = new GODictionary<GameClient>();

        internal virtual void AddVisibleClient(GameClient client)
        {
            visibleClients.Add(client);
        }

        internal virtual void RemoveVisibleClient(GameClient client)
        {
            visibleClients.Remove(client.ID);
        }

        protected void CleanClientList()
        {
            // Remove clients which are out of range now
            if (visibleClients.Count > 0)
            {
                PacketWriter stream = GameServer.SetupStream(ServerMessages.VobDespawnMessage);
                stream.Write((ushort)this.ID);

                visibleClients.ForEach(client =>
                {
                    if (this.pos.GetDistance(client.IsSpectating ? client.SpecGetPos() : client.Character.GetPosition()) > World.SpawnRemoveRange)
                    {
                        client.Send(stream, PktPriority.Low, PktReliability.ReliableOrdered, 'W');
                        client.RemoveVisibleVob(this);
                        RemoveVisibleClient(client);
                    }
                });
            }
        }

        protected void UpdateClientList()
        {
            PacketWriter stream = null;
            // add clients which are in range
            if (visibleClients.Count > 0)
            {
                this.world.ForEachClientRougher(this.pos, World.SpawnInsertRange, client =>
                {
                    if (!visibleClients.Contains(client.ID))
                    {
                        if (this.pos.GetDistance(client.IsSpectating ? client.SpecGetPos() : client.Character.GetPosition()) < World.SpawnInsertRange)
                        {
                            AddVisibleClient(client);
                            client.AddVisibleVob(this);

                            if (stream == null)
                            {
                                stream = GameServer.SetupStream(ServerMessages.VobSpawnMessage);
                                stream.Write((byte)this.VobType);
                                this.WriteStream(stream);
                            }
                            client.Send(stream, PktPriority.Low, PktReliability.ReliableOrdered, 'W');
                        }
                    }
                });
            }
            else
            {
                this.world.ForEachClientRougher(this.pos, World.SpawnInsertRange, client =>
                {
                    if (this.pos.GetDistance(client.IsSpectating ? client.SpecGetPos() : client.Character.GetPosition()) < World.SpawnInsertRange)
                    {
                        AddVisibleClient(client);
                        client.AddVisibleVob(this);

                        if (stream == null)
                        {
                            stream = GameServer.SetupStream(ServerMessages.VobSpawnMessage);
                            stream.Write((byte)this.VobType);
                            this.WriteStream(stream);
                        }
                        client.Send(stream, PktPriority.Low, PktReliability.ReliableOrdered, 'W');
                    }
                });
            }
        }

        #endregion

        #region Script streams

        public PacketWriter GetScriptVobStream()
        {
            if (!this.IsSpawned)
                throw new Exception("Vob is not ingame!");

            var strm = GameServer.SetupStream(ServerMessages.ScriptVobMessage);
            strm.Write((ushort)this.ID);

            return strm;
        }

        public void SendScriptVobStream(PacketWriter stream)
        {
            if (stream == null)
                throw new Exception("Stream is null!");

            if (!this.IsSpawned)
                throw new Exception("Vob is not ingame!");

            ForEachVisibleClient(c => c.Send(stream, PktPriority.Low, PktReliability.ReliableOrdered, 'W'));
        }

        #endregion
    }
}
