using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;
using Gothic.Types;
using Gothic.Objects;

namespace Gothic
{
    public static class oCGame
    {
        public const int ogame = 0xAB0884;

        public abstract class VarOffsets
        {
            public const int vtbl = 0,
            zCCSManager = 4,
            world = 8,
            camera = 12,
            aiCamera = 16,
            camVob = 20,
            viewPort = 24,
            WorldTimer = 0x114;
        }

        public abstract class FuncAddresses
        {
            public const int OpenLoadscreen = 0x6C2690,
            LoadWorld = 0x6C90B0,
            LoadGame = 0x6C65A0,
            SetTime = 0x006C4DE0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="useStandardTex"> Use the standard loading screen texture </param>
        /// <param name="screenName">loading_'screenName'.tga</param>
        public static void OpenLoadscreen(bool useStandardTex, zString screenName)
        {
            Process.THISCALL<NullReturnCall>(Process.ReadInt(ogame), FuncAddresses.OpenLoadscreen, new BoolArg(useStandardTex), screenName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="useStandardTex"> Use the standard loading screen texture </param>
        /// <param name="screenName">loading_'screenName'.tga</param>
        public static void OpenLoadscreen(bool useStandardTex, string screenName)
        {
            using (zString z = zString.Create(screenName))
                OpenLoadscreen(useStandardTex, z);
        }

        public static void LoadWorld(bool savegame_slot_new, zString worldName)
        {
            Process.THISCALL<NullReturnCall>(Process.ReadInt(ogame), FuncAddresses.LoadWorld, new IntArg(savegame_slot_new == true ? -2 : 0), worldName);
        }

        public static void LoadWorld(bool savegame_slot_new, string worldName)
        {
            using (zString z = zString.Create(worldName))
                LoadWorld(savegame_slot_new, z);
        }

        public static void LoadGame(bool savegame_slot_new, zString worldName)
        {
            Process.THISCALL<NullReturnCall>(Process.ReadInt(ogame), FuncAddresses.LoadGame, new IntArg(savegame_slot_new == true ? -2 : 0), worldName);
        }

        public static void LoadGame(bool savegame_slot_new, string worldName)
        {
            using (zString z = zString.Create(worldName))
                LoadGame(savegame_slot_new, z);
        }

        public static void SetTime(int day, int hour, int minute)
        {
            Process.THISCALL<NullReturnCall>(Process.ReadInt(ogame), FuncAddresses.SetTime, new IntArg(day), new IntArg(hour), new IntArg(minute));
        }

        public static zCWorld GetWorld()
        {
            return new zCWorld(Process.ReadInt(Process.ReadInt(ogame) + VarOffsets.world));
        }

        public static oCWorldTimer WorldTimer
        {
            get { return new oCWorldTimer(Process.ReadInt(Process.ReadInt(ogame) + VarOffsets.WorldTimer)); }
        }
    }
}
