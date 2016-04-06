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

        public NPCStates State { get { return this.BaseInst.State; } }

        #endregion

        partial void pConstruct();
        public NPCInst() : base(new NPC())
        {
            pConstruct();
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

        #region Overlays

        public void ApplyOverlay(Overlay overlay)
        {
            this.ApplyOverlay((ScriptOverlay)overlay.ScriptObject);
        }

        public void ApplyOverlay(ScriptOverlay overlay)
        {
            this.BaseInst.ApplyOverlay(overlay.BaseOverlay);
        }

        public void RemoveOverlay(Overlay overlay)
        {
            this.RemoveOverlay((ScriptOverlay)overlay.ScriptObject);
        }

        public void RemoveOverlay(ScriptOverlay overlay)
        {
            this.BaseInst.RemoveOverlay(overlay.BaseOverlay);
        }

        #endregion

        public bool TryGetAniFromJob(ScriptAniJob job, out ScriptAni ani)
        {
            Animation baseAni;
            if (this.BaseInst.TryGetAniFromJob(job.BaseAniJob, out baseAni))
            {
                ani = (ScriptAni)baseAni.ScriptObject;
                return true;
            }
            ani = null;
            return false;
        }

        public void StartAnimation(Animation ani)
        {
            this.StartAnimation((ScriptAni)ani.ScriptObject);
        }

        public void StartAnimation(ScriptAni ani, Action onStop = null)
        {
            this.BaseInst.StartAnimation(ani.BaseAni, onStop);
        }

        public void StopAnimation(bool fadeOut = false)
        {
            this.BaseInst.StopAnimation(fadeOut);
        }

        public bool IsInAni { get { return this.BaseInst.IsInAnimation; } }
        public ScriptAni CurrentAni { get { return this.BaseInst.IsInAnimation ? (ScriptAni)this.BaseInst.CurrentAni.ScriptObject : null; } }

        public bool IsInFightAni { get { return this.CurrentAni.AniJob.IsFightMove; } }
        public bool IsInAttackAni { get { return this.CurrentAni.AniJob.IsAttack; } }

        #endregion

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

        public void SetHealth(int hp, int hpmax)
        {
            this.BaseInst.SetHealth(hp, hpmax);
        }
    }
}
