using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;
using Gothic.Types;

namespace Gothic
{
    public static class CGameManager
    {
        public const int gameMan = 0x008C2958;

        public abstract class VarOffsets
        {
            public const int gameSession = 12,
            backLoop = 16, //oCGame
            menu = 128,
            menu_ext = 132,
            exitGame = 20;
        }

        public abstract class FuncAddresses
        {
            public const int ExitGame = 0x00425780,
            Done = 0x004254E0,
            ApplySomeSettings = 0x4276B0,
            PlayVideo = 0x0042B940,
            GameSessionDone = 0x426F70;
        }

        /*public enum HookSize : uint
        {
            ExitGame = 5,
            Done = 7
        }*/

        public static int ExitGameVar
        {
            get { return Process.ReadInt(Process.ReadInt(gameMan) + VarOffsets.exitGame); }
            set { Process.Write(Process.ReadInt(gameMan) + VarOffsets.exitGame, value); }
        }

        public static void ExitGame()
        {
            Process.THISCALL<NullReturnCall>(Process.ReadInt(gameMan), FuncAddresses.ExitGame);
        }

        public static void Done()
        {
            Process.THISCALL<NullReturnCall>(Process.ReadInt(gameMan), FuncAddresses.Done);
        }

        public static int PlayVideo(zString video)
        {
            return Process.THISCALL<IntArg>(Process.ReadInt(gameMan), FuncAddresses.PlayVideo, (IntArg)video.VTBL, (IntArg)video.ALLOCATER, (IntArg)video.PTR, (IntArg)video.Length, (IntArg)video.Res);
        }

        public static int PlayVideo(String video)
        {
            int ret;
            using (zString str = zString.Create(video))
                ret = PlayVideo(str);

            return ret;
        }

        public static void ApplySomeSettings()
        {
            Process.THISCALL<NullReturnCall>(Process.ReadInt(gameMan), FuncAddresses.ApplySomeSettings);
        }
    }
}
