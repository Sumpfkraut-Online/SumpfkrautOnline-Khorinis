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

            // mobs
            { "mob_chair_standToSit", "Mob.Chair.Sit" },
            { "mob_chair_sitToStand", "Mob.Chair.StandUp" },
            { "mob_bed_standToSleep", "Mob.Bed.Sleep" },
            { "mob_bed_sleepToStand", "Mob.Bed.StandUp" },
            { "mob_ladder_climbUp0", "Mob.Ladder.ClimbUp" },
            { "mob_ladder_climbDown0", "Mob.Ladder.ClimbDown" },

            // climbing
            { "climb_high", "Climbs.High" },
            { "climb_mid", "Climbs.Mid" },
            { "climb_low", "Climbs.Low" },

            // unconsciousness
            { "uncon_dropfront", "Unconscious.DropFront" },
            { "uncon_dropback", "Unconscious.DropBack" },
            { "uncon_standupfront", "Unconscious.StandUpFront" },
            { "uncon_standupback", "Unconscious.StandUpBack" },

            // bows
            { "drawbow_part0", "DrawBow.Draw" },
            { "undrawbow_part0", "DrawBow.Undraw" },
            { "drawbow_running", "DrawBow.DrawWhileRunning" },
            { "undrawbow_running", "DrawBow.UndrawWhileRunning" },

            { "aim_bow", "FightBow.Aim" },
            { "aiming_bow", "FightBow.Aiming" },
            { "reload_bow", "FightBow.Reload" },
            { "unaim_bow", "FightBow.Unaim" },
            
            // xbows
            { "drawXbow_part0", "DrawXBow.Draw" },
            { "undrawXbow_part0", "DrawXBow.Undraw" },
            { "drawXbow_running", "DrawXBow.DrawWhileRunning" },
            { "undrawXbow_running", "DrawXBow.UndrawWhileRunning" },

            { "aim_xbow", "FightXBow.Aim" },
            { "aiming_xbow", "FightXBow.Aiming" },
            { "reload_xbow", "FightXBow.Reload" },
            { "unaim_xbow", "FightXBow.Unaim" },

            // item handling
            { "take_item", "ItemHandling.TakeItem" },
            { "drop_item", "ItemHandling.DropItem" },
            { "chug_potion", "ItemHandling.DrinkPotion" },

            // gestures
            { "gesture_dontknow", "Gestures.DontKnow" },
            { "plunder", "Gestures.Plunder" },


            { "t_HORN_Stand_2_S0", "ItemHandling.BlowHorn" },
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

        #region Mobs
        public class MobAnis : AniCatalog
        {
            #region Chair
            public class ChairAnis : AniCatalog
            {
                public ScriptAniJob Sit { get; private set; }
                public ScriptAniJob StandUp { get; private set; }
            }
            public ChairAnis Chair { get; private set; }
            #endregion

            #region Bed
            public class BedAnis : AniCatalog
            {
                public ScriptAniJob Sleep { get; private set; }
                public ScriptAniJob StandUp { get; private set; }
            }
            public BedAnis Bed { get; private set; }
            #endregion

            #region Ladder
            public class LadderAnis : AniCatalog
            {
                public ScriptAniJob ClimbUp { get; private set; }
                public ScriptAniJob ClimbDown { get; private set; }
            }
            public LadderAnis Ladder { get; private set; }
            #endregion
        }
        public MobAnis Mob { get; private set; }
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

        #region Ranged Weapons

        public class RangedAnis : AniCatalog
        {
            public ScriptAniJob Aim { get; private set; }
            public ScriptAniJob Aiming { get; private set; }
            public ScriptAniJob Reload { get; private set; }
            public ScriptAniJob Unaim { get; private set; }
        }

        public RangedAnis FightBow { get; private set; }
        public RangedAnis FightXBow { get; private set; }

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

        #region Climbing
        public class ClimbAnis : AniCatalog
        {
            public ScriptAniJob High { get; private set; }
            public ScriptAniJob Mid { get; private set; }
            public ScriptAniJob Low { get; private set; }
        }
        public ClimbAnis Climbs { get; private set; }
        #endregion

        #region Unconscious

        public class UnconsciousAnis : AniCatalog
        {
            public ScriptAniJob DropFront { get; private set; }
            public ScriptAniJob DropBack { get; private set; }

            public ScriptAniJob StandUpFront { get; private set; }
            public ScriptAniJob StandUpBack { get; private set; }
        }
        public UnconsciousAnis Unconscious { get; private set; }

        #endregion

        #region Gestures

        public class GestureAnis : AniCatalog
        {
            public ScriptAniJob DontKnow { get; private set; }
            public ScriptAniJob Plunder { get; private set; }
        }
        public GestureAnis Gestures { get; private set; }

        #endregion
    }
}
