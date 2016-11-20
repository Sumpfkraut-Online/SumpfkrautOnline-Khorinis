using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;
using Gothic.Types;

namespace Gothic.System
{
    public class zFile
    {
        public static zString s_rootPathString
        {
            get { return new zString(0x8C3468); }
        }

        public static zString s_virtPathString
        {
            get { return new zString(0x8C3494); }
        }
    }
}
