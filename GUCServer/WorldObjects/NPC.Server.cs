using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.WorldObjects.Mobs;
using GUC.Types;
using GUC.WorldObjects.Cells;
using GUC.Animations;
using GUC.WorldObjects.ItemContainers;

namespace GUC.WorldObjects
{
    public partial class NPC : Vob, ItemContainer
    {
        #region Network Messages

        public delegate void NPCMoveHandler(NPC npc, Vec3f oldPos, Vec3f oldDir, NPCMovement oldMovement);
        public static event NPCMoveHandler OnNPCMove;

        new internal static class Messages
        {
            #region Equipment

            public static void WriteEquipAdd(NPC npc, Item item)
            {
                PacketWriter stream = GameServer.SetupStream(ServerMessages.NPCEquipAddMessage);
                stream.Write((ushort)npc.ID);
                stream.Write((byte)item.slot);
                item.WriteEquipProperties(stream);
                npc.ForEachVisibleClient(client => client.Send(stream, NetPriority.Low, NetReliability.ReliableOrdered, 'W'));
            }

            public static void WriteEquipSwitch(NPC npc, Item item)
            {
                PacketWriter stream = GameServer.SetupStream(ServerMessages.NPCEquipSwitchMessage);
                stream.Write((ushort)npc.ID);
                stream.Write((byte)item.ID);
                stream.Write((byte)item.slot);
                npc.ForEachVisibleClient(client => client.Send(stream, NetPriority.Low, NetReliability.ReliableOrdered, 'W'));
            }

            public static void WriteEquipRemove(NPC npc, Item item)
            {
                PacketWriter stream = GameServer.SetupStream(ServerMessages.NPCEquipRemoveMessage);
                stream.Write((ushort)npc.ID);
                stream.Write((byte)item.ID);
                npc.ForEachVisibleClient(client => client.Send(stream, NetPriority.Low, NetReliability.ReliableOrdered, 'W'));
            }

            #endregion

            #region Health

            public static void WriteHealth(NPC npc)
            {
                var stream = GameServer.SetupStream(ServerMessages.NPCHealthMessage);
                stream.Write((ushort)npc.ID);
                stream.Write((ushort)npc.HPMax);
                stream.Write((ushort)npc.HP);
                npc.ForEachVisibleClient(client => client.Send(stream, NetPriority.High, NetReliability.ReliableOrdered, 'W'));
            }

            #endregion

            #region Fight Mode

            public static void WriteFightMode(NPC npc, bool fightMode)
            {
                PacketWriter stream = GameServer.SetupStream(fightMode ? ServerMessages.NPCFightModeSetMessage : ServerMessages.NPCFightModeUnsetMessage);
                stream.Write((ushort)npc.ID);
                npc.ForEachVisibleClient(client => client.Send(stream, NetPriority.High, NetReliability.ReliableOrdered, 'W'));
            }

            #endregion

            #region Positions

            public static void ReadPosDir(PacketReader stream, GameClient client, World world)
            {
                int id = stream.ReadUShort();
                NPC npc;
                if (world.TryGetVob(id, out npc) && (npc.guide == client || npc.client == client))
                {
                    var oldPos = npc.GetPosition();
                    var oldDir = npc.GetDirection();
                    var oldMovement = npc.Movement;

                    var pos = stream.ReadCompressedPosition();
                    var dir = stream.ReadCompressedDirection();
                    int bitfield = stream.ReadShort();

                    bool inAir = (bitfield & 0x8000) != 0;
                    NPCMovement movement = (NPCMovement)((bitfield >> 12) & 0x7);
                    float waterDepth = ((bitfield >> 6) & 0x3F) / (float)0x3F;
                    float waterLevel = (bitfield & 0x3F) / (float)0x3F;
                    Environment env = new Environment(inAir, waterLevel, waterDepth);

                    npc.movement = movement;
                    npc.environment = env;

                    npc.SetPosDir(pos, dir, client);
                    //vob.ScriptObject.OnPosChanged();

                    if (npc == client.Character)
                    {
                        client.UpdateVobList(world, pos);
                    }

                    if (OnNPCMove != null)
                        OnNPCMove(npc, oldPos, oldDir, oldMovement);
                }
            }

            #endregion
        }

        #endregion

        #region ScriptObject

        public partial interface IScriptNPC
        {
        }

        #endregion

        #region Cells

        internal NPCCell NpcCell = null;
        internal int npcCellID = -1;

        internal override void AddToCell(BigCell cell)
        {
            base.AddToCell(cell);
            if (this.IsPlayer)
                cell.AddClient(this.client);
        }

        internal override void RemoveFromCell()
        {
            if (this.IsPlayer)
            {
                this.Cell.RemoveClient(this.client);
            }
            base.RemoveFromCell();
        }

        #endregion

        #region Properties

        internal GameClient client;
        /// <summary> The client which is controlling this npc. </summary>
        public GameClient Client { get { return client; } }

        /// <summary> Returns true if this npc is controlled by a client. </summary>
        public bool IsPlayer { get { return client != null; } }

        #endregion

        #region Position & Direction

        protected override void WritePosDirMessage(GameClient exclude)
        {
            PacketWriter stream = GameServer.SetupStream(ServerMessages.NPCPosDirMessage);
            stream.Write((ushort)this.ID);
            stream.WriteCompressedPosition(this.pos);
            stream.WriteCompressedDirection(this.dir);
            stream.Write((byte)this.movement);

            if (exclude == null)
            {
                this.visibleClients.ForEach(client => client.Send(stream, NetPriority.Low, NetReliability.Unreliable, 'W'));
            }
            else
            {
                this.visibleClients.ForEach(client =>
                {
                    if (client != exclude)
                        client.Send(stream, NetPriority.Low, NetReliability.Unreliable, 'W');
                });
            }

            for (int i = 0; i < this.targetOf.Count; i++)
                this.targetOf[i].Send(stream, NetPriority.Low, NetReliability.Unreliable, 'W');
        }

        #endregion

        #region Health

        partial void pSetHealth()
        {
            if (this.isCreated)
                Messages.WriteHealth(this);
        }

        #endregion

        #region Spawn

        /// <summary> 
        /// Spawns the NPC in the given world at the given position & direction.
        /// If the NPC is a player, the spawn will be postponed until the client has loaded the map. 
        /// </summary>
        public override void Spawn(World world, Vec3f position, Vec3f direction)
        {
            if (this.IsPlayer)
            {
                if (world == null)
                    throw new ArgumentNullException("World is null!");

                if (this.Instance == null)
                    throw new Exception("Vob has no Instance!");

                if (this.ScriptObject == null)
                    throw new Exception("Vob has no ScriptObject!");

                if (this.isCreated)
                    throw new Exception("Vob is already spawned!");

                this.pos = position.CorrectPosition();
                this.dir = direction.CorrectDirection();
                this.world = world;

                // wait until the client has loaded the map
                World.Messages.WriteLoadWorld(this.client, world); // tell the client to change worlds first
            }
            else
            {
                base.Spawn(world, position, direction);
                world.AddToNPCCells(this);
            }
        }

        // spawn the client's character
        internal void SpawnPlayer()
        {
            this.world.AddClient(this.client);

            if (!this.IsSpawned)
            {
                base.Spawn(this.world, this.pos, this.dir);
                world.AddToNPCCells(this);
            }

            GameClient.Messages.WritePlayerControl(this.client, this);
        }

        partial void pBeforeDespawn()
        {
            if (this.IsPlayer)
            {
                this.client.SetControl(null);
            }
            this.world.RemoveFromNPCCells(this);
        }

        #endregion

        #region Equipment

        partial void pEquipItem(int slot, Item item)
        {
            if (this.isCreated)
                Messages.WriteEquipAdd(this, item);
        }

        partial void pEquipSwitch(int slot, Item item)
        {
            if (this.isCreated)
                Messages.WriteEquipSwitch(this, item);
        }

        partial void pUnequipItem(Item item)
        {
            if (this.isCreated)
                Messages.WriteEquipRemove(this, item);
        }

        #endregion

        #region Fight Mode

        partial void pSetFightMode(bool fightMode)
        {
            if (this.IsSpawned)
                Messages.WriteFightMode(this, fightMode);
        }

        #endregion
    }
}
