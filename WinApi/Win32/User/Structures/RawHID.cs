using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace WinApi.User.Structures
{
    /// <summary>
    /// Value type for raw input from a HID.
    /// </summary>    
    [StructLayout(LayoutKind.Sequential)]
    public struct RawHID
    {
        /// <summary>Size of the HID data in bytes.</summary>
        public int Size;
        /// <summary>Number of HID in Data.</summary>
        public int Count;
        /// <summary>Data for the HID.</summary>
        public IntPtr Data;
    }
}
