using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace Network
{
    public class SpawnFunction
    {
        public String name;
        public List<SpawnContent> Spawns = new List<SpawnContent>();
    }
    public class SpawnContent
    {
        public String instance;
        public List<Object> Spawns = new List<object>();
        public String world;
    }
    public class SpawnList
    {

        public List<SpawnFunction> SpawnFunctions = new List<SpawnFunction>();
        public String StartSpawnFunction = "";


        public SpawnList()
        {
            //SpawnFunction func = new SpawnFunction();
            //func.name = "START";
            //SpawnContent sc = new SpawnContent();
            //sc.instance = "WOLF";
            //sc.Spawns.Add(new Vec3());
            //func.Spawns.Add(sc);
            //sc.world = "NEWWORLD";
            //SpawnFunctions.Add(func);
            //StartSpawnFunction = "START";
        }

        public void Save()
        {
            Save(@"./conf/spawn.xml");
        }

        public void Save(String strf)
        {
            XmlSerializer ser = new XmlSerializer(typeof(SpawnList), new Type[] { typeof(Vec3) });

            FileStream str = new FileStream(strf, FileMode.Create);
            ser.Serialize(str, this);
            str.Close();
        }

        public static SpawnList Load(String name)
        {
            XmlSerializer ser = new XmlSerializer(typeof(SpawnList), new Type[] { typeof(Vec3) });
            StreamReader sr = new StreamReader(name);
            SpawnList Kunde1 = (SpawnList)ser.Deserialize(sr);
            sr.Close();
            return Kunde1;
        }

        public static SpawnList Load()
        {
            return Load(@"./conf/spawn.xml");
        }
    }
}
