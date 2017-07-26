using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using GUC.Utilities;

namespace GUC.Scripts.Sumpfkraut.Visuals.AniCatalogs
{
    public class NPCCatalog : AniCatalog
    {
        protected override Dictionary<string, string> aniDict { get { return AniDict; } }
        static readonly Dictionary<string, string> AniDict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            // Fist attacks
            { "fistattack_fwd0", "FightFist.Fwd[0]" },
            { "fistattack_fwd1", "FightFist.Fwd[1]" },

            { "fistattack_run", "FightFist.Run" },
            { "fist_parade", "FightFist.Parry[0]" },
            { "fist_jumpback", "FightFist.Dodge" },


            // 1H Handling
            { "s_1HAttack", "Fight1H.Fwd" },
            { "t_1HAttackL", "Fight1H.Left" },
            { "t_1HAttackR", "Fight1H.Right" },
            { "t_1HAttackMove", "Fight1H.Run" },
            { "t_1HParade_0", "Fight1H.Parry[0]" },
            { "t_1HParade_0_A2", "Fight1H.Parry[1]" },
            { "t_1HParade_0_A3", "Fight1H.Parry[2]" },
            { "t_1HParadeJumpB", "Fight1H.Dodge" },
            
            // 2h attacks
            { "s_2HAttack", "Fight2H.Fwd" },
            { "t_2HAttackL", "Fight2H.Left" },
            { "t_2HAttackR", "Fight2H.Right" },
            { "t_2HAttackMove", "Fight2H.Run" },
            { "t_2HParade_0", "Fight2H.Parry[0]" },
            { "t_2HParade_0_A2", "Fight2H.Parry[1]" },
            { "t_2HParade_0_A3", "Fight2H.Parry[2]" },
            { "t_2HParadeJumpB", "Fight2H.Dodge" },

            // jumps
            { "t_Stand_2_Jump", "Jumps.Fwd" },
            { "t_RunL_2_Jump", "Jumps.Run" },
            { "t_Stand_2_JumpUp", "Jumps.Up" },

            // item handling
            { "t_IGet_2_Stand", "ItemHandling.TakeItem" },
            { "t_IDrop_2_Stand", "ItemHandling.DropItem" },
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

            // Weapon Drawing
            { "drawfists_part0", "DrawFists.Draw" },
            { "undrawfists_part0", "DrawFists.Undraw" },
            { "drawfists_running", "DrawFists.DrawWhileRunning" },
            { "undrawfists_running", "DrawFists.UndrawWhileRunning" },

            { "sok_draw1H", "Draw1H.Draw" },
            { "sok_undraw1H", "Draw1H.Undraw" },
            { "t_Move_2_1hMove", "Draw1H.DrawWhileRunning" },
            { "t_1hMove_2_Move", "Draw1H.UndrawWhileRunning" },

            { "sok_draw2H", "Draw2H.Draw" },
            { "sok_undraw2H", "Draw2H.Undraw" },
            { "t_Move_2_2hMove", "Draw2H.DrawWhileRunning" },
            { "t_2hMove_2_Move", "Draw2H.UndrawWhileRunning" },

            { "sok_drawBow", "DrawBow.Draw" },
            { "sok_undrawBow", "DrawBow.Undraw" },
            { "t_Move_2_BowMove", "DrawBow.DrawWhileRunning" },
            { "t_BowMove_2_Move", "DrawBow.UndrawWhileRunning" },

            { "sok_drawXBow", "DrawXBow.Draw" },
            { "sok_undrawXBow", "DrawXBow.Undraw" },
            { "t_Move_2_CBowMove", "DrawXBow.DrawWhileRunning" },
            { "t_CBowMove_2_Move", "DrawXBow.UndrawWhileRunning" },

        };

        #region FightAnis
        public class FightAnis : AniCatalog
        {
            public AniJobCollection Fwd { get; private set; }
            public ScriptAniJob Left { get; private set; }
            public ScriptAniJob Right { get; private set; }
            public ScriptAniJob Run { get; private set; }

            public ScriptAniJob Dodge { get; private set; }
            public AniJobCollection Parry { get; private set; }

            public ScriptAniJob GetRandomParry()
            {
                return Parry.ElementAtOrDefault(Parry.Count > 1 ? Randomizer.GetInt(0, Parry.Count) : 0);
            }

        }
        public FightAnis FightFist { get; private set; }
        public FightAnis Fight1H { get; private set; }
        public FightAnis Fight2H { get; private set; }
        #endregion

        #region Jumps
        public class JumpAnis : AniCatalog
        {
            public ScriptAniJob Run { get; private set; }
            public ScriptAniJob Fwd { get; private set; }
            public ScriptAniJob Up { get; private set; }
        }
        public JumpAnis Jumps { get; private set; }
        #endregion

        #region Weapondrawing
        public class DrawWeaponAnis : AniCatalog
        {
            public ScriptAniJob Draw { get; private set; }
            public ScriptAniJob Undraw { get; private set; }
            public ScriptAniJob DrawWhileRunning { get; private set; }
            public ScriptAniJob UndrawWhileRunning { get; private set; }
        }
        public DrawWeaponAnis Draw1H { get; private set; }
        public DrawWeaponAnis Draw2H { get; private set; }
        public DrawWeaponAnis DrawFists { get; private set; }
        public DrawWeaponAnis DrawBow { get; private set; }
        public DrawWeaponAnis DrawXBow { get; private set; }
        public DrawWeaponAnis DrawMagic { get; private set; }
        #endregion

        #region Itemhandling
        public class ItemHandlingAnis : AniCatalog
        {
            public ScriptAniJob TakeItem { get; private set; }
            public ScriptAniJob DropItem { get; private set; }
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
        public ItemHandlingAnis ItemHandling { get; private set; }
        #endregion
    }
}
