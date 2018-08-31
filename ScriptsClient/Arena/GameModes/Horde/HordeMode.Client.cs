using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripts.Sumpfkraut.Menus;


namespace GUC.Scripts.Arena.GameModes.Horde
{
    partial class HordeMode
    {

        public override Action OpenJoinMenu { get { return MenuClassSelect.Instance.Open; } }

        public override ScoreBoardScreen ScoreBoard { get { return HordeScoreBoard.Instance; } }

        public static void End(bool win)
        {
            if (!HordeMode.IsActive)
                return;

            HordeScoreBoard.Instance.Open();
            ScreenScrollText.AddText(win ? "Sieg!" : "Niederlage!", GUI.GUCView.Fonts.Menu);
        }
    }
}
