using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using WinApi.User.Enumeration;
using WinApi.User.Structures;

namespace WinApi.User
{
    public class Input
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

            public POINT(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }
        }

        [DllImport("user32.dll")]
        public static extern short GetAsyncKeyState(int vKey);
        [DllImport("User32.dll")]
        public static extern short GetAsyncKeyState(VirtualKeys vKey);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SendMessage(long hWnd, int msg, int wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)] // used for button-down & button-up
        private static extern int PostMessage(int hWnd, int msg, int wParam, IntPtr lParam);




        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, StringBuilder lParam);
        //If you use '[Out] StringBuilder', initialize the string builder with proper length first.

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, [MarshalAs(UnmanagedType.LPStr)] string lParam);
        //Also can add 'ref' or 'out' ahead of 'String lParam', don't use CharSet.Auto since we use MarshalAs(..) in this
        //example.

        //Works for unicode. One can also use CharSet = CharSet.Unicode instead of [MarshalAs(UnmanagedType.LPWStr)]
        [DllImport("user32.dll", EntryPoint = "SendMessageW")]
        public static extern IntPtr SendMessageW(IntPtr hWnd, UInt32 Msg, IntPtr wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);


        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 msg, IntPtr wParam, ref POINT lParam);

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, Int32 wParam, Int32 lParam);






        /// <summary>
        /// Function to retrieve raw input data.
        /// </summary>
        /// <param name="hRawInput">Handle to the raw input.</param>
        /// <param name="uiCommand">Command to issue when retrieving data.</param>
        /// <param name="pData">Raw input data.</param>
        /// <param name="pcbSize">Number of bytes in the array.</param>
        /// <param name="cbSizeHeader">Size of the header.</param>
        /// <returns>0 if successful if pData is null, otherwise number of bytes if pData is not null.</returns>
        [DllImport("user32.dll")]
        public static extern int GetRawInputData(IntPtr hRawInput, RawInputCommand uiCommand, out RawInput pData, ref int pcbSize, int cbSizeHeader);

        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(ref POINT lpPoint);

        /// <summary>
        /// Function to retrieve raw input data.
        /// </summary>
        /// <param name="hRawInput">Handle to the raw input.</param>
        /// <param name="uiCommand">Command to issue when retrieving data.</param>
        /// <param name="pData">Raw input data.</param>
        /// <param name="pcbSize">Number of bytes in the array.</param>
        /// <param name="cbSizeHeader">Size of the header.</param>
        /// <returns>0 if successful if pData is null, otherwise number of bytes if pData is not null.</returns>
        [DllImport("user32.dll")]
        public static extern int GetRawInputData(IntPtr hRawInput, RawInputCommand uiCommand, byte[] pData, ref int pcbSize, int cbSizeHeader);
    }
}
