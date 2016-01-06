using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using WinApi.SetupApi.Structures;

namespace WinApi.SetupApi
{
    class ClassDev
    {
        [Flags]
        public enum DiGetClassFlags : uint
        {
            DIGCF_DEFAULT = 0x00000001,  // only valid with DIGCF_DEVICEINTERFACE
            DIGCF_PRESENT = 0x00000002,
            DIGCF_ALLCLASSES = 0x00000004,
            DIGCF_PROFILE = 0x00000008,
            DIGCF_DEVICEINTERFACE = 0x00000010,
        }

        [DllImport("setupapi.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SetupDiGetClassDevs(
                                                      ref Guid ClassGuid,
                                                      [MarshalAs(UnmanagedType.LPTStr)] string Enumerator,
                                                      IntPtr hwndParent,
                                                      uint Flags
                                                     );
        [DllImport("setupapi.dll", CharSet = CharSet.Auto)]
        static extern IntPtr SetupDiGetClassDevs(           // 1st form using a ClassGUID only, with null Enumerator
           ref Guid ClassGuid,
           IntPtr Enumerator,
           IntPtr hwndParent,
           int Flags
        );
        [DllImport("setupapi.dll", CharSet = CharSet.Auto)]     // 2nd form uses an Enumerator only, with null ClassGUID
        static extern IntPtr SetupDiGetClassDevs(
           IntPtr ClassGuid,
           string Enumerator,
           IntPtr hwndParent,
           int Flags
        );

        [DllImport(@"setupapi.dll", CharSet=CharSet.Auto, SetLastError = true)]
        public static extern Boolean SetupDiEnumDeviceInterfaces(
           IntPtr hDevInfo,
           ref DeviceInterfaceData devInfo,
           ref Guid interfaceClassGuid,
           UInt32 memberIndex,
           ref DeviceInterfaceData deviceInterfaceData
        );

        // Alternate signature if you do not care about SP_DEVINFO_DATA and wish to pass NULL (IntPtr.Zero). Note example below uses this signature.
        [DllImport(@"setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern Boolean SetupDiEnumDeviceInterfaces(
           IntPtr hDevInfo,
           IntPtr devInfo,
           ref Guid interfaceClassGuid,
           UInt32 memberIndex,
           ref DeviceInterfaceData deviceInterfaceData
        );

        [DllImport(@"setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern Boolean SetupDiGetDeviceInterfaceDetail(
           IntPtr hDevInfo,
           ref DeviceInterfaceData deviceInterfaceData,
           ref DeviceInterfaceDetailData deviceInterfaceDetailData,
           UInt32 deviceInterfaceDetailDataSize,
           out UInt32 requiredSize,
           ref DeviceInterfaceData deviceInfoData
        );

        //[DllImport(@"setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        //public static extern Boolean SetupDiGetDeviceInterfaceDetail(
        //   IntPtr hDevInfo,
        //   ref DeviceInterfaceData deviceInterfaceData,
        //   ref DeviceInterfaceDetailData deviceInterfaceDetailData,
        //   UInt32 deviceInterfaceDetailDataSize,
        //   ref UInt32 requiredSize,
        //   IntPtr deviceInfoData
        //);

        [DllImport(@"setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern Boolean SetupDiGetDeviceInterfaceDetail(
           IntPtr hDevInfo,
           ref DeviceInterfaceData deviceInterfaceData,
           IntPtr deviceInterfaceDetailData,
           UInt32 deviceInterfaceDetailDataSize,
           ref UInt32 requiredSize,
           IntPtr deviceInfoData
        );

        [DllImport(@"setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern Boolean SetupDiGetDeviceInterfaceDetail(
           IntPtr hDevInfo,
           ref DeviceInterfaceData deviceInterfaceData,
           ref DeviceInterfaceDetailData deviceInterfaceDetailData,
           uint deviceInterfaceDetailDataSize,
           out uint requiredSize,
           IntPtr deviceInfoData
        );
    }
}
