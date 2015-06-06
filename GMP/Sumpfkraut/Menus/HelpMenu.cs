using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Sumpfkraut.GUI;

namespace GUC.Sumpfkraut.Menus
{
    partial class MainMenus
    {
        class HelpMenu : AbstractMenu
        {
            //Chatbefehle
            //RP-Guide
            //Tasten
            //Zurück

            protected override void CreateMenu()
            {
                thisMenu = new GUCMainMenu(0);
                thisMenu.AddText("Hilfe-Menü", 80);
                thisMenu.AddMenuButton("Chatbefehle", "Eine Liste aller nutzbaren Chatbefehle.", null, 180);
                thisMenu.AddMenuButton("RP-Guide", "Eine kurze Einführung ins Rollenspiel.", RPGuide.Open, 220);
                //int offsetx = 200;
                int offsety = 280;
                int dist = 24;
                thisMenu.AddSmallText("ENTER - Chat öffnen", offsety + dist * 0);
                thisMenu.AddSmallText("F2 - Wechsel zw. OOC-/RP-Chat",offsety + dist * 1);
                thisMenu.AddSmallText("T - Handelsanfrage", offsety + dist * 2);
                thisMenu.AddSmallText("X - Animationsmenü", offsety + dist * 3);

                thisMenu.AddMenuButton("Zurück", "Zurück zum Hauptmenü.", Main.Open, 400);
                thisMenu.OnEscape = Main.Open;
            }
        }
    }
}
