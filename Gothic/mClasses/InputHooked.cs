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
    public interface InputReceiver
    {
        void KeyReleased(int key);
        void KeyPressed(int key);
        void wheelChanged(int steps);
    }
    public class InputHooked
    {
        public static int MAXVALUE = 8192;
        static byte[] keys = new byte[0xFF];
        static int wheel;

        public static List<InputReceiver> receivers = new List<InputReceiver>();
        public InputHooked()
        {
        }

        public static int[] GetScreenSize(Process process)
        {
            return new int[] { 
                Convert.ToInt32(zCOption.GetOption(process).getEntryValue("VIDEO", "zVidResFullscreenX")), 
                Convert.ToInt32(zCOption.GetOption(process).getEntryValue("VIDEO", "zVidResFullscreenY")) 
            };
        }

        public static int[] PixelToVirtual(Process process, int[] pos)
        {
            int[] screenpos = GetScreenSize(process);
            pos[0] *= 8192; pos[1] *= 8192;
            pos[0] /= screenpos[0];
            pos[1] /= screenpos[1];

            return pos;
        }

        public static int PixelToVirtualY(Process process, int pos)
        {
            int[] screenpos = GetScreenSize(process);
            pos *= 8192;
            pos /= screenpos[1];

            return pos;
        }

        public static int PixelToVirtualX(Process process, int pos)
        {
            int[] screenpos = GetScreenSize(process);
            pos *= 8192;
            pos /= screenpos[0];

            return pos;
        }

        public static void deaktivateFullControl(Process Process)
        {
            Cursor.noHandle = true;
            byte[] arr = new byte[] { 0xC3 };
            Process.Write(arr, (int)oCAiHuman.FuncOffsets.Moving);

            arr = new byte[] { 0xC2, 0x04, 0x00 };
            Process.Write(arr, (int)oCGame.FuncOffsets.HandleEvent);
        }

        public static void activateFullControl(Process Process)
        {
            Cursor.noHandle = false;
            byte[] arr = new byte[] { 0x53 };
            Process.Write(arr, (int)oCAiHuman.FuncOffsets.Moving);

            arr = new byte[] { 0x6A,  0xff, 0x68};
            Process.Write(arr, (int)oCGame.FuncOffsets.HandleEvent);
        }

        public static int[] VirtualToPixel(Process process, int[] pos)
        {
            int[] screenpos = GetScreenSize(process);
            pos[0] *= screenpos[0];
            pos[1] *= screenpos[1];

            pos[0] /= 8192; pos[1] /= 8192;
            return pos;
        }

        public static bool IsPressed(int key)
        {
            //if ((Input.GetAsyncKeyState((int)key) & 0x8001) == 0x8001)
            if ((Input.GetAsyncKeyState((int)key) & 0x8001) == 0x8001 || (Input.GetAsyncKeyState((int)key) & 0x8000) == 0x8000)
                return true;
            return false;
        }

        public static void SendKeyPressed(byte key)
        {
            if (keys[key] == 0x01)
                return;
            InputReceiver[] rec = receivers.ToArray();
            foreach (InputReceiver receiver in rec)
            {
                receiver.KeyPressed(key);
            }
            keys[key] = 0x01;
        }

        public static void SendKeyReleased(byte key)
        {
            if (keys[key] == 0x00)
                return;
            InputReceiver[] rec = receivers.ToArray();
            foreach (InputReceiver receiver in rec)
            {
                receiver.KeyReleased(key);
            }
            keys[key] = 0x00;
        }

        public static void WheelChanged(int value)
        {
            //if (wheel == value)
            //    return;
            //wheel = value;
            //InputReceiver[] rec = receivers.ToArray();
            //foreach (InputReceiver receiver in rec)
            //{
            //    receiver.wheelChanged(value - wheel);
            //}
            //zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', "WheelValue: " + value, 0, "InputHooked.cs", 117);
            if (value == 0)
                return;

            InputReceiver[] rec = receivers.ToArray();
            foreach (InputReceiver receiver in rec)
            {
                receiver.wheelChanged(value);
            }
        }

        public static void Update()
        {
            //if ((Input.GetAsyncKeyState((int)VirtualKeys.F11) & 0x8001) == 0x8001)
            for (byte i = 1; i <= 0xFE; i++)
            {
                if (IsPressed(i))
                {
                    if (keys[i] == 0x00)
                    {
                        SendKeyPressed(i);
                    }
                }
                else
                {
                    if (keys[i] != 0x00)
                    {
                        SendKeyReleased(i);
                    }
                }
            }
        }
    }
}
