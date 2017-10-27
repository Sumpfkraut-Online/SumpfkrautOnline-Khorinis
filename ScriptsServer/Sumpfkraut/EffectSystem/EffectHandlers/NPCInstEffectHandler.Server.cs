using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;
using GUC.Utilities;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers
{
    partial class NPCInstEffectHandler
    {
        private static void OnHit(NPCInst attacker, NPCInst target, int damage)
        {
            throw new NotImplementedException();
        }

        LockTimer jumpLockTimer = new LockTimer(500);
        public void TryJump(JumpMoves move)
        {
            if (Host.IsDead || Host.Environment.InAir)
                return;

            if (Host.ModelInst.GetActiveAniFromLayer(1) != null)
                return;

            if (!jumpLockTimer.IsReady) // don't spam
                return;

            Host.DoJump(move, Host.GetDirection() * 200f + new Types.Vec3f(0, 1000, 0));
        }

        public void TryDrawFists()
        {
            if (Host.ModelDef.Visual != "HUMANS.MDS")
                return;

            if (Host.IsDead || this.Host.ModelInst.IsInAnimation())
                return;

            if (this.Host.IsInFightMode)
            {
                if (this.Host.DrawnWeapon != null)
                {
                    return;
                    //this.UnequipItem(this.DrawnWeapon);
                }
                else
                {
                    this.Host.DoUndrawFists();
                }
            }
            else
            {
                this.Host.DoDrawFists();
            }
        }

        public void TryDrawWeapon(ItemInst item)
        {
            if (Host.ModelDef.Visual != "HUMANS.MDS" && Host.ModelDef.Visual != "ORC.MDS")
                return;

            if (Host.IsDead || this.Host.ModelInst.IsInAnimation())
                return;
            
            if (this.Host.IsInFightMode)
            {
                if (this.Host.DrawnWeapon != null)
                {
                    this.Host.DoUndrawWeapon(Host.DrawnWeapon);
                }
                else
                {
                    this.Host.DoUndrawFists();
                }
            }
            else
            {
                this.Host.DoDrawWeapon(item);
            }
        }

        public void TryFightMove(FightMoves move)
        {
            if (Host.IsDead || (Host.Environment.InAir && move != FightMoves.Run) || Host.ModelInst.GetActiveAniFromLayer(2) != null)
                return;
            
            var otherAni = Host.ModelInst.GetActiveAniFromLayer(1);
            if (otherAni != null && otherAni != Host.FightAnimation)
                return;
            
            if (Host.FightAnimation != null)
            {
                if (Host.CanCombo)
                {
                    var current = Host.CurrentFightMove;
                    if (current == FightMoves.Right && move == FightMoves.Right)
                        return;
                    if (current == FightMoves.Left && move == FightMoves.Left)
                        return;

                    Host.DoFightMove(move, current == FightMoves.Fwd ? Host.ComboNum + 1 : 0);
                }
            }
            else
            {
                Host.DoFightMove(move, 0);
            }
        }

        public void TryEquipItem(ItemInst item)
        {
            if (this.Host.IsDead || this.Host.ModelInst.IsInAnimation() 
                || this.Host.Environment.InAir || this.Host.DrawnWeapon != null
                || this.Host.IsInFightMode)
                return;

            this.Host.EquipItem(item);
        }


        public void TryUnequipItem(ItemInst item)
        {
            if (this.Host.IsDead || this.Host.ModelInst.IsInAnimation()
                || this.Host.Environment.InAir || this.Host.DrawnWeapon != null
                || this.Host.IsInFightMode)
                return;

            this.Host.UnequipItem(item);
        }
    }
}
