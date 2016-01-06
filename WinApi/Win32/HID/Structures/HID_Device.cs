using System;
using System.Collections.Generic;
using System.Text;

namespace WinApi.HID.Structures
{
    using PCHAR = IntPtr;
    using Handle = IntPtr;
    using PHIDP_PREPARSED_DATA = IntPtr;

    struct HID_Device
    {
        PCHAR DevicePath;
        Handle HidDevice;
        bool OpenedForRead;
        bool OpenedForWrite;
        bool OpenedOverlapped;
        bool OpenedExclusive;

        PHIDP_PREPARSED_DATA Ppd;
        HidCaps Caps;


    }
}
