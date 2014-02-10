using System;
using System.Collections.Generic;
using System.Text;
using WinApi;

namespace Gothic.zClasses
{
    public class oCInformationManager : zClass
    {
        #region OffsetLists
        public enum Offsets : uint
        {
            NPC = 32,
            Player =  36

        }


        public enum FuncOffsets : uint
        {
            CameraStart = 0x006613A0//Achtung fastcall
        }

        public enum HookSize : uint
        {
            CameraStart = 7
        }
        #endregion

        #region Standard
        public oCInformationManager(Process process, int address) : base (process, address)
        {
            
        }

        public oCInformationManager()
        {

        }

        public override uint ValueLength()
        {
            return 4;
        }
        #endregion

        #region statics

        public static oCInformationManager GetInformationManager(Process process)
        {
            return process.CDECLCALL<oCInformationManager>(0x0065F790, new CallValue[]{});
        }

        
        #endregion

        #region Fields
        public oCNpc NPC
        {
            get { return new oCNpc(Process, Process.ReadInt(Address + (int)Offsets.NPC)); }
        }
        #endregion


        #region methods
        public void CameraStart(Process process)
        {
            process.FASTCALL<NullReturnCall>((uint)Address, 0, (uint)FuncOffsets.CameraStart, null);
        }

        public int HasFinished(Process process)
        {
            return process.FASTCALL<IntArg>((uint)Address, 0, 0x006609D0, new CallValue[] { }).Address;
        }

        public int WaitingForEnd(Process process)
        {
            return process.FASTCALL<IntArg>((uint)Address, 0, 0x006609E0, new CallValue[] { }).Address;
        }

        
        #endregion
    }
}
