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
            if (this.isCreated && !this.IsStatic)
            {
                this.world.UpdateVobCell(this, this.pos);
                CleanClientList();

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

                /*if (visibleClients.Count > 0)
                {
                    PacketWriter stream = GameServer.SetupStream(NetworkIDs.VobPosDirMessage);
                    stream.Write(pos);
                    stream.Write(dir);

                    for (int i = 0; i < visibleClients.Count; i++)
                    {
                        visibleClients[i].Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W');
                    }
                }*/

                UpdateClientList();
            }
        }

        #endregion

        internal int CellID = -1;
        internal BigCell Cell = null;

        #region Spawn & Despawn

        partial void pSpawn()
        {
            UpdateClientList();
        }

        partial void pDespawn()
        {
            /*PacketWriter stream = GameServer.SetupStream(NetworkIDs.WorldDespawnMessage);
            stream.Write((ushort)this.ID);
            ForEachVis(pair =>
            {
                for (int i = visibleClients.Count - 1; i >= 0; i--)
                {
                    visibleClients[i].Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W');
                    visibleClients[i].RemoveVisibleVob(this);
                }
                visibleClients.Clear();
            });*/
        }

        #endregion

        #region Client visibility

        public void ForEachVisibleClient(Action<GameClient> action)
        {

        }

        internal Dictionary<int, GameClient> visibleClients = new Dictionary<int, GameClient>();

        internal void AddVisibleClient(GameClient client)
        {
            visibleClients.Add(client.ID, client);
        }

        internal void RemoveVisibleClient(GameClient client)
        {
            visibleClients.Remove(client.ID);
        }

        static List<int> cleanList = new List<int>();
        void CleanClientList()
        {
            if (visibleClients.Count > 0)
            {
                PacketWriter stream = GameServer.SetupStream(NetworkIDs.WorldDespawnMessage);
                stream.Write((ushort)this.ID);
                
                foreach (GameClient client in visibleClients.Values)
                {
                    /*if (this.pos.GetDistance(client.GetPosition()) > World.SpawnRemoveRange)
                    {
                        client.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W');
                        client.RemoveVisibleVob(this);
                        cleanList.Add(client.ID);
                    }*/
                }

                for (int i = 0; i < cleanList.Count; i++)
                {
                    visibleClients.Remove(cleanList[i]);
                }
                cleanList.Clear();
            }
        }


        void UpdateClientList()
        {
            PacketWriter stream = null;
            if (visibleClients.Count > 0)
            {
                this.world.ForEachClientRougher(this, World.SpawnInsertRange, client =>
                {
                    if (visibleClients.ContainsKey(client.ID))
                    {
                        /*if (this.pos.GetDistance(client.GetPosition()) < World.SpawnInsertRange)
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
                        }*/
                    }
                });
            }
            else
            {
                this.world.ForEachClientRougher(this, World.SpawnInsertRange, client =>
                {
                    /*if (this.pos.GetDistance(client.GetPosition()) < World.SpawnInsertRange)
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
                    }*/
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
