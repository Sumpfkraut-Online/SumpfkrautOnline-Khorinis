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
            useWithItem = 420,
            sceme = 440,
            conditionFunc = 460,
            onStateFuncName = 480,
            state = 500,
            state_num = 504,
            state_target = 508,
            rewind = 512,
            stateAni = 516,
            stateNPCAni = 520
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
            AI_UseMobToState = 0x00721F00,
            SetUseWithItem = 0x0071DA00
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

        public static oCMobInter Create(Process process)
        {
            IntPtr ptr = process.Alloc(0x234);
            zCClassDef.ObjectCreated(process, ptr.ToInt32(), 0x00AB19A0);//0x00AB1518) => MobDoor;
            process.THISCALL<NullReturnCall>((uint)ptr.ToInt32(), (uint)0x0071D010, new CallValue[] { });

            return new oCMobInter(process, ptr.ToInt32());
        }

        public int State
        {
            get { return  Process.ReadInt((int)Address + (int)Offsets.state); }
            set { Process.Write(value, (int)Address + (int)Offsets.state); }
        }

        public int StateNum
        {
            get { return Process.ReadInt((int)Address + (int)Offsets.state_num); }
        }

        public int StateTarget
        {
            get { return Process.ReadInt((int)Address + (int)Offsets.state_target); }
        }

        public bool Rewind
        {
            get { if (Process.ReadInt((int)Address + (int)Offsets.rewind) >= 1) return true; else return false; }
            set { if (value)Process.Write(1, Address + (int)Offsets.rewind); else Process.Write(0, Address + (int)Offsets.rewind); }
        }

        public int StateAniID
        {
            get { return Process.ReadInt((int)Address + (int)Offsets.stateAni); }
            set { Process.Write(value, (int)Address + (int)Offsets.stateAni); }
        }

        public int StateNPCAniID
        {
            get { return Process.ReadInt((int)Address + (int)Offsets.stateNPCAni); }
        }

        public zString Sceme
        {
            get { return new zString(Process, (int)Address + (int)Offsets.sceme); }
        }

        public zString ConditionFunc
        {
            get { return new zString(Process, (int)Address + (int)Offsets.conditionFunc); }
        }

        public zString UseWithItem
        {
            get { return new zString(Process, (int)Address + (int)Offsets.useWithItem); }
        }

        public zString TriggerTarget
        {
            get { return new zString(Process, (int)Address + (int)Offsets.triggerTarget); }
        }

        public zString OnStateFunc
        {
            get { return new zString(Process, (int)Address + (int)Offsets.onStateFuncName); }
        }

        public void SetUseWithItem(String str)
        {
            zString zS = zString.Create(Process, str);
            SetUseWithItem(zS);
            zS.Dispose();
        }
        public void SetUseWithItem(zString str)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.SetUseWithItem, new CallValue[] { str });
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
