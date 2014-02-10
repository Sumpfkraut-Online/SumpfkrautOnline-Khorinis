using System;
using System.Collections.Generic;
using System.Text;
using Network;
using Gothic.zClasses;
using WinApi;
using Gothic.zTypes;

namespace GUC.GUI
{
    public class Text : View
    {
        zCView thisView = null;
        String text = null;
        String font = "";
        zCViewText textView = null;
        Texture parent = null;

        int colorR = 255;
        int colorG = 255;
        int colorB = 255;
        int colorA = 255;

        public Text(int id, String text, String font, int x, int y, Texture parent, int r, int g, int b, int a)
            : base(id)
        {
            this.x = x;
            this.y = y;
            this.text = text;
            this.parent = parent;
            this.font = font;

            //Creation:
            Process process = Process.ThisProcess();
            
            thisView = zCView.Create(Process.ThisProcess(), 0, 0, 0x2000, 0x2000);
            setFont(font);
            createText();



            setColor(r,g,b,a);
        }

        private void createText()
        {
            Process process = Process.ThisProcess();
            zString str = zString.Create(process, this.text);
            textView = thisView.CreateText(x, y, str);
            str.Dispose();

            textView.Timed = 0;
            textView.Timer = -1;

            
        }

        public void setColor(int r, int g, int b, int a)
        {
            colorR = r;
            colorG = g;
            colorB = b;
            colorA = a;

            textView.Color.R = (byte)this.colorR;
            textView.Color.G = (byte)this.colorG;
            textView.Color.B = (byte)this.colorB;
            textView.Color.A = (byte)this.colorA;
        }

        public void setText(String tex)
        {
            this.text = tex;

            Process process = Process.ThisProcess();
            zString str = zString.Create(process, tex);
            textView.Text = str;
            str.Dispose();
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
            thisView.SetFont(str);
            str.Dispose();

            if (textView != null)
            {
                textView.Timed = 1;
                textView.Timer = 0;
                createText();
                setColor(colorR, colorG, colorB, colorA);
            }
        }
        
        public override void setPosition(int x, int y)
        {
            this.x = x;
            this.y = y;

            textView.PosX = x;
            textView.PosY = y;
        }

        public override void hide()
        {
            if (!isShown)
                return;
            Process process = Process.ThisProcess();

            if (parent == null)
                zCView.GetStartscreen(process).RemoveItem(this.thisView);
            else
                parent.getView().RemoveItem(this.thisView);

            isShown = false;
            
        }
        public override void show()
        {
            if (isShown)
                return;
            Process process = Process.ThisProcess();

            if (parent == null)
                zCView.GetStartscreen(process).InsertItem(this.thisView, 0);
            else
                parent.getView().InsertItem(this.thisView, 0);

            isShown = true;
        }


        public override void Destroy()
        {
            if (textView != null)
            {
                textView.Timed = 1;
                textView.Timer = 0;
            }
            hide();

            thisView.Dispose();
        }

    }
}
