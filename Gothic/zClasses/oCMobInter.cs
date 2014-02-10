using System;
using System.Collections.Generic;
using System.Text;
using WinApi;
using Gothic.zTypes;

namespace Gothic.zClasses
{
    public class oCMobInter : oCMob
    {
        #region OffsetLists
        public enum Offsets
        {
            triggerTarget = 400,
            onStateFuncName = 480
        }
        public enum FuncOffsets : uint
        {
            StartInteraction = 0x00721580,
            StopInteraction = 0x00721C20,
            StartStateChange = 0x0071FEA0,
            SetMobBodyState = 0x0071D610,
            CallOnStateFunc = 0x00720870,
            OnTrigger = 0x0071E7D0,
            OnUnTrigger= 0x0071EAC0,
            AI_UseMobToState = 0x00721F00
        }

        public enum HookSize : uint
        {
            StartInteraction = 6,
            StopInteraction = 6,
            StartStateChange = 7,
            SetMobBodyState = 6,
            CallOnStateFunc = 7,
            OnTrigger = 7,
            OnUnTrigger = 7,
            AI_UseMobToState = 6
        }

        #endregion

        public oCMobInter()
        {

        }

        public oCMobInter(Process process, int address)
            : base(process, address)
        {
        }

        public zString OnStateFunc
        {
            get { return new zString(Process, (int)Address + (int)Offsets.onStateFuncName); }
        }

        public void OnTrigger(zCVob triggertarget, zCVob sender)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.OnTrigger, new CallValue[] { triggertarget, sender });
        }

        public void OnUnTrigger(zCVob triggertarget, zCVob sender)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.OnUnTrigger, new CallValue[] { triggertarget, sender });
        }

        public void CallOnStateFunc(oCNpc npc, int x)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.CallOnStateFunc, new CallValue[] { npc, new IntArg(x) });
        }

        public void AI_UseMobToState(oCNpc npc, int x)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.AI_UseMobToState, new CallValue[] { npc, new IntArg(x) });
        }

        public void StartStateChange(oCNpc npc, int x, int y)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.StartStateChange, new CallValue[] { npc, new IntArg(x), new IntArg(y) });
        }
        public void SetMobBodyState(oCNpc npc)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.SetMobBodyState, new CallValue[] { npc });
        }
        public void StartInteraction(oCNpc npc)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.StartInteraction, new CallValue[] { npc });
        }

        public void StopInteraction(oCNpc npc)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.StopInteraction, new CallValue[] { npc });
        }
    }
}
