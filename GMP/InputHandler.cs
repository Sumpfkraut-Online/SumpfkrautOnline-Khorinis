using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;
using WinApi.User;
using WinApi.User.Enumeration;
using GUC.Client.Menus;

namespace GUC.Client
{
    static class InputHandler
    {
        public static void DeactivateGothicControl()
        {
            Process.Write(new byte[] { 0xE9, 0xA8, 0x00 }, 0x4D4D3D); // disable ingame keyboard movement
            Process.Write(new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90, 0x90 }, 0x4D3E50); // disable x-mouse movement  
            Process.Write(new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90, 0x90 }, 0x4D3E5C); // disable y-mouse movement  
        }

        public static void ActivateGothicControl()
        {
            Process.Write(new byte[] { 0x0F, 0x84, 0xA7 }, 0x4D4D3D); //enable ingame keyboard movement
            Process.Write(new byte[] { 0x89, 0x0D, 0x5C, 0x16, 0x8D, 0x00 }, 0x4D3E50); // enable x-mouse movement    
            Process.Write(new byte[] { 0x89, 0x15, 0x60, 0x16, 0x8D, 0x00 }, 0x4D3E5C); // enable y-mouse movement   
        }

        public static bool IsPressed(VirtualKeys key)
        {
            return ((Input.GetAsyncKeyState(key) & 0x8001) == 0x8001 || (Input.GetAsyncKeyState(key) & 0x8000) == 0x8000);
        }

        static void SendKeyPressed(VirtualKeys key)
        {
            if (GUCMenus._ActiveMenus.Count == 0)
            {
                //no active menus, check for shortcuts
                Action action = null;
                //Program._state.Shortcuts.TryGetValue(key, out action);
                if (action != null)
                {
                    action();
                }
            }
            else //a menu is open
            {
                GUCMenus._ActiveMenus[0].KeyPressed(key);
            }
        }
        
        static long[] keys = new long[0xFF];
        public static void Update()
        {
            if (WinApi.Process.IsForeground())
            {
                long ticks = DateTime.UtcNow.Ticks;
                for (int i = 1; i < keys.Length; i++)
                {
                    if (IsPressed((VirtualKeys)i))
                    {
                        if (keys[i] == 0) //newly pressed
                        {
                            keys[i] = ticks + 5500000;
                            SendKeyPressed((VirtualKeys)i);
                        }
                        else //hold
                        {
                            if (ticks > keys[i])
                            {
                                keys[i] = ticks + 1000000;
                                SendKeyPressed((VirtualKeys)i);
                            }
                        }
                    }
                    else
                    {
                        if (keys[i] > 0) //release
                        {
                            keys[i] = 0;
                            //SendKeyReleased(i);
                        }
                    }

                }

                if (GUCMenus._ActiveMenus.Count > 0)
                {
                    GUCMenus._ActiveMenus[0].Update(ticks);
                }
            }
            else
            {
                Array.Clear(keys, 0, keys.Length);
            }
        }
    }
}
