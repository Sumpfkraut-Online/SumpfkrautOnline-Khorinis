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


        public NPCEffectHandler (List<Effect> effects, NPCDef host)
            : this("NPCEffectHandler (default)", effects, host)
        { }

        public NPCEffectHandler (List<Effect> effects, NPCInst host)
            : this("NPCEffectHandler (default)", effects, host)
        { }

        public NPCEffectHandler (string objName, List<Effect> effects, NPCDef host) 
            : base(objName, effects, host)
        { }

        public NPCEffectHandler (string objName, List<Effect> effects, NPCInst host) 
            : base(objName, effects, host)
        { }

        public NPCInst Self { get { return (NPCInst)this.host; } }

        private static void OnHit (NPCInst attacker, NPCInst target, int damage)
        {
            throw new NotImplementedException();
        }
        
        public void DoAttack(FightMoves move)
        {
            if (Self.IsDead)
                return;
        }

        //protected override void ApplyEffect (Effect effect, bool reverse = false)
        //{
        //    Print("Apply what? Naaaa!");
        //}
        
    }

}
