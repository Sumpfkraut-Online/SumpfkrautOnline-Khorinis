using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Utilities;
using WinApi.User.Enumeration;

namespace GUC.Scripts.Sumpfkraut.Options
{
    public static class ClientOptions
    {
        const string FilePath = "options.xml";
        

        public struct Options
        {
            public VirtualKeys key1;
        }

        static Options options = new Options();

        public static void Save()
        {
            XMLHelper.SaveObject<Options>(options, Program.GetFullPath(FilePath));
        }

        public static void Load()
        {
            options = XMLHelper.LoadObject<Options>(FilePath);
        }
    }
}
