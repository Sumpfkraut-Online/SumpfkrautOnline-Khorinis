using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;
using Gothic.zClasses;
using GUC.Types;
using Gothic.mClasses;
using Gothic.zTypes;

namespace GUC.GUI
{
    public class MessageBox : View
    {

        Texture parent;
        zCView thisView = null;
        String font = "";

        zCViewText[] textarr = null;
        zString emptyString = null;

        List<Row> rows = new List<Row>();

        class Row
        {
            public String message;
            public ColorRGBA color = ColorRGBA.White;
        }

        public MessageBox(int id, int lines, String font, Vec2i pos, Texture parent)
            : base(id, pos)
        {

            this.parent = parent;

            //Creation:
            Process process = Process.ThisProcess();

            thisView = zCView.Create(Process.ThisProcess(), 0, 0, 0x2000, 0x2000);
            setFont(font);


            emptyString = zString.Create(process, "");

            createTextViews(lines);
        }

        private void createTextViews(int lines)
        {
            if (textarr != null)
            {
                foreach (zCViewText vt in textarr)
                {
                    vt.Timed = 1;
                    vt.Timer = 0;
                }
            }
            textarr = new zCViewText[lines];
            for (int i = 0; i < lines; i++)
            {

                textarr[i] = thisView.CreateText(this.position.X, this.position.Y + i * (InputHooked.PixelToVirtualY(Process.ThisProcess(), thisView.Font.GetFontY())), emptyString);
            }
        }

        public void updateTextes()
        {
            int startPos = rows.Count - textarr.Length;
            if (startPos < 0)
                startPos = 0;

            for (int i = 0; i < textarr.Length; i++)
            {
                if (startPos + i >= rows.Count)
                    return;
                textarr[i].Text.Set(rows[startPos + i].message);
                textarr[i].Color.R = (byte)rows[startPos + i].color.R;
                textarr[i].Color.G = (byte)rows[startPos + i].color.G;
                textarr[i].Color.B = (byte)rows[startPos + i].color.B;
                textarr[i].Color.A = (byte)rows[startPos + i].color.A;
            }
        }

        public void addMessage(String message, ColorRGBA color)
        {
            Row row = new Row();
            row.message = message;
            row.color.set(color);

            this.rows.Add(row);
            this.updateTextes();
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
        }

        public override void setPosition(Vec2i pos)
        {
            this.position.set(pos);

            for (int i = 0; i < textarr.Length; i++)
            {
                textarr[i].PosX = this.position.X;
                textarr[i].PosY = this.position.Y + i * (InputHooked.PixelToVirtualY(Process.ThisProcess(), thisView.Font.GetFontY() + 5));
            }

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
            hide();

            emptyString.Dispose();


            thisView.Dispose();
        }

    }
}
