using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers
{
    partial class NPCInstEffectHandler
    {
        private static void OnHit(NPCInst attacker, NPCInst target, int damage)
        {
            throw new NotImplementedException();
        }

        public void TryJump(JumpMoves move)
        {
            if (Host.IsDead || Host.Environment.InAir)
                return;

            if (Host.ModelInst.IsInAnimation())
                return;

            Host.DoJump(move, Host.GetDirection() * 100f + new Types.Vec3f(0, 500, 0));
        }

        public void TryDrawFists()
        {
            if (Host.IsDead || this.Host.ModelInst.IsInAnimation())
                return;

            if (this.Host.IsInFightMode)
            {
                if (this.Host.DrawnWeapon != null)
                {
                    throw new NotImplementedException();
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

        public void TryFightMove(FightMoves move)
        {
            if (Host.IsDead || Host.Environment.InAir)
                return;
            
            var otherAni = Host.ModelInst.GetActiveAniFromLayer(1);
            if (otherAni != null && otherAni.Ani.ScriptObject != Host.FightAnimation)
                return;
            
            if (Host.FightAnimation != null)
            {
                if (Host.CanCombo)
                    Host.DoFightMove(move, Host.ComboNum + 1);
            }
            else
            {
                Host.DoFightMove(move, 0);
            }
        }
    }
}
