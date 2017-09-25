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
            public const int ModelAni = 0,
                ActFrame = 0x0C;
        }
        public abstract class FuncAddresses
        {
            public const int SetActFrame = 0x00576CF0,
            SetProgressPercent = 0x00576CA0,
            GetProgressPercent = 0x00576C60;
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

        public float ActFrame
        {
            get { return Process.ReadFloat(Address + VarOffsets.ActFrame); }
            set { Process.Write(Address + VarOffsets.ActFrame, value); }
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

        public float GetProgressPercent()
        {
            // does not work???
            //return Process.THISCALL<FloatArg>(Address, FuncAddresses.GetProgressPercent);

            var modelAni = ModelAni;
            return modelAni.IsReversed ? (1.0f - ActFrame / (float)(modelAni.NumFrames-1)) : ActFrame / (float)(modelAni.NumFrames-1);
        }
    }
}
