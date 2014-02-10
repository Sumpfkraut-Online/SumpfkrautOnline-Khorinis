using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace WinApi.User.Structures
{
    [StructLayout(LayoutKind.Sequential)]
    public struct RawInput
    {
        /// <summary>
        /// Header for the data.
        /// </summary>
        public RawInputHeader Header;
        public Union Data;
        [StructLayout(LayoutKind.Explicit)]
        public struct Union
        {
            /// <summary>
            /// Mouse raw input data.
            /// </summary>
            [FieldOffset(0)]
            public RawMouse Mouse;
            /// <summary>
            /// Keyboard raw input data.
            /// </summary>
            [FieldOffset(0)]
            public RawKeyboard Keyboard;
            /// <summary>
            /// HID raw input data.
            /// </summary>
            [FieldOffset(0)]
            public RawHID HID;
        }
    }
}
