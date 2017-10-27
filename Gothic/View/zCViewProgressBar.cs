using System;
using System.Collections.Generic;
using System.Text;
using WinApi;
using Gothic.Types;

namespace Gothic.View
{
    public class zCViewProgressBar : zCView
    {
        new public const int ByteSize = 0x130;

        new public abstract class FuncAddresses : zCView.FuncAddresses
        {
            public const int ResetRange = 0x0046F400,
            SetPercent = 0x0046EEC0,
            SetRange = 0x0046F340,
            zCViewProgressBar = 0x0046E9D0;
        }

        /*public enum HookSize : uint
        {
            ResetRange = 7,
            SetPercent = 7,
            SetRange = 5,
            zCViewProgressBar = 7
        }*/
        

        public zCViewProgressBar(int address) : base (address)
        {
        }

        public zCViewProgressBar()
        {
        }

        new public static zCViewProgressBar Create(int x, int y, int width, int height, zTviewID type)
        {
            int ptr = Process.Alloc(ByteSize).ToInt32();
            Process.THISCALL<NullReturnCall>(ptr, FuncAddresses.zCViewProgressBar, new IntArg(x), new IntArg(y), new IntArg(width), new IntArg(height), new IntArg((int)type));
            return new zCViewProgressBar(ptr);
        }
        
        public void ResetRange()
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.ResetRange);
        }

        public void SetPercent(int value, string message = "")
        {
            using (zString z = zString.Create(message))
                SetPercent(value, z);
        }

        public void SetPercent(int value, zString message)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.SetPercent, new IntArg(value), (IntArg)message.VTBL, (IntArg)message.ALLOCATER, (IntArg)message.PTR, (IntArg)message.Length, (IntArg)message.Res);
        }

        public void SetRange(int min, int max)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.SetRange, new IntArg(min), new IntArg(max));
        }
    }
}
