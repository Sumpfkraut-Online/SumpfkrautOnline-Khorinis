using System;
using System.Collections.Generic;
using System.Text;
using WinApi;
using Gothic.Types;

namespace Gothic.Objects
{
    public class oCMobInter : oCMob
    {
        new public abstract class VarOffsets : oCMob.VarOffsets
        {
            public const int triggerTarget = 400,
            useWithItem = 420,
            sceme = 440,
            conditionFunc = 460,
            onStateFuncName = 480,
            state = 500,
            state_num = 504,
            state_target = 508,
            rewind = 512,
            stateAni = 516,
            stateNPCAni = 520;
        }

        new public abstract class FuncAddresses : oCMob.FuncAddresses
        {
            public const int StartInteraction = 0x00721580,
            StopInteraction = 0x00721C20,
            StartStateChange = 0x0071FEA0,
            SetMobBodyState = 0x0071D610,
            CallOnStateFunc = 0x00720870,
            OnTrigger = 0x0071E7D0,
            OnUnTrigger = 0x0071EAC0,
            AI_UseMobToState = 0x00721F00,
            SetUseWithItem = 0x0071DA00;
        }

        /*public enum HookSize : uint
        {
            StartInteraction = 6,
            StopInteraction = 6,
            StartStateChange = 7,
            SetMobBodyState = 6,
            CallOnStateFunc = 7,
            OnTrigger = 7,
            OnUnTrigger = 7,
            AI_UseMobToState = 6
        }*/

        public oCMobInter()
        {
        }

        public oCMobInter(int address)
            : base(address)
        {
        }

        public new static oCMobInter Create()
        {
            int address = Process.CDECLCALL<IntArg>(0x7187E0); //_CreateInstance()
            //Process.THISCALL<NullReturnCall>(address, 0x71D010); //Konstruktor...
            return new oCMobInter(address);
        }

        public override void Dispose()
        {
            Process.THISCALL<NullReturnCall>(Address, 0x71D1A0, new BoolArg(true));
        }

        public int State
        {
            get { return  Process.ReadInt(Address + VarOffsets.state); }
            set { Process.Write(value, Address + VarOffsets.state); }
        }

        public int StateNum
        {
            get { return Process.ReadInt(Address + VarOffsets.state_num); }
        }

        public int StateTarget
        {
            get { return Process.ReadInt(Address + VarOffsets.state_target); }
        }

        public bool Rewind
        {
            get { return Process.ReadBool(Address + VarOffsets.rewind); }
            set { Process.Write(value, Address + VarOffsets.rewind); }
        }

        public int Bitfield
        {
            get { return Process.ReadInt(Address + 0x20C); }
            set { Process.Write(value, Address + 0x20C); }
        }

        public int StateAniID
        {
            get { return Process.ReadInt(Address + VarOffsets.stateAni); }
            set { Process.Write(value, Address + VarOffsets.stateAni); }
        }

        public int StateNPCAniID
        {
            get { return Process.ReadInt(Address + VarOffsets.stateNPCAni); }
        }

        public zString Sceme
        {
            get { return new zString(Address + VarOffsets.sceme); }
        }

        public zString ConditionFunc
        {
            get { return new zString(Address + VarOffsets.conditionFunc); }
        }

        public zString UseWithItem
        {
            get { return new zString(Address + VarOffsets.useWithItem); }
        }

        public zString TriggerTarget
        {
            get { return new zString(Address + VarOffsets.triggerTarget); }
        }

        public zString OnStateFunc
        {
            get { return new zString(Address + VarOffsets.onStateFuncName); }
        }

        public void SetUseWithItem(String str)
        {
            using (zString zS = zString.Create(str))
                SetUseWithItem(zS);
        }
        public void SetUseWithItem(zString str)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.SetUseWithItem, str);
        }

        public void OnTrigger(zCVob triggertarget, zCVob sender)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.OnTrigger, triggertarget, sender);
        }

        public void OnUnTrigger(zCVob triggertarget, zCVob sender)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.OnUnTrigger, triggertarget, sender );
        }

        public void CallOnStateFunc(oCNpc npc, int x)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.CallOnStateFunc, npc, new IntArg(x) );
        }

        public void AI_UseMobToState(oCNpc npc, int x)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.AI_UseMobToState, npc, new IntArg(x) );
        }

        public void StartStateChange(oCNpc npc, int x, int y)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.StartStateChange, npc, new IntArg(x), new IntArg(y) );
        }
        public void SetMobBodyState(oCNpc npc)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.SetMobBodyState, npc );
        }
        public void StartInteraction(oCNpc npc)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.StartInteraction, npc );
        }

        public void StopInteraction(oCNpc npc)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.StopInteraction, npc );
        }

        public void SetIdealPosition(oCNpc npc)
        {
            Process.THISCALL<NullReturnCall>(Address, 0x71E240, npc );
        }

        public void ScanIdealPositions()
        {
            Process.THISCALL<NullReturnCall>(Address, 0x71DC30);
        }

        public int GetFreePosition(oCNpc npc, zVec3 vec)
        {
            return Process.THISCALL<IntArg>(Address, 0x71DF50, npc, vec);
        }
    }
}
