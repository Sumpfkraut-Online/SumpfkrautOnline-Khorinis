using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripting;
using GUC.WorldObjects;
using GUC.Enumeration;
using RakNet;

namespace GUC.Network.Messages
{
    static class WorldMessage
    {
        #region World Loading & Joining

        public static void ReadLoadWorldMessage(PacketReader stream)
        {
            var world = ScriptManager.Interface.CreateWorld();
            world.ID = 0;
            world.ReadStream(stream);
            bool startClock = stream.ReadBit();
            world.Create();
            World.current = world;
            world.ScriptObject.Load();
            if (startClock)
            {
                world.Clock.ScriptObject.Start();
            }
            world.SkyCtrl.ScriptObject.SetRainTime(world.SkyCtrl.TargetTime, world.SkyCtrl.TargetWeight);
            world.SkyCtrl.ScriptObject.SetWeatherType(world.SkyCtrl.WeatherType);

            var hero = Gothic.Objects.oCNpc.GetPlayer();
            hero.Disable();
            Gothic.oCGame.GetWorld().RemoveVob(hero);

            SendConfirmation();
        }

        static void SendConfirmation()
        {
            PacketWriter stream = GameClient.SetupStream(NetworkIDs.LoadWorldMessage);
            GameClient.Send(stream, PacketPriority.IMMEDIATE_PRIORITY, PacketReliability.RELIABLE);
        }

        public static void ReadJoinWorldMessage(PacketReader stream)
        {
            for (int i = stream.ReadUShort(); i > 0; i--)
            {
                ReadVobSpawnMessage(stream);
            }
        }

        public static void ReadLeaveWorldMessage(PacketReader stream)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Spawns

        public static void ReadCellMessage(PacketReader stream)
        {
            // remove vobs
            for (int i = stream.ReadUShort(); i > 0; i--)
            {
                ReadVobDespawnMessage(stream);
            }

            // add vobs
            for (int i = stream.ReadUShort(); i > 0; i--)
            {
                ReadVobSpawnMessage(stream);
            }
        }

        public static void ReadVobSpawnMessage(PacketReader stream)
        {
            BaseVob vob = ScriptManager.Interface.CreateVob((VobTypes)stream.ReadByte());
            vob.ReadStream(stream);
            vob.ScriptObject.Spawn(World.current);
        }

        public static void ReadVobDespawnMessage(PacketReader stream)
        {
            BaseVob vob;
            if (World.current.TryGetVob(stream.ReadUShort(), out vob))
            {
                vob.ScriptObject.Despawn();
            }
        }

        #endregion

        #region WorldClock

        public static void ReadTimeMessage(PacketReader stream)
        {
            var clock = World.Current.Clock;
            clock.ReadStream(stream);
            clock.ScriptObject.SetTime(clock.Time, clock.Rate);
        }

        public static void ReadTimeStartMessage(PacketReader stream)
        {
            if (stream.ReadBit())
            {
                World.Current.Clock.ScriptObject.Start();
            }
            else
            {
                World.Current.Clock.ScriptObject.Stop();
            }
        }

        #endregion

        #region Weather

        public static void ReadWeatherMessage(PacketReader stream)
        {
            var skyCtrl = World.current.SkyCtrl;
            skyCtrl.ReadSetRainTime(stream);
            skyCtrl.ScriptObject.SetRainTime(skyCtrl.TargetTime, skyCtrl.TargetWeight);
        }

        public static void ReadWeatherTypeMessage(PacketReader stream)
        {
            var skyCtrl = World.current.SkyCtrl;
            skyCtrl.ReadSetWeatherType(stream);
            skyCtrl.ScriptObject.SetWeatherType(skyCtrl.WeatherType);
        }

        #endregion
    }
}
