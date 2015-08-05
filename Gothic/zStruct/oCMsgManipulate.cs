using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;
using Gothic.zTypes;
using Gothic.zClasses;

namespace Gothic.zStruct
{
    public class oCMsgManipulate : oCNpcMessage, IDisposable
    {
        //VBTL => 8641948

        public enum SubTypes
        {
            TakeVob = 0,
            DropVob = 1,
            ThrowVob = 2,
            Exchange = 3, //???
            UseMob = 4,
            UseItem = 5,
            InsertInteractItem = 6,
            RemoveInteractItem = 7,
            CreateInteractItem = 8,
            DestroyInteractItem = 9,
            PlaceInteractItem = 10,
            ExchangeInteractItem = 11,
            UseMobWithItem = 12,
            CallScript = 13,
            EquipItem = 14,
            UseItemToState = 15,
            TakeMob = 16,
            DropMob = 17
        }

        public new enum Offsets
        {
            //60 => ?
            InstanceName = 68, //ItemName zString
            SlotName = 88, //ZS_LEFTHAND SlotName zString
            Item = 108
        }

        public static oCMsgManipulate Create(Process process, int subType, zCVob vob, int val)
        {
            IntPtr ptr = process.Alloc(0x7C);
            zCClassDef.ObjectCreated(process, ptr.ToInt32(), 0xAB2860);
            process.THISCALL<NullReturnCall>((uint)ptr.ToInt32(), (uint)0x768CB0, new CallValue[] { (IntArg)subType , vob, (IntArg)val });

            return new oCMsgManipulate(process, ptr.ToInt32());
        }

        public oCMsgManipulate(Process process, int address)
            : base(process, address)
        {

        }

        public oCItem Item { get { return new oCItem(Process, Process.ReadInt(Address + (int)Offsets.Item)); } }
        public zString InstanceName { get { return new zString(Process, Address + (int)Offsets.InstanceName); } }
        public zString SlotName { get { return new zString(Process, Address + (int)Offsets.SlotName); } }

        private bool disposed = false;
        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                Process.Free(new IntPtr(Address), 0x6C);
                disposed = true;
            }
        }
    }
}
