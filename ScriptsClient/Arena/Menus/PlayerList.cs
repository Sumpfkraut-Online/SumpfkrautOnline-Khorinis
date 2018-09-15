using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.GUI;

namespace GUC.Scripts.Arena.Menus
{
    static class PlayerList
    {
        const int Width = 400;
        const int Height = 600;
        const string BackTex = "MENU_INGAME.TGA";

        static GUCVisual vis;
        static GUCVisual listVis;

        static PlayerList()
        {
            var screen = GUCView.GetScreenSize();

            int x = (screen.X - Width) / 2;
            int y = (screen.Y - Height) / 2;

            vis = new GUCVisual(x, y - GUCView.FontsizeMenu, Width, Height + GUCView.FontsizeMenu);
            vis.Font = GUCView.Fonts.Menu;
            vis.CreateTextCenterX("Spielerliste", 0);

            listVis = new GUCVisual(x, y, Width, Height);
            vis.AddChild(listVis);

            listVis.SetBackTexture(BackTex);
            for (int offset = 20; offset < Height - GUCView.FontsizeDefault - 5; offset += GUCView.FontsizeDefault + 1)
                listVis.CreateText("", 17, offset);
        }

        public static void TogglePlayerList()
        {
            if (vis.Shown)
            {
                vis.Hide();
                PlayerInfo.OnPlayerListChange -= UpdateList;
            }
            else
            {
                vis.Show();
                UpdateList();
                PlayerInfo.OnPlayerListChange += UpdateList;
            }
        }

        static void UpdateList()
        {
            int index = 0;
            foreach(var info in PlayerInfo.GetInfos())
            {
                if (index == listVis.Texts.Count)
                    return;

                listVis.Texts[index].Font = info.ID == PlayerInfo.HeroInfo.ID ? GUCView.Fonts.Default_Hi : GUCView.Fonts.Default;
                listVis.Texts[index].Text = string.Format("({0}) {1}", info.ID, info.Name);
                index++;
            }

            for (; index < listVis.Texts.Count; index++)
                listVis.Texts[index].Text = string.Empty;
        }
    }
}
