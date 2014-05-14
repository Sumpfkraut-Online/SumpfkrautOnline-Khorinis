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



        public Dictionary<uint, List<Vob>> VobPositionList = new Dictionary<uint, List<Vob>>();
        public Dictionary<uint, List<Item>> ItemPositionList = new Dictionary<uint, List<Item>>();
        public Dictionary<uint, List<NPCProto>> NPCPositionList = new Dictionary<uint, List<NPCProto>>();
        public Dictionary<uint, List<Mobs.MobInter>> MobInterPositionList = new Dictionary<uint, List<Mobs.MobInter>>();
        public Dictionary<uint, List<Player>> PlayerPositionList = new Dictionary<uint, List<Player>>();
        

        public static uint[] getImportantKeysByPosition(float[] position, float distance)
        {
            if (position == null)
                return null;

            
            int x = 0;
            int z = 0;

            if (position[0] > 0)
                x = (int)position[0] >> 9;
            else
                x = ((int)position[0] >> 9) + 1;

            if (position[2] > 0)
                z = (int)position[2] >> 9;
            else
                z = ((int)position[2] >> 9) + 1;

            

            int c = 0;
            int count = (int)Math.Round(distance / 512, MidpointRounding.AwayFromZero);
            int halfC = (int)Math.Round(count / 2.0, MidpointRounding.AwayFromZero);
            uint[] strings = new uint[(int)Math.Pow(halfC + halfC+1, 2)];

            for (int a = -1 * halfC; a <= halfC; a++)
            {
                for (int b = -1 * halfC; b <= halfC; b++)
                {
                    strings[c++] = (uint)(((short)(x + a)) + (((short)(z + b)) << 16));
                }
            }


            return strings;
        }

        public static UInt32 getKeyByPosition(float[] position)
        {
            if (position == null)
                return 0;
            int x = 0;
            int z = 0;

            if (position[0] > 0)
                x = (int)position[0] >> 9;
            else
                x = ((int)position[0] >> 9) + 1;

            if (position[2] > 0)
                z = (int)position[2] >> 9;
            else
                z = ((int)position[2] >> 9) + 1;
            
            return (uint)( ((short)x) + (((short)z) << 16) );//y-Position is not important!
        }

        public void setVobPosition(float[] oldPos, float[] newPos, Vob vob)
        {
            uint oldKey = getKeyByPosition(oldPos);
            uint newKey = getKeyByPosition(newPos);

            if (oldKey == newKey && newPos != null)
                return;


            if (oldPos != null && VobPositionList.ContainsKey(oldKey) && VobPositionList[oldKey].Contains(vob))//Needs to delete!
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

            if (newPos != null)
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
