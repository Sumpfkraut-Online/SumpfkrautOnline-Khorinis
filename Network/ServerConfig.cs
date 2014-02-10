using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace Network
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

    public class BlockOptions
    {
        public bool blockInWater = false;
        public bool blockWhenDead = false;
        public bool blockWhenUnconscious = false;
        public bool blockWhenSleep = false;
    }
    public class ChatOptions
    {
        public bool All = true;
        public bool Distance = true;
        public bool Private = true;
        public bool Friends = false;
        public bool Guild = false;

        public float DistanceRange = 500.0f;

        public BlockOptions BlockOptions = new BlockOptions();
        

        public void SetOptions(byte x)
        {
            if ((x & 1) == 1)
                All = true;
            else
                All = false;

            if ((x & 2) == 2)
                Distance = true;
            else
                Distance = false;

            if ((x & 4) == 4)
                Private = true;
            else
                Private = false;

            if ((x & 8) == 8)
                Friends = true;
            else
                Friends = false;

            if ((x & 16) == 16)
                Guild = true;
            else
                Guild = false;
        }

        public byte GetOptions()
        {
            byte op = 0;

            if (All)
                op += 1;
            if (Distance)
                op += 2;
            if (Private)
                op += 4;
            if (Friends)
                op += 8;
            if (Guild)
                op += 16;

            return op;
        }
    }
    
    public class Vec3
    {
        [XmlAttribute]
        public float x;

        [XmlAttribute]
        public float y;

        [XmlAttribute]
        public float z;
    }

    public class ServerConfig
    {
        public String ServerName = "Test-Server";
        public int Port = 9054;
        public int Slots = 20;
        public String World = @"NewWorld\NewWorld.zen";
        public String password = "";
        public List<Object> Spawn = new List<Object>();
        public List<Module> Modules = new List<Module>();
        public ChatOptions chatOptions = new ChatOptions();
        public bool HideNames = false;
        public bool OnlyHumanNames = false;
        public bool ShowConnectionMessages = true;
        public bool AvailableFriends = true;
        public bool ImmortalFriends = true;

        public bool useScripts = true;
        public bool useScriptedFile = false;
        public bool generateScriptedFile = false;

        public int RemoveNPCTime = 30;

        public bool LoadItemFromFile = false;
        public bool UnLockAll = false;

        public bool DamageOnServer = false;
        public bool NPCAIOnServer = false;

        public int day = 0;
        public int hour = 12;
        public int minute = 00;


        public bool enableStamina = true;
        public String staminaValue = "dex*100";

        public bool time24h = false;
        public bool timeStopped = false;

        public short InventoryLimit = -1;

        public bool blockAllScriptFunctions = false;
        public bool blockStartupScriptFunctions = false;
        public bool removeAllContainers = false;//All containers in the zen will be removed



        public ServerConfig()
        {
            
        }


        public Object GetRandomSpawn()
        {
            if(Spawn.Count == 0)
                return new Vec3() { x=0, y=0, z=0 };
            Random rand = new Random();
            int spawn = rand.Next(0, Spawn.Count - 1);

            return Spawn[spawn];
        }

        public static bool Exist()
        {
            return File.Exists(@"server.config");
        }

        public void Save()
        {
            Save(@"server.config");
        }

        public void Save(String strf)
        {
            XmlSerializer ser = new XmlSerializer(typeof(ServerConfig), new Type[]{typeof(Vec3)});
            
            FileStream str = new FileStream(strf, FileMode.Create);
            ser.Serialize(str, this);
            str.Close();
        }

        public static ServerConfig Load(String name)
        {
            XmlSerializer ser = new XmlSerializer(typeof(ServerConfig), new Type[] { typeof(Vec3) });
            StreamReader sr = new StreamReader(name);
            ServerConfig Kunde1 = (ServerConfig)ser.Deserialize(sr);
            sr.Close();
            return Kunde1;
        }

        public static ServerConfig Load()
        {
            return Load(@"server.config");
        }
    }
}
