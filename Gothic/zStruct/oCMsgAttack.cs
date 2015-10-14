using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.zClasses;
using WinApi;

namespace Gothic.zStruct
{
    public class oCMsgAttack : oCNpcMessage, IDisposable
    {
        public enum SubTypes
        {
            AttackForward = 0,
            AttackLeft = 1,
            AttackRight = 2,
            AttackRun = 3,

            Parade = 5,
            Something = 6,
            ShootAt = 7,
            StopAim = 8,
            Defend = 9
        }

        public struct BitFlag
        {
            public const int Dodge = 2;
        }

       public oCMsgAttack()
            : base()
        {

        }

        public oCMsgAttack(Process process, int address)
            : base(process, address)
        {

        }

        public int Animation
        {
            get { return Process.ReadInt(Address + 0x4C); }
        }

        public SubTypes SubType
        {
            get { return (SubTypes)Process.ReadUShort(Address + 0x24); }
        }

        public int Bitfield
        {
            get { return Process.ReadInt(Address + 0x54); }
            set { Process.Write(value, Address + 0x54); }
        }

        public static oCMsgAttack Create(Process process, SubTypes type, int arg1, int arg2)
        {
            int address = process.CDECLCALL<IntArg>(0x00763D40, null); //_CreateInstance()

            //Konstruktor...
            process.THISCALL<NullReturnCall>((uint)address, 0x00767060, new CallValue[] { (IntArg)(int)type, (IntArg)arg1, (IntArg)arg2 });
            return new oCMsgAttack(process, address);
        }

        public static oCMsgAttack Create(Process process, SubTypes subType, zCVob vob, float arg2)
        {
            int address = process.CDECLCALL<IntArg>(0x00763D40, null); //_CreateInstance()

            //Konstruktor...
            process.THISCALL<NullReturnCall>((uint)address, 0x00767130, new CallValue[] { (IntArg)(int)subType, vob, (FloatArg)arg2 });
            return new oCMsgAttack(process, address);
        }

        private bool disposed = false;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!this.disposed)
            {
                if (disposing)
                {
                    Process.Free(new IntPtr(this.Address), 0x58);
                }
                disposed = true;

            }
        }
    }
}
