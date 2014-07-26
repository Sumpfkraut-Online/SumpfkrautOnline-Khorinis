using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Types;
using Gothic.mClasses;
using WinApi.User.Enumeration;
using Gothic.zTypes;
using WinApi;
using GUC.Enumeration;
using RakNet;
using GUC.WorldObjects.Character;
using Gothic.zClasses;

namespace GUC.GUI
{
    public class Button : Texture
    {
        String text = "";
        String font = "";

        zCViewText textView = null;
        ColorRGBA color = ColorRGBA.White;


        public Button(int id, String text, String tex, String font, ColorRGBA color, Vec2i position, Vec2i size, Texture parent, GUIEvents evts)
            : base(id, tex, position, size, parent, evts)
        {
            setFont(font);
            setText(text);
            setColor(color);
        }

        public void setText(String text)
        {
            this.text = text;

            if (this.textView != null)
            {
                textView.Timed = 1;
                textView.Timer = 0;
            }
            createText();
        }

        public void setColor(ColorRGBA color)
        {
            this.color.set(color);

            textView.Color.R = (byte)this.color.R;
            textView.Color.G = (byte)this.color.G;
            textView.Color.B = (byte)this.color.B;
            textView.Color.A = (byte)this.color.A;
        }

        public void setFont(String font)
        {
            if (font == null)
                return;
            String oldfont = this.font;
            this.font = font;

            if (oldfont.Trim().ToUpper() == font.Trim().ToUpper())
                return;




            Process process = Process.ThisProcess();
            zString str = zString.Create(process, this.font);
            view.SetFont(str);
            str.Dispose();

            
            setText(text);
            setColor(this.color);
            
        }

        private void createText()
        {
            Process process = Process.ThisProcess();
            zString str = zString.Create(process, this.text);
            zColor c = zColor.Create(process, color.R, color.G, color.B, color.A);
            textView = view.PrintTimedCXY_TV(str, -1, c);
            str.Dispose();
            c.Dispose();

            textView.Timed = 0;
            textView.Timer = -1;
        }





        
    }
}
