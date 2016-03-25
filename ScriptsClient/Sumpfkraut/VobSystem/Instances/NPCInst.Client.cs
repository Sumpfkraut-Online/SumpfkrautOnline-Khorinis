using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.Scripts.Sumpfkraut.Visuals;
using GUC.Scripts.Sumpfkraut.WorldSystem;

namespace GUC.Scripts.Sumpfkraut.VobSystem.Instances
{
    public partial class NPCInst
    {
        partial void pOnCmdMove(NPCStates state)
        {
            this.SetState(state);
        }

        partial void pOnCmdJump()
        {
            this.Jump();
        }

        partial void pOnCmdApplyOverlay(ScriptOverlay overlay)
        {
            this.ApplyOverlay(overlay);
        }

        partial void pOnCmdRemoveOverlay(ScriptOverlay overlay)
        {
            this.RemoveOverlay(overlay);
        }


        partial void pOnCmdStartAni(ScriptAniJob job)
        {
            this.StartAnimation(job);
        }

        partial void pOnCmdStopAni(bool fadeOut)
        {
            this.StopAnimation(fadeOut);
        }

        public override void Spawn(WorldInst world)
        {
            base.Spawn(world);
            this.BaseInst.ForEachEquippedItem(i => this.pEquipItem((ItemInst)i.ScriptObject));
        }

        partial void pEquipItem(ItemInst item)
        {

            if (!this.IsSpawned)
                return;

            switch (item.ItemType)
            {
                case Definitions.ItemTypes.Wep2H:
                    Log.Logger.Log(item.BaseInst.Model.Visual);
                    item.BaseInst.gVob.MainFlag = Gothic.Objects.oCItem.MainFlags.ITEM_KAT_NF;
                    item.BaseInst.gVob.Flags = Gothic.Objects.oCItem.ItemFlags.ITEM_2HD_SWD;
                    item.BaseInst.gVob.Flags |= item.BaseInst.gVob.MainFlag;
                    this.BaseInst.gVob.EquipWeapon(item.BaseInst.gVob);
                    break;

                default:
                    break;
            }
        }

        partial void pUnequipItem(ItemInst item)
        {
            this.BaseInst.gVob.UnequipItem(item.BaseInst.gVob);
        }
    }
}
