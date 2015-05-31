using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gothic.mClasses;
using Gothic.zClasses;
using Gothic.zTypes;
using WinApi;
using GUC.Types;
using WinApi.User.Enumeration;
using GUC.Sumpfkraut;

namespace GUC.Sumpfkraut.GUI
{
    class GUCMenuTextList : GUCInputReceiver, GUCMVisual
    {
        Process proc;

        zCView thisView;
        List<zCViewText> textLines;
        int width;
        int height;

        //zCView upArrow;
        zCView dnArrow;
        //bool showUpArrow;
        bool showDnArrow;

        List<Row> rows = new List<Row>();
        int startPos;

        public bool Enabled { get; set; }

        class Row
        {
            public String message;
            public ColorRGBA color = ColorRGBA.White;
        }

        public GUCMenuTextList(int x, int y, int w, int h, string tex)
        {
            proc = Process.ThisProcess();

            width = w;
            height = h;

            //Pixels to virtuals
            int[] arrowSize = InputHooked.PixelToVirtual(proc, new int[] { 10, 10 });
            int[] vpos = InputHooked.PixelToVirtual(proc, new int[] { x, y });
            int[] vsize = InputHooked.PixelToVirtual(proc, new int[] { w, h });

            //Create the view
            thisView = zCView.Create(proc, vpos[0], vpos[1], vpos[0] + vsize[0], vpos[1] + vsize[1]);
            if (tex != null && tex.Length > 0)
            {//Background texture
                using (zString z = zString.Create(proc, tex))
                    thisView.InsertBack(z);
            }

            //Create the text lines
            int dist = PixelToViewVirtualY(InputHandler.DefaultFontYPixels);
            int numLines = 0x2000 / dist;
            using (zString emptyString = zString.Create(proc, ""))
            {
                textLines = new List<zCViewText>();
                for (int i = 0; i < numLines; i++)
                {
                    textLines.Add(thisView.CreateText(0, i*dist, emptyString));
                }
            }

            /*upArrow = zCView.Create(proc, vpos[0], vpos[1], vpos[0] + arrowSize[0], vpos[1] + arrowSize[1]);
            using (zString z = zString.Create(proc, "O.tga"))
                upArrow.InsertBack(z);*/

            dnArrow = zCView.Create(proc, vpos[0], vpos[1] + vsize[1] - arrowSize[1], vpos[0] + arrowSize[0], vpos[1] + vsize[1]);
            using (zString z = zString.Create(proc, "U.tga"))
                dnArrow.InsertBack(z);

            //showUpArrow = false;
            showDnArrow = false;

            rows = new List<Row>();
            startPos = 0;
            Enabled = true;
        }

        public void Show()
        {
            zCView.GetStartscreen(proc).InsertItem(thisView, 1);
            //if (showUpArrow) zCView.GetStartscreen(proc).InsertItem(upArrow, 1);
            if (showDnArrow) zCView.GetStartscreen(proc).InsertItem(dnArrow, 1);
        }

        public void Hide()
        {
            zCView.GetStartscreen(proc).RemoveItem(thisView);
            //if (showUpArrow) zCView.GetStartscreen(proc).RemoveItem(upArrow);
            if (showDnArrow) zCView.GetStartscreen(proc).RemoveItem(dnArrow);
        }

        public void AddLine(string text, ColorRGBA color)
        {
            Row row;
            int num = 0;

            if (InputHandler.StringPixelWidth(text) > width)
            {
                string line = "";
                string nextLine;
                foreach(char c in text)
                {
                    nextLine = (line + c).TrimEnd(); //cut of all white spaces at the end
                    if (InputHandler.StringPixelWidth(nextLine) > width)
                    {
                        row = new Row();
                        row.color = color;

                        nextLine = line.TrimEnd();
                        int index = nextLine.LastIndexOf(' ');
                        if (nextLine.Length - index > 10) //cut big words
                        {
                            row.message = nextLine; 
                            line = "   ";
                        }
                        else //put this word into the next line
                        {
                            row.message = line.Remove(index);
                            line = "   " + line.Substring(index).TrimStart();
                        }
                        rows.Add(row);
                        num++;
                    }
                    line += c;
                }
                if (line.Length > 0)
                {
                    row = new Row();
                    row.message = line;
                    row.color = color;
                    rows.Add(row);
                    num++;
                }
            }
            else
            {
                row = new Row();
                row.message = text;
                row.color = color;
                rows.Add(row);
                num++;
            }


            if (!showDnArrow)
            {
                startPos += num;
            }
            UpdateOutputTexts();
        }

        private void UpdateOutputTexts()
        {
            int x = rows.Count - textLines.Count;
            if (startPos > x)
            {
                startPos = x;
            }
            if (startPos < 0) startPos = 0;

            for (int i = 0; i < textLines.Count; i++)
            {
                if (startPos + i >= rows.Count) return;
                textLines[i].Text.Set(rows[startPos + i].message);
                textLines[i].Color.R = rows[startPos + i].color.R;
                textLines[i].Color.G = rows[startPos + i].color.G;
                textLines[i].Color.B = rows[startPos + i].color.B;
                textLines[i].Color.A = rows[startPos + i].color.A;
            }

            /*if (startPos > 0 && rows.Count > textLines.Count)
            {
                if (!showUpArrow)
                {
                    zCView.GetStartscreen(proc).InsertItem(upArrow, 1);
                    textLines[0].PosX += arrowSize[0];
                    showUpArrow = true;
                }
            }
            else
            {
                if (showUpArrow)
                {
                    zCView.GetStartscreen(proc).RemoveItem(upArrow);
                    textLines[0].PosX -= arrowSize[0];
                    showUpArrow = false;
                }
            }*/

            if (startPos + textLines.Count < rows.Count)
            {
                if (!showDnArrow)
                {
                    zCView.GetStartscreen(proc).InsertItem(dnArrow, 1);
                    textLines[textLines.Count-1].PosX += PixelToViewVirtualX(10);
                    showDnArrow = true;
                }
            }
            else
            {
                if (showDnArrow)
                {
                    zCView.GetStartscreen(proc).RemoveItem(dnArrow);
                    textLines[textLines.Count - 1].PosX -= PixelToViewVirtualX(10);
                    showDnArrow = false;
                }
            }
        }

        public void Update(long ticks)
        {
        }

        public void KeyPressed(int key)
        {
            if (!Enabled)
                return;

            if (key == (int)VirtualKeys.Prior)
            {
                startPos--;
                UpdateOutputTexts();
            }
            else if (key == (int)VirtualKeys.Next)
            {
                startPos++;
                UpdateOutputTexts();
            }
        }

        private int PixelToViewVirtualX(int p)
        {
            int screenWidth = InputHooked.GetScreenSize(proc)[0];
            return (p * 0x2000 / screenWidth) * 0x2000 / (width * 0x2000 / screenWidth);
        }

        private int PixelToViewVirtualY(int p)
        {
            int screenHeight = InputHooked.GetScreenSize(proc)[1];
            return (p * 0x2000 / screenHeight) * 0x2000 / (height * 0x2000 / screenHeight);
        }
    }
}
