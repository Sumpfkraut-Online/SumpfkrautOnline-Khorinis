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

        static bool shown = false;
        static int movedX, movedY;
        public static int MouseDistX { get { return movedX; } }
        public static int MouseDistY { get { return movedY; } }
        const int DefaultMousePosX = 320;
        const int DefaultMousePosY = 240;
        static Input.POINT oriPos;

        static bool[] keys = new bool[0xFF];
        internal static void Update()
        {
            long ticks = GameTime.Ticks;
            if (Process.IsForeground())
            {
                if (!shown)
                {
                    shown = true;
                    while (Input.ShowCursor(false) >= 0)
                    {
                    }

                    Input.GetCursorPos(out oriPos);
                    Input.SetCursorPos(DefaultMousePosX, DefaultMousePosY);
                    movedX = 0;
                    movedY = 0;
                }
                else
                {
                    Input.POINT pos;
                    if (Input.GetCursorPos(out pos))
                    {
                        movedX = pos.X - DefaultMousePosX;
                        movedY = pos.Y - DefaultMousePosY;

                        Input.SetCursorPos(DefaultMousePosX, DefaultMousePosY);
                    }
                }
                
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
                        else if (key == VirtualKeys.F5)
                        {
                            int bitField = Process.ReadInt(GUC.Network.GameClient.Client.Character.gVob.HumanAI.Address + 0x1204);
                            if ((bitField & 0x10) != 0)
                            {
                                bitField &= ~0x10;
                            }
                            else
                            {
                                bitField |= 0x10;
                            }
                            Process.Write(bitField, GUC.Network.GameClient.Client.Character.gVob.HumanAI.Address + 0x1204);
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
                if (shown)
                {
                    shown = false;
                    while (Input.ShowCursor(true) < 0)
                    {
                    }

                    Input.SetCursorPos(oriPos.X, oriPos.Y);

                    movedX = 0;
                    movedY = 0;

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
}
