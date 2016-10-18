using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using RakNet;
using GUC.Network;
using GUC.WorldObjects.Mobs;
using GUC.Types;
using GUC.Network.Messages;
using GUC.WorldObjects.Cells;
using GUC.Animations;

namespace GUC.WorldObjects
{
    public partial class NPC
    {
        #region ScriptObject

        public partial interface IScriptNPC
        {
            void OnCmdMove(MoveState state);
            void OnCmdEquipItem(int slot, Item item);
            void OnCmdUnequipItem(Item item);
            void OnCmdAniStart(Animation ani);
            void OnCmdAniStart(Animation ani, object[] netArgs);
            void OnCmdAniStop(bool fadeOut);
            void OnCmdSetFightMode(bool fightMode);



            void OnCmdUseMob(MobInter mob);
            void OnCmdUseItem(Item item);
            void OnCmdPickupItem(Item item);
            void OnCmdDropItem(Item item);
        }

        #endregion

        public void UpdatePropertiesFast()
        {
            throw new NotImplementedException();
        }

        public void UpdateProperties()
        {
            throw new NotImplementedException();
        }

        partial void pSetMovement(MoveState state)
        {
            if (this.isCreated)
                NPCMessage.WriteMoveState(this, state);
        }

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

        #region Health

        partial void pSetHealth()
        {
            if (this.isCreated)
                NPCMessage.WriteHealthMessage(this);
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

                if (this.instance == null)
                    throw new Exception("Vob has no Instance!");

                if (this.ScriptObject == null)
                    throw new Exception("Vob has no ScriptObject!");

                if (this.isCreated)
                    throw new Exception("Vob is already spawned!");

                this.pos = position.CorrectPosition();
                this.dir = direction.CorrectDirection();
                this.world = world;

                // wait until the client has loaded the map
                WorldMessage.WriteLoadMessage(this.client, world); // tell the client to change worlds first
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
            PacketWriter stream = GameServer.SetupStream(NetworkIDs.PlayerControlMessage);
            stream.Write((ushort)this.ID);
            this.WriteTakeControl(stream);
            this.Client.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, '\0');

            base.Spawn(this.world, this.pos, this.dir);
            world.AddToNPCCells(this);
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
            {
                NPCMessage.WriteEquipMessage(this, item);
                if (this.IsPlayer)
                    InventoryMessage.WriteEquipMessage(this, item);
            }
        }

        partial void pEquipSwitch(int slot, Item item)
        {
            if (this.isCreated)
            {
                NPCMessage.WriteEquipSwitchMessage(this, item);
            }
        }

        partial void pUnequipItem(Item item)
        {
            if (this.isCreated)
            {
                NPCMessage.WriteUnequipMessage(this, item);
                if (this.IsPlayer)
                    InventoryMessage.WriteUnequipMessage(this, item);
            }
        }

        #endregion

        #region Animations

        partial void pAddOverlay(Overlay overlay)
        {
            if (this.isCreated)
                NPCMessage.WriteApplyOverlayMessage(this, overlay);
        }

        partial void pRemoveOverlay(Overlay overlay)
        {
            if (this.isCreated)
                NPCMessage.WriteRemoveOverlayMessage(this, overlay);
        }

        public void StartAnimation(Animation ani, Action onStop, params object[] netArgs)
        {
            if (this.PlayAni(ani, onStop))
            {
                NPCMessage.WriteAniStart(this, ani, netArgs);
            }
        }

        partial void pStartAnimation(Animation ani)
        {
            NPCMessage.WriteAniStart(this, ani);
        }

        partial void pStopAnimation(Animation ani, bool fadeOut)
        {
            NPCMessage.WriteAniStop(this, ani, fadeOut);
        }

        #endregion

        #region Fight Mode

        partial void pSetFightMode(bool fightMode)
        {
            if (this.IsSpawned)
                if (fightMode)
                {
                    NPCMessage.WriteSetFightMode(this);
                }
                else
                {
                    NPCMessage.WriteUnsetFightMode(this);
                }
        }

        #endregion
    }
}
