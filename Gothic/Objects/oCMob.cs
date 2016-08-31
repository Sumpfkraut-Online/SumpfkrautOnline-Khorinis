using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;
using Gothic.Types;

namespace Gothic.Objects
{
    public class oCMob : oCVob
    {
        new public abstract class VarOffsets : zCVob.VarOffsets
        {
            public const int Model = 0x0C8,
            Name = 0x0120;
        }

        new public abstract class FuncAddresses : zCVob.FuncAddresses
        {
            public const int GetModel = 0x0071BEE0,
            GetName = 0x0071BC30,
            GetScemeName = 0x0071BD00,
            SetName = 0x0071BC10;
        }

        /*public enum HookSize : uint
        {
            GetModel = 6,
            GetName = 7,
            GetScemeName = 7,
            SetName = 5
        }*/

        public oCMob()
        {
        }

        public oCMob(int address)
            : base(address)
        {
        }
        /*
        public zCModel GetModel()
        {
            return Process.THISCALL<zCModel>((uint)Address, (uint)FuncOffsets.GetModel, new CallValue[] { });
        }
        */
        public zString GetName()
        {
            return Process.THISCALL<zString>(Address, FuncAddresses.GetName);
        }

        public zString GetScemeName()
        {
            return Process.THISCALL<zString>(Address, FuncAddresses.GetScemeName);
        }

        public void SetName(String name)
        {
            using (zString str = zString.Create(name))
                SetName(str);
        }
        public void SetName(zString name)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.SetName, name);
        }


        public zString Name
        {
            get { return new zString(Address + VarOffsets.Name); }
        }

        public new static oCMob Create()
        {
            int address = Process.CDECLCALL<IntArg>(0x718590); //_CreateInstance()
            //Process.THISCALL<NullReturnCall>(address, 0x71B8F0); //Konstruktor...
            return new oCMob(address);
        }
    }
}
