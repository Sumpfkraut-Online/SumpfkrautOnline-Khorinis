using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.Types;
using WinApi;

namespace Gothic.Objects.EventManager
{
    public class oCMsgMovement : oCNpcMessage
    {
        public enum SubTypes : ushort
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

        new public abstract class VarOffsets : oCNpc.VarOffsets
        {
            public const int ani = 0x78;
        }

        public static oCMsgMovement Create(SubTypes type, zVec3 vec)
        {
            int address = Process.CDECLCALL<IntArg>(0x764680); //_CreateInstance()

            //Konstruktor...
            Process.THISCALL<NullReturnCall>(address, 0x765A30, (IntArg)(int)type, vec);
            return new oCMsgMovement(address);
        }

        public static oCMsgMovement Create(SubTypes type, zCVob vob)
        {
            int address = Process.CDECLCALL<IntArg>(0x764680, null); //_CreateInstance()

            //Konstruktor...
            Process.THISCALL<NullReturnCall>(address, 0x765930, (IntArg)(int)type, vob);
            return new oCMsgMovement(address);
        }

        public oCMsgMovement() : base()
        {
        }

        public oCMsgMovement(int address) : base(address)
        {
        }

        new public SubTypes SubType
        {
            get { return (SubTypes)base.SubType; }
            set { base.SubType = (ushort)value; }
        }

        public int Animation
        {
            get { return Process.ReadInt(Address + VarOffsets.ani); }
            set { Process.Write(Address + VarOffsets.ani, value); }
        }
    }
}
