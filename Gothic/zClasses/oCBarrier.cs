using System;
using System.Collections.Generic;
using System.Text;

namespace Gothic.zClasses
{
    public class oCBarrier : zClass
    {
        public enum FuncOffsets
        {
            Render = 0x006B9F30,
            Init = 0x006B9440
        }

        public enum HookSize
        {
            Render = 6,
            Init = 7
        }
    }
}
