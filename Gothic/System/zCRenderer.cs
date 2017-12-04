using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;
using WinApi.Calls;
using Gothic.Types;

namespace Gothic.System
{
    public static class zCRenderer
    {
        public const int zrenderer = 0x982F08;
        
        public static int PlayerLightInt
        {
            get { return Process.ReadInt(0x89EBB4); }
            set { Process.Write(0x89EBB4, value); }
        }

        public static int GetRendererAddress()
        {
            return Process.ReadInt(zrenderer);
        }

        static readonly ThisCall<int, int> vidClear = new ThisCall<int, int>(0x00657E20);
        public static void Vid_Clear(zColor color, int arg)
        {
            vidClear.Call(GetRendererAddress(), color.Address, arg);
            //Process.THISCALL<NullReturnCall>(GetRendererAddress(), 0x00657E20, color, new IntArg(arg));
        }

        static readonly ThisCall beginFrame = new ThisCall(0x0064DD20);
        public static void BeginFrame()
        {
            beginFrame.Call(GetRendererAddress());
            //Process.THISCALL<NullReturnCall>(GetRendererAddress(), 0x0064DD20);
        }

        static readonly ThisCall endFrame = new ThisCall(0x0064DF20);
        public static void EndFrame()
        {
            endFrame.Call(GetRendererAddress());
            //Process.THISCALL<NullReturnCall>(GetRendererAddress(), 0x0064DF20);
        }

        static readonly ThisCall<int, int, int> vidBlit = new ThisCall<int, int, int>(0x00657670);
        public static void Vid_Blit(int arg1, int tagRect1, int tagRect2)
        {
            vidBlit.Call(GetRendererAddress(), arg1, tagRect1, tagRect2);
            //Process.THISCALL<NullReturnCall>(GetRendererAddress(), 0x00657670, new IntArg(arg1), new IntArg(tagRect1), new IntArg(tagRect2));
        }

        public static void SetFogRange(float near, float far, int mode)
        {
            Process.THISCALL<NullReturnCall>(GetRendererAddress(), 0x6521E0, new FloatArg(near), new FloatArg(far), new IntArg(mode));
        }

        public static void FlushPolys()
        {
            Process.THISCALL<NullReturnCall>(GetRendererAddress(), 0x64DD10);
        }

        public static void SetZBufferWriteEnabled(bool enabled)
        {
            Process.THISCALL<NullReturnCall>(GetRendererAddress(), 0x6524E0, (BoolArg)enabled);
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
            Process.Write(zCRenderer.GetRendererAddress() + 0x464, (int)blend);
            //Process.THISCALL<NullReturnCall>(GetRendererAddress(), 0x652610, (POINTER!!!)(IntArg)(int)blend);
        }
    }

}
