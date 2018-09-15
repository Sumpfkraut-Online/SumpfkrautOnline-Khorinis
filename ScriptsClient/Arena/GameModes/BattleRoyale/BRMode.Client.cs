using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Arena.GameModes.BattleRoyale
{
    partial class BRMode
    {
        public override Action OpenJoinMenu { get { return BRJoinMenu.Instance.Open; } }
        public override ScoreBoardScreen ScoreBoard { get { return null; } }
        public override void OpenStatusMenu()
        {
            BRStatusMenu.Instance.Open();
        }
    }
}
