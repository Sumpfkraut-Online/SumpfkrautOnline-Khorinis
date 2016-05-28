using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Client.Scripts.Sumpfkraut.GUI.MainMenu;
using GUC.Client.Scripts.Sumpfkraut.Menus.MainMenus;
using GUC.Scripts.TFFA;
using GUC.Network;

namespace GUC.Client.Scripts.TFFA
{
    class ClassMenu : GUCMainMenu
    {
        public static readonly ClassMenu Menu = new ClassMenu();

        MainMenuButton bLight, bHeavy;

        public void UpdateCounts()
        {
            if (!this.isOpen)
                return;

            SetTeam(TFFAClient.Info.Team);

            int tLight = 0, tHeavy = 0;
            foreach (ClientInfo ci in ClientInfo.ClientInfos.Values)
            {
                if (ci.Team == TFFAClient.Info.Team)
                {
                    if (ci.Class == PlayerClass.Light) tLight++;
                    else if (ci.Class == PlayerClass.Heavy) tHeavy++;
                }
            }

            if (tLight == 0 && tHeavy == 0)
                return;

            bLight.Text += ": " + tLight;
            bHeavy.Text += ": " + tHeavy;
        }

        public void SetTeam(Team team)
        {
            if (team == Team.AL)
            {
                bLight.Text = "Schatten";
                bHeavy.Text = "Gardist";
            }
            else
            {
                bLight.Text = "Bandit";
                bHeavy.Text = "Söldner";
            }
        }

        protected override void OnCreate()
        {
            Back.CreateTextCenterX("Klasse wählen", 70);
            OnEscape = TeamMenu.Menu.Open;

            const int offset = 200;
            const int dist = 38;
            bLight = AddButton("", "", offset + dist * 0, () => SelectClass(PlayerClass.Light));
            bHeavy = AddButton("", "", offset + dist * 1, () => SelectClass(PlayerClass.Heavy));
        }

        public override void Open()
        {
            if (this.isOpen)
                return;

            if (TFFAClient.Info.Team == Team.Spec)
                return;

            base.Open();
            UpdateCounts();
        }

        public override void Close()
        {
            if (!this.isOpen)
                return;

            base.Close();
        }

        void SelectClass(PlayerClass c)
        {
            if (TFFAClient.Info.Class == c)
                return;

            PacketWriter stream = GameClient.Client.GetMenuMsgStream();
            stream.Write((byte)MenuMsgID.ClientClass);
            stream.Write((byte)c);
            GameClient.Client.SendMenuMsg(stream, PktPriority.LOW_PRIORITY, PktReliability.RELIABLE);
            Close();
        }
    }
}
