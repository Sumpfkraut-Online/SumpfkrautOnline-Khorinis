using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi.User.Enumeration;

namespace GUC.Client.GUI
{
    public class GUCTextBox : GUCView
    {
        public int CharacterLimit = 512;
        public bool AllowSpaces = true;
        public bool AllowSymbols = true;
        public bool OnlyNumbers = false;
        public int HideChars = 0;

        int[] pos;
        int width;
        int height = FontsizeDefault;

        bool fixedBorders;

        GUCVisual inputVis;
        GUCVisualText inputText { get { return inputVis.Texts[0]; } }

        int startPos = 0; //how much characters are out of the window

        const int cursorSize = 16;
        const int cursorOffsetX = 1 - cursorSize / 2;
        const int cursorOffsetY = 1;
        int cursorPos = 0;
        GUCVisual cursorVis;

        const int arrowSize = 10;
        GUCVisual leftArrow;
        GUCVisual rightArrow;
        bool leftArrowShown = false;
        bool rightArrowShown = false;

        bool enabled = false;
        public bool Enabled
        {
            get { return enabled; }
            set 
            { 
                enabled = value; 
                if (value) cursorVis.Show(); 
                else cursorVis.Hide(); 
            }
        }

        StringBuilder input = new StringBuilder();
        public string Input 
        { 
            get { return input.ToString(); }
            set
            {
                input.Clear();
                input.Append(value);
                cursorPos = 0;
                UpdateInputVisual();
            }
        }

        public GUCTextBox(int x, int y, int w, bool fixedBorders)
        {
            pos = new int[] { x, y };
            width = w;
            this.fixedBorders = fixedBorders;

            inputVis = new GUCVisual(x, y, w, height);
            inputVis.CreateText("",0,0);

            cursorVis = new GUCVisual(x+cursorOffsetX, y+cursorOffsetY, cursorSize, cursorSize);
            cursorVis.SetBackTexture("CURSOR.TGA");

            leftArrow = new GUCVisual(x, y + (height - arrowSize) / 2, arrowSize, arrowSize);
            leftArrow.SetBackTexture("L.TGA");

            rightArrow = new GUCVisual(x + w - arrowSize, y + (height - arrowSize) / 2, arrowSize, arrowSize);
            rightArrow.SetBackTexture("R.TGA");
        }

        public override void Show()
        {
            inputVis.Show();
            if (enabled) cursorVis.Show();
            if (leftArrowShown) leftArrow.Show();
            if (rightArrowShown) rightArrow.Show();
        }

        public override void Hide()
        {
            inputVis.Hide();
            cursorVis.Hide();
            leftArrow.Hide();
            rightArrow.Hide();
        }

        long cursorTime = 0;
        public void Update(long now)
        {
            if (!enabled)
                return;

            if (now > cursorTime)
            {
                if (cursorVis.Shown)
                {
                    cursorVis.Hide();
                }
                else
                {
                    cursorVis.Show();
                }
                cursorTime = now + 300*TimeSpan.TicksPerMillisecond;
            }
        }

        public void KeyPressed(VirtualKeys key)
        {
            if (!enabled) return;

            if (key == VirtualKeys.Back) //Backspace, delete char behind the cursor
            {
                if (cursorPos > 0)
                {
                    input.Remove(cursorPos - 1, 1);
                    cursorPos--;
                }
            }
            else if (key == VirtualKeys.Delete) //delete char in front of cursor
            {
                if (input.Length > cursorPos)
                {
                    input.Remove(cursorPos, 1);
                }
            }
            else if (key == VirtualKeys.Left)
            {
                if (cursorPos > 0) cursorPos--;
            }
            else if (key == VirtualKeys.Right)
            {
                if (input.Length > cursorPos) cursorPos++;
            }
            else
            {
                if (input.Length > CharacterLimit)
                    return;

                string str = GetCharFromKey(key);
                if (str == null || str.Length == 0)
                    return;

                char c = str[0];
                if (!GUCView.GothicChars.ContainsKey(c)) //check if typed char is supported by gothic's font
                    return;

                if (OnlyNumbers && Char.GetNumericValue(c) < 0)
                    return;

                if (!AllowSpaces && c == ' ')
                    return; //we don't want spaces

                if (!AllowSymbols && GUCView.GothicChars.Keys.ToList().IndexOf(c) > 52)
                    return;


                if (fixedBorders && StringPixelWidth(Input) + GUCView.GothicChars[c] > width) //check if fixed borders are reached
                    return;

                input.Insert(cursorPos, c);
                cursorPos++;
            }
            cursorVis.Show();
            UpdateInputVisual();
        }

        void UpdateInputVisual()
        {
            int sub = 0;
            if (cursorPos >= HideChars)
            { //don't hide chars when the cursor is between them
                sub = HideChars;
            }

            string substractedText = Input.Substring(sub);
            inputText.Text = substractedText;
            int cursorLen = StringPixelWidth(substractedText.Substring(0,cursorPos - sub));
            int inputLen = StringPixelWidth(substractedText);

            if (fixedBorders)
            {
                cursorVis.SetPosX(pos[0] + cursorOffsetX + cursorLen - sub);
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
                else if (leftArrowShown && inputLen + startPos < width) //we want to move to the start position
                {
                    startPos = width - inputLen;
                }

                inputText.SetPosX(startPos);
                cursorVis.SetPosX(pos[0] + cursorOffsetX + cursorLen + startPos);

                if (startPos < 0)
                {
                    if (!leftArrowShown)
                    {
                        leftArrow.Show();
                        width -= 10; pos[0] += 10;
                        leftArrowShown = true;
                        changed = true;
                    }
                }
                else
                {
                    if (leftArrowShown)
                    {
                        leftArrow.Hide();
                        width += 10; pos[0] -= 10;
                        leftArrowShown = false;
                        changed = true;
                    }
                }

                if (inputLen + startPos > width)
                {
                    if (!rightArrowShown)
                    {
                        rightArrow.Show();
                        width -= 10;
                        rightArrowShown = true;
                        changed = true;
                    }
                }
                else
                {
                    if (rightArrowShown)
                    {
                        rightArrow.Hide();
                        width += 10;
                        rightArrowShown = false;
                        changed = true;
                    }
                }

                if (changed)
                {
                    inputVis.SetPosX(pos[0]);
                    inputVis.SetSizeX(width);
                    UpdateInputVisual();
                }
            }
        }

        string GetCharFromKey(VirtualKeys key)
        {
            var buf = new StringBuilder(256);
            var keyboardState = new byte[256];
            if (InputHandler.IsPressed(VirtualKeys.Shift))
                keyboardState[(int)VirtualKeys.Shift] = 0xff;
            if (InputHandler.IsPressed(VirtualKeys.Control) && InputHandler.IsPressed(VirtualKeys.Menu))
            {
                keyboardState[(int)VirtualKeys.Control] = 0xff;
                keyboardState[(int)VirtualKeys.Menu] = 0xff;
            }
            WinApi.User.Input.ToUnicode((uint)key, 0, keyboardState, buf, 256, 0);

            return buf.ToString();
        }
    }
}
