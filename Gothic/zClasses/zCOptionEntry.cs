using System;
using System.Collections.Generic;
using System.Text;
using WinApi;
using Gothic.zTypes;

namespace Gothic.zClasses
{
    public class zCOptionEntry : zClass
    {
        public zCOptionEntry(Process process, int address)
            : base(process, address)
        {

        }

        public zCOptionEntry()
        {

        }


        #region OffsetLists
        public enum Offsets : uint
        {
            VarName = 16,
            VarValue = 36
        }

        public enum FuncOffsets : uint
        {
        }

        public enum HookSize : uint
        {

        }
        #endregion


        #region statics


        #endregion

        #region Fields

        public zString VarName
        {
            get { return new zString(Process, Address + (int)Offsets.VarName); }
        }

        public zString VarValue
        {
            get { return new zString(Process, Address + (int)Offsets.VarValue); }
        }

        #endregion

        #region methods

        #endregion
    }
}
