using System;
using System.Collections.Generic;
using System.Text;
using WinApi;
using Gothic.zClasses;
using Gothic.zTypes;

namespace Gothic.zClasses
{
    public class oCNpcInventory :oCItemContainer
    {
        public oCNpcInventory(Process process, int Address)
            : base(process, Address)
        {

        }

        public enum Offsets
        {
            Owner = 0xA0
        }

        public enum FuncOffsets
        {
            //Remove = 0x0070BA30,
            Remove_Int_Int = 0x0070D080,
            Remove_Item = 0x0070CBE0,
            Remove_Item_Int = 0x0070CE20,
            Remove_zString_Int = 0x0070D170
        }

        public enum HookSize
        {
            //Remove = 0x6,
            Remove_Int_Int = 7,
            Remove_Item = 6,
            Remove_Item_Int = 7,
            Remove_zString_Int = 5
        }

        public zCListSort<oCItem> ItemList
        {
            get
            {
                return new zCListSort<oCItem>(Process, Address + 0xA8);
            }
        }

        public oCNpc Owner
        {
            get { return new oCNpc(Process, Process.ReadInt(Address + (int)Offsets.Owner)); }
        }

    }
}
