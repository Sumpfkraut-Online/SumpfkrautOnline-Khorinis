using System;
using System.Collections.Generic;
using System.Text;
using WinApi;
using Gothic.zTypes;

namespace Gothic.zClasses
{
    public class oCMob: zCVob
    {
        #region OffsetLists
        public enum Offsets
        {
            Model = 0x0C8,
            Name = 0x0120
        }
        public enum FuncOffsets : uint
        {
            GetModel = 0x0071BEE0,
            GetName = 0x0071BC30,
            GetScemeName = 0x0071BD00,
            SetName = 0x0071BC10
        }

        public enum HookSize : uint
        {
            GetModel = 6,
            GetName = 7,
            GetScemeName = 7,
            SetName = 5
        }

        #endregion

        public oCMob()
        {

        }

        public oCMob(Process process, int address)
            : base(process, address)
        {
        }

        public zCModel GetModel()
        {
            return Process.THISCALL<zCModel>((uint)Address, (uint)FuncOffsets.GetModel, new CallValue[] {  });
        }

        public zString GetName()
        {
            return Process.THISCALL<zString>((uint)Address, (uint)FuncOffsets.GetName, new CallValue[] { });
        }

        public zString GetScemeName()
        {
            return Process.THISCALL<zString>((uint)Address, (uint)FuncOffsets.GetScemeName, new CallValue[] { });
        }

        public void SetName(String name)
        {
            zString str = zString.Create(Process, name);
            SetName(str);
            str.Dispose();
        }
        public void SetName(zString name)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.SetName, new CallValue[] { name });
        }


        public zString Name
        {
            get { return new zString(Process, Address + (int)Offsets.Name); }
        }

        public new static oCMob Create(Process process)
        {
            int address = process.CDECLCALL<IntArg>(0x718590, null); //_CreateInstance()
            process.THISCALL<NullReturnCall>((uint)address, 0x71B8F0, null); //Konstruktor...
            return new oCMob(process, address);
        }
    }
}
