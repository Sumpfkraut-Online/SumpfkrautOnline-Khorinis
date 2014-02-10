using System;
using System.Collections.Generic;
using System.Text;
using WinApi;

namespace Gothic.zClasses
{
    public class zCModelAniActive : zClass
    {
        #region OffsetList
        public enum Offsets : uint
        {
            ModelAni = 0
        }
        public enum FuncOffsets : uint
        {
            SetActFrame = 0x00576CF0
        }

        public enum HookSize : uint
        {
            SetActFrame = 6
        }
        #endregion

        #region Standard
        public zCModelAniActive(Process process, int address) : base (process, address)
        {
            
        }

        public zCModelAniActive()
        {

        }

        public zCModelAni ModelAni
        {
            get { return new zCModelAni(Process, Process.ReadInt((int)Address + (int)Offsets.ModelAni)); }
        }

        public void SetActFrame(float value)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.SetActFrame, new CallValue[] { new FloatArg(value) });
        }

        public override uint ValueLength()
        {
            return 4;
        }


        #endregion
    }
}
