using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;
using Gothic.Types;

namespace Gothic
{
    public static class oCGame
    {
        public const int ogame = 0xAB0884;

        public abstract class FuncAddresses
        {
            public const int OpenLoadscreen = 0x6C2690;
            public const int LoadWorld = 0x6C90B0;
            public const int LoadGame = 0x6C65A0;
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
    }
}
