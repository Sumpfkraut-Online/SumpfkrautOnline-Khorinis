using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUC.Scripts.Arena
{
    enum ScriptMessages
    {
        GameInfo,

        JoinGame,
        Spectate,
        CharEdit,

        DuelRequest,
        DuelStart,
        DuelWin,
        DuelEnd,

        TOWarmup,
        TOStart,
        TOFinish,
        TOEnd,

        TOJoinTeam,
        TOSelectClass,
        SpectateTeam,

        TOTeamCount,

        ChatMessage,
        ChatTeamMessage,
        ChatPrivateMessage,

        ScoreDuelMessage,
        ScoreTOMessage,
        ScoreHordeMessage,

        PlayerInfoMessage,
        PlayerQuitMessage,

        PointsMessage,

        HordeJoin,
        HordeSpectate,
        HordeStart,
        HordePhase,
    }
}
