using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using WinApi.User.Enumeration;

namespace WinApi.User.Structures
{
    /// <summary>
    /// Value type for raw input from a keyboard.
    /// </summary>    
    [StructLayout(LayoutKind.Sequential)]
    public struct RawKeyboard
    {
        /// <summary>Scan code for key depression.</summary>
        public short MakeCode;
        /// <summary>Scan code information.</summary>
        public RawKeyboardFlags Flags;
        /// <summary>Reserved.</summary>
        public short Reserved;
        /// <summary>Virtual key code.</summary>
        public VirtualKeys VirtualKey;
        /// <summary>Corresponding window message.</summary>
        public WindowsMessages Message;
        /// <summary>Extra information.</summary>
        public int ExtraInformation;
    }
}
