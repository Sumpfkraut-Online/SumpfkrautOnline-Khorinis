using System;
using System.Collections.Generic;
using System.Text;
using WinApi;
using Gothic.mClasses.Elements;
using Gothic.zClasses;
using Gothic.zTypes;
using WinApi.User.Enumeration;

namespace Gothic.mClasses
{
    public class ManagedListBox : InputReceiver
    {
        public Process process;
        List<mEItem> mData = new List<mEItem>();
        List<zCViewText> viewTextList = new List<zCViewText>();
        //List<zCView> ViewList = new List<zCView>();
        zCView viewBG;
        bool mEnabled;
        bool mInputEnabled;

        int activeID;

        int mPosX = 2596;
        int mPosY = 2096;
        int mWidth = 3000;
        int mHeight = 4000;
        int size;
        
        public ManagedListBox(Process process)
        {
            this.process = process;
        }

        public bool isEnabled()
        {
            return mEnabled;
        }

        public void Enable()
        {
            mEnabled = true;

            zString tex = zString.Create(process, "MENU_INGAME");
            viewBG = zCView.Create(process, mPosX, mPosY, mPosX+mWidth, mPosY+mHeight);
            viewBG.InsertBack(tex);
            tex.Dispose();

            zCView.GetStartscreen(process).InsertItem(viewBG, 0);

            



            SetSize(19);

            EnableInput();
            InputHooked.receivers.Add(this);
        }

        public void EnableInput()
        {
            mInputEnabled = true;
        }

        public void Disable()
        {
            DisableInput();
            mEnabled = false;

            foreach (zCViewText vtext in viewTextList)
            {
                vtext.Timer = 0;
                vtext.Timed = 1;
            }
            //Aufräumen
            zCView.GetStartscreen(process).RemoveItem(viewBG);
            viewBG.Dispose();

            InputHooked.receivers.Remove(this);
        }

        public void DisableInput()
        {
            mInputEnabled = false;
        }


        public List<mEItem> Data
        {
            get { return mData; }
        }

        public int ActiveID
        {
            get { return activeID; }
            set
            {
                if (value < 0)
                    activeID = mData.Count - 1;
                else if (value >= mData.Count)
                    activeID = 0;
                else
                    activeID = value;


                UpdateListElements();
            }
        }

        public void Update()
        {
            if (!mEnabled)
                return;

        }

        public void UpdateListElements()
        {
            int pos = 0;

            foreach (mEItem itm in mData)
                itm.TextView = null;
            foreach (zCViewText textView in viewTextList)
                textView.Text.Set("");

            int start = activeID - size+1;
            if (start < 0)
                start = 0;
            int end = start + size;
            if (end > mData.Count)
                end = mData.Count;

            //zString empty = zString.Create(process, "");
            //zString active =  zString.Create(process, "MENU_CHOICE_BACK");
            for (int i = start; i < end; i++, pos++)
            {
                if (pos > size)
                    break;

                //if (i == activeID)
                //    this.ViewList[pos].InsertBack(active);
                //else
                //    this.ViewList[pos].InsertBack(empty);
                if (i == activeID)
                {
                    viewTextList[pos].Color.R = 255;
                    viewTextList[pos].Color.G = 255;
                    viewTextList[pos].Color.B = 255;
                }
                else
                {
                    viewTextList[pos].Color.R = 212;
                    viewTextList[pos].Color.G = 212;
                    viewTextList[pos].Color.B = 212;
                }
                mData[i].TextView = this.viewTextList[pos];

            }
        }



        public void KeyReleased(int key)
        {
        }
        public void wheelChanged(int steps) 
        {
            if (!mInputEnabled)
                return;

            if (steps > 0)
                Data[activeID].InputUpdate(this, (VirtualKeys)VirtualKeys.Up);
            else
                Data[activeID].InputUpdate(this, (VirtualKeys)VirtualKeys.Down);
        }

        public void KeyPressed(int key)
        {
            if (!mInputEnabled)
                return;
            if (WinApi.User.Window.GetWindowThreadProcessId(WinApi.User.Window.GetForegroundWindow()) != process.ProcessID
             || zCConsole.Console(process).IsVisible() == 1)
                return;


            if (Data.Count < 0)
                return;

            Data[activeID].InputUpdate(this, (VirtualKeys)key);
        }









        public void SetSize(int size)
        {
            if (size > this.size)
            {
                for (int i = this.size; i < size; i++)
                {
                    zString str = zString.Create(process, "");
                    int height =  (int)(mHeight * 0.1f);
                    int width = (int)(mWidth * 0.05f);

                    zCViewText vt = viewBG.CreateText(0, 0, str);
                    vt.Timed = 0;
                    vt.Timer = -1;
                    vt.PosX = width;
                    vt.PosY = height + i * height;
                    
                    viewTextList.Add(vt);

                    str.Dispose();
                }
            }
            else if (size < this.size)
            {
                zCViewText[] txview = viewTextList.ToArray();
                for (int i = size; i < this.size; i++)
                {
                    txview[i].Timed = 1;
                    txview[i].Timer = 0;
                    viewTextList.Remove(txview[i]);
                }
            }

            this.size = size;
        }




    }
}
