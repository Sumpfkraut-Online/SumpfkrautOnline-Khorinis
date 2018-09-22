using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApiNew;
using Gothic.Types;

namespace Gothic.System
{
    public static class zCRenderer
    {
        public const int zrenderer = 0x982F08;
        
        public static int PlayerLightInt
        {
            get { return Process.ReadInt(0x89EBB4); }
            set { Process.WriteInt(0x89EBB4, value); }
        }

        public static int GetRendererAddress()
        {
            return Process.ReadInt(zrenderer);
        }

        public static void Vid_Clear(zColor color, int arg)
        {
            WinApi.Process.THISCALL<WinApi.NullReturnCall>(GetRendererAddress(), 0x00657E20, color, new WinApi.IntArg(arg));
        }

        public static void BeginFrame()
        {
            WinApi.Process.THISCALL<WinApi.NullReturnCall>(GetRendererAddress(), 0x0064DD20);
        }

        public static void EndFrame()
        {
            WinApi.Process.THISCALL<WinApi.NullReturnCall>(GetRendererAddress(), 0x0064DF20);
        }

        public static void Vid_Blit(int arg1, int tagRect1, int tagRect2)
        {
            WinApi.Process.THISCALL<WinApi.NullReturnCall>(GetRendererAddress(), 0x00657670, new WinApi.IntArg(arg1), new WinApi.IntArg(tagRect1), new WinApi.IntArg(tagRect2));
        }

        public static void SetFogRange(float near, float far, int mode)
        {
            WinApi.Process.THISCALL<WinApi.NullReturnCall>(GetRendererAddress(), 0x6521E0, new WinApi.FloatArg(near), new WinApi.FloatArg(far), new WinApi.IntArg(mode));
        }

        public static void FlushPolys()
        {
            WinApi.Process.THISCALL<WinApi.NullReturnCall>(GetRendererAddress(), 0x64DD10);
        }

        public static void SetZBufferWriteEnabled(bool enabled)
        {
            WinApi.Process.THISCALL<WinApi.NullReturnCall>(GetRendererAddress(), 0x6524E0, (WinApi.BoolArg)enabled);
        }
        
        public enum AlphaBlendFuncs
        {
            Default,
            None,
            Blend,
            Add,
            Sub,
            Mul,
            Mul2
        };

        public static void SetAlphaBlendFunc(AlphaBlendFuncs blend)
        {
            Process.WriteInt(GetRendererAddress() + 0x464, (int)blend);
            //Process.THISCALL<NullReturnCall>(GetRendererAddress(), 0x652610, (POINTER!!!)(IntArg)(int)blend);
        }
    }

}
