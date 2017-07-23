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

        public void FightMove(FightMoves move)
        {
            if (Host.IsDead)
                return;

            Host.DoFightMove(move, 0);
        }
    }
}
