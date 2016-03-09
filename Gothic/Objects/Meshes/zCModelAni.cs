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
        public abstract class VarOffsets
        {
            public const int name = 0x024,//zString
            id = 0x04C,//int
            type = 0x0DC,//int -> zTMdl_AniType
            pos = 0x0B8;//vec3
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

        public zString AniName
        {
            get { return new zString(Address + VarOffsets.name); }
        }

        #endregion
        public int GetAniID()
        {
            return Process.THISCALL<IntArg>(Address, FuncAddresses.GetAniID).Value;
        }

        /// <summary>
        /// Klappt nicht, falsche Behandlung der Parameter? Nutze Feld, AniName
        /// </summary>
        public zString GetAniName()
        {
            return Process.THISCALL<zString>(Address, FuncAddresses.GetAniName);
        }
    }
}
