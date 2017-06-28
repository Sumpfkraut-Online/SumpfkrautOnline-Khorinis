using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers
{
    public class NPCEffectHandler : VobEffectHandler
    {

        new public static readonly string _staticName = "NPCEffectHandler (static)";



        static NPCEffectHandler ()
        {
            PrintStatic(typeof(BaseEffectHandler), "Start subscribing ChangeDestinations and EventHandler...");

            NPCInst.sOnHit += OnHit;

            PrintStatic(typeof(BaseEffectHandler), "Finished subscribing ChangeDestinations and EventHandler...");
        }
        

        public NPCEffectHandler (List<Effect> effects, NPCInst host)
            : this("NPCEffectHandler (default)", effects, host)
        { }
        
        public NPCEffectHandler (string objName, List<Effect> effects, NPCInst host) 
            : base(objName, effects, host)
        { }
        
        new public NPCInst Host { get { return (NPCInst)base.Host; } }

        private static void OnHit (NPCInst attacker, NPCInst target, int damage)
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

        //protected override void ApplyEffect (Effect effect, bool reverse = false)
        //{
        //    Print("Apply what? Naaaa!");
        //}
        
    }

}
