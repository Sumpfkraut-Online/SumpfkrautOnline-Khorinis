using System;
using System.Collections.Generic;
using System.Text;
using Network;

namespace ClassChooserServerModule
{
    public class talent
    {
        public byte id;
        public byte value;
    }
    public class GothicClass
    {
        public String name;
        public String description;
        public String instance;

        public int guild;

        public int hp;
        public int mp;
        public int str;
        public int dex;

        public string weapon;
        public string rangeweapon;
        public string armor;

        public List<item> items = new List<item>();
        public List<talent> talents = new List<talent>();
        public List<Object> Spawn = new List<Object>();
        public List<Object> Respawn = new List<Object>();
    }
    public class GothicClasses
    {
        public List<GothicClass> classes;
        public int respawnTime;
        public static GothicClasses load()
        {
            return null;
        }
    }
}
