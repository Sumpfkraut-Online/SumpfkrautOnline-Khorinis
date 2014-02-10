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
            Remove = 0x0070BA30
        }

        public enum HookSize
        {
            Remove = 0x6
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
