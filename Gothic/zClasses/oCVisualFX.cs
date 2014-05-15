using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;
using Gothic.zTypes;

namespace Gothic.zClasses
{
    public class oCVisualFX : zCObject
    {
        public enum Offsets
        {
            
        }

        public enum FuncOffsets
        {
            CreateAndPlay = 0x0048E760
        }

        public enum HookSize
        {
            
        }

        public oCVisualFX(Process process, int address)
            : base(process, address)
        {
        }

        public oCVisualFX() { }

        public override uint ValueLength()
        {
            return 4;
        }



        public static oCVisualFX CreateAndPlay(Process Process, zString effect, zCVob vob, zCVob targetVob, int a, float b, int c, int d)
        {
            return Process.CDECLCALL<oCVisualFX>((uint)FuncOffsets.CreateAndPlay, new CallValue[] { effect, vob, targetVob, (IntArg)a, (FloatArg)b, (IntArg)c, (IntArg)d });
        }

    }
}
