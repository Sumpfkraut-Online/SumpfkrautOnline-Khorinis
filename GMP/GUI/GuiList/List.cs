using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Types;
using RakNet;
using Gothic.zClasses;
using WinApi;
using Gothic.mClasses;
using Gothic.zTypes;

namespace GUC.GUI.GuiList
{
    public class List : View
    {
        Texture parent;
        zCView thisView = null;
        String font = "";


        zCViewText[] textarr = null;
        List<ListRow> rowList = new List<ListRow>();

        protected zString emptyString = null;
        protected int activeRow = 0;

        public List(int id, int lines, String font, Vec2i pos, Texture parent)
            : base(id, pos)
        {

            this.parent = parent;
            this.font = font;

            //Creation:
            Process process = Process.ThisProcess();

            thisView = zCView.Create(Process.ThisProcess(), 0, 0, 0x2000, 0x2000);
            setFont(font);


            emptyString = zString.Create(process, "");

            createTextViews(lines);
        }

        public void addRow(ListRow row)
        {
            rowList.Add(row);

            updateTextBoxes();
        }

        public void clearRows()
        {
            rowList.Clear();
            ActiveID = 0;

            updateTextBoxes();
        }

        public int ActiveID {
            get { return this.activeRow; }
            set
            {
                if (value == this.activeRow)
                    return;
                this.activeRow = value;

                if (rowList.Count > textarr.Length)
                {
                    setInputActive(this.activeRow);
                    return;
                }
            }
        }

        protected void updateTextBoxes()
        {
            foreach (zCViewText tv in textarr)
            {
                tv.Text.Set("");
            }

            foreach (ListRow row in rowList)
            {
                row.setControl(null);
            }

            int start = 0;
            int end = (rowList.Count < textarr.Length) ? rowList.Count : textarr.Length;

            if (rowList.Count > textarr.Length && activeRow >= textarr.Length)
            {
                start = activeRow - (textarr.Length - 1);
                end = activeRow + 1;
            }

            for (int i = 0; start < end; start++, i++)
            {
                rowList[start].setControl(textarr[i]);
            }

            setInputActive(this.activeRow);
        }
        protected void setInputActive( int id )
        {
            foreach (ListRow row in rowList)
            {
                row.IsInputActive = false;
            }
            rowList[id].IsInputActive = true;
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

        public override void Destroy()
        {
            hide();

            emptyString.Dispose();


            thisView.Dispose();
        }

        public override void setPosition(Vec2i position)
        {
            this.position.set(position);

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
    }
}
