using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace Gothic_Untold_Chapter
{
    public class Players
    {
        public String name;
    }
    public class Server
    {
        public String name;
        public String ip;
        public int port;
        public int slots;

        public Players[] players;
    }
    public class ServerList
    {
        public Server[] List;


        public static ServerList Load(String filename)
        {
            XmlSerializer ser = new XmlSerializer(typeof(ServerList));
            StreamReader sr = new StreamReader(filename);
            ServerList op = (ServerList)ser.Deserialize(sr);
            sr.Close();
            return op;
        }

        public void Save(String strf)
        {
            XmlSerializer ser = new XmlSerializer(typeof(ServerList));

            FileStream str = new FileStream(strf, FileMode.Create);
            ser.Serialize(str, this);
            str.Close();
        }
    }
}
