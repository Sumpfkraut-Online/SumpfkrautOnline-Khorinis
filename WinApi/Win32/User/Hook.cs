using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace WinApi.Win32.User
{
    public class Hook
    {
        public const int WH_KEYBOARD_LL = 13;


        [StructLayout(LayoutKind.Sequential)]
        public class keyboardHookStruct
        {
            public int vkCode;
            public int scanCode;
            public int flags;
            public int time;
            public int dwExtraInfo;
        }
        public delegate int HookProc(int code, IntPtr wParam, IntPtr lParam);
        //Import for SetWindowsHookEx function.
        //Use this function to install thread-specific hook.
        [DllImport("user32.dll", CharSet = CharSet.Auto,
         CallingConvention = CallingConvention.StdCall)]
        public static extern int SetWindowsHookEx(int idHook, HookProc lpfn,
        IntPtr hInstance, int threadId);

        //Import for UnhookWindowsHookEx.
        //Call this function to uninstall the hook.
        [DllImport("user32.dll", CharSet = CharSet.Auto,
         CallingConvention = CallingConvention.StdCall)]
        public static extern bool UnhookWindowsHookEx(int idHook);

        //Import for CallNextHookEx.
        //Use this function to pass the hook information to next hook procedure in chain.
        [DllImport("user32.dll", CharSet = CharSet.Auto,
         CallingConvention = CallingConvention.StdCall)]
        public static extern int CallNextHookEx(int idHook, int nCode,
        IntPtr wParam, IntPtr lParam);
    }
}
