using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;

namespace Gothic.System
{
    public class zFile_VDFS : zFile_File
    {
        new public static bool InitFileSystem()
        {
            return Process.CDECLCALL<BoolArg>(0x44AD60);
        }

        new public static bool DeinitFileSystem()
        {
            return Process.CDECLCALL<BoolArg>(0x44B440);
        }

        public static bool VDFSInitialized
        {
            get { return Process.ReadBool(0x8C34C4); }
            set { Process.Write(0x8C34C4, value); }
        }
    }
}
