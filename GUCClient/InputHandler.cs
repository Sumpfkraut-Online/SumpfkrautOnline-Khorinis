using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;
using WinApi.User;
using WinApi.User.Enumeration;

namespace GUC
{
    public static class InputHandler
    {
        static ushort curVobID = 0;
        static Dictionary<VirtualKeys, Action> gucKeys = new Dictionary<VirtualKeys, Action>()
        {
            { VirtualKeys.F4, Program.Exit },
            { VirtualKeys.F5, () =>
                {
                    var ai = Network.GameClient.Client?.character?.gVob?.HumanAI;
                    if (ai != null)
                    {
                        int bitField = Process.ReadInt(ai.Address + 0x1204);
                        if ((bitField & 0x10) != 0)
                        {
                            bitField &= ~0x10;
                        }
                        else
                        {
                            bitField |= 0x10;
                        }
                        Process.Write(bitField, ai.Address + 0x1204);
                    }
                }
            },
            { VirtualKeys.F6, () =>
                {
                    for (int i = 0; i < ushort.MaxValue; i++)
                    {
                        WorldObjects.BaseVob vob;
                        if (WorldObjects.World.current.TryGetVob(curVobID++, out vob))
                        {
                            var pos = vob.GetPosition();
                            Log.Logger.Log("Teleport to vob " + vob.ID + " at " + pos);
                            if (Network.GameClient.Client.character == null)
                            {
                                if (Network.GameClient.Client.IsSpectating)
                                {
                                    Gothic.oCGame.GetCameraVob().SetPositionWorld(pos.X, pos.Y, pos.Z);
                                }
                            }
                            else
                            {
                                Network.GameClient.Client.character.SetPosition(pos);
                            }
                            return;
                        }
                    }
                }
            },
            { VirtualKeys.F7, () =>
                {
                }
            },
            { VirtualKeys.F9, () =>
                {
                }
            }
        };

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
                        if (!keys[i]) //newly pressed
                        {
                            keys[i] = true;
                            Action gucAction;
                            if (gucKeys.TryGetValue(key, out gucAction))
                                gucAction();
                            else if (OnKeyDown != null)
                                OnKeyDown(key, ticks);
                        }
                    }
                    else
                    {
                        if (keys[i]) //release
                        {
                            keys[i] = false;
                            if (!gucKeys.ContainsKey(key) && OnKeyUp != null)
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
                            VirtualKeys key = (VirtualKeys)i;
                            keys[i] = false;
                            if (!gucKeys.ContainsKey(key) && OnKeyUp != null)
                                OnKeyUp(key, ticks);
                        }
                    }
                }
            }
        }
    }
}
