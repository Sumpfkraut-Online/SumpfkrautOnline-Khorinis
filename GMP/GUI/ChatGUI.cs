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

namespace GUC.GUI
{
    class ChatGUI : InputReceiver
    {
        public int startWritingKey = (int)VirtualKeys.Return;
        public int resetKey = (int)VirtualKeys.Escape;
        public int sendKey = (int)VirtualKeys.Return;

        Process process;

        zString emptyString;
        zCView thisView;

        zCViewText[] textLines;
        zCViewText textInput;

        List<Row> rows = new List<Row>();
        //int scrollPosition;

        bool writing;

        public Action<String> SendInput;

        class Row
        {
            public String message;
            public ColorRGBA color = ColorRGBA.White;
        }

        public ChatGUI(int lines)
        {
            process = Process.ThisProcess();
            emptyString = zString.Create(process, "");

            thisView = zCView.Create(process, 0, 0, 0x2000, 0x2000);

            int dist = InputHooked.PixelToVirtualY(process, thisView.Font.GetFontY());
            textLines = new zCViewText[lines];
            for (int i = 0; i < lines; i++)
            {
                textLines[i] = thisView.CreateText(10, 10 + i * dist, emptyString);
            }
            textInput = thisView.CreateText(10, 10 + lines * dist, emptyString);

            rows = new List<Row>();
            //scrollPosition = 0;

            writing = false;

            InputHooked.receivers.Add(this);
            zCView.GetStartscreen(process).InsertItem(thisView, 0);
        }

        public void AddLine(string text, ColorRGBA color)
        {
            Row row = new Row();
            row.message = text;
            row.color = color;

            rows.Add(row);
            UpdateTexts();
        }

        private void UpdateTexts()
        {
            int startPos = rows.Count - textLines.Length;
            if (startPos < 0) startPos = 0;
            for (int i = 0; i < textLines.Length; i++)
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
        }

        public void KeyPressed(int key)
        {
            if (WinApi.User.Window.GetWindowThreadProcessId(WinApi.User.Window.GetForegroundWindow()) != process.ProcessID || zCConsole.Console(process).IsVisible() == 1)
                return;

            if (!writing && key == startWritingKey)
            {
                KeyEnable();
                return;
            }

            if (!writing)
                return;

            if (key == sendKey || key == resetKey)
            {
                KeyDisable();
                string text = textInput.Text.ToString().Trim();
                if (SendInput != null && key != resetKey && text.Length > 1)
                {
                    SendInput(text);
                    textInput.Text.Clear();
                }

                if (key == resetKey)
                {
                    textInput.Text.Clear();
                }
            }

            if (key == (int)VirtualKeys.Back) //Backspace
            {
                if (textInput.Text.Length > 0)
                {
                    textInput.Text.Set(textInput.Text.ToString().Remove(textInput.Text.Length - 1));
                }
                return;
            }

            String keyVal = Convert.ToString((char)key);
            keyVal = GetCharsFromKeys((VirtualKeys)key, InputHooked.IsPressed((int)VirtualKeys.Shift), InputHooked.IsPressed((int)VirtualKeys.Control) && InputHooked.IsPressed((int)VirtualKeys.Menu));

            textInput.Text.Add(keyVal);
        }

        private static string GetCharsFromKeys(VirtualKeys keys, bool shift, bool altGr)
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

        public void wheelChanged(int steps)
        {
        }

        private void KeyEnable()
        {
            InputHooked.deaktivateFullControl(process);
            writing = true;
        }

        private void KeyDisable()
        {
            InputHooked.activateFullControl(process);
            writing = false;
        }
    }
}
