using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.zClasses;
using GUC.Types;
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

        ColorRGBA color = ColorRGBA.White;

        public Text(int id, String text, String font, Vec2i position, Texture parent, ColorRGBA color)
            : base(id, position)
        {
            this.text = text;
            this.parent = parent;
            this.font = font;

            //Creation:
            Process process = Process.ThisProcess();

            thisView = zCView.Create(Process.ThisProcess(), 0, 0, 0x2000, 0x2000);
            setFont(font);
            createText();



            setColor(color);
        }

        private void createText()
        {
            Process process = Process.ThisProcess();
            zString str = zString.Create(process, this.text);
            textView = thisView.CreateText(this.position.X, this.position.Y, str);
            str.Dispose();

            textView.Timed = 0;
            textView.Timer = -1;


        }

        public void setColor(ColorRGBA color)
        {
            this.color.set(color);

            textView.Color.R = (byte)this.color.R;
            textView.Color.G = (byte)this.color.G;
            textView.Color.B = (byte)this.color.B;
            textView.Color.A = (byte)this.color.A;
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
                setColor(this.color);
            }
        }

        public override void setPosition(Vec2i pos)
        {
            this.position.set(pos);

            textView.PosX = pos.X;
            textView.PosY = pos.Y;
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
