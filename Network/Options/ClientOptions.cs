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
        public byte keyToogleChat = 0x73;
        public byte killSummoned = 0x4B;
        public byte keySprint = 0x12;


        public bool autoupdate = false;
        public String updateUser = "";
        public String updatePassword = "";
        public int loglevel = 0;
        public int fps = 30;

        public bool startWindowed = false;

        public bool allowModules = true;

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
            ClientOptions Kunde1 = null;
            using(Stream fileStream = new FileStream(name, FileMode.Open))
            {
                XmlSerializer ser = new XmlSerializer(typeof(ClientOptions));
                Kunde1 = (ClientOptions)ser.Deserialize(fileStream);
            }

            return Kunde1;
        }

        public static ClientOptions Load()
        {
            return Load(@"gmp.xml");
        }
    }
}
