using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using WinApi.User.Enumeration;
using Network;

namespace AdminAndListModule
{
    public class Animations
    {
        public String name;
        public String animation;
        public String SVM;
        public int key;


        public override string ToString()
        {
            if (name != null && name != "")
                return name;
            else if (animation != null && animation != "")
                return animation;
            return SVM;
        }
    }
    public class Options
    {
        public bool PlayerListAvailable;
        public bool AdminAvailable;
        public bool animationListAvailable;
        public bool speakListAvailable;


        public BlockOptions BlockOptionsAnimation = new BlockOptions();
        public BlockOptions BlockOptionsSpeak = new BlockOptions();

        public bool animationListWhiteList;
        public List<String> AvailableAnimations = new List<string>();
    }

    public class ClientOptions
    {
        public int playerListKey = (int)VirtualKeys.F1;
        public int animationKey = (int)VirtualKeys.F2;
        public int soundKey = (int)VirtualKeys.F3;
        public List<Animations> AnimationList = new List<Animations>();
        public List<Animations> SoundList = new List<Animations>();
        public List<Animations> AnimationKeyList = new List<Animations>();


        
        public void Save(Module module)
        {
            Save(System.IO.Path.GetDirectoryName(module.assembly.Location)+@"/conf/listmodule.xml");
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

        public static ClientOptions Load(Module module)
        {
            if (!File.Exists(System.IO.Path.GetDirectoryName(module.assembly.Location) + @"/conf/listmodule.xml"))
            {
                ClientOptions co = new ClientOptions();
                co.Save(module);
                return co;
            }
            return Load(System.IO.Path.GetDirectoryName(module.assembly.Location)+@"/conf/listmodule.xml");
        }
    }
}
