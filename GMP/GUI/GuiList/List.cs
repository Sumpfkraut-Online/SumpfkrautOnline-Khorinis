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
using GUC.Enumeration;

namespace GUC.GUI.GuiList
{
    public class List : Texture
    {
        String font = "";


        zCViewText[] textarr = null;
        List<ListRow> rowList = new List<ListRow>();

        protected zString emptyString = null;
        protected int activeRow = 0;



        protected int startRowIndex = 0;
        protected int endRowIndex = 0;

        public List(int id, int lines, String font, Vec2i pos, Vec2i size, String texture, Texture parent, GUIEvents evts)
            : base(id, texture, pos, size, parent, evts)
        {

            //Creation:
            Process process = Process.ThisProcess();
            setFont(font);


            emptyString = zString.Create(process, "");

            createTextViews(lines);
        }

        public void addRow(ListRow row)
        {
            rowList.Add(row);

            if (rowList.Count <= textarr.Length)
            {
                endRowIndex = rowList.Count;
            }

            updateTextBoxes();
        }

        public void clearRows()
        {
            rowList.Clear();
            endRowIndex = 0;
            ActiveID = 0;
            

            updateTextBoxes();
        }

        public int ActiveID {
            get { return this.activeRow; }
            set
            {
                if (value == this.activeRow)
                    return;

                if (value < 0)
                    value = rowList.Count - 1;
                else if (value >= rowList.Count)
                    value = 0;

                this.activeRow = value;

                updateTextBoxes();
                setInputActive(this.activeRow);
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
            if (rowList.Count == 0)
                return;

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
            this.getView().SetFont(str);
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

                textarr[i] = this.getView().CreateText(this.position.X, this.position.Y + i * (InputHooked.PixelToVirtualY(Process.ThisProcess(), this.getView().Font.GetFontY())), emptyString);
            }
        }

        public override void Destroy()
        {
            base.Destroy();
            emptyString.Dispose();

        }

        public override void setPosition(Vec2i position)
        {
            this.position.set(position);

            for (int i = 0; i < textarr.Length; i++)
            {
                textarr[i].PosX = this.position.X;
                textarr[i].PosY = this.position.Y + i * (InputHooked.PixelToVirtualY(Process.ThisProcess(), getView().Font.GetFontY() + 5));
            }
        }
    }
}
