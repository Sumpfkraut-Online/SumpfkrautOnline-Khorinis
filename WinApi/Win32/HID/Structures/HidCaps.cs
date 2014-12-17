using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace WinApi.HID.Structures
{
    public enum ntStatus : long
    {
        HIDP_STATUS_SUCCESS = 0x00110000,
        HIDP_STATUS_NULL = 0x80110001,
        HIDP_STATUS_INVALID_PREPARSED_DATA = 0xC0110001,
        HIDP_STATUS_INVALID_REPORT_TYPE = 0xC0110002,
        HIDP_STATUS_INVALID_REPORT_LENGTH = 0xC0110003,
        HIDP_STATUS_USAGE_NOT_FOUND = 0xC0110004,
        HIDP_STATUS_VALUE_OUT_OF_RANGE = 0xC0110005,
        HIDP_STATUS_BAD_LOG_PHY_VALUES = 0xC0110006,
        HIDP_STATUS_BUFFER_TOO_SMALL = 0xC0110007,
        HIDP_STATUS_INTERNAL_ERROR = 0xC0110008,
        HIDP_STATUS_I8042_TRANS_UNKNOWN = 0xC0110009,
        HIDP_STATUS_INCOMPATIBLE_REPORT_ID = 0xC011000A,
        HIDP_STATUS_NOT_VALUE_ARRAY = 0xC011000B,
        HIDP_STATUS_IS_VALUE_ARRAY = 0xC011000C,
        HIDP_STATUS_DATA_INDEX_NOT_FOUND = 0xC011000D,
        HIDP_STATUS_DATA_INDEX_OUT_OF_RANGE = 0xC011000E,
        HIDP_STATUS_BUTTON_NOT_PRESSED = 0xC011000F,
        HIDP_STATUS_REPORT_DOES_NOT_EXIST = 0xC0110010,
        HIDP_STATUS_NOT_IMPLEMENTED = 0xC0110020,
        HIDP_STATUS_I8242_TRANS_UNKNOWN = HIDP_STATUS_I8042_TRANS_UNKNOWN,
        HIDP_LINK_COLLECTION_ROOT = -1,
        HIDP_LINK_COLLECTION_UNSPECIFIED = 0
    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct HidCaps
    {
        public short Usage;
        public short UsagePage;
        public short InputReportByteLength;
        public short OutputReportByteLength;
        public short FeatureReportByteLength;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)]
        public short[] Reserved;
        public short NumberLinkCollectionNodes;
        public short NumberInputButtonCaps;
        public short NumberInputValueCaps;
        public short NumberInputDataIndices;
        public short NumberOutputButtonCaps;
        public short NumberOutputValueCaps;
        public short NumberOutputDataIndices;
        public short NumberFeatureButtonCaps;
        public short NumberFeatureValueCaps;
        public short NumberFeatureDataIndices;
    }
    //public struct HidCaps
    //{
    //    public ushort usage;
    //    public ushort usagepage;
    //    public ushort InputReportByteLength;
    //    public ushort OutputReportByteLength;
    //    public ushort FeatureReportByteLength;
    //    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)]
    //    public ushort[] Reserved;
    //    public ushort NumberLinkCollectionNodes;
    //    public ushort NumberInputButtonCaps;
    //    public ushort NumberInputValueCaps;
    //    public ushort NumberInputDataIndices;
    //    public ushort NumberOutputButtonCaps;
    //    public ushort NumberOutputValueCaps;
    //    public ushort NumberOutputDataIndices;
    //    public ushort NumberFeatureButtonCaps;
    //    public ushort NumberFeatureValueCaps;
    //    public ushort NumberFeatureDataIndices;
    //}
}
