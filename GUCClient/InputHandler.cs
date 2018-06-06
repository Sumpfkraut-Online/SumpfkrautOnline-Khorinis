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
        #region Testing

        static Dictionary<VirtualKeys, Action> gucKeys = new Dictionary<VirtualKeys, Action>()
        {
            { VirtualKeys.F4, Program.Exit },
            { VirtualKeys.F7, () =>
                {
                    var ai = Network.GameClient.Client?.Character?.gVob?.HumanAI;
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
                        Process.Write(ai.Address + 0x1204, bitField);
                    }
                }
            },
            { VirtualKeys.F8, () =>
                {
                    WorldObjects.World.Current.BarrierCtrl.SetNextWeight(0, 1.0f);
                    return;
                    try
                    {
                        /*var vob = Gothic.Objects.zCVob.Create();
                        var vis = new Gothic.Objects.Meshes.zCVisual(GothicGlobals.Game.GetWorld().SkyControlerOutdoor.Barrier.FrontierMesh.Address);
                        vob.SetVisual("NW_City_Ship_01.3ds");
                       
                        vob.FarZClipScale = 1E30f;
                        vob.SetPositionWorld(0, 1000, 0);
                        GothicGlobals.Game.GetWorld().AddVob(vob);
                        vob.SetPositionWorld(0, 1000, 0);
                        Log.Logger.Log("Done");*/
                    }
                    catch (Exception e)
                    {
                        Log.Logger.Log(e);
                    }

                }
            }
        };

        #endregion

        public static bool IsPressed(VirtualKeys key)
        {
            return keys[(int)key];
        }

        public delegate void KeyPressEventHandler(VirtualKeys key);
        public static event KeyPressEventHandler OnKeyDown = null;
        public static event KeyPressEventHandler OnKeyUp = null;

        // cursor stuff
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
            long now = GameTime.Ticks;
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
                    if (Input.GetCursorPos(out Input.POINT pos))
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
                                OnKeyDown(key);
                        }
                    }
                    else
                    {
                        if (keys[i]) //release
                        {
                            keys[i] = false;
                            if (!gucKeys.ContainsKey(key) && OnKeyUp != null)
                                OnKeyUp(key);
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
                                OnKeyUp(key);
                        }
                    }
                }
            }
        }
    }
}
