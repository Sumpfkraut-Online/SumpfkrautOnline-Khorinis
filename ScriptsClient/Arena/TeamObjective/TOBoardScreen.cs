using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;

namespace GUC.Scripts.Arena
{
    class TOBoardScreen : ScoreBoardScreen
    {
        public static readonly TOBoardScreen Instance = new TOBoardScreen();

        private TOBoardScreen() : base(ScriptMessages.ScoreTOMessage)
        {
            OnOpen += UpdateBoards;
        }

        public override void ReadMessage(PacketReader stream)
        {
            if (TeamMode.ActiveTODef == null)
                return;

            UpdateBoards();

            var list = new List<ScoreBoard.Entry>(10);

            int teamCount = stream.ReadByte();
            for (int t = 0; t < teamCount; t++)
            {
                if (t >= UsedCount)
                    return;

                list.Clear();
                int teamScore = stream.ReadShort();
                int count = stream.ReadByte();
                if (count > list.Capacity)
                    list.Capacity = count;

                for (int i = 0; i < count; i++)
                {
                    list.Add(new ScoreBoard.Entry()
                    {
                        ID = stream.ReadByte(),
                        Score = stream.ReadShort(),
                        Kills = stream.ReadShort(),
                        Deaths = stream.ReadShort(),
                        Ping = stream.ReadShort()
                    });
                }

                var board = GetBoard(t);
                board.Fill(list);
                board.SetTitle(string.Format("{0} ({1}/50)", activeDef.Teams[t].Name, teamScore));
            }
        }

        TODef activeDef = null;
        void UpdateBoards()
        {
            if (activeDef == TeamMode.ActiveTODef)
                return;
            activeDef = TeamMode.ActiveTODef;
            
            SetUsedCount(activeDef.Teams.Count);
            for (int i = 0; i < UsedCount; i++)
            {
                TOTeamDef team = activeDef.Teams[i];
                GetBoard(i).SetTitle(team.Name, team.Color);
            }
        }
    }
}
