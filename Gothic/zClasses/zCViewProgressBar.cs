using System;
using System.Collections.Generic;
using System.Text;
using WinApi;
using Gothic.zTypes;

namespace Gothic.zClasses
{
    public class zCViewProgressBar : zCView
    {
        //size 0x130???
        #region OffsetLists
        public enum Offsets : uint
        {
        }

        public enum FuncOffsets : uint
        {
            ResetRange = 0x0046F400,
            SetPercent = 0x0046EEC0,
            SetRange = 0x0046F340,
            zCViewProgressBar = 0x0046E9D0
        }

        public enum HookSize : uint
        {
            ResetRange = 7,
            SetPercent = 7,
            SetRange = 5,
            zCViewProgressBar = 7

        }
        #endregion

        #region Standard
        public zCViewProgressBar(Process process, int address) : base (process, address)
        {
            
        }

        public zCViewProgressBar()
        {

        }

        public override uint ValueLength()
        {
            return 4;
        }
        #endregion

        #region statics
        

        #endregion

        #region Fields
        
        #endregion


        public static zCViewProgressBar Create(Process process, int x, int y, int width, int height, zTviewID type)
        {//0x130
            int ptr = process.Alloc(0x130).ToInt32();

            process.THISCALL<NullReturnCall>((uint)ptr, (uint)FuncOffsets.zCViewProgressBar, new CallValue[]{ 
                new IntArg(x), 
                new IntArg(y), 
                new IntArg(width), 
                new IntArg(height), 
                new IntArg((int)type)});


            return new zCViewProgressBar(process, ptr);
        }

        #region methods
        public void ResetRange()
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.ResetRange, new CallValue[] { });
        }

        public void SetPercent(int value, zString message)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.SetPercent, new CallValue[] { new IntArg(value), message });
        }

        public void SetRange(int min, int max)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.SetPercent, new CallValue[] { new IntArg(min), new IntArg(max) });
        }
        #endregion
    }
}
