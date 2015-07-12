using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using WinApi.Win32.User;
using WinApi.User;
using WinApi.User.Enumeration;
using Gothic.zClasses;
using WinApi;

namespace Gothic.mClasses
{

    public class InputHooked
    {
        public static int MAXVALUE = 8192;
        static byte[] keys = new byte[0xFF];
        static int wheel;
        
        public static void deactivateLogScreen(Process process, bool enable)
        {
            if (!enable)
            {
                int jmpAddress = 0x006FD47D - 0x006FC2A9 - 5;//JUMPTO - JUMPFROM - 5
                process.Write(new byte[] { 0xE9 }, 0x006FC2A9);//Jump
                process.Write(jmpAddress, 0x006FC2A9 + 1);//To End!
            }
            else
            {
                process.Write(new byte[] { 0x8B, 0x0D, 0x84, 0x26, 0xAB }, 0x006FC2A9);
            }
        }

        public static void deactivateStatusScreen(Process process, bool enable)
        {
            if (!enable)
            {
                int jmpAddress = 0x006FD47D - 0x006FC270 - 5;//JUMPTO - JUMPFROM - 5
                process.Write(new byte[] { 0xE9 }, 0x006FC270);//Jump
                process.Write(jmpAddress, 0x006FC270 + 1);//To End!
            }
            else
            {
                process.Write(new byte[] { 0x8B, 0x0D, 0x84, 0x26, 0xAB }, 0x006FC270);
            }
        }

        public static void deactivateInventory(Process process, bool enable)
        {
            if (!enable)
            {
                int jmpAddress = 0x006FD47D - 0x006FC575 - 5;//JUMPTO - JUMPFROM - 5
                process.Write(new byte[] { 0xE9 }, 0x006FC575);//Jump
                process.Write(jmpAddress, 0x006FC575 + 1);//To End!
            }
            else
            {
                process.Write(new byte[] { 0xD9, 0x05, 0xD8, 0xB3, 0x99 }, 0x006FC575);
            }
        }
        
        public static void deaktivateFullControl(Process Process)
        {
            //key pressed
            byte[] arr = new byte[] { 0x31, 0xC0, 0xC2, 0x04, 0x00 };
            Process.Write(arr, (int)0x004D51C0);
            //key toggled
            arr = new byte[] { 0x31, 0xC0, 0xC2, 0x04, 0x00 };
            Process.Write(arr, (int)0x004D51D0);

            /*byte[] arr = new byte[] { 0xC3 };
            Process.Write(arr, (int)oCAiHuman.FuncOffsets.Moving);

            arr = new byte[] { 0xC2, 0x04, 0x00 };
            Process.Write(arr, (int)oCGame.FuncOffsets.HandleEvent);
            Process.Write(new byte[] { 0x6A, 0x0 }, 0x0069D354);//Set Key for FirstPerson to undef*/
        }

        public static void activateFullControl(Process Process)
        {
            //key pressed
            byte[] arr = new byte[] { 0x8B, 0x44, 0x24, 0x04, 0x0F  };
            Process.Write(arr, (int)0x004D51C0);
            // key toggled
            arr = new byte[] { 0x8B, 0x4C, 0x24, 0x04, 0x0F  };
            Process.Write(arr, (int)0x004D51D0);

            /*byte[] arr = new byte[] { 0x53 };
            Process.Write(arr, (int)oCAiHuman.FuncOffsets.Moving);

            arr = new byte[] { 0x6A,  0xff, 0x68};
            Process.Write(arr, (int)oCGame.FuncOffsets.HandleEvent);
            Process.Write(new byte[] { 0x6A, 0x17 }, 0x0069D354);//Set Key for FirstPerson back to 23*/
        }

        public static bool IsPressed(Process process, int key)
        {
            if ((Input.GetAsyncKeyState((int)key) & 0x8001) == 0x8001 || (Input.GetAsyncKeyState((int)key) & 0x8000) == 0x8000)
                return true;
            return false;
        }

        public static bool IsPressed(int key)
        {
            Process process = Process.ThisProcess();

            return IsPressed(process, key);
        }

    }
}
