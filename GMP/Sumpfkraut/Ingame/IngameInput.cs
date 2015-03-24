using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.mClasses;
using WinApi;

namespace GUC.Sumpfkraut.Ingame
{
    class IngameInput : InputReceiver
    {
        private static IngameInput ingRec = null;
        public static void Init()
        {
            if (ingRec == null)
            {
                ingRec = new IngameInput();
            }
        }

        private IngameInput()
        {
            InputHooked.receivers.Add(this);
        }

        public static List<InputReceiver> receivers = new List<InputReceiver>();
        private static InputReceiver singleRec = null;

        public static void activateFullControl(InputReceiver rec)
        {
            singleRec = rec;
            InputHooked.deaktivateFullControl(Process.ThisProcess());
        }

        public static void deactivateFullControl()
        {
            singleRec = null;
            InputHooked.activateFullControl(Process.ThisProcess());
        }

        public void KeyReleased(int key)
        {
            if (WinApi.User.Window.GetWindowThreadProcessId(WinApi.User.Window.GetForegroundWindow()) == Process.ThisProcess().ProcessID)
            {
                if (singleRec == null)
                {
                    foreach (InputReceiver rec in receivers)
                    {
                        rec.KeyReleased(key);
                    }
                }
                else
                {
                    singleRec.KeyReleased(key);
                }
            }
        }

        public void KeyPressed(int key)
        {
            if (WinApi.User.Window.GetWindowThreadProcessId(WinApi.User.Window.GetForegroundWindow()) == Process.ThisProcess().ProcessID)
            {
                if (singleRec == null)
                {
                    foreach (InputReceiver rec in receivers)
                    {
                        rec.KeyPressed(key);
                    }
                }
                else
                {
                    singleRec.KeyPressed(key);
                }
            }
        }

        public void wheelChanged(int steps)
        {
        }
    }
}
