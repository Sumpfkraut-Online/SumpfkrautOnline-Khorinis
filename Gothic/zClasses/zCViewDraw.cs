using System;
using System.Collections.Generic;
using System.Text;
using WinApi;
using Gothic.zTypes;

namespace Gothic.zClasses
{
    public class zCViewDraw : zClass
    {

        public enum Offsets
        {
            
        }

        public enum FuncOffsets
        {
            DrawChildren = 0x00690300
        }

        public enum HookSizes
        {
            DrawChildren = 6
        }

        public zCViewDraw()
        {
            
        }

        public zCViewDraw(Process process, int address)
            : base(process, address)
        {

        }



        public static zCViewDraw GetScreen(Process process)
        {
            return new zCViewDraw(process, process.ReadInt(0x00AAD298));
        }
    }
}
