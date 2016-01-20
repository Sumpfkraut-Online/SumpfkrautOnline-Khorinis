using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Utilities;
using System.IO;

namespace GUC.Server.Options
{
    class ServerOptions : XmlObj
    {
        public string ServerName = "Test-Server";
        public ushort Port = 9054;
        public ushort Slots = 100;
        public string Password = "";

        public static ServerOptions Init()
        {
            if (!Directory.Exists("Config"))
                Directory.CreateDirectory("Config");

            return XmlObj.Load<ServerOptions>("Config\\Server.xml");
        }
    }
}
