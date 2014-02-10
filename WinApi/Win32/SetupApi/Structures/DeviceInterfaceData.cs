using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace WinApi.SetupApi.Structures
{
    [StructLayout(LayoutKind.Sequential)]
    public struct DeviceInterfaceData
    {
        public uint size;
        public Guid guid;
        public uint flags;
        public IntPtr reserved;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct DeviceInterfaceDetailData
    {
        public uint size;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string DevicePath;
    }
}
