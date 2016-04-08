using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUC.Scripts.TFFA
{
    public enum Team
    {
        Spec,
        AL,
        NL,
        Max    
    }

    public enum PlayerClass
    {
        None,
        Light,
        Heavy
    }

    public enum MenuMsgID
    {
        OpenTeamMenu,
        CloseTeamMenu,
        OpenClassMenu,
        CloseClassMenu,
        SelectTeam,
        SelectClass,
        SetName,
        OpenScoreboard,
        CloseScoreboard
    }
}
