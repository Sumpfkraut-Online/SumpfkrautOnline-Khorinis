using System;
using System.Collections.Generic;
using System.Text;
using WinApi;
using Gothic.Types;

namespace Gothic.Objects
{
    public class oCMobContainer : oCMobLockable
    {
        new public abstract class VarOffsets : oCMobLockable.VarOffsets
        {
            public const int ItemContainer = 628,
            ItemList = 632,//632
            Contains = 608;//608
        }

        new public abstract class FuncAddresses : oCMobLockable.FuncAddresses
        {
            public const int CreateContents = 0x00726190,
            Close = 0x00726640,
            Insert = 0x00725FC0,
            IsIn = 0x007264C0,
            Open = 0x00726500,
            Remove = 0x00725FF0,
            Remove_Item_Int = 0x00726080;
        }

        /*public enum HookSize : uint
        {
            CreateContents = 7,
            Close = 6,
            Insert =5,
            IsIn = 8,
            Open = 6,
            Remove = 6,
            Remove_Item_Int = 0xA
        }*/

        public oCMobContainer()
        {
        }

        public oCMobContainer(int address)
            : base(address)
        {
        }

        new public static oCMobContainer Create()
        {
            int address = Process.CDECLCALL<IntArg>(0x719440); //_CreateInstance()
            Process.THISCALL<NullReturnCall>(address, 0x7257E0); //Konstruktor...
            return new oCMobContainer(address);
        }

        /*public List<oCItem> getItemList()
        {
            List<oCItem> items = new List<oCItem>();


            zCListSort<oCItem> itemList = this.ItemList;
            do
            {
                oCItem item = itemList.Data;

                if (item == null || item.Address == 0)
                    continue;
                items.Add(item);

            } while ((itemList = itemList.Next).Address != 0);

            return items;
        }*/

        public void CreateContents(zString str)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.CreateContents, str);
        }

        public void Open(oCNpc vob)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.Open, vob);
        }

        public void Close(oCNpc vob)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.Close, vob);
        }

        /*public oCItemContainer ItemContainer
        {
            get { return new oCItemContainer(Process, Process.ReadInt(Address + (int)Offsets.ItemContainer)); }
        }
    
        public zCListSort<oCItem> ItemList
        {
            get { return new zCListSort<oCItem>(Address + VarOffsets.ItemList); }
        }*/

        public zString Contains
        {
            get { return new zString(Address + VarOffsets.Contains); }
        }

        public void Remove(oCItem item, int amount)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.Remove_Item_Int, item, new IntArg(amount));
        }

        public void Remove(oCItem item)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.Remove, item);
        }

        public void Insert(oCItem item)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.Insert, item);
        }
    }
}
