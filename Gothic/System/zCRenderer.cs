using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;
using Gothic.Types;

namespace Gothic.System
{
    public static class zCRenderer
    {
        public const int zrenderer = 0x982F08;

        public static int GetRendererAddress()
        {
            return Process.ReadInt(zrenderer);
        }

        public static void Vid_Clear(zColor color, int arg)
        {
            Process.THISCALL<NullReturnCall>(GetRendererAddress(), 0x00657E20, color, new IntArg(arg));
        }

        public static void BeginFrame()
        {
            Process.THISCALL<NullReturnCall>(GetRendererAddress(), 0x0064DD20);
        }

        public static void EndFrame()
        {
            Process.THISCALL<NullReturnCall>(GetRendererAddress(), 0x0064DF20);
        }

        public static void Vid_Blit(int arg1, int tagRect1, int tagRect2)
        {
            Process.THISCALL<NullReturnCall>(GetRendererAddress(), 0x00657670, new IntArg(arg1), new IntArg(tagRect1), new IntArg(tagRect2));
        }

        public static void SetFogRange(float near, float far, int mode)
        {
            Process.THISCALL<NullReturnCall>(GetRendererAddress(), 0x6521E0, new FloatArg(near), new FloatArg(far), new IntArg(mode));
        }
    }
}
