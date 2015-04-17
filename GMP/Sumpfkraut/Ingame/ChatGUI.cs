using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.mClasses;
using Gothic.zClasses;
using Gothic.zTypes;
using WinApi;
using WinApi.User.Enumeration;
using GUC.Types;

namespace GUC.Sumpfkraut.Ingame
{
    class ChatGUI : InputReceiver
    {
        Process process;

        zCView thisView;

        List<zCViewText> textLines;
        zCViewText textInput;

        String input;

        List<Row> rows = new List<Row>();
        //int scrollPosition;

        bool writing;

        public delegate void SendHandler(String text);
        public event SendHandler SendInput;

        class Row
        {
            public String message;
            public ColorRGBA color = ColorRGBA.White;
        }

        public ChatGUI()
        {
            process = Process.ThisProcess();

            //our graphic view field
            thisView = zCView.Create(process, 0, 0, 0x2000, 0x2000);

            int[] startPos = new int[] { 5, 5 }; //top left start position of texties
            int lines = (InputHooked.GetScreenSize(process)[1] - startPos[1]) / (thisView.Font.GetFontY()) / 4; //number of text lines on top of the screen, 1/4 of the screen 

            startPos = InputHooked.PixelToVirtual(process, startPos); //to virtual units
            int dist = InputHooked.PixelToVirtualY(process, thisView.Font.GetFontY()); //distance between the lines

            using (zString emptyString = zString.Create(process, ""))
            {
                //create text lines
                textLines = new List<zCViewText>();
                for (int i = 0; i < lines-1; i++)
                {
                    textLines.Add(thisView.CreateText(startPos[0], startPos[1] + i * dist, emptyString));
                }
                //last line is input line with some extra distance
                textInput = thisView.CreateText(10, 10 + lines * dist + 1, emptyString);
            }

            input = "";
            rows = new List<Row>();  //saved messages (add a limit?)
            //scrollPosition = 0;
            writing = false;

            //IngameInput.receivers.Add(this); //add for input
            zCView.GetStartscreen(process).InsertItem(thisView, 0); //add for graphics
        }

        public void AddLine(string text, ColorRGBA color)
        {
            Row row = new Row();
            row.message = text;
            row.color = color;

            rows.Add(row);
            UpdateOutputTexts();
        }

        private void UpdateOutputTexts()
        {
            int startPos = rows.Count - textLines.Count;
            if (startPos < 0) startPos = 0;
            for (int i = 0; i < textLines.Count; i++)
            {
                if (startPos + i > rows.Count) return;
                textLines[i].Text.Set(rows[startPos + i].message);
                textLines[i].Color.R = rows[startPos + i].color.R;
                textLines[i].Color.G = rows[startPos + i].color.G;
                textLines[i].Color.B = rows[startPos + i].color.B;
                textLines[i].Color.A = rows[startPos + i].color.A;
            }
        }

        public void KeyReleased(int key)
        {
            if (key == (int)VirtualKeys.Escape)
            {
                KeyDisable();
            }
        }

        public void KeyPressed(int key)
        {
            if (!writing && key == (int)VirtualKeys.Return) //"open" chat
            {
                KeyEnable();
                return;
            }

            if (writing)
            {
                if (key == (int)VirtualKeys.Return) //send the input
                {
                    string text = input.Trim();
                    if (SendInput != null && text.Length > 1)
                    {
                        if (!text.EndsWith(".") && !text.EndsWith("!") && !text.EndsWith("?"))
                        {
                            text += "."; //add a fullstop to the end
                        }
                        SendInput(text);
                        input = "";
                    }
                    KeyDisable();
                    return;
                }
                else if (key == (int)VirtualKeys.Back) //Backspace, delete last char
                {
                    if (input.Length > 0)
                    {
                        input = input.Remove(input.Length - 1);
                    }
                }
                else
                { //add the char
                    input += GetCharsFromKeys((VirtualKeys)key, InputHooked.IsPressed((int)VirtualKeys.Shift), InputHooked.IsPressed((int)VirtualKeys.Control) && InputHooked.IsPressed((int)VirtualKeys.Menu));
                }
                ShowInputText();
           }
        }

        private void ShowInputText()
        {
            textInput.Text.Set(">" + input);
        }

        private void HideInputText()
        {
            textInput.Text.Clear();
        }

        private string GetCharsFromKeys(VirtualKeys keys, bool shift, bool altGr)
        {
            var buf = new StringBuilder(256);
            var keyboardState = new byte[256];
            if (shift)
                keyboardState[(int)VirtualKeys.Shift] = 0xff;
            if (altGr)
            {
                keyboardState[(int)VirtualKeys.Control] = 0xff;
                keyboardState[(int)VirtualKeys.Menu] = 0xff;
            }
            WinApi.User.Input.ToUnicode((uint)keys, 0, keyboardState, buf, 256, 0);
            return buf.ToString();
        }

        public void wheelChanged(int steps) {}

        private void KeyEnable()
        {
            //IngameInput.activateFullControl(this);
            ShowInputText();
            writing = true;
        }

        private void KeyDisable()
        {
            IngameInput.deactivateFullControl();
            HideInputText();
            writing = false;
        }
    }
}
