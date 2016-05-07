using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.Scripting;
using GUC.WorldObjects;
using GUC.Enumeration;
using RakNet;

namespace GUC.Client.Network.Messages
{
    static class WorldMessage
    {
        #region World Loading

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

        #endregion

        #region Spawns & Cells

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

        public static void ReadCellMessage(PacketReader stream)
        {
            for (int t = 0; t < (int)VobTypes.Maximum; t++)
            {
                int vobCount = stream.ReadUShort();
                for (int i = 0; i < vobCount; i++)
                {
                    BaseVob vob = ScriptManager.Interface.CreateVob((VobTypes)t);
                    vob.ReadStream(stream);
                    vob.ScriptObject.Spawn(World.current);
                }
            }
            int delCount = stream.ReadUShort();
            for (int i = 0; i < delCount; i++)
            {
                BaseVob vob;
                if (World.Current.TryGetVob(stream.ReadUShort(), out vob))
                {
                    vob.ScriptObject.Despawn();
                }
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
