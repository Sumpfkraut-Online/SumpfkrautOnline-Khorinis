using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.Types;
using WinApi;

namespace Gothic.Objects.Meshes
{
    public class zCModelAni : zClass
    {
        public const int ByteSize = 0xE4;

        public abstract class VarOffsets
        {
            public const int name = 0x24,//zString
            asc = 0x38,
            id = 0x4C,//int
            alias = 0x50,
            layer = 0x6C,
            blendInSpeed = 0x70,
            blendOutSpeed = 0x74,
            collVolumeScale = 0x90,
            nextAni = 0x94,
            nextAniName = 0x98,
            aniEvents = 0xAC, // aniEvents*
            fpsRate = 0xB0,
            fpsRateSource = 0xB4,
            numFrames = 0xD8,
            numNodes = 0xDA,
            type = 0xDC,//int -> zTMdl_AniType
            pos = 0xB8;//vec3
        }

        public abstract class FuncAddresses
        {
            public const int GetAniID = 0x006A9800,
            GetAniName = 0x0059D160,
            GetAniType = 0x0059D170;
        }

        public zCModelAni()
        {

        }

        public zCModelAni(int address)
            : base(address)
        {

        }


        #region Fields

        public zString Name
        {
            get { return new zString(Address + VarOffsets.name); }
        }

        public zString ASC
        {
            get { return new zString(Address + VarOffsets.asc); }
        }

        public int Layer
        {
            get { return Process.ReadInt(Address + VarOffsets.layer); }
            set { Process.Write(Address + VarOffsets.layer, value); }
        }

        public int ID
        {
            get { return Process.ReadInt(Address + VarOffsets.id); }
            set { Process.Write(Address + VarOffsets.id, value); }
        }

        public int NumFrames
        {
            get { return Process.ReadUShort(Address + VarOffsets.numFrames); }
        }

        public bool IsReversed
        {
            get { return (Process.ReadByte(Address + 0xDC) & (1 << 6)) != 0; }
        }

        public zCModelAni NextAni
        {
            get { return new zCModelAni(Process.ReadInt(Address + VarOffsets.nextAni)); }
            set { Process.Write(Address + VarOffsets.nextAni, value.Address); }
        }

        public int NumAniEvents
        {
            get { return (Process.ReadInt(Address + 0xDD) & 0x3F); }
        }

        public zCModelAniEvent GetAniEvent(int index)
        {
            return new zCModelAniEvent(Process.ReadInt(Address + VarOffsets.aniEvents) + index * zCModelAniEvent.ByteSize);
        }

        #endregion

        public static zCModelAni Create()
        {
            int ptr = Process.CDECLCALL<IntArg>(0x576980); // createinstance
            return new zCModelAni(ptr);
        }
    }
}
