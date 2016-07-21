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

        #endregion

        #region Properties

        internal GameClient client = null;
        public GameClient Client { get { return client; } }
        public bool IsPlayer { get { return Client != null; } }

        partial void pSetHealth()
        {
            if (this.isCreated)
                NPCMessage.WriteHealthMessage(this);
        }

        #endregion

        #region Spawn

        public override void Spawn(World world, Vec3f position, Vec3f direction)
        {
            if (this.IsPlayer)
            {
                if (world == null)
                    throw new ArgumentNullException("World is null!");

                if (this.isCreated)
                    throw new Exception("Vob is already spawned!");

                this.pos = position;
                this.dir = direction;
                this.world = world;

                WorldMessage.WriteLoadMessage(this.client, world); // tell the client to change worlds first
            }
            else
            {
                base.Spawn(world, position, direction);
                this.world.AddToNPCCells(this);
            }
        }

        // wait until the client has loaded the map
        internal void SpawnPlayer()
        {
            PacketWriter stream = GameServer.SetupStream(NetworkIDs.PlayerControlMessage);
            stream.Write((ushort)this.ID);
            this.WriteTakeControl(stream);
            this.Client.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, '\0');

            base.Spawn(world, this.pos, this.dir);
            this.world.AddToNPCCells(this);
        }

        partial void pDespawn()
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
