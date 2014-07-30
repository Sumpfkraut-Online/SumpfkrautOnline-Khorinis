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


        /// <summary>
        /// Returns the world with the specified name.
        /// </summary>
        /// <param name="name">The name of the World 
        /// in example: "NewWorld/NewWorld.zen". This is case insensitive.</param>
        /// <returns></returns>
        public static World getWorld(String name)
        {
            return sWorld.getWorld(name).ScriptingWorld;
        }

        /// <summary>
        /// The name of the world.
        /// The name is always written with uppercase letters and the seperator is always \
        /// Example: NEWWORLD\NEWWORLD.ZEN
        /// </summary>
        public String Name { get { return this.world.Map; } }

        /// <summary>
        /// Returns the name of the world
        /// </summary>
        /// <returns></returns>
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
        /// Returns a list of all Near Npcs
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


        /// <summary>
        /// This enumerator is used, so you can iterate through the World to iterate trough all vobs.
        /// Example:
        /// foreach(Vob vob in world){}
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator()
        {
            foreach (GUC.WorldObjects.Vob item in world.VobList)
            {
                yield return item.ScriptingVob;
            }
        }
        /// <summary>
        /// This function does the same like GetEnumerator.
        /// </summary>
        /// <returns></returns>
        public System.Collections.IEnumerable VobIterator()
        {
            return VobIterator(0, VobCount);
        }

        /// <summary>
        /// With this function you can iterate trough a specified amount of vobs.
        /// Example:
        /// foreach(Vob vob in world.VobIterator(0, 125)){}
        /// </summary>
        /// <param name="start">The start index for iteration</param>
        /// <param name="end">The end index for iteration.</param>
        /// <returns></returns>
        public System.Collections.IEnumerable VobIterator(int start, int end)
        {
            for (int i = start; i < end; i++)
            {
                WorldObjects.Vob vob = world.VobList[i];
                yield return vob.ScriptingVob;
            }
        }

        /// <summary>
        /// Iterate through each object of NPCProto (Players and NPC).
        /// Example:
        /// foreach(NPCProto proto in world.NPCIterator()){}
        /// </summary>
        /// <returns></returns>
        public System.Collections.IEnumerable NPCIterator()
        {
            return NPCIterator(0, NPCCount);
        }

        /// <summary>
        /// Iterate through each object of NPCProto (Players and NPC) between the start and end Index.
        /// Example:
        /// foreach(NPCProto proto in world.NPCIterator(0, 10)){}
        /// </summary>
        /// <param name="start">The start index for iteration</param>
        /// <param name="end">The end index for iteration.</param>
        /// <returns></returns>
        public System.Collections.IEnumerable NPCIterator(int start, int end)
        {
            for (int i = start; i < end; i++)
            {
                WorldObjects.Character.NPCProto vob = world.NPCList[i];
                yield return vob.ScriptingNPC;
            }
        }

        /// <summary>
        /// Iterate through each object of the itemtype.
        /// Example:
        /// foreach(Item item in world.ItemIterator()){}
        /// </summary>
        /// <returns></returns>
        public System.Collections.IEnumerable ItemIterator()
        {
            return ItemIterator(0, ItemCount);
        }

        /// <summary>
        /// Iterate through each object of the item-type between the start and end Index.
        /// Example:
        /// foreach(Item item in world.ItemIterator(0, 10)){}
        /// </summary>
        /// <param name="start">The start index for iteration</param>
        /// <param name="end">The end index for iteration.</param>
        /// <returns></returns>
        public System.Collections.IEnumerable ItemIterator(int start, int end)
        {
            for (int i = start; i < end; i++)
            {
                WorldObjects.Item vob = world.ItemList[i];
                yield return vob.ScriptingProto;
            }
        }

        /// <summary>
        /// Returns the count of items spawned in the world.
        /// </summary>
        public int ItemCount { get { return world.ItemList.Count; } }

        /// <summary>
        /// Returns the count of vobs spawned in the world.
        /// Items and NPC are vobs too.
        /// </summary>
        public int VobCount { get { return world.VobList.Count; } }

        /// <summary>
        /// Returns the count of npcprotos (NPC and Players) spawned in the world.
        /// </summary>
        public int NPCCount { get { return world.NPCList.Count; } }

        /// <summary>
        /// You can iterate trough all Vobs with an index too.
        /// Use world[0] to get the first Vob.
        /// </summary>
        /// <param name="i">The vob-index.</param>
        /// <returns></returns>
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
