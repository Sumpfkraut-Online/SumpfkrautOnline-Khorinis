using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;

namespace Gothic.System
{
    public class zFile_File : zFile
    {
        public static bool InitFileSystem()
        {
            return Process.CDECLCALL<BoolArg>(0x4485E0);
        }

        public static bool DeinitFileSystem()
        {
            return Process.CDECLCALL<BoolArg>(0x448650);
        }
    }
}
