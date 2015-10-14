using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.zClasses;
using WinApi;
using Gothic.zTypes;

namespace Gothic.zStruct
{
    public class oCMsgMovement : oCNpcMessage, IDisposable
    {
        public enum SubTypes
        {
            RobustTrace = 0,
            GotoPos = 1,
            GotoVob = 2,
            GoRoute = 3,
            Turn = 4,

            TurnToPos = 5,
            TurnToVob = 6,
            TurnAway = 7,

            Jump = 8, //???
            SetWalkMode = 9, //???
            WhirlAround = 10, // ???
            StandUp = 11,
            CanSeeNpc = 12,
            Strafe = 13,

            Dodge = 15,

            AlignToFP = 17
        }

        public enum Offsets : uint
        {
            ani = 0x78
        }

        public static oCMsgMovement Create(Process process, SubTypes type, zVec3 vec)
        {
            int address = process.CDECLCALL<IntArg>(0x764680, null); //_CreateInstance()

            //Konstruktor...
            process.THISCALL<NullReturnCall>((uint)address, 0x765A30, new CallValue[] { (IntArg)(int)type, vec });
            return new oCMsgMovement(process, address);
        }

        public static oCMsgMovement Create(Process process, SubTypes type, zCVob vob)
        {
            int address = process.CDECLCALL<IntArg>(0x764680, null); //_CreateInstance()

            //Konstruktor...
            process.THISCALL<NullReturnCall>((uint)address, 0x765930, new CallValue[] { (IntArg)(int)type, vob });
            return new oCMsgMovement(process, address);
        }

        public oCMsgMovement(Process process, int address)
            : base(process, address)
        {

        }

        public SubTypes SubType
        {
            get { return (SubTypes)Process.ReadUShort(Address + 0x24); }
            set { Process.Write((ushort)value, Address + 0x24); }
        }

        public int Animation
        {
            get { return Process.ReadInt(Address + (int)Offsets.ani); }
            set { Process.Write(value, Address + (int)Offsets.ani); }
        }

        private bool disposed = false;
        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                Process.Free(new IntPtr(Address), 0x7C);
                disposed = true;
            }
        }

    }
}
