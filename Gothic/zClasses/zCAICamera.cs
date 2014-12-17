using System;
using System.Collections.Generic;
using System.Text;
using WinApi;
using Gothic.zTypes;

namespace Gothic.zClasses
{
    public class zCAICamera : zClass
    {
        #region OffsetLists
        public enum Offsets : uint
        {

        }


        public enum FuncOffsets : uint
        {
            StartDialogCam = 0x004B20F0,
            SetByScript = 0x004A26E0,
            AI_LookingCam = 0x004A3690,
            AI_FirstPerson = 0x004A40E0,
            AI_Normal = 0x004A4370,
            CheckKeys = 0x004A45C0,
            SetMode = 0x004A09C0,
            SetTarget = 0x004A1120
        }

        public enum HookSize : uint
        {
            StartDialogCam = 7,
            SetByScript =6,
            AI_LookingCam =7
        }
        #endregion

        #region Standard
        public zCAICamera(Process process, int address) : base (process, address)
        {
            
        }

        public zCAICamera()
        {

        }

        public override uint ValueLength()
        {
            return 4;
        }
        #endregion

        #region statics
        public static zCAICamera Current(Process process)
        {
            return new zCAICamera(process, process.ReadInt(0x008CEAB8));
        }

        public static zCVob getSpeaker(Process process)
        {
            return new zCVob(process, process.ReadInt(0x008CEE4C));
        }

        public static void setSpeaker(Process process, zCVob vob)
        {
            process.Write(vob.Address, 0x008CEE4C);
        }

        public static zCVob getListener(Process process)
        {
            return new zCVob(process, process.ReadInt(0x008CEE50));
        }

        public static void setListener(Process process, zCVob vob)
        {
            process.Write(vob.Address, 0x008CEE50);
        }

        public static int getPlaying(Process process)
        {
            return process.ReadInt(0x008D11B0);
        }

        public static void setPlaying(Process process, int x)
        {
            process.Write(x, 0x008D11B0);
        }

        public static zString getFirstPersonString(Process process)
        {
            return new zString(process, 0x008CEA18);
        }
        #endregion

        #region Fields
        
        #endregion


        #region methods
        public void AI_Normal()
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.AI_Normal, new CallValue[] { });
        }
        public void AI_LookingCam()
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.AI_LookingCam, new CallValue[] {  });
        }

        public void AI_FirstPerson()
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.AI_FirstPerson, new CallValue[] { });
        }

        public void StartDialogCam(int x)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.StartDialogCam, new CallValue[] { new IntArg(x) });
        }

        /// <summary>
        /// cammodnormal
        /// cammodinventory
        /// cammodmobbedhigh_front
        /// cammoddialog
        /// cammodmobdefault
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public int SetByScript(zString str)
        {
            return Process.THISCALL<IntArg>((uint)Address, (uint)FuncOffsets.SetByScript, new CallValue[] { str }).Address;
        }

        public void SetMode(zString str)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.SetMode, new CallValue[] { str, new IntArg(0) });
        }

        public void SetTarget(zCVob vob)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.SetTarget, new CallValue[] { vob });
        }
        #endregion
    }
}
