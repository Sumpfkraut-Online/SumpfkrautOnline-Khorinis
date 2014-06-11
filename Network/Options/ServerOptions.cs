using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace GUC.Options
{
    public class Module
    {
        public String name;
        public String fileName;
        public String Hash;

        public Boolean downloaded;
        public Boolean started;
        public Boolean ended;
        public bool loaded;
        public long size;
        public long loadingSize;



        [XmlIgnore]
        public System.Reflection.Assembly assembly;
        [XmlIgnore]
        public List<Object> ModuleObj = new List<object>();
    }


    public class ServerOptions
    {
        public String ServerName = "Test-Server";
        public int Port = 9054;
        public int Slots = 20;
        public String password = "";

        public List<Module> Modules = new List<Module>();
        public List<String> AdditionalLibs = new List<string>();
        public List<String> AdditionalSymbols = new List<string>();

        public bool useScriptedFile = false;
        public bool generateScriptedFile = false;

        public ServerOptions()
        {
            
        }

        public static bool Exist()
        {
            return File.Exists(@"Config/Server.config");
        }

        public void Save()
        {
            Save(@"Config/Server.config");
        }

        public void Save(String strf)
        {
            XmlSerializer ser = new XmlSerializer(typeof(ServerOptions), new Type[] { });

            FileStream str = new FileStream(strf, FileMode.Create);
            ser.Serialize(str, this);
            str.Close();
        }

        public static ServerOptions Load(String name)
        {
            XmlSerializer ser = new XmlSerializer(typeof(ServerOptions), new Type[] { });
            StreamReader sr = new StreamReader(name);
            ServerOptions Kunde1 = (ServerOptions)ser.Deserialize(sr);
            sr.Close();
            return Kunde1;
        }

        public static ServerOptions Load()
        {
            return Load(@"Config/Server.config");
        }
    }
}
