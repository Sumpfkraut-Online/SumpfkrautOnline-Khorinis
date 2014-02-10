using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using Network;

namespace ClassChooserServerModule
{
    public class item
    {
        public String code;
        public int amount;
    }

    public class talent
    {
        public byte id;
        public byte value;
    }
    public class GothicClass
    {
        public String name = "";
        public String description= "";
        public String instance = "PC_HERO";
        public int hp;
        public int mp;
        public int str;
        public int dex;

        public List<Object> Spawn = new List<object>();
        public List<Object> Respawn = new List<object>();

        public string weapon = "";
        public string rangeweapon = "";
        public string armor = "";

        public List<item> items = new List<item>();
        public List<talent> talents = new List<talent>();
    }
    public class GothicClasses
    {
        public int respawnTime = 30;
        public List<GothicClass> classes;

        public void Save()
        {
            Save(@"./conf/class.xml");
        }

        public void Save(String strf)
        {
            XmlSerializer ser = new XmlSerializer(typeof(GothicClasses), new Type[] { typeof(Vec3) });

            FileStream str = new FileStream(strf, FileMode.Create);
            ser.Serialize(str, this);
            str.Close();
        }

        public static GothicClasses Load(String name)
        {
            XmlSerializer ser = new XmlSerializer(typeof(GothicClasses), new Type[] { typeof(Vec3) });
            StreamReader sr = new StreamReader(name);
            GothicClasses Kunde1 = (GothicClasses)ser.Deserialize(sr);
            sr.Close();
            return Kunde1;
        }

        public static GothicClasses Load()
        {
            return Load(@"./conf/class.xml");
        }
    }
}
