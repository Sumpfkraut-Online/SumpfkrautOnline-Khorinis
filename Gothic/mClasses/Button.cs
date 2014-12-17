using System;
using System.Collections.Generic;
using System.Text;
using WinApi;
using WinApi.User.Enumeration;
using Gothic.zClasses;
using Gothic.zTypes;
using System.Windows.Forms;

namespace Gothic.mClasses
{
    public class Button : InputReceiver
    {
        Process process;
        zCView pView;
        bool isShown;
        String text;

        public int posX;
        public int posY;

        public int sizeX = 1700;
        public int sizeY = 600;

        String texture = "buttons_03.tga";

        public Button(Process process, String text)
        {
            this.process = process;
            this.text = text;

            
        }

        /// <summary>
        /// Vor Show ausführen!
        /// </summary>
        /// <param name="texture">Standard buttons_03.tga</param>
        public void setTexture(String texture)
        {
            this.texture = texture;
        }

        public void setSize(int x, int y)
        {
            sizeX = x;
            sizeY = y;
        }

        public void setPos(int x, int y)
        {
            posX = x;
            posY = y;
            if(pView != null)
                pView.SetPos(posX, posY);
        }

        public void Show()
        {
            if (isShown)
                return;
            if (pView == null)
            {
                pView = zCView.Create(process, 0, 0, sizeX, sizeY);

                zString texStr = zString.Create(process, texture);
                zString fontStr = zString.Create(process, "Font_Old_20_White_Hi.tga");
                pView.InsertBack(texStr);
                pView.SetFont(fontStr);

                texStr.Dispose();
                fontStr.Dispose();

                pView.PrintTimedCXY(zString.Create(process, text), -1, zColor.Create(process, 255, 255, 255, 255));
                pView.SetPos(posX, posY);
            }


            zCView.GetStartscreen(process).InsertItem(pView, 0);
            InputHooked.receivers.Add(this);
            isShown = true;
        }

        public void Hide()
        {
            if (!isShown)
                return;
            zCView.GetStartscreen(process).RemoveItem(pView);
            pView.Dispose();
            pView = null;

            isShown = false;
            InputHooked.receivers.Remove(this);
        }

        public void KeyReleased(int key)
        {

        }
        public void wheelChanged(int steps) { }

        public event EventHandler<EventArgs> ButtonPressed;
        public void KeyPressed(int key)
        {
            if (key != (int)VirtualKeys.LeftButton)
                return;

            if(posX < Cursor.CursorX() && posX+sizeX > Cursor.CursorX()
                && posY < Cursor.CursorY() && posY + sizeY > Cursor.CursorY())
            {
                if (ButtonPressed != null)
                {
                    ButtonPressed(this, new EventArgs());
                }
            }
        }
    }
}
