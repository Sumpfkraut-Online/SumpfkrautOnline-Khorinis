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


namespace GUC.Sumpfkraut.Ingame.GUI
{
    class GUCMenuTextBox : GUCMInputReceiver, GUCMVisual
    {
        Process proc;

        public int charLimit; //maximum of writable chars
        public bool allowWhiteSpaces;

        zCView thisView;
        zCViewText viewText;

        Vec2i pos;
        int width;
        int height;

        zCView cursorView;
        zCViewText cursorText;
        long cursorTime;
        int cursorPos;
        int startPos;

        bool fixedBorders;
        zCView leftArrow;
        bool showLeftArrow;
        zCView rightArrow;
        bool showRightArrow;

        int sentCursorPos;
        List<string> sentTexts;

        StringBuilder text;
        public string input
        {
            get { return text.ToString(); }
            set { }
        }

        //chat utils
        public delegate void InputChangedHandler();
        public event InputChangedHandler InputChangedEvent;
        public int numHideChars;

        public GUCMenuTextBox(int x, int y, int w, bool fixedBorders)
        {
            proc = Process.ThisProcess();

            //Settings
            charLimit = 512;
            allowWhiteSpaces = true;
            this.fixedBorders = fixedBorders;

            //Position & size
            pos = new Vec2i(x, y);
            width = w;
            height = IngameInput.DefaultFontYPixels;

            //Pixels to virtuals
            int[] vpos = InputHooked.PixelToVirtual(proc, new int[] { pos.X, pos.Y });
            int[] vsize = InputHooked.PixelToVirtual(proc, new int[] { width, height });

            //Input line & cursor
            thisView = zCView.Create(proc, vpos[0], vpos[1], vpos[0] + vsize[0], vpos[1] + vsize[1]);
            cursorView = zCView.Create(proc, vpos[0], vpos[1], vpos[0] + vsize[0], vpos[1] + vsize[1]);
            using (zString emptyString = zString.Create(proc, ""))
            {
                viewText = thisView.CreateText(0, 0, emptyString);
                cursorText = cursorView.CreateText(0, 0, emptyString);
            }

            //Left arrow
            int[] arrowSize = InputHooked.PixelToVirtual(proc, new int[] { 10, 10 });
            int arrowY = vpos[1] + (vsize[1] - arrowSize[1]) / 2;
            leftArrow = zCView.Create(proc, vpos[0], arrowY, vpos[0] + arrowSize[0], arrowY + arrowSize[1]);
            using (zString z = zString.Create(proc, "L.tga"))
                leftArrow.InsertBack(z);

            //Right arrow
            int arrowX = vpos[0] + vsize[0] - arrowSize[0];
            rightArrow = zCView.Create(proc, arrowX, arrowY, arrowX + arrowSize[0], arrowY + arrowSize[1]);
            using (zString z = zString.Create(proc, "R.tga"))
                rightArrow.InsertBack(z);

            //Reset everything
            showLeftArrow = false;
            showRightArrow = false;
            cursorPos = 0;

            sentCursorPos = -1;
            sentTexts = new List<string>();
            text = new StringBuilder();

            numHideChars = 0;
        }

        public void Show()
        {
            zCView.GetStartscreen(proc).InsertItem(thisView, 1);
            zCView.GetStartscreen(proc).InsertItem(cursorView, 1);
            if (showLeftArrow) zCView.GetStartscreen(proc).InsertItem(leftArrow, 1);
            if (showRightArrow) zCView.GetStartscreen(proc).InsertItem(rightArrow, 1);
        }

        public void Hide()
        {
            zCView.GetStartscreen(proc).RemoveItem(thisView);
            zCView.GetStartscreen(proc).RemoveItem(cursorView);
            if (showLeftArrow) zCView.GetStartscreen(proc).RemoveItem(leftArrow);
            if (showRightArrow) zCView.GetStartscreen(proc).RemoveItem(rightArrow);
        }

        public void ResetInput(bool saveInput)
        {
            if (saveInput)
            {
                if (sentTexts.Contains(input))
                    sentTexts.Remove(input);
                sentTexts.Insert(0, input);
            }
            text.Clear();
            cursorPos = 0;
            sentCursorPos = -1;
            UpdateViewText();
        }
        
        public void KeyPressed(int key)
        {
            if (key == (int)VirtualKeys.Back) //Backspace, delete char behind the cursor
            {
                if (cursorPos > 0)
                {
                    text.Remove(cursorPos - 1, 1);
                    cursorPos--;
                }
            }
            else if (key == (int)VirtualKeys.Delete) //delete char in front of cursor
            {
                if (input.Length > cursorPos)
                {
                    text.Remove(cursorPos, 1);
                }
            }
            else if (key == (int)VirtualKeys.Left)
            {
                if (cursorPos > 0) cursorPos--;
            }
            else if (key == (int)VirtualKeys.Right)
            {
                if (input.Length > cursorPos) cursorPos++;
            }
            else if (key == (int)VirtualKeys.Up) //recall sent messages
            {
                if (sentCursorPos < sentTexts.Count - 1)
                {
                    sentCursorPos++;
                    cursorPos = sentTexts[sentCursorPos].Length;
                    SetInput(sentTexts[sentCursorPos]);
                }
            }
            else if (key == (int)VirtualKeys.Down)
            {
                if (sentCursorPos > -1)
                {
                    sentCursorPos--;
                    if (sentCursorPos == -1)
                    {
                        cursorPos = 0;
                        text.Clear();
                    }
                    else
                    {
                        cursorPos = sentTexts[sentCursorPos].Length;
                        SetInput(sentTexts[sentCursorPos]);
                    }
                }
            }
            else
            {
                if (text.Length >= charLimit) //char limit hit
                    return;

                string str = GetCharFromKey((VirtualKeys)key);
                if (str == null || str.Length == 0)
                    return;

                char c = str[0];
                if (!IngameInput.AllChars.ContainsKey(c)) //check if typed char is supported by gothic's font
                    return;

                if (allowWhiteSpaces == false && c == ' ')
                    return; //we don't want spaces

                text.Insert(cursorPos, c);
                if (fixedBorders && IngameInput.StringPixelWidth(input) > width) //check if fixed borders are reached
                {
                    text.Remove(cursorPos, 1);
                    return;
                }
                cursorPos++;
            }
            if (InputChangedEvent != null && key != (int)VirtualKeys.Up && key != (int)VirtualKeys.Down)
            {
                InputChangedEvent();
            }
            else
            {
                UpdateViewText();
            }
        }

        private void UpdateViewText()
        {
            cursorText.Text.Set("|");

            int sub = 0;
            if (cursorPos >= numHideChars)
            {
                sub = numHideChars;
            }
            string substractedText = input.Substring(sub);
            viewText.Text.Set(substractedText);
            int cursorLen = IngameInput.StringPixelWidth(substractedText.Substring(0, cursorPos - sub));
            int inputLen = IngameInput.StringPixelWidth(substractedText);

            if (fixedBorders)
            {
                cursorText.PosX = PixelToViewVirtual(cursorLen - sub - 1);
            }
            else
            {
                bool changed = false;
                if (cursorLen + startPos > width) //cursor is outside of the right window border
                {
                    startPos = width - cursorLen; //move to the left
                }
                else if (cursorLen + startPos < 0) //cursor is outside of the left window border
                {
                    startPos = -cursorLen; //move to the right
                }
                else if (showLeftArrow && inputLen + startPos < width) //we want to move to the start position
                {
                    startPos = width - inputLen;
                }

                viewText.PosX = PixelToViewVirtual(startPos);
                cursorText.PosX = PixelToViewVirtual(cursorLen + startPos - 1);

                if (startPos < 0)
                {
                    if (!showLeftArrow)
                    {
                        zCView.GetStartscreen(proc).InsertItem(leftArrow, 1);
                        width -= 10; pos.X += 10;
                        showLeftArrow = true;
                        changed = true;
                    }
                }
                else
                {
                    if (showLeftArrow)
                    {
                        zCView.GetStartscreen(proc).RemoveItem(leftArrow);
                        width += 10; pos.X -= 10;
                        showLeftArrow = false;
                        changed = true;
                    }
                }

                if (inputLen + startPos > width)
                {
                    if (!showRightArrow)
                    {
                        zCView.GetStartscreen(proc).InsertItem(rightArrow, 1);
                        width -= 10;
                        showRightArrow = true;
                        changed = true;
                    }
                }
                else
                {
                    if (showRightArrow)
                    {
                        zCView.GetStartscreen(proc).RemoveItem(rightArrow);
                        width += 10;
                        showRightArrow = false;
                        changed = true;
                    }
                }

                if (changed)
                {
                    int[] vpos = InputHooked.PixelToVirtual(proc, new int[] { pos.X, pos.Y });
                    int[] vsize = InputHooked.PixelToVirtual(proc, new int[] { width, height });

                    thisView.SetPos(vpos[0], vpos[1]);
                    thisView.SetSize(vsize[0], vsize[1]);
                    cursorView.SetPos(vpos[0], vpos[1]);
                    cursorView.SetSize(vsize[0], vsize[1]);
                    UpdateViewText();
                }
            }
            if (cursorText.PosX < 0)
            {
                cursorText.PosX = 0;
            }
            else if (cursorText.PosX > 0x2000)
            {
                cursorText.PosX = 0x2000;
            }
        }

        public void SetXSize(int x, int w)
        {
            pos.X = x;
            width = w;

            int[] vpos = InputHooked.PixelToVirtual(proc, new int[] { pos.X, pos.Y });
            int[] vsize = InputHooked.PixelToVirtual(proc, new int[] { width, height });

            thisView.SetPos(vpos[0], vpos[1]);
            thisView.SetSize(vsize[0], vsize[1]);
            cursorView.SetPos(vpos[0], vpos[1]);
            cursorView.SetSize(vsize[0], vsize[1]);

            //Left arrow
            int[] arrowSize = InputHooked.PixelToVirtual(proc, new int[] { 10, 10 });
            int arrowY = vpos[1] + (vsize[1] - arrowSize[1]) / 2;
            leftArrow.SetPos(vpos[0], arrowY);

            //Right arrow
            int arrowX = vpos[0] + vsize[0] - arrowSize[0];
            rightArrow.SetPos(arrowX, arrowY);

            showLeftArrow = false;
            showRightArrow = false;
            zCView.GetStartscreen(proc).RemoveItem(leftArrow);
            zCView.GetStartscreen(proc).RemoveItem(rightArrow);

            UpdateViewText();
        }

        public void Update(long ticks)
        {
            if (ticks > cursorTime)
            {
                if (cursorText.Text.Length == 0)
                {
                    cursorText.Text.Set("|");
                }
                else
                {
                    cursorText.Text.Clear();
                }
                cursorTime = ticks + 3000000;
            }
        }

        private int PixelToViewVirtual(int p)
        {
            int screenWidth = InputHooked.GetScreenSize(proc)[0];
            return (p * 0x2000 / screenWidth) * 0x2000 / (width * 0x2000 / screenWidth);
        }

        private void SetInput(string str)
        {
            text.Clear();
            text.Append(str);
        }

        private string GetCharFromKey(VirtualKeys key)
        {
            var buf = new StringBuilder(256);
            var keyboardState = new byte[256];
            if (InputHooked.IsPressed((int)VirtualKeys.Shift))
                keyboardState[(int)VirtualKeys.Shift] = 0xff;
            if (InputHooked.IsPressed((int)VirtualKeys.Control) && InputHooked.IsPressed((int)VirtualKeys.Menu))
            {
                keyboardState[(int)VirtualKeys.Control] = 0xff;
                keyboardState[(int)VirtualKeys.Menu] = 0xff;
            }
            WinApi.User.Input.ToUnicode((uint)key, 0, keyboardState, buf, 256, 0);

            return buf.ToString();
        }
    }
}
