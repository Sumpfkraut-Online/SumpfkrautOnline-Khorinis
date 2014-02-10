using System;
using System.Collections.Generic;
using System.Text;
using WinApi;
using Gothic.zTypes;

namespace Gothic.zClasses
{
    public class zCModelAni : zClass
    {
        #region Offsets
        public enum Offsets
        {
            name = 0x024,//zString
            id = 0x04C,//int
            type = 0x0DC,//int -> zTMdl_AniType
            pos = 0x0B8//vec3
        }

        public enum FuncOffsets
        {
            GetAniID = 0x006A9800,
            GetAniName = 0x0059D160,
            GetAniType = 0x0059D170
        }

        public enum HookSize
        {
            GetAniID = 4, //zu klein!
            GetAniName = 4, // zu klein für ein Hook!
            GetAniType = 6
        }
        #endregion

        public zCModelAni()
        {

        }

        public zCModelAni(Process process, int address)
            : base(process, address)
        {

        }


        #region Fields

        public zString AniName
        {
            get { return new zString(Process, Address + (int)Offsets.name); }
        }

        #endregion
        public int GetAniID()
        {
            return Process.THISCALL<IntArg>((uint)Address, (uint)FuncOffsets.GetAniID, new CallValue[] { }).Address;
        }
        
        /// <summary>
        /// Klappt nicht, falsche Behandlung der Parameter? Nutze Feld, AniName
        /// </summary>
        /// <returns></returns>
        public zString GetAniName()
        {
            return Process.THISCALL<zString>((uint)Address, (uint)FuncOffsets.GetAniName, new CallValue[] { });
        }



        public override uint ValueLength()
        {
            return 4;
        }
    }
}
