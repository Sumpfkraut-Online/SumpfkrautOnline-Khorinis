using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUC.Scripts.TFFA
{
    public enum TFFAPhase
    {
        Waiting,
        Fight,
        End
    }

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
        ClientInfoGroup,
        ClientConnect,
        ClientDisconnect,
        
        ClientTeam,
        ClientClass,
        ClientName,
        ClientNPC,

        PhaseMsg,
        WinMsg,

        OpenScoreboard,
        CloseScoreboard,

        AllChat,
        TeamChat
    }
}
