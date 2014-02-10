using System;
using System.Collections.Generic;
using System.Text;
using WinApi;
using Gothic.zTypes;

namespace Gothic.zClasses
{
    public class oCGame : zCSession
    {
        #region OffsetLists
        public enum Offsets
        {
            Game_Text = 124,
            ArrayView = 48,//6!!!,
            Progressbar = 380,
            LoadScreen = 128,
            SaveScreen = 132,
            PauseScreen = 136,
            WorldTimer = 0x114//oder 244???
            
        }

        public enum FuncOffsets : uint
        {
            LoadGame = 0x006C65A0,
            LoadWorldStartup = 0x006C9C10,
            LoadWorld = 0x006C4EA0,
            Init = 0x006C1060,
            GetSpawnManager = 0x006C2D00,
            OpenLoadscreen = 0x006C2690,
            CloseLoadscreen = 0x006C2BD0,
            ChangeLevel = 0x006C7290,
            HandleEvent = 0x006FC170,//Nur für b m n usw. nicht für spielersteuerung
            SetTime = 0x006C4DE0,
            SetObjectRoutineTimeChange = 0x006CACB0
        }

        public enum HookSize : uint
        {
            LoadGame = 5,
            LoadWorldStartup = 7,
            Init = 7,
            GetSpawnManager = 6,
            OpenLoadscreen= 7,
            CloseLoadScreen = 9,
            ChangeLevel = 7
        }
        #endregion


        public oCGame(Process process, int address)
            : base(process, address)
        { }


        public zCWorld World { get { return new zCWorld(Process, Process.ReadInt(this.Address + 0x8)); } }

        public zCView GameText { get { return new zCView(Process, Process.ReadInt(this.Address + (int)Offsets.Game_Text)); } }

        public zCViewProgressBar Progressbar { 
            get { return new zCViewProgressBar(Process, Process.ReadInt(this.Address + (int)Offsets.Progressbar)); }
            set { Process.Write(value.Address, this.Address + (int)Offsets.Progressbar); }
        }

        public zCView LoadScreen { 
            get { return new zCView(Process, Process.ReadInt(this.Address + (int)Offsets.LoadScreen)); }
            set { Process.Write(value.Address, this.Address + (int)Offsets.LoadScreen); }
        }

        public zCView SaveScreen
        {
            get { return new zCView(Process, Process.ReadInt(this.Address + (int)Offsets.SaveScreen)); }
            set { Process.Write(value.Address, this.Address + (int)Offsets.SaveScreen); }
        }

        public zCView PauseScreen
        {
            get { return new zCView(Process, Process.ReadInt(this.Address + (int)Offsets.PauseScreen)); }
            set { Process.Write(value.Address, this.Address + (int)Offsets.PauseScreen); }
        }

        public oCWorldTimer WorldTimer
        {
            get { return new oCWorldTimer(Process, Process.ReadInt(this.Address + (int)Offsets.WorldTimer)); }
        }

        public zCView[] ArrayView
        {
            get 
            { 
                zCView[] viewList = new zCView[6];
                for (int i = 0; i < viewList.Length; i++)
                    viewList[i] = new zCView(Process, Process.ReadInt(Address + (int)Offsets.ArrayView + i * 4));
                return viewList;
            }

        }

        public void OpenLoadscreen(bool t, zString str)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.OpenLoadscreen, new CallValue[] { new BoolArg(t), str });
        }

        public void CloseLoadScreen()
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.CloseLoadscreen, new CallValue[] { });
        }

        public void ChangeLevel(zString t, zString str)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.ChangeLevel, new CallValue[] { t, str });
        }

        public void LoadGame(int ptr, zString str)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.LoadGame, new CallValue[] {new IntArg(ptr), str });
        }

        public void LoadWorldStartup(zString str)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.LoadWorldStartup, new CallValue[] { str });
        }

        public int LoadWorld(zString str, int mode)
        {
            return Process.THISCALL<CallValue>((uint)Address, (uint)FuncOffsets.LoadWorld, new CallValue[] { str, new IntArg(mode) }).Address;
        }

        public oCSpawnManager GetSpawnManager()
        {
            return Process.THISCALL<oCSpawnManager>((uint)Address, (uint)FuncOffsets.GetSpawnManager, new CallValue[] { });
        }

        public void SetTime(int day, int hour, int minute)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.SetTime, new CallValue[] { new IntArg(day), new IntArg(hour), new IntArg(minute) });
        }

        public void SetObjectRoutineTimeChange(int x, int y, int hour, int minute)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.SetObjectRoutineTimeChange, new CallValue[] {new IntArg(x), new IntArg(y), new IntArg(hour), new IntArg(minute) });
        }

        public void Init()
        {

            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.Init, new CallValue[] { });
        }

        public static oCGame Game(Process process)
        {
            return new oCGame(process, process.ReadInt(0xAB0884));
        }
    }
}
