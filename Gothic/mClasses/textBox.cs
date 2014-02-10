using System;
using System.Collections.Generic;
using System.Text;
using Gothic.zClasses;
using Gothic.zTypes;
using WinApi;
using WinApi.User.Enumeration;

namespace Gothic.mClasses
{
    public class textBox : InputReceiver
    {

        public int startWritingKey = (int)VirtualKeys.T;
        public int resetKey = (int)VirtualKeys.Escape;
        public int sendKey = (int)VirtualKeys.Return;

        zCView view;
        String text = "";
        public zCViewText vt;
        Process process;

        public Boolean WriteEnabled;
        public textBox(zCView view, Process process)
        {
            this.process = process;
            this.view = view;
            zString empty = zString.Create(process, "");
            vt = view.CreateText(0, 0, empty);
            empty.Dispose();
            vt.Timed = 0;
            vt.Timer = -1;


            Inputenabled = true;
            InputHooked.receivers.Add(this);
        }

        public void Delete()
        {
            InputHooked.receivers.Remove(this);
            vt.Timed = 1;
            vt.Timer = 0;
        }
        

        public void setText(String t)
        {
            text = t;
            vt.Text.Set(text);
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
                    text = "";
                    vt.Text.Set("");
                }
            }
        }
        public void wheelChanged(int steps) { }

        
        public void KeyEnable()
        {
            InputHooked.deaktivateFullControl(process);
            WriteEnabled = true;
        }

        public void KeyDisable()
        {
            InputHooked.activateFullControl(process);
            WriteEnabled = false;
        }

        public void KeyPressed(int key)
        {
            if (!Inputenabled)
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
                vt.Text.Set(text);
                return;
            }
            if (((int)key < 0x30 || (int)key > 0x5A) && (int)key != 0x20 && (int)key != 222 && (int)key != 192
                && (int)key != 186 && (int)key != 219 && (int)key != (int)VirtualKeys.OEMPeriod
                && (int)key != (int)VirtualKeys.OEMComma && (int)key != (int)VirtualKeys.OEMMinus)
                return;


            String keyVal = Convert.ToString((char)key);
            if ((int)key == 222)
                keyVal = "Ä";
            if ((int)key == 192)
                keyVal = "Ö";
            if ((int)key == 186)
                keyVal = "Ü";
            if ((int)key == 219)
                keyVal = "ß";

            if ((int)key == (int)VirtualKeys.N1 && InputHooked.IsPressed((int)VirtualKeys.Shift))
                keyVal = "!";
            if ((int)key == (int)VirtualKeys.N2 && InputHooked.IsPressed((int)VirtualKeys.Shift))
                keyVal = "\"";
            if ((int)key == (int)VirtualKeys.N3 && InputHooked.IsPressed((int)VirtualKeys.Shift))
                keyVal = "§";
            if ((int)key == (int)VirtualKeys.N4 && InputHooked.IsPressed((int)VirtualKeys.Shift))
                keyVal = "$";
            if ((int)key == (int)VirtualKeys.N5 && InputHooked.IsPressed((int)VirtualKeys.Shift))
                keyVal = "%";
            if ((int)key == (int)VirtualKeys.N7 && InputHooked.IsPressed((int)VirtualKeys.Shift))
                keyVal = "/";
            if ((int)key == (int)VirtualKeys.N8 && InputHooked.IsPressed((int)VirtualKeys.Shift))
                keyVal = "(";
            if ((int)key == (int)VirtualKeys.N9 && InputHooked.IsPressed((int)VirtualKeys.Shift))
                keyVal = ")";
            if ((int)key == (int)VirtualKeys.N0 && InputHooked.IsPressed((int)VirtualKeys.Shift))
                keyVal = "=";
            if ((int)key == 219 && InputHooked.IsPressed((int)VirtualKeys.Shift))
                keyVal = "?";


            if ((int)key == (int)VirtualKeys.N7 && InputHooked.IsPressed((int)VirtualKeys.RightMenu))
                keyVal = "{";
            if ((int)key == (int)VirtualKeys.N0 && InputHooked.IsPressed((int)VirtualKeys.RightMenu))
                keyVal = "}";
            if ((int)key == 219 && InputHooked.IsPressed((int)VirtualKeys.RightMenu))
                keyVal = "\\";
            
            if ((int)key == (int)VirtualKeys.OEMPeriod)
                keyVal = ".";
            if ((int)key == (int)VirtualKeys.OEMPeriod && InputHooked.IsPressed((int)VirtualKeys.Shift))
                keyVal = ":";
            if ((int)key == (int)VirtualKeys.OEMComma)
                keyVal = ",";
            if ((int)key == (int)VirtualKeys.OEMComma && InputHooked.IsPressed((int)VirtualKeys.Shift))
                keyVal = ";";
            if ((int)key == (int)VirtualKeys.OEMMinus)
                keyVal = "-";
            if ((int)key == (int)VirtualKeys.OEMMinus && InputHooked.IsPressed((int)VirtualKeys.Shift))
                keyVal = "_";
            if (!InputHooked.IsPressed((int)VirtualKeys.Shift))
                keyVal = keyVal.ToLower();
            
            text += keyVal;
            vt.Text.Add(keyVal);
        }
    }
}
