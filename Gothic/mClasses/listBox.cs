using System;
using System.Collections.Generic;
using System.Text;
using Gothic.zClasses;
using WinApi;
using Gothic.zTypes;

namespace Gothic.mClasses
{
    public class listBox
    {
        public class Row
        {
            String text = "";
            byte r, g, b, a;

            public Row(String te) : this(te, 0xff, 0xff, 0xff, 0xff)
            {

            }

            public Row(String t, byte r, byte g, byte b, byte a)
            {
                this.text = t;
                this.r = r;
                this.g = g;
                this.b = b;
                this.a = a;
            }

            public override string ToString()
            {
                return text;
            }
        }
        Process process;
        int size;
        int tempSize;
        public List<Row> rows = new List<Row>();
        List<zCViewText> textViews = new List<zCViewText>();

        int posX;
        int posY = 200;

        zString SpaceText = null;
        zCView view = null;

        public listBox(Process process, int size, zCView view)
        {
            this.process = process;
            this.view = view;
            

            tempSize = 100;
            SpaceText = zString.Create(process, "");
            SetSize(size);
        }

        public void delete()
        {
            if(SpaceText != null)
                SpaceText.Dispose();
            foreach (zCViewText vt in textViews)
            {
                vt.Timed = 1;
                vt.Timer = 0;
            }


        }

        public void addRow(String str)
        {
            rows.Add(new Row(str));
            if (rows.Count > tempSize)
                rows.RemoveAt(0);
            UpdateRows();
        }


        private List<String> getTexts()
        {
            List<String> strList = new List<String>();
            for (int i = rows.Count; i > rows.Count - size; i--)
            {
                if(i == 0)
                    break;
                strList.Insert(0, rows[i-1].ToString());
            }

                return strList;
        }

        public void UpdateRows()
        {
            List<String> strList  = getTexts();
            for (int i = 0; i < size; i++)
            {
                //textViews[i].Text.Dispose();
                if (strList.Count > i)
                    textViews[i].Text = zString.Create(process, strList[i]);
                else
                    textViews[i].Text = SpaceText;
                textViews[i].PosX = posX;
                textViews[i].PosY = posY + i * textViews[i].Font.GetFontY()*10;
            }
        }

        public int getBottom()
        {
            return posY + size * textViews[0].Font.GetFontY() * 10;
        }

        public void SetSize(int size)
        {
            if (size > this.size)
            {
                for (int i = this.size; i < size; i++)
                {
                    zCViewText vt = view.CreateText(0, 0, SpaceText);
                    vt.Timed = 0;
                    vt.Timer = -1;
                    vt.PosX = posX;
                    vt.PosY = posY + i * vt.Font.GetFontY();
                    textViews.Add(vt);
                }
            }
            if (size < this.size)
            {
                for (int i = size; i < this.size; i++)
                {
                    textViews[i].Timed = 1;
                    textViews[i].Timer = 1;
                }
            }

            this.size = size;
        }

        public int GetSize()
        {
            return size;
        }
    }
}
