using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace GUC.Options
{
    public class ClientOptions
    {
        public String ip = "localhost";
        public String password = "";
        public String name = "Spieler 1";
        public String lang = "de";
        public ushort port = 9054;


        public bool autoupdate = false;
        public String updateUser = "";
        public String updatePassword = "";
        public int loglevel = 0;
        public int fps = 30;

        public bool startWindowed = false;

        public bool SaveMode = false;


        public void Save()
        {
            Save(@"gmp.xml");
        }

        public void Save(String strf)
        {
            XmlSerializer ser = new XmlSerializer(typeof(ClientOptions));

            FileStream str = new FileStream(strf, FileMode.Create);
            ser.Serialize(str, this);
            str.Close();
        }

        public static ClientOptions Load(String name)
        {
            ClientOptions co = null;
            using(Stream fileStream = new FileStream(name, FileMode.Open))
            {
                XmlSerializer ser = new XmlSerializer(typeof(ClientOptions));
                co = (ClientOptions)ser.Deserialize(fileStream);
            }

            return co;
        }

        public static ClientOptions Load()
        {
            return Load(@"gmp.xml");
        }
    }
}
