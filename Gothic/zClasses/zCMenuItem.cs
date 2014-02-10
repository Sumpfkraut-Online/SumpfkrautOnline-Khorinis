using System;
using System.Collections.Generic;
using System.Text;
using WinApi;
using Gothic.zTypes;

namespace Gothic.zClasses
{
    public class zCMenuItem : zCView
    {
        public static String NAME_MENUITEM_MAIN_EXIT = "MENUITEM_MAIN_EXIT";
        public static String NAME_MENUITEM_MAIN_NEWGAME = "MENUITEM_MAIN_NEWGAME";
        public enum Offsets
        {
            id = 956
        }
        public zCMenuItem()
        {}

        public zCMenuItem(Process process, int address)
            : base(process, address)
        {

        }
        public override uint ValueLength()
        {
            return 4;
        }

        public int GetEventFunc(int itemEvent)
        {
            return Process.THISCALL<IntArg>((uint)Address, 0x004E1BF0, new CallValue[] { new IntArg(itemEvent) }).Address;
        }

        public zString ID
        {
            get { return new zString(Process, Address + (int)Offsets.id); }
        }

        public static zCArray<zCMenuItem> GetMenuitemList(Process process)
        {
            return new zCArray<zCMenuItem>(process, 0x008D1EFC);
        }

        public static zCMenuItem GetMenuByName(Process process, String name)
        {
            zCArray<zCMenuItem> menuList = GetMenuitemList(process);
            for (int i = 0; i < menuList.Size; i++)
            {
                if (menuList.get(i).ID.Value.ToLower().Equals(name.ToLower()))
                    return menuList.get(i);
            }
            return null;
        }
    }
}
