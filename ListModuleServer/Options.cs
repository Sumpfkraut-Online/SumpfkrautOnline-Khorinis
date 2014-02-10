using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using Network;

namespace ListModuleServer
{
    public class User
    {
        public String name;
        public String password;
        public String PermName;
    }




    public class Permission
    {
        public String name;
        public bool kick = true;
        public bool ban = true;
        public bool mute = true;
        public bool teleportToPlayer = true;
        public bool TeleportPlayer = true;
        public bool makeInvisible = true;
        public bool makeImmortal = true;
        public bool giveItem = true;
        public bool freeze = true;
        public bool kill = true;
        public bool revive = true;
        public bool giveTempRights = true;
        public bool Scale = true;
        public bool SetFatness = true;
        public bool setTalents = true;
    }


    public class Options
    {
        public int maxTries = 10;
        public String MasterPassword = "";
        public List<User> UserList;
        public List<Permission> PermissionList;

        public bool PlayerListAvailable;
        public bool AdminAvailable;
        public bool animationListAvailable;
        public bool speakListAvailable;


        public BlockOptions BlockOptionsAnimation = new BlockOptions();
        public BlockOptions BlockOptionsSpeak = new BlockOptions();


        public bool animationListWhiteList;
        public List<String> AvailableAnimations = new List<string>();



        public Permission GetPermissionByName(String name)
        {
            foreach (Permission perm in PermissionList)
            {
                if (perm.name.Trim().ToLower() == name.Trim().ToLower())
                    return perm;
            }

            return null;
        }

        public void Save()
        {
            Save(@"./conf/listmodule.xml");
        }

        public void Save(String strf)
        {
            XmlSerializer ser = new XmlSerializer(typeof(Options));

            FileStream str = new FileStream(strf, FileMode.Create);
            ser.Serialize(str, this);
            str.Close();
        }

        public static Options Load(String name)
        {
            XmlSerializer ser = new XmlSerializer(typeof(Options));
            StreamReader sr = new StreamReader(name);
            Options Kunde1 = (Options)ser.Deserialize(sr);
            sr.Close();
            return Kunde1;
        }

        public static Options Load()
        {
            if (!File.Exists(@"./conf/listmodule.xml"))
            {
                Options op = new Options();
                op.Save();
                return op;
            }
            return Load(@"./conf/listmodule.xml");
        }
    }
}
