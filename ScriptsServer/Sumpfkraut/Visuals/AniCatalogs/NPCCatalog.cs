using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace GUC.Scripts.Sumpfkraut.Visuals.AniCatalogs
{
    public class NPCCatalog : AniCatalog
    {
        protected override Dictionary<string, string> aniDict { get { return AniDict; } }
        static readonly Dictionary<string, string> AniDict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            // Fist attacks
            { "s_FistAttack", "FightFist.Fwd" },
            { "t_FistAttackMove", "FightFist.Run" },
            { "t_FistParade_0", "FightFist.Parry1" },
            { "t_FistParadeJumpB", "FightFist.Dodge" },

            // 1h attacks
            { "s_1HAttack", "Fight1H.Fwd" },
            { "t_1HAttackL", "Fight1H.Left" },
            { "t_1HAttackR", "Fight1H.Right" },
            { "t_1HAttackMove", "Fight1H.Run" },
            { "t_1HParade_0", "Fight1H.Parry1" },
            { "t_1HParade_0_A2", "Fight1H.Parry2" },
            { "t_1HParade_0_A3", "Fight1H.Parry3" },
            { "t_1HParadeJumpB", "Fight1H.Dodge" },
            
            // 2h attacks
            { "s_2HAttack", "Fight2H.Fwd" },
            { "t_2HAttackL", "Fight2H.Left" },
            { "t_2HAttackR", "Fight2H.Right" },
            { "t_2HAttackMove", "Fight2H.Run" },
            { "t_2HParade_0", "Fight2H.Parry1" },
            { "t_2HParade_0_A2", "Fight2H.Parry2" },
            { "t_2HParade_0_A3", "Fight2H.Parry3" },
            { "t_2HParadeJumpB", "Fight2H.Dodge" },

            // jumps
            { "t_Stand_2_Jump", "Jumps.Fwd" },
            { "t_RunL_2_Jump", "Jumps.Run" },
            { "t_Stand_2_JumpUp", "Jumps.Up" },

            // item handling
            { "t_IGet_2_Stand", "TakeItem" },
            { "t_IDrop_2_Stand", "DropItem" },
            { "t_HORN_Stand_2_S0", "ItemHandling.BlowHorn" },
            { "t_potionfast_Stand_2_S0", "ItemHandling.DrinkPotion" },
            { "t_Food_S0_2_Stand", "ItemHandling.EatSmall" },
            { "t_FoodHuge_S0_2_Stand", "ItemHandling.EatLarge" },
            { "t_RICE_Stand_2_S0", "ItemHandling.EatRice" },
            { "t_Meat_Stand_2_S0", "ItemHandling.EatMutton" },
            { "t_FIRESPIT_Stand_2_S0", "ItemHandling.FireSpit" },
            { "t_LUTE_Stand_2_S0", "ItemHandling.PlayLute" },
            { "t_MAP_Stand_2_S0", "ItemHandling.ReadScroll" },
            { "t_JOINT_Stand_2_S0", "ItemHandling.SmokeAJoint" },
            { "s_FIRESPIT_S2", "ItemHandling.UseTorch" },
        };

        public class FightAnis : AniCatalog
        {
            public ScriptAniJob Fwd { get; private set; }
            public ScriptAniJob Left { get; private set; }
            public ScriptAniJob Right { get; private set; }
            public ScriptAniJob Run { get; private set; }

            public ScriptAniJob Dodge { get; private set; }
            public ScriptAniJob Parry1 { get; private set; }
            public ScriptAniJob Parry2 { get; private set; }
            public ScriptAniJob Parry3 { get; private set; }
        }

        public FightAnis FightFist { get; private set; }
        public FightAnis Fight1H { get; private set; }
        public FightAnis Fight2H { get; private set; }

        public class JumpAnis : AniCatalog
        {
            public ScriptAniJob Run { get; private set; }
            public ScriptAniJob Fwd { get; private set; }
            public ScriptAniJob Up { get; private set; }
        }

        public class ItemHandlingAnis : AniCatalog
        {
            public ScriptAniJob BlowHorn { get; private set; }
            public ScriptAniJob DrinkPotion { get; private set; }
            public ScriptAniJob EatLarge { get; private set; }
            public ScriptAniJob EatSmall { get; private set; }
            public ScriptAniJob EatRice { get; private set; }
            public ScriptAniJob EatMutton { get; private set; }
            public ScriptAniJob FireSpit { get; private set; }
            public ScriptAniJob PlayLute { get; private set; }
            public ScriptAniJob ReadScroll { get; private set; }
            public ScriptAniJob SmokeAJoint { get; private set; }
            public ScriptAniJob UseTorch { get; private set; }
        }

        public ScriptAniJob TakeItem { get; private set; }
        public ScriptAniJob DropItem { get; private set; }

        public JumpAnis Jumps { get; private set; }
        public ItemHandlingAnis ItemHandling { get; private set; }

        public NPCCatalog()
        {
            FightFist = new FightAnis();
            Fight1H = new FightAnis();
            Fight2H = new FightAnis();
            Jumps = new JumpAnis();
            ItemHandling = new ItemHandlingAnis();
        }
    }
}
