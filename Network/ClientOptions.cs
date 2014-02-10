using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace Network
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


        public bool autoupdate = true;
        public String updateUser = "";
        public String updatePassword = "";
        public int loglevel = 0;
        public int fps = 30;

        public bool startWindowed = false;

        public bool allowModules = true;


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
            XmlSerializer ser = new XmlSerializer(typeof(ClientOptions));
            StreamReader sr = new StreamReader(name);
            ClientOptions Kunde1 = (ClientOptions)ser.Deserialize(sr);
            sr.Close();
            return Kunde1;
        }

        public static ClientOptions Load()
        {
            return Load(@"gmp.xml");
        }
    }
}
