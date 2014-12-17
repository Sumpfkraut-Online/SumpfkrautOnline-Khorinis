using System;
using System.Collections.Generic;
using System.Text;
using WinApi;
using Gothic.zTypes;

namespace Gothic.zClasses
{
    public class CGameManager : zClass
    {
        #region OffsetLists
        public enum Offsets
        {
            gameSession = 12,
            backLoop = 16, //oCGame
            menu = 128,
            menu_ext = 132,
            exitGame = 20
        }
        public enum FuncOffsets : uint
        {
            ExitGame = 0x00425780,
            Done = 0x004254E0,
            PlayVideo = 0x0042B940
        }

        public enum HookSize : uint
        {
            ExitGame = 5,
            Done = 7
        }

        #endregion

        public CGameManager()
        {

        }

        public CGameManager(Process process, int address)
            : base(process, address)
        {

        }

        public  static CGameManager GameManager(Process process)
        {
            return  new CGameManager( process, process.ReadInt(0x008C2958));
        }

        public zCMenu Menu
        {
            get
            {
                return new zCMenu(Process, Process.ReadInt(Address + (int)Offsets.menu));
            }
        }

        public int ExitGameVar
        {
            get
            {
                return Process.ReadInt(Address + (int)Offsets.exitGame);
            }
            set
            {
                Process.Write(value, Address + (int)Offsets.exitGame);
            }
        }

        public void ExitGame()
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (int)FuncOffsets.ExitGame, new CallValue[] { });
        }

        public void Done()
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (int)FuncOffsets.Done, new CallValue[] { });
        }

        public int PlayVideo(String video)
        {
            zString str = zString.Create(Process, video);
            int rVal = Process.THISCALL<IntArg>((uint)Address, (int)FuncOffsets.PlayVideo, new CallValue[] { (IntArg)str.VTBL, (IntArg)str.ALLOCATER, (IntArg)str.PTR, (IntArg)str.Length, (IntArg)str.Res });
            Process.Free(new IntPtr(str.Address), 20);

            return rVal;
        }

        public static void ExitGameFunc(Process Process)
        {
            Process.CDECLCALL<NullReturnCall>(0x00425F30, new CallValue[] { });
        }
    }
}
