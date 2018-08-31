using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Arena.GameModes.Horde
{
    partial class HordeMode
    {

        public override Action OpenJoinMenu { get { return MenuClassSelect.Instance.Open; } }

        public override ScoreBoardScreen ScoreBoard { get { return HordeScoreBoard.Instance; } }
    }
}
