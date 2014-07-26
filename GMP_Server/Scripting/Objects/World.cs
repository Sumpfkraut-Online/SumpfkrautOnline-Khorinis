using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects.Character;
using GUC.WorldObjects;
using GUC.Types;
using RakNet;
using GUC.Enumeration;
using System.Collections;

namespace GUC.Server.Scripting.Objects
{
    public class World : IEnumerable
    {
        public enum WeatherType
        {
            Snow = 0,
            Rain = 1,
            Undefined = 2
        }
        internal WorldObjects.World world;


        internal World(WorldObjects.World world)
        {
            this.world = world;
        }

        public static World getWorld(String name)
        {
            return sWorld.getWorld(name).ScriptingWorld;
        }


        public String Name { get { return this.world.Map; } }
        public override string ToString()
        {
            return Name;
        }
        /// <summary>
        /// Returns a list of all near items in the world
        /// </summary>
        /// <param name="position"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        public List<Item> getNearItems(Vec3f position, int distance)
        {
            List<Item> itemList = new List<Item>();

            uint[] keys = WorldObjects.World.getImportantKeysByPosition(position.Data, distance);

            foreach (uint key in keys)
            {
                if (!sWorld.getWorld(this.world.Map).ItemPositionList.ContainsKey(key))
                    continue;

                List<WorldObjects.Item> mobs = sWorld.getWorld(this.world.Map).ItemPositionList[key];
                foreach (WorldObjects.Item m in mobs)
                {
                    float mD = (m.Position - position).Length;

                    if (mD <= distance)
                    {
                        itemList.Add(m.ScriptingProto);
                    }
                }
            }

            return itemList;
        }

        /// <summary>
        /// this function is not implemented yet
        /// </summary>
        /// <param name="position"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        public List<NPCProto> getNearNPC(Vec3f position, int distance)
        {
            List<NPCProto> playerList = new List<NPCProto>();

            uint[] keys = WorldObjects.World.getImportantKeysByPosition(position.Data, distance);

            foreach (uint key in keys)
            {
                if (!sWorld.getWorld(this.world.Map).NPCPositionList.ContainsKey(key))
                    continue;

                List<WorldObjects.Character.NPCProto> mobs = sWorld.getWorld(this.world.Map).NPCPositionList[key];
                foreach (WorldObjects.Character.NPCProto m in mobs)
                {
                    if (m.ScriptingNPC is Player && !((Player)m.ScriptingVob).IsSpawned())
                        continue;
                    float mD = (m.Position - position).Length;

                    if (mD <= distance)
                    {
                        playerList.Add((NPCProto)m.ScriptingNPC);
                    }
                }
            }

            return playerList;
        }

        /// <summary>
        /// Returns an array of all npcs and player.
        /// </summary>
        /// <returns></returns>
        public NPCProto[] getNPCList()
        {
            NPCProto[] arrayList = new NPCProto[world.NPCList.Count];
            WorldObjects.Character.NPCProto[] protoList = world.NPCList.ToArray();
            for (int i = 0; i < arrayList.Length; i++)
            {
                arrayList[i] = protoList[i].ScriptingNPC;
            }
            return arrayList;
        }


        public IEnumerator GetEnumerator()
        {
            foreach (GUC.WorldObjects.Vob item in world.VobList)
            {
                yield return item.ScriptingVob;
            }
        }
        public System.Collections.IEnumerable VobIterator()
        {
            return VobIterator(0, VobCount);
        }

        public System.Collections.IEnumerable VobIterator(int start, int end)
        {
            for (int i = start; i < end; i++)
            {
                WorldObjects.Vob vob = world.VobList[i];
                yield return vob.ScriptingVob;
            }
        }

        public System.Collections.IEnumerable NPCIterator()
        {
            return NPCIterator(0, NPCCount);
        }
        public System.Collections.IEnumerable NPCIterator(int start, int end)
        {
            for (int i = start; i < end; i++)
            {
                WorldObjects.Character.NPCProto vob = world.NPCList[i];
                yield return vob.ScriptingNPC;
            }
        }

        public System.Collections.IEnumerable ItemIterator()
        {
            return ItemIterator(0, ItemCount);
        }
        public System.Collections.IEnumerable ItemIterator(int start, int end)
        {
            for (int i = start; i < end; i++)
            {
                WorldObjects.Item vob = world.ItemList[i];
                yield return vob.ScriptingProto;
            }
        }

        public int ItemCount { get { return world.ItemList.Count; } }
        public int VobCount { get { return world.VobList.Count; } }
        public int NPCCount { get { return world.NPCList.Count; } }

        public Vob this[int i]
        {
            get { return world.VobList[i].ScriptingVob; }
        }





        /// <summary>
        /// Adds an Item to the world
        /// </summary>
        /// <param name="instance">The item instance whick will be spawned</param>
        /// <param name="amount">The amount of the item to spawn at that location</param>
        /// <param name="position">The position where you want to spawn the item</param>
        /// <returns>returns the generated item</returns>
        public Item addItem(ItemInstance instance, int amount, Vec3f position)
        {
            return this.addItem(instance, amount, position, new Vec3f(0.0f, 0.0f, 1.0f));
        }

        /// <summary>
        /// Adds an Item to the world
        /// </summary>
        /// <param name="instance">The item instance whick will be spawned</param>
        /// <param name="amount">The amount of the item to spawn at that location</param>
        /// <param name="position">The position where you want to spawn the item</param>
        /// <param name="direction">The direction of the item</param>
        /// <returns>returns the generated item</returns>
        public Item addItem(ItemInstance instance, int amount, Vec3f position, Vec3f direction)
        {
            Item rITem = new Item(instance, amount);
            rITem.Spawn(this.world.Map, position, direction);
            return rITem;
        }


        /// <summary>
        /// Sets the rain time. 12:00 can not lie between start and endtime
        /// 12:00-11:59 should work.
        /// 
        /// This function is static and all worlds will have the weather.
        /// </summary>
        /// <param name="wt">The weathertype, define if you want it to rain or snow. If you want to use the default parameters, use Undefined</param>
        /// <param name="startHour">the start hour from 0 - 23</param>
        /// <param name="startMinute">the start minute from 0 - 59</param>
        /// <param name="endHour">the end Hour from 0 - 23</param>
        /// <param name="endMinute">the end minute from 0 - 59</param>
        public static void setRainTime(WeatherType wt, byte startHour, byte startMinute, byte endHour, byte endMinute)
        {
            sWorld.WeatherType = (byte)wt;
            sWorld.StartRainHour = startHour;
            sWorld.StartRainMinute = startMinute;
            sWorld.EndRainHour = endHour;
            sWorld.EndRainMinute = endMinute;


            BitStream stream = Program.server.sendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkIDS.RainMessage);
            stream.Write(sWorld.WeatherType);
            stream.Write(sWorld.StartRainHour);
            stream.Write(sWorld.StartRainMinute);
            stream.Write(sWorld.EndRainHour);
            stream.Write(sWorld.EndRainMinute);
            
            Program.server.server.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }

        /// <summary>
        /// Sets the actual time.
        /// It will be send insecure, so it can be that the client does not get the message.
        /// 
        /// This function is static and all worlds will have the same time.
        /// </summary>
        /// <param name="day">The new day</param>
        /// <param name="hour">The new hour</param>
        /// <param name="minute">The new minute</param>
        public static void setTimeFast(int day, byte hour, byte minute)
        {
            iSetTime(day, hour, minute, true);
        }

        /// <summary>
        /// Sets the actual time.
        /// It will be send secure, The clients get definetly the message, but it can be slower.
        /// 
        /// This function is static and all worlds will have the same time.
        /// </summary>
        /// <param name="day">The new day</param>
        /// <param name="hour">The new hour</param>
        /// <param name="minute">The new minute</param>
        public static void setTime(int day, byte hour, byte minute)
        {
            iSetTime(day, hour, minute, false);
        }

        private static void iSetTime( int day, byte hour, byte minute, bool fast)
        {
            sWorld.Day = day;
            sWorld.Hour = hour;
            sWorld.Minute = minute;


            BitStream stream = Program.server.sendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkIDS.TimerMessage);
            stream.Write(day);
            stream.Write(hour);
            stream.Write(minute);

            if(fast)
                Program.server.server.Send(stream, PacketPriority.MEDIUM_PRIORITY, PacketReliability.UNRELIABLE_SEQUENCED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
            else
                Program.server.server.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }


        /// <summary>
        /// Returns the time.
        /// 
        /// This function is static and all worlds will have the same time.
        /// </summary>
        /// <param name="day"></param>
        /// <param name="hour"></param>
        /// <param name="minute"></param>
        public static void getTime(out int day, out int hour, out int minute)
        {
            day = sWorld.Day;
            hour = sWorld.Hour;
            minute = sWorld.Minute;
        }
    }
}
