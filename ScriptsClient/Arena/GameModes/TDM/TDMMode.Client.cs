using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.Types;
using GUC.Scripts.Sumpfkraut.Menus;

namespace GUC.Scripts.Arena.GameModes.TDM
{
    partial class TDMMode
    {
        public override Action OpenJoinMenu { get { return MenuTeamSelect.Instance.Open; } }
        public override ScoreBoardScreen ScoreBoard { get { return TDMScoreBoard.Instance; } }

        public static TDMScenario.TeamDef HeroTeam { get { return (IsActive && PlayerInfo.HeroInfo.TeamID >= TeamIdent.GMPlayer) ? ActiveMode.Scenario.Teams[(int)PlayerInfo.HeroInfo.TeamID] : null; } }

        public static void HeroTeamChange()
        {
            if (PlayerInfo.HeroInfo.ID >= 0)
                MenuClassSelect.Instance.Open();
        }

        protected override void Start(GameScenario scenario)
        {
            base.Start(scenario);
            RespawnNote.Toggle(true);
        }

        protected override void End()
        {
            RespawnNote.Toggle(false);
            base.End();
        }
        
        public static void ReadWinMessage(PacketReader stream)
        {
            var teams = ActiveMode.Scenario.Teams;

            int winnerCount = stream.ReadByte();
            List<int> winners = new List<int>(winnerCount);
            for (int i = 0; i < winnerCount; i++)
                winners.Add(stream.ReadByte());

            if (winnerCount == teams.Length)
            {
                PrintMessage("UNENTSCHIEDEN!", ColorRGBA.White);
                return;
            }
            else
            {
                if (PlayerInfo.HeroInfo.TeamID >= TeamIdent.GMPlayer)
                    PrintMessage(winners.Contains((int)PlayerInfo.HeroInfo.TeamID) ? "SIEG!" : "NIEDERLAGE!", ColorRGBA.White);

                foreach (int i in winners)
                    if (i < teams.Length)
                        PrintMessage(teams[i].Name + " hat gewonnen.", teams[i].Color);
            }

            //if (TDMMode.IsActive)
            //    TDMScoreBoard.Instance.Open();
        }

        static void PrintMessage(string text, ColorRGBA color)
        {
            //ChatMenu.Menu.AddMessage(ChatMode.Private, text);
            ScreenScrollText.AddText(text, GUI.GUCView.Fonts.Menu, color, 20 * TimeSpan.TicksPerSecond);
            Log.Logger.Log(text);
        }
    }
}
