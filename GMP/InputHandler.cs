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
                    if (IsPressed(key))
                    {
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
