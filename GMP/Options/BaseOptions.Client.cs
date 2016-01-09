using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Options
{
    public partial class BaseOptions
    {
        public String ServerAddress = "localhost";
        public ushort ServerPort = 9054;

        public String LastAccountName = "";
        public String LastAccountPassword = "";
        
        public int zSpyLevel = 0;
        public int MaxFps = 0;

        public bool StartWindowed = false;
    }
}
