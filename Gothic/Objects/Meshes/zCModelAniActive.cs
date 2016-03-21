using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;

namespace Gothic.Objects.Meshes
{
    public class zCModelAniActive : zClass
    {
        public abstract class VarOffsets
        {
            public const int ModelAni = 0;
        }
        public abstract class FuncAddresses
        {
            public const int SetActFrame = 0x00576CF0,
            SetProgressPercent = 0x00576CA0;
        }

        /* public enum HookSize : uint
         {
             SetActFrame = 6
         }*/

        public zCModelAniActive(int address) : base(address)
        {

        }

        public zCModelAniActive()
        {

        }

        public zCModelAni ModelAni
        {
            get { return new zCModelAni(Process.ReadInt(Address + VarOffsets.ModelAni)); }
        }

        public void SetActFrame(float value)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.SetActFrame, new FloatArg(value));
        }

        public void SetProgressPercent(float value)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.SetProgressPercent, new FloatArg(value));
        }
    }
}
