using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;
using Gothic.Types;
using Gothic.Objects;
using Gothic.View;

namespace Gothic.Session
{
    public class oCGame : zCSession
    {
        public const int ogame = 0xAB0884;

        public oCGame()
        {
        }

        public oCGame(int address) : base(address)
        {
        }

        public static int GameAddress
        {
            get { return Process.ReadInt(ogame); }
        }

        public static oCGame GetGame()
        {
            return new oCGame(GameAddress);
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
            progressBar = 0x17C,
            worldTimer = 0x114;
        }

        public abstract class FuncAddresses
        {
            public const int OpenLoadscreen = 0x6C2690,
            CloseLoadscreen = 0x6C2BD0,
            LoadWorld = 0x6C90B0,
            LoadGame = 0x6C65A0,
            GetCameraVob = 0x5DE7B0,
            GetCameraAI = 0x5DE7A0,
            SetShowPlayerStatus = 0x6C2D90,
            ChangeLevel = 0x6C7290,
            EnterWorld = 0x6C96F0,
            ClearGameState = 0x6C5ED0,
            SetTime = 0x006C4DE0;
        }

        public zCVob GetCameraVob()
        {
            return Process.THISCALL<zCVob>(Address, FuncAddresses.GetCameraVob);
        }

        public oCAICamera GetCameraAI()
        {
            return Process.THISCALL<oCAICamera>(Address, FuncAddresses.GetCameraAI);
        }

        public void CloseLoadscreen()
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.CloseLoadscreen);
        }

        /// <param name="useStandardTex"> Use the standard loading screen texture </param>
        /// <param name="screenName">loading_'screenName'.tga</param>
        public void OpenLoadscreen(bool useStandardTex, zString screenName)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.OpenLoadscreen, (BoolArg)useStandardTex, (IntArg)screenName.VTBL, (IntArg)screenName.ALLOCATER, (IntArg)screenName.PTR, (IntArg)screenName.Length, (IntArg)screenName.Res);
        }
        
        /// <param name="useStandardTex"> Use the standard loading screen texture </param>
        /// <param name="screenName">loading_'screenName'.tga</param>
        public void OpenLoadscreen(bool useStandardTex, string screenName)
        {
            using (zString z = zString.Create(screenName))
                OpenLoadscreen(useStandardTex, z);
        }
        public void EnterWorld(oCNpc player = null, bool setPlayerPos = true, string startWP = "")
        {
            using (zString z = zString.Create(startWP))
                Process.THISCALL<NullReturnCall>(Address, FuncAddresses.EnterWorld, player != null ? player : new oCNpc(0), (BoolArg)setPlayerPos, z);
        }

        public void EnterWorld(oCNpc player, bool setPlayerPos, zString startWP)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.EnterWorld, player, (BoolArg)setPlayerPos, startWP);
        }

        public void LoadWorld(bool savegame_slot_new, zString worldName)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.LoadWorld, new IntArg(savegame_slot_new == true ? -2 : 0), worldName);
        }

        public void LoadWorld(bool savegame_slot_new, string worldName)
        {
            using (zString z = zString.Create(worldName))
                LoadWorld(savegame_slot_new, z);
        }

        public void ChangeLevel(string levelName, string startWP)
        {
            using (zString z = zString.Create(levelName))
            using (zString z2 = zString.Create(startWP))
                ChangeLevel(z, z2);
        }

        public void ChangeLevel(zString levelName, zString startWP)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.ChangeLevel, levelName, startWP);
        }

        public void LoadGame(bool savegame_slot_new, zString worldName)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.LoadGame, new IntArg(savegame_slot_new == true ? -2 : 0), worldName);
        }

        public void LoadGame(bool savegame_slot_new, string worldName)
        {
            using (zString z = zString.Create(worldName))
                LoadGame(savegame_slot_new, z);
        }

        public void SetTime(int day, int hour, int minute)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.SetTime, new IntArg(day), new IntArg(hour), new IntArg(minute));
        }

        public zCWorld GetWorld()
        {
            return new zCWorld(Process.ReadInt(Address + VarOffsets.world));
        }

        public oCWorldTimer WorldTimer
        {
            get { return new oCWorldTimer(Process.ReadInt(Address + VarOffsets.worldTimer)); }
        }

        public void SetShowPlayerStatus(bool show)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.SetShowPlayerStatus, (BoolArg)show);
        }

        public void ClearGameState()
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.ClearGameState);
        }
        
        public zCViewProgressBar ProgressBar
        {
            get { return new zCViewProgressBar(Process.ReadInt(Address + VarOffsets.progressBar)); }
        }
    }
}
