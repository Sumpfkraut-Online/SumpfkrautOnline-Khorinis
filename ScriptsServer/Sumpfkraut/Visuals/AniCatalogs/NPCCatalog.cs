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
            // Fists
            { "drawfists_part0", "DrawFists.Draw" },
            { "undrawfists_part0", "DrawFists.Undraw" },
            { "drawfists_running", "DrawFists.DrawWhileRunning" },
            { "undrawfists_running", "DrawFists.UndrawWhileRunning" },

            { "fistattack_fwd0", "FightFist.Fwd[0]" },
            { "fistattack_fwd1", "FightFist.Fwd[1]" },

            { "fistattack_run", "FightFist.Run" },
            { "fist_parade", "FightFist.Parry[0]" },
            { "fist_jumpback", "FightFist.Dodge" },

            // 1H
            { "draw1h_part0", "Draw1H.Draw" },
            { "undraw1h_part0", "Draw1H.Undraw" },
            { "draw1h_running", "Draw1H.DrawWhileRunning" },
            { "undraw1h_running", "Draw1H.UndrawWhileRunning" },

            { "1HAttack_fwd0", "Fight1H.Fwd[0]" },
            { "1HAttack_fwd1", "Fight1H.Fwd[1]" },
            { "1HAttack_fwd2", "Fight1H.Fwd[2]" },
            { "1HAttack_fwd3", "Fight1H.Fwd[3]" },
            { "1hAttack_left", "Fight1H.Left" },
            { "1hAttack_right", "Fight1H.Right" },
            { "1hattack_run", "Fight1H.Run" },
            { "1h_parade0", "Fight1H.Parry[0]" },
            { "1h_parade1", "Fight1H.Parry[1]" },
            { "1h_parade2", "Fight1H.Parry[2]" },
            { "1h_dodge", "Fight1H.Dodge" },
            
            // 2H
            { "draw2h_part0", "Draw2H.Draw" },
            { "undraw2h_part0", "Draw2H.Undraw" },
            { "draw2h_running", "Draw2H.DrawWhileRunning" },
            { "undraw2h_running", "Draw2H.UndrawWhileRunning" },

            { "2HAttack_fwd0", "Fight2H.Fwd[0]" },
            { "2HAttack_fwd1", "Fight2H.Fwd[1]" },
            { "2HAttack_fwd2", "Fight2H.Fwd[2]" },
            { "2HAttack_fwd3", "Fight2H.Fwd[3]" },
            { "2hAttack_left", "Fight2H.Left" },
            { "2hAttack_right", "Fight2H.Right" },
            { "2hattack_run", "Fight2H.Run" },
            { "2h_parade0", "Fight2H.Parry[0]" },
            { "2h_parade1", "Fight2H.Parry[1]" },
            { "2h_parade2", "Fight2H.Parry[2]" },
            { "2h_dodge", "Fight2H.Dodge" },

            // jumps
            { "jump_fwd", "Jumps.Fwd" },
            { "jump_run", "Jumps.Run" },
            { "jump_up", "Jumps.Up" },

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
