using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using WinApi.HID.Structures;

namespace WinApi.HID
{
    public class HID
    {
        [DllImport("hid.dll", SetLastError = true)]
        public static extern void HidD_GetHidGuid(ref Guid hidGuid);

        [DllImport("hid.dll", SetLastError = true)]
        public static extern bool HidD_GetPreparsedData(SafeFileHandle hFile, out IntPtr lpData);

        [DllImport("hid.dll", SetLastError = true)]
        public static extern Boolean HidD_FreePreparsedData(ref IntPtr PreparsedData);

        [DllImport("hid.dll", SetLastError = true)]
        public static extern int HidP_GetCaps(IntPtr preparsedData, out HidCaps hitcaps);
    }
}
