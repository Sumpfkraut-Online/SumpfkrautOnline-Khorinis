using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripts.Sumpfkraut.Menus.MainMenus;
using GUC.GUI;

namespace GUC.Scripts.Sumpfkraut.Menus
{
    class StatusMenu : GUCMainMenu
    {
        public static readonly StatusMenu Instance = new StatusMenu();

        GUCVisual stat;

        GUCVisualText guild; GUCVisualText level;
        GUCVisualText exp;
        GUCVisualText nextExp;
        GUCVisualText LP;
        GUCVisualText TP;

        GUCVisualText str;
        GUCVisualText dex;
        GUCVisualText wis;
        GUCVisualText hp;
        GUCVisualText mp;
        GUCVisualText ap;

        GUCVisualText protW;
        GUCVisualText protR;
        GUCVisualText protF;
        GUCVisualText protM;

        protected override void OnCreate()
        {
            Back.SetBackTexture("STATUS_BACK.TGA");

            stat = new GUCVisual(pos[0], pos[1], 640, 480); //for small fonts
            Back.AddChild(stat);

            // left side

            const int dist = GUCView.FontsizeDefault;

            GUCVisualText title = stat.CreateText("CHARAKTERPROFIL", 152, 59);
            title.Format = GUCVisualText.TextFormat.Center;
            const int cpyoffset = 84;
            const int cpx1 = 39; const int cpx2 = 180;
            guild = stat.CreateText("Gildenlos", cpx1, cpyoffset); stat.CreateText("Stufe", cpx2, cpyoffset); level = stat.CreateText("0", cpx2+55, cpyoffset);
            stat.CreateText("Erfahrung", cpx1, cpyoffset + dist); exp = stat.CreateText("0", cpx2, cpyoffset + dist);
            stat.CreateText("Nächste Stufe", cpx1, cpyoffset + dist * 2); nextExp = stat.CreateText("500", cpx2, cpyoffset + dist * 2);
            stat.CreateText("Lernpunkte", cpx1, cpyoffset + dist * 3); LP = stat.CreateText("0", cpx2, cpyoffset + dist * 3);
            stat.CreateText("Talentpunkte", cpx1, cpyoffset + dist * 4); TP = stat.CreateText("0", cpx2, cpyoffset + dist * 4);

            title = stat.CreateText("ATTRIBUTE", 152, 190);
            title.Format = GUCVisualText.TextFormat.Center;
            const int atyoffset = 215;
            stat.CreateText("Stärke", cpx1, atyoffset); str = stat.CreateText("10/10", cpx2, atyoffset);
            stat.CreateText("Geschick", cpx1, atyoffset + dist); dex = stat.CreateText("10/10", cpx2, atyoffset + dist);
            stat.CreateText("Weisheit", cpx1, atyoffset + dist * 2); wis = stat.CreateText("10/10", cpx2, atyoffset + dist * 2);
            const int trimdist = 4;
            stat.CreateText("Lebensenergie", cpx1, trimdist + atyoffset + dist * 3); hp = stat.CreateText("100/100", cpx2, trimdist + atyoffset + dist * 3);
            stat.CreateText("Mana", cpx1, trimdist + atyoffset + dist * 4); mp = stat.CreateText("10/10", cpx2, trimdist + atyoffset + dist * 4);
            stat.CreateText("Ausdauer", cpx1, trimdist + atyoffset + dist * 5); ap = stat.CreateText("100/100", cpx2, trimdist + atyoffset + dist * 5);

            title = stat.CreateText("RÜSTUNGSSCHUTZ", 152, 347);
            title.Format = GUCVisualText.TextFormat.Center;
            const int rsyoffset = 371; const int rsx2 = 230;
            stat.CreateText("vor Waffen", cpx1, rsyoffset); protW = stat.CreateText("0", rsx2, rsyoffset);
            stat.CreateText("vor Geschossen", cpx1, rsyoffset + dist); protR = stat.CreateText("0", rsx2, rsyoffset + dist);
            stat.CreateText("vor Drachenfeuer", cpx1, rsyoffset + dist * 2); protF = stat.CreateText("0", rsx2, rsyoffset + dist * 2);
            stat.CreateText("vor Magie", cpx1, rsyoffset + dist * 3); protM = stat.CreateText("0", rsx2, rsyoffset + dist * 3);

            // right side

            const int dist2 = GUCView.FontsizeMenu;

            const int tx = 450; const int ty1 = 80;
            title = stat.CreateText("TALENTE", tx, 59);
            title.Format = GUCVisualText.TextFormat.Center;      
            AddButton("Kampf", "", tx, ty1, null);
            AddButton("Magie", "", tx, ty1 + dist2, null);
            AddButton("Diebeskunst", "", tx, ty1 + 2*dist2, null);
 
            title = stat.CreateText("FERTIGKEITEN", tx, 191);
            title.Format = GUCVisualText.TextFormat.Center;
            const int ty2 = 217;
            AddButton("Handwerker", "", tx, ty2, null);
            AddButton("Waffenschmied", "", tx, ty2 + dist2, null);
            AddButton("Schmied", "", tx, ty2 + 2 * dist2, null);
            AddButton("Jäger & Sammler", "", tx, ty2 + 3 * dist2, null);
            AddButton("Verpfleger", "", tx, ty2 + 4 * dist2, null);
            AddButton("Alchemist", "", tx, ty2 + 5 * dist2, null);
            AddButton("Gelehrter", "", tx, ty2 + 6 * dist2, null);
        }
    }
}
