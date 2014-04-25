using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Character;
using RakNet;
using GUC.Network;
using GUC.Types;

namespace GUC.WorldObjects
{
    internal partial class World
    {
        protected GUC.Server.Scripting.Objects.World scriptingWorld;

        

        public Dictionary<String, List<Vob>> VobPositionList = new Dictionary<string, List<Vob>>();
        public Dictionary<String, List<Item>> ItemPositionList = new Dictionary<string, List<Item>>();
        public Dictionary<String, List<NPCProto>> NPCPositionList = new Dictionary<string, List<NPCProto>>();
        public Dictionary<String, List<Mobs.MobInter>> MobInterPositionList = new Dictionary<string, List<Mobs.MobInter>>();
        public Dictionary<String, List<Player>> PlayerPositionList = new Dictionary<string, List<Player>>();
        

        public static String[] getImportantKeysByPosition(float[] position)
        {
            if (position == null)
                return null;

            String[] strings = new String[9];
            int x = (int)(position[0] / 4000);
            int z = (int)(position[2] / 4000);

            strings[0] = (x - 1) + ";" + (z - 1);
            strings[1] = (x - 1) + ";" + (z);
            strings[2] = (x - 1) + ";" + (z + 1);

            strings[3] = (x) + ";" + (z - 1);
            strings[4] = (x) + ";" + (z);
            strings[5] = (x) + ";" + (z + 1);

            strings[6] = (x + 1) + ";" + (z - 1);
            strings[7] = (x + 1) + ";" + (z);
            strings[8] = (x + 1) + ";" + (z + 1);

            return strings;//y-Position is not important!
        }

        public static String getKeyByPosition(float[] position)
        {
            if (position == null)
                return null;
            int x = (int)(position[0] / 4000);
            int z = (int)(position[2] / 4000);
            return (x) + ";" + (z);//y-Position is not important!
        }

        public void setVobPosition(float[] oldPos, float[] newPos, Vob vob)
        {
            String oldKey = getKeyByPosition(oldPos);
            String newKey = getKeyByPosition(newPos);

            if (oldKey == newKey)
                return;


            if (oldKey != null && VobPositionList.ContainsKey(oldKey) && VobPositionList[oldKey].Contains(vob))//Needs to delete!
            {
                VobPositionList[oldKey].Remove(vob);
                if (VobPositionList[oldKey].Count == 0)
                    VobPositionList.Remove(oldKey);
                if (vob is Item)
                {
                    ItemPositionList[oldKey].Remove((Item)vob);
                    if (ItemPositionList[oldKey].Count == 0)
                        ItemPositionList.Remove(oldKey);
                }
                else if (vob is NPCProto)
                {
                    NPCPositionList[oldKey].Remove((NPCProto)vob);
                    if (NPCPositionList[oldKey].Count == 0)
                        NPCPositionList.Remove(oldKey);

                    if (vob is Player)
                    {
                        PlayerPositionList[oldKey].Remove((Player)vob);
                        if (PlayerPositionList[oldKey].Count == 0)
                            PlayerPositionList.Remove(oldKey);
                    }
                }
                else if (vob is Mobs.MobInter)
                {
                    MobInterPositionList[oldKey].Remove((Mobs.MobInter)vob);
                    if (MobInterPositionList[oldKey].Count == 0)
                        MobInterPositionList.Remove(oldKey);
                }
            }

            if (newKey != null)
            {
                if (!VobPositionList.ContainsKey(newKey))
                    VobPositionList.Add(newKey, new List<Vob>());
                VobPositionList[newKey].Add(vob);

                if (vob is Item)
                {
                    if (!ItemPositionList.ContainsKey(newKey))
                        ItemPositionList.Add(newKey, new List<Item>());
                    ItemPositionList[newKey].Add((Item)vob);

                }
                else if (vob is NPCProto)
                {
                    if (!NPCPositionList.ContainsKey(newKey))
                        NPCPositionList.Add(newKey, new List<NPCProto>());
                    NPCPositionList[newKey].Add((NPCProto)vob);

                    if (vob is Player)
                    {
                        if (!PlayerPositionList.ContainsKey(newKey))
                            PlayerPositionList.Add(newKey, new List<Player>());
                        PlayerPositionList[newKey].Add((Player)vob);
                    }
                }
                else if (vob is Mobs.MobInter)
                {
                    if (!MobInterPositionList.ContainsKey(newKey))
                        MobInterPositionList.Add(newKey, new List<Mobs.MobInter>());
                    MobInterPositionList[newKey].Add((Mobs.MobInter)vob);
                }
            }



        }


        public GUC.Server.Scripting.Objects.World ScriptingWorld
        {
            get
            {
                if (scriptingWorld == null)
                    scriptingWorld = new GUC.Server.Scripting.Objects.World(this);
                return scriptingWorld;
            }
        }

        public virtual void Write(BitStream stream)
        {
            stream.Write(Map);

            stream.Write(itemList.Count);
            foreach (Item item in itemList)
            {
                stream.Write(item.ID);
                stream.Write(item.Position);
                stream.Write(item.Direction);
            }

            stream.Write(this.VobList.Count);
            foreach (Vob vob in VobList)
            {
                stream.Write(vob.ID);
                stream.Write(vob.Position);
                stream.Write(vob.Direction);
            }

            stream.Write(npcList.Count);
            foreach (NPCProto proto in npcList)
            {
                stream.Write(proto.ID);
                stream.Write(proto.Position);
                stream.Write(proto.Direction);
            }
        }

        
        
    }
}
