using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects.Character;
using GUC.WorldObjects;
using GUC.Types;
using RakNet;
using GUC.Enumeration;

namespace GUC.Server.Scripting.Objects
{
    public class World
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

        public List<Item> getNearItems(Vec3f position, int distance)
        {
            List<Item> itemList = new List<Item>();

            throw new NotImplementedException("this function was not implemented yet!");

            return itemList;
        }

        public List<NPCProto> getNearNPC(Vec3f position, int distance)
        {
            throw new NotImplementedException("this function was not implemented yet!");
        }

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

        public Item addItem(ItemInstance instance, int amount, Vec3f position)
        {
            return this.addItem(instance, amount, position, new Vec3f(0.0f, 0.0f, 1.0f));
        }
        public Item addItem(ItemInstance instance, int amount, Vec3f position, Vec3f direction)
        {
            Item rITem = new Item(instance, amount);
            rITem.Spawn(this.world.Map, position, direction);




            return rITem;
        }


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

        public static void setTimeFast(int day, byte hour, byte minute)
        {
            iSetTime(day, hour, minute, true);
        }

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


        public static void getTime(out int day, out int hour, out int minute)
        {
            day = sWorld.Day;
            hour = sWorld.Hour;
            minute = sWorld.Minute;
        }
    }
}
