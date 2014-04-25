using System;
using System.Collections.Generic;
using System.Text;
using Gothic.zClasses;
using Gothic.zTypes;
using WinApi;
using WinApi.User.Enumeration;

namespace Gothic.mClasses
{
    public class textArea : InputReceiver
    {

        public int startWritingKey = (int)VirtualKeys.T;
        public int resetKey = (int)VirtualKeys.Escape;
        public int sendKey = (int)0;
        public int newLineKey = (int)VirtualKeys.Return;

        zCView view;
        String text = "";
        public zCViewText[] vt;
        Process process;

        public Boolean WriteEnabled;



        protected int fontY = 0;

        public textArea(Process process, zCView view, int height)
        {
            this.process = process;
            this.view = view;

            fontY = this.view.Font.GetFontY();
            fontY = InputHooked.PixelToVirtualY(process, fontY);

            int lines = height / fontY;

            zString empty = zString.Create(process, "");
            vt = new zCViewText[lines];
            for(int i = 0; i < lines; i++){
                vt[i] = view.CreateText(0, i * fontY, empty);
                vt[i].Timed = 0;
                vt[i].Timer = -1;
            }
            empty.Dispose();


            Inputenabled = true;
            InputHooked.receivers.Add(this);
        }

        public void Delete()
        {
            InputHooked.receivers.Remove(this);
            for (int i = 0; i < vt.Length; i++)
            {
                vt[i].Timed = 1;
                vt[i].Timer = 0;
            }
        }
        

        public void setText(String t)
        {
            text = t;
            String[] strList = t.Split(new String[]{"\n"}, StringSplitOptions.None);
            for (int i = 0; i < vt.Length; i++)
            {
                String str = "";
                if (strList.Length > i)
                    str = strList[i];
                vt[i].Text.Set(str);
            }
        }

        public String getText()
        {
            return text;
        }

        public bool Inputenabled { get; set; }

        public event EventHandler<EventArgs> SendInput;


        public void KeyReleased(int key)
        {
            if (!Inputenabled)
                return;
            if (WinApi.User.Window.GetWindowThreadProcessId(WinApi.User.Window.GetForegroundWindow()) != process.ProcessID
             || zCConsole.Console(process).IsVisible() == 1)
                return;

            if (WriteEnabled && (key == sendKey || key == resetKey))
            {
                KeyDisable();
                if (SendInput != null && key != resetKey)
                {
                    SendInput(this, new EventArgs());
                }

               
                if (key == resetKey)
                {
                    setText("");
                }
            }
        }
        public void wheelChanged(int steps) { }

        public static textArea activeTB = null;
        public void KeyEnable()
        {
            InputHooked.deaktivateFullControl(process);
            WriteEnabled = true;

            activeTB = this;
        }

        public void KeyDisable()
        {
            InputHooked.activateFullControl(process);
            WriteEnabled = false;

            activeTB = null;
        }

        public static string GetCharsFromKeys(VirtualKeys keys, bool shift, bool altGr)
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

        public void KeyPressed(int key)
        {
            if (!Inputenabled || (activeTB != null && activeTB != this) || (textBox.activeTB != null))
                return;
            if (WinApi.User.Window.GetWindowThreadProcessId(WinApi.User.Window.GetForegroundWindow()) != process.ProcessID
             || zCConsole.Console(process).IsVisible() == 1)
                return;

            if (!WriteEnabled && key == startWritingKey)
            {
                KeyEnable();
                return;
            }
            
            if (!WriteEnabled)
                return;
            if (key == 8)
            {
                if (text.Length == 0)
                    return;
                text = text.Substring(0, text.Length-1);
                setText(text);
                return;
            }

            if (key == newLineKey)
            {
                text += "\n";
                setText(text);
                return;
            }

            String keyVal = Convert.ToString((char)key);
            keyVal = GetCharsFromKeys((VirtualKeys)key, InputHooked.IsPressed((int)VirtualKeys.Shift), InputHooked.IsPressed((int)VirtualKeys.Control) && InputHooked.IsPressed((int)VirtualKeys.Menu));
            
            
            text += keyVal;
            setText(text);
        }
    }
}
