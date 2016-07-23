using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Types;
using GUC.WorldObjects.Cells;
using GUC.Network;
using GUC.Enumeration;
using RakNet;

namespace GUC.WorldObjects
{
    public partial class BaseVob
    {
        public override void Update()
        {
            throw new NotImplementedException();
        }

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
                        visibleClients[i].Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W');
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
                        visibleClients[i].Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W');
                    }
                }*/
            }
        }

        /// <summary>
        /// Set the position & direction of this vob
        /// </summary>
        public virtual void SetPosDir(Vec3f position, Vec3f direction)
        {
            this.pos = position.CorrectPosition();
            this.dir = direction.CorrectDirection();
            if (this.isCreated && !this.IsStatic)
            {
                this.world.UpdateVobCell(this, pos);
                CleanClientList();

                if (visibleClients.Count > 0)
                {
                    PacketWriter stream = GameServer.SetupStream(NetworkIDs.VobPosDirMessage);
                    stream.WriteCompressedPosition(pos);
                    stream.WriteCompressedDirection(dir);

                    visibleClients.ForEach(client => client.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W'));
                }

                UpdateClientList();
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

        partial void pSpawn()
        {
            UpdateClientList();
        }

        partial void pDespawn()
        {
            if (visibleClients.Count > 0)
            {
                PacketWriter stream = GameServer.SetupStream(NetworkIDs.WorldDespawnMessage);
                stream.Write((ushort)this.ID);
                visibleClients.ForEach(client =>
                {
                    client.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W');
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

        internal void AddVisibleClient(GameClient client)
        {
            visibleClients.Add(client);
        }

        internal void RemoveVisibleClient(GameClient client)
        {
            visibleClients.Remove(client.ID);
        }

        protected void CleanClientList()
        {
            // Remove clients which are out of range now
            if (visibleClients.Count > 0)
            {
                PacketWriter stream = GameServer.SetupStream(NetworkIDs.WorldDespawnMessage);
                stream.Write((ushort)this.ID);

                visibleClients.ForEach(client =>
                {
                    if (this.pos.GetDistance(client.IsSpectating ? client.SpecGetPos() : client.Character.GetPosition()) > World.SpawnRemoveRange)
                    {
                        client.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W');
                        client.RemoveVisibleVob(this);
                        visibleClients.Remove(client.ID);
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
                            this.AddVisibleClient(client);
                            client.AddVisibleVob(this);

                            if (stream == null)
                            {
                                stream = GameServer.SetupStream(NetworkIDs.WorldSpawnMessage);
                                stream.Write((byte)this.VobType);
                                this.WriteStream(stream);
                            }
                            client.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W');
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
                        this.AddVisibleClient(client);
                        client.AddVisibleVob(this);

                        if (stream == null)
                        {
                            stream = GameServer.SetupStream(NetworkIDs.WorldSpawnMessage);
                            stream.Write((byte)this.VobType);
                            this.WriteStream(stream);
                        }
                        client.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W');
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

            var strm = GameServer.SetupStream(NetworkIDs.ScriptVobMessage);
            strm.Write((ushort)this.ID);

            return strm;
        }

        public void SendScriptVobStream(PacketWriter stream)
        {
            if (stream == null)
                throw new Exception("Stream is null!");

            if (!this.IsSpawned)
                throw new Exception("Vob is not ingame!");

            //this.Cell.ForEachSurroundingClient(c => c.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W'));
        }

        #endregion
    }
}
