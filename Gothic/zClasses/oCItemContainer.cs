using System;
using System.Collections.Generic;
using System.Text;
using WinApi;
using Gothic.zTypes;

namespace Gothic.zClasses
{
    public class oCItemContainer :zClass 
    {

        public enum Offsets
        {
            VTBL = 0,
            Contents = 4,
            NPC = 8,
            titleText = 12,
        }

        public enum FuncOffsets
        {
            Remove = 0x00709430,
            Remove_2 = 0x007094E0,
            RemoveCurrencyItem = 0x00704B50,
            IsOpen = 0x00709200,
            Insert = 0x00709360,
            Draw = 0x007076B0,
            Close = 0x00708F30,
            Open = 0x00708550
        }

        public enum HookSizes
        {
            Remove = 6,
            Remove_2 = 6,
            RemoveCurrencyItem = 6,
            Insert = 8
        }


        public oCItemContainer(Process process, int Address)
            : base(process, Address)
        {

        }

        public static oCItemContainer Create(Process process)
        {
            oCItemContainer r = null;

            IntPtr ptr = process.Alloc(0x0A0);
            process.THISCALL<NullReturnCall>((uint)ptr.ToInt32(), (int)0x00704D00, new CallValue[] { });

            r = new oCItemContainer(process, ptr.ToInt32());
            return r;
        }


        public oCNpc NPC
        {
            get { return new oCNpc(Process, Process.ReadInt(Address + (int)Offsets.NPC)); }
        }

        public zString TitleText
        {
            get { return new zString(Process,Address + (int)Offsets.titleText); }
        }

        public zCListSort<oCItem> Content
        {
            get { return new zCListSort<oCItem>(Process, Process.ReadInt(Address + (int)Offsets.Contents)); }
        }

        public void Remove(oCItem item, int amount)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (int)FuncOffsets.Remove_2, new CallValue[] { item, new IntArg(amount) });
        }

        public void Insert(oCItem item)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (int)FuncOffsets.Insert, new CallValue[] { item });
        }

        public void Draw()
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (int)FuncOffsets.Draw, new CallValue[] { });
        }

        public void Close()
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (int)FuncOffsets.Close, new CallValue[] { });
        }

        public void Open(int x, int y, int z)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (int)FuncOffsets.Open, new CallValue[] { (IntArg)x, (IntArg)y, (IntArg)z });
        }
    }
}
