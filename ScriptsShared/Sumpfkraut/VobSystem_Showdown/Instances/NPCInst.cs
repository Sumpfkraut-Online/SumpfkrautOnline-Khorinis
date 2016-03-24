using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUC.Enumeration;
using GUC.WorldObjects;
using GUC.WorldObjects.Mobs;
using GUC.Network;
using GUC.Animations;
using GUC.Scripts.Sumpfkraut.Visuals;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;

namespace GUC.Scripts.Sumpfkraut.VobSystem.Instances
{
    public partial class NPCInst : VobInst, NPC.IScriptNPC
    {
        #region Properties

        public new NPC BaseInst { get { return (NPC)base.BaseInst; } }

        public new NPCDef Definition { get { return (NPCDef)base.Definition; } }

        public ModelDef Model { get { return this.Definition.Model; } }

        #endregion

        public NPCInst() : base(new NPC())
        {
        }

        public void SetState(NPCStates state)
        {
            this.BaseInst.SetState(state);
        }

        public void Jump()
        {
            this.BaseInst.Jump();
        }

        #region Animations

        public void ApplyOverlay(ScriptOverlay overlay)
        {
            this.BaseInst.ApplyOverlay(overlay.BaseOverlay);
        }

        public void RemoveOverlay(ScriptOverlay overlay)
        {
            this.BaseInst.RemoveOverlay(overlay.BaseOverlay);
        }

        public void StartAnimation(ScriptAniJob job, Action onStop = null)
        {
            this.BaseInst.StartAnimation(job.BaseAniJob, onStop);
        }

        public void StopAnimation(bool fadeOut = false)
        {
            this.BaseInst.StopAnimation(fadeOut);
        }

        #endregion

        #region Client Commands

        partial void pOnCmdMove(NPCStates state);
        public void OnCmdMove(NPCStates state)
        {
            pOnCmdMove(state);
        }

        partial void pOnCmdJump();
        public void OnCmdJump()
        {
            pOnCmdJump();
        }

        public void OnCmdDrawItem(Item item)
        {
            throw new NotImplementedException();
        }

        public void OnCmdDropItem(Item item)
        {
            throw new NotImplementedException();
        }


        public void OnCmdPickupItem(Item item)
        {
            throw new NotImplementedException();
        }

        public void OnCmdUseItem(Item item)
        {
            throw new NotImplementedException();
        }

        public void OnCmdUseMob(MobInter mob)
        {
            throw new NotImplementedException();
        }

        public void OnWriteTakeControl(PacketWriter stream)
        {
            // write everything the player needs to know about this npc
            // i.e. abilities, level, guild etc
        }

        public void OnReadTakeControl(PacketReader stream)
        {
            // read everything the player needs to know about this npc
            // i.e. abilities, level, guild etc
        }

        partial void pOnCmdApplyOverlay(ScriptOverlay overlay);
        public void OnCmdApplyOverlay(Overlay overlay)
        {
            this.pOnCmdApplyOverlay((ScriptOverlay)overlay.ScriptObject);
        }

        partial void pOnCmdRemoveOverlay(ScriptOverlay overlay);
        public void OnCmdRemoveOverlay(Overlay overlay)
        {
            this.pOnCmdRemoveOverlay((ScriptOverlay)overlay.ScriptObject);
        }

        partial void pOnCmdStartAni(ScriptAniJob job);
        public void OnCmdStartAni(AniJob job)
        {
            this.pOnCmdStartAni((ScriptAniJob)job.ScriptObject);
        }

        partial void pOnCmdStopAni(bool fadeOut);
        public void OnCmdStopAni(bool fadeOut)
        {
            this.pOnCmdStopAni(fadeOut);
        }

        public void AddItem(Item item)
        {
            this.AddItem((ItemInst)item.ScriptObject);
        }

        public void AddItem(ItemInst item)
        {
            this.BaseInst.Inventory.Add(item.BaseInst);
        }

        public void RemoveItem(Item item)
        {
            this.RemoveItem((ItemInst)item.ScriptObject);
        }

        public void RemoveItem(ItemInst item)
        {
            this.BaseInst.Inventory.Remove(item.BaseInst);
        }

        public void EquipItem(int slot, Item item)
        {
            this.EquipItem(slot, (ItemInst)item.ScriptObject);
        }

        partial void pEquipItem(ItemInst item);
        public void EquipItem(int slot, ItemInst item)
        {
            this.BaseInst.EquipItem(slot, item.BaseInst);
            pEquipItem(item);
        }

        public void UnequipItem(Item item)
        {
            this.UnequipItem((ItemInst)item.ScriptObject);
        }

        partial void pUnequipItem(ItemInst item);
        public void UnequipItem(ItemInst item)
        {
            this.BaseInst.UnequipItem(item.BaseInst);
            pUnequipItem(item);
        }

        #endregion
    }
}
