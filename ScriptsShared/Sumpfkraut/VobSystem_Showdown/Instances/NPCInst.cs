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

        public new WorldObjects.NPC BaseInst { get { return (WorldObjects.NPC)base.BaseInst; } }

        public new NPCDef Definition { get { return (NPCDef)base.Definition; } }

        #endregion

        public NPCInst(PacketReader stream) : base(new NPC(), stream)
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

        #endregion
    }
}
