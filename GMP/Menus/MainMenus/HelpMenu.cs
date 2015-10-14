using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Client.GUI;

namespace GUC.Client.Menus.MainMenus
{
    class HelpMenu : GUCMainMenu
    {
        GUCVisual keyHelp;

        protected override void OnCreate()
        {
            Back.CreateTextCenterX("Kurzhilfe", 100);
            
            AddButton("Chatbefehle", "Eine Liste aller nutzbaren Chatbefehle.", 180, GUCMenus.HelpChat.Open);
            AddButton("RP-Guide", "Eine kurze Einführung ins Rollenspiel.", 220, GUCMenus.HelpRP.Open);

            const int offsetY = 280;
            const int dist = 24;
            keyHelp = (GUCVisual)Back.AddChild(new GUCVisual());
            keyHelp.CreateTextCenterX("ENTER - Chat öffnen", pos[1] + offsetY + dist * 0);
            keyHelp.CreateTextCenterX("F2 - Wechsel zw. OOC-/RP-Chat", pos[1] + offsetY + dist * 1);
            keyHelp.CreateTextCenterX("T - Handelsanfrage", pos[1] + offsetY + dist * 2);
            keyHelp.CreateTextCenterX("X - Animationsmenü", pos[1] + offsetY + dist * 3);

            AddButton("Zurück", "Zurück zum Hauptmenü.",400, GUCMenus.Main.Open);
            OnEscape = GUCMenus.Main.Open;
        }
    }
}
