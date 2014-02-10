using System;
using System.Collections.Generic;
using System.Text;
using Network;
using Gothic.zClasses;
using WinApi;
using Gothic.zTypes;
using Gothic.mClasses;

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
            public int r, g, b, a;
        }

        public MessageBox(int id, int lines, String font, int x, int y, Texture parent)
            : base(id)
        {
            this.x = x;
            this.y = y;

            this.parent = parent;
            this.font = font;

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

                textarr[i] = thisView.CreateText(this.x, this.y + i * (InputHooked.PixelToVirtualY(Process.ThisProcess(), thisView.Font.GetFontY())), emptyString);
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
                textarr[i].Color.R = (byte)rows[startPos + i].r;
                textarr[i].Color.G = (byte)rows[startPos + i].g;
                textarr[i].Color.B = (byte)rows[startPos + i].b;
                textarr[i].Color.A = (byte)rows[startPos + i].a;
            }
        }

        public void addMessage(String message, int r, int g, int b, int a)
        {
            Row row = new Row();
            row.message = message;
            row.r = r;
            row.g = g;
            row.b = b;
            row.a = a;

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


        }
        
        public override void setPosition(int x, int y)
        {
            this.x = x;
            this.y = y;

            for (int i = 0; i < textarr.Length; i++){
                textarr[i].PosX = x;
                textarr[i].PosY = this.y + i * (InputHooked.PixelToVirtualY(Process.ThisProcess(), thisView.Font.GetFontY() + 5));
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
