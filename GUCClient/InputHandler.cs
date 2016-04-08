using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;
using WinApi.User;
using WinApi.User.Enumeration;

namespace GUC.Client
{
    public static class InputHandler
    {
        public static bool IsPressed(VirtualKeys key)
        {
            return keys[(int)key];
        }

        public delegate void KeyPressEventHandler(VirtualKeys key, long ticks);
        public static event KeyPressEventHandler OnKeyDown = null;
        public static event KeyPressEventHandler OnKeyUp = null;

        static bool[] keys = new bool[0xFF];
        internal static void Update()
        {
            long ticks = DateTime.UtcNow.Ticks;
            if (Process.IsForeground())
            {
                for (int i = 1; i < keys.Length; i++)
                {
                    VirtualKeys key = (VirtualKeys)i;
                    if ((Input.GetAsyncKeyState(key) & 0x8001) == 0x8001 || (Input.GetAsyncKeyState(key) & 0x8000) == 0x8000)
                    {
                        if (key == VirtualKeys.F4)
                        {
                            Program.Exit();
                            return;
                        }

                        if (!keys[i]) //newly pressed
                        {
                            keys[i] = true;
                            if (OnKeyDown != null)
                                OnKeyDown(key, ticks);
                        }
                    }
                    else
                    {
                        if (keys[i]) //release
                        {
                            keys[i] = false;
                            if (OnKeyUp != null)
                                OnKeyUp(key, ticks);
                        }
                    }
                }
            }
            else
            {
                for (int i = 1; i < keys.Length; i++)
                {
                    if (keys[i]) //release
                    {
                        keys[i] = false;
                        if (OnKeyUp != null)
                            OnKeyUp((VirtualKeys)i, ticks);
                    }
                }
            }
        }
    }
}
