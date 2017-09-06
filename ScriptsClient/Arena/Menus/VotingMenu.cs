using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GUC.GUI;
using GUC.Types;
using GUC.Scripts.Sumpfkraut.Menus;
using WinApi.User.Enumeration;

/*
 * Voting
 * Zeigt einem Spieler eins odere mehrere Votings
 * Spieler trifft eine Auswahl zu einer gegebenen Frage
 * Auswahl wird getroffen via Chat oder numpad
 * Welche Nachrichten müssen ausgetauscht werden?
 * Ein Vote will gestartet werden -> /vote tdmburg man startet ein vote mit einem namen gesendet wird -> voteanforderung byte 1 = vote starten
 * und votebyte2 = votetype
 * Jemand möchte seine Stimme abgenen /vote -> voteanforderung= stimme abgeben , votebyte 2 = auswahl
 *
 * Der Server zählt die Zeit herunter, wertet nach Ablauf des votings die stimmen aus und führt die action aus
 * Der Server muss also wissen welche action es zu einem voting gibt
 * 
 */
namespace GUC.Scripts.Arena.Menus
{
    class VotingDef
    {

        public VotingDef(string name, )
        {
            // add our new def as possible option
            VotingMenu.VotingDict.Add(name, this);
        }

    }

    class VotingMenu : GUCMenu
    {
        public static readonly VotingMenu voteMenu = new VotingMenu();
        public static Dictionary<string, VotingDef> VotingDict = new Dictionary<string, VotingDef>();

        GUCVisual background;
        int[] screenSize;
        int originX, originY, height, width;

        public VotingMenu()
        {
            screenSize = GUCView.GetScreenSize();
            originX = 50;
            originY = screenSize[1] / 5 + 50;
            height = 500;
            width = 300;

            background = new GUCVisual(originX, originY, width, height);
            background.SetBackTexture("Dlg_Conversation.tga");
        }

        public override void Open()
        {
            background.Show();
            base.Open();
        }

        public override void Close()
        {
            background.Hide();
            base.Close();
        }

        protected override void KeyDown(VirtualKeys key)
        {
            switch(key)
            {
            }
            base.KeyDown(key);
        }

        public void StartVote()
        {

        }

    }
}
