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

        public static int GetGameAddress()
        {
            return Process.ReadInt(ogame);
        }

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
            GetCameraVob = 0x5DE7B0,
            GetCameraAI = 0x5DE7A0,
            SetTime = 0x006C4DE0;
        }

        public static zCVob GetCameraVob()
        {
            return Process.THISCALL<zCVob>(GetGameAddress(), FuncAddresses.GetCameraVob);
        }

        public static oCAICamera GetCameraAI()
        {
            return Process.THISCALL<oCAICamera>(GetGameAddress(), FuncAddresses.GetCameraAI);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="useStandardTex"> Use the standard loading screen texture </param>
        /// <param name="screenName">loading_'screenName'.tga</param>
        public static void OpenLoadscreen(bool useStandardTex, zString screenName)
        {
            Process.THISCALL<NullReturnCall>(GetGameAddress(), FuncAddresses.OpenLoadscreen, new BoolArg(useStandardTex), screenName);
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
            Process.THISCALL<NullReturnCall>(GetGameAddress(), FuncAddresses.LoadWorld, new IntArg(savegame_slot_new == true ? -2 : 0), worldName);
        }

        public static void LoadWorld(bool savegame_slot_new, string worldName)
        {
            using (zString z = zString.Create(worldName))
                LoadWorld(savegame_slot_new, z);
        }

        public static void LoadGame(bool savegame_slot_new, zString worldName)
        {
            Process.THISCALL<NullReturnCall>(GetGameAddress(), FuncAddresses.LoadGame, new IntArg(savegame_slot_new == true ? -2 : 0), worldName);
        }

        public static void LoadGame(bool savegame_slot_new, string worldName)
        {
            using (zString z = zString.Create(worldName))
                LoadGame(savegame_slot_new, z);
        }

        public static void SetTime(int day, int hour, int minute)
        {
            Process.THISCALL<NullReturnCall>(GetGameAddress(), FuncAddresses.SetTime, new IntArg(day), new IntArg(hour), new IntArg(minute));
        }

        public static zCWorld GetWorld()
        {
            return new zCWorld(Process.ReadInt(GetGameAddress() + VarOffsets.world));
        }

        public static oCWorldTimer WorldTimer
        {
            get { return new oCWorldTimer(Process.ReadInt(GetGameAddress() + VarOffsets.WorldTimer)); }
        }
    }
}
