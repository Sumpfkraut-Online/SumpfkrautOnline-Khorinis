using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripts.Sumpfkraut.Menus.MainMenus;
using GUC.Scripts.Sumpfkraut.GUI.MainMenu;
using GUC.Network;

namespace GUC.Scripts.TFFA
{
    class TeamMenu : GUCMainMenu
    {
        public static readonly TeamMenu Menu = new TeamMenu();

        public void UpdateCounts()
        {
            if (!this.isOpen)
                return;

            bTeamAL.Text = "Team Gomez";
            bTeamNL.Text = "Tetriandoch";
            bTeamAL.Enabled = true;
            bTeamNL.Enabled = true;

            int tAL = 0, tNL = 0, spec = 0;

            foreach (ClientInfo ci in ClientInfo.ClientInfos.Values)
            {
                if (ci.Team == Team.AL) tAL++;
                else if (ci.Team == Team.NL) tNL++;
                else spec++;
            }

            bSpec.Text = "Zuschauer: " + spec;

            if (tAL == 0 && tNL == 0)
                return;

            int al = tAL; int nl = tNL;
            if (TFFAClient.Info.Team == Team.AL)
            {
                al--;
            }
            else if (TFFAClient.Info.Team == Team.NL)
            {
                nl--;
            }

            bTeamAL.Text += ": " + tAL;
            bTeamAL.Enabled = al <= nl;

            bTeamNL.Text += ": " + tNL;
            bTeamNL.Enabled = al >= nl;
        }

        MainMenuButton bSpec, bTeamAL, bTeamNL;

        protected override void OnCreate()
        {
            Back.CreateTextCenterX("Team Wählen", 70);

            const int offset = 200;
            const int dist = 38;
            bTeamAL = AddButton("Team Gomez", "", offset + dist * 0, () => SelectTeam(Team.AL));
            bTeamNL = AddButton("Tetriandoch", "", offset + dist * 1, () => SelectTeam(Team.NL));
            bSpec = AddButton("Zuschauer", "", offset + dist * 2, () => SelectTeam(Team.Spec));

            OnEscape = MainMenu.Menu.Open;
        }

        public override void Open()
        {
            if (this.isOpen)
                return;

            base.Open();
            UpdateCounts();
            if (TFFAClient.Info != null)
            {
                MainMenuButton set;
                if (!bTeamAL.Enabled)
                {
                    set = bTeamNL;
                }
                else
                {
                    set = bTeamAL;
                }
                SetCursor(set);
            }
        }

        public override void Close()
        {
            if (!this.isOpen)
                return;

            base.Close();
        }

        void SelectTeam(Team team)
        {
            Close();
            if (TFFAClient.Info.Team != team)
            {
                PacketWriter stream = GameClient.Client.GetMenuMsgStream();
                stream.Write((byte)MenuMsgID.ClientTeam);
                stream.Write((byte)team);
                GameClient.Client.SendMenuMsg(stream, PktPriority.LOW_PRIORITY, PktReliability.RELIABLE);
            }
            else
            {
                ClassMenu.Menu.Open();
            }
        }
    }
}
