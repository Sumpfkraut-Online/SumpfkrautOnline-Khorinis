using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.zClasses;
using Gothic.zTypes;

namespace GUC.Sumpfkraut.GUI
{
    public enum GUCFonts
    {
        Default,
        Default_Hi,
        Menu,
        Menu_Hi
    }

    class GUCVisualText
    {
        zCView parent;
        int[] parentSize;
        zCViewText viewText;
        bool single;
        bool shown = true;
        string text;

        public GUCVisualText(string text, int x, int y) //call this if you want only one text
        {
            parentSize = new int[] { 0x2000, 0x2000 };
            parent = zCView.Create(Program.process, 0, 0, 0x2000, 0x2000);
            int[] vpos = InputHandler.PixelToVirtual(x, y);
            using (zString z = zString.Create(Program.process, text))
                viewText = parent.CreateText(vpos[0], vpos[1], z);
            
            single = true;
            shown = true;
        }

        public GUCVisualText(string text, int x, int y, zCView parent, int[] psize)
        {
            this.parent = parent;
            parentSize = psize;
            int[] vpos = InputHandler.PixelToVirtual(x, y);
            vpos = new int[] { vpos[0] * 0x2000 / parentSize[0], vpos[1] * 0x2000 / parentSize[1] };
            using (zString z = zString.Create(Program.process, text))
                viewText = parent.CreateText(vpos[0], vpos[1], z);

            single = false;
            shown = true;
        }

        public void SetText(string text)
        {
            this.text = text;
            viewText.Text.Set(text);
        }

        public void Show() {
            if (!shown) {
                viewText.Text.Set(text);
                shown = true;
            }
        }

        public void Hide() {
            if (shown) {
                viewText.Text.Clear();
                shown = false;
            }
        }
    }

    class GUCVisual
    {
        static Dictionary<GUCFonts, string> fonts = new Dictionary<GUCFonts, string>
        {
            { GUCFonts.Default, "Font_Old_10_White.tga"},
            { GUCFonts.Default_Hi, "Font_Old_10_White_Hi.tga"},
            { GUCFonts.Menu, "Font_Old_20_White.tga"},
            { GUCFonts.Menu_Hi, "Font_Old_20_White_Hi.tga"}
        };

        zCView thisView;
        int[] vpos;
        int[] vsize;
        bool shown;

        public GUCVisual()
        {
            vpos = new int[] { 0, 0 };
            vsize = new int[] { 0x2000, 0x2000 };
            thisView = zCView.Create(Program.process, 0, 0, 0x2000, 0x2000);
            shown = false;
        }

        public GUCVisual(int x, int y, int w, int h)
        {
            vpos = InputHandler.PixelToVirtual(x,y);
            vsize = InputHandler.PixelToVirtual(w,h);
            thisView = zCView.Create(Program.process,vpos[0],vpos[1],vpos[0]+vsize[0],vpos[1]+vsize[1]);
            shown = false;
        }

        public void Show() {
            if (!shown) {
                zCView.GetStartscreen(Program.process).InsertItem(thisView, 0);
                shown = true;
            }
        }

        public void Hide() {
            if (shown) {
                zCView.GetStartscreen(Program.process).RemoveItem(thisView);
                shown = false;
            }
        } 

        public void SetFont(GUCFonts font)
        {
            thisView.SetFont(fonts[font]);
        }
    }
}
