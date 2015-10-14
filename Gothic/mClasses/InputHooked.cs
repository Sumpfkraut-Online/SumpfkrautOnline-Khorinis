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
        
        public static void deactivateFullControl(Process Process)
        {
            Process.Write(new byte[] { 0xE9, 0xA8, 0x00 }, 0x4D4D3D); // disable ingame keyboard movement
            Process.Write(new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90, 0x90 }, 0x4D3E50); // disable x-mouse movement  
            Process.Write(new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90, 0x90 }, 0x4D3E5C); // disable y-mouse movement  
        }

        public static void activateFullControl(Process Process)
        {
            Process.Write(new byte[] { 0x0F, 0x84, 0xA7 }, 0x4D4D3D); //enable ingame keyboard movement
            Process.Write(new byte[] { 0x89, 0x0D, 0x5C, 0x16, 0x8D, 0x00 }, 0x4D3E50); // enable x-mouse movement    
            Process.Write(new byte[] { 0x89, 0x15, 0x60, 0x16, 0x8D, 0x00 }, 0x4D3E5C); // enable y-mouse movement   
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
