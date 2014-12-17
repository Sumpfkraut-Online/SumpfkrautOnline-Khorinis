using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;
using Gothic.zTypes;

namespace Gothic.zClasses
{
    public class oCVisualFX : zCVob
    {
        public enum Offsets
        {
            SpellType = 0x568,
            SpellTargetTypes = 0x570,
            SpellCat = 0x56C,
            Target = 0x4B0, //zCVob Target
            Level = 0x55C,//Flag?
            Inflictor = 0x4AC,//zCVob Inflictor
            Name = 0x4C4, //zString name
            NUMCollisionCandidates = 0x414,
            Origin = 0x4A8,//zCvob

        }

        public enum FuncOffsets
        {
            CreateAndPlay = 0x0048E760,
            EndEffect = 0x00494C80,
            Kill = 0x00493F70,
            StopInvestFX = 0x00491830,
            Stop = 0x00493BE0,
            InitEffect = 0x004943E0,
            SetShowVisual = 0x00494D20,
            Open = 0x004918E0,
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

        public void Open()
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.Open, new CallValue[] { });
        }

        public void InitEffect()
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.InitEffect, new CallValue[] { });
        }

        public void Kill()
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.Kill, new CallValue[] { });
        }

        public void StopInvestFX()
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.StopInvestFX, new CallValue[] { });
        }

        public void Stop(int arg)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.Stop, new CallValue[] { (IntArg)arg });
        }

        public void SetShowVisual(int arg)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.SetShowVisual, new CallValue[] { (IntArg)arg });
        }

        public void EndEffect(int arg)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.EndEffect, new CallValue[] { (IntArg)arg });
        }

        public static oCVisualFX CreateAndPlay(Process Process, zString effect, zCVob vob, zCVob targetVob, int a, float b, int c, int d)
        {
            return Process.CDECLCALL<oCVisualFX>((uint)FuncOffsets.CreateAndPlay, new CallValue[] { effect, vob, targetVob, (IntArg)a, (FloatArg)b, (IntArg)c, (IntArg)d });
        }

    }
}
