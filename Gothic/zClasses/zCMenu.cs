using System;
using System.Collections.Generic;
using System.Text;
using WinApi;
using Gothic.zTypes;

namespace Gothic.zClasses
{
    public class zCMenu : zClass
    {
        public static String MainMenu = "Menu_Main";
        public enum Offsets
        {
            name = 3112,
            menuItem = 3256
        }

        public zCMenu()
        {

        }

        public zCMenu(Process process, int address)
            : base(process, address)
        {

        }

        public zString Name
        {
            get { return new zString(Process, Address + (int)Offsets.name); }
        }

        public zCArray<zCMenuItem> MenuItems
        {
            get { return new zCArray<zCMenuItem>(Process, Address + (int)Offsets.menuItem); }
        }

        public void Leave()
        {
            //0x004DB910
            Process.THISCALL<NullReturnCall>((uint)Address, 0x004DB910, new CallValue[] { });
        }

        public void Release()
        {
            //0x004DB910
            Process.THISCALL<NullReturnCall>((uint)Address, 0x004DAB80, new CallValue[] { });
        }

        public void ScreenDone()
        {
            Process.THISCALL<NullReturnCall>((uint)Address, 0x004DD9E0, new CallValue[] { });
        }

        public int HandleSelAction(int t, zString str, zCMenuItem menuitem)
        {
            return Process.THISCALL<IntArg>((uint)Address, 0x004DC430, new CallValue[] { new IntArg(t), str, menuitem }).Address;
        }

        public int HandleActivateItem(zCMenuItem menuitem)
        {
            return Process.THISCALL<IntArg>((uint)Address, 0x004DE100, new CallValue[] { menuitem }).Address;
        }

        public int HandleAction()
        {
            return Process.THISCALL<IntArg>((uint)Address, 0x004DC250, new CallValue[] { }).Address;
        }

        public zCMenuItem GetActiveItem()
        {
            return Process.THISCALL<zCMenuItem>((uint)Address, 0x004DDD80, new CallValue[] { });
        }
        
        public static void Shutdown(Process process)
        {
            process.CDECLCALL<NullReturnCall>(0x004DA380, new CallValue[] { });
        }

        public static zCArray<zCMenu> GetMenuList(Process process)
        {
            return new zCArray<zCMenu>(process, 0x8D1E44);
        }

        public static zCMenu GetActive(Process process)
        {
            return process.CDECLCALL<zCMenu>(0x004DDD60, new CallValue[] { });
        }
        
        public static zCMenu GetMenuByName(Process process, String name)
        {
            zCArray<zCMenu> menuList = GetMenuList(process);
            for (int i = 0; i < menuList.Size; i++)
            {
                if (menuList.get(i).Name.Value.ToLower().Equals(name.ToLower()))
                    return menuList.get(i);
            }
            return null;
        }


        public override uint ValueLength()
        {
            return 4;
        }
    }
}
