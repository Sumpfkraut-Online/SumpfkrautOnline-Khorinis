using System;
using System.Collections.Generic;
using System.Text;
using WinApi;
using Gothic.zClasses;
using Gothic.zTypes;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using WinApi.User.Enumeration;

namespace Gothic.mClasses
{
    public static class Cursor
    {
        private static Boolean pShow;
        public static zCView pView;


        static float cursor_fX = 0;
        static float cursor_fY = 0;
        public static bool noHandle;

        //PTR = 0x8D165C
        //Cursor X = 0x89A148
        //Cursor Y = 0x89A14C

        static Process Process;
        static HookInfos hookInfo = null;
        public static void Init(Process process)
        {
            if (hookInfo != null)
                return;
            Process = process;
            hookInfo = process.Hook("UntoldChapter\\DLL\\Gothic.dll", typeof(Cursor).GetMethod("Update"), 5062907, 5, 0);
        }



        public static float CursorX()
        {
            return cursor_fX;
        }

        public static float CursorY()
        {
            return cursor_fY;
        }

        public static void ToTop(Process Process)
        {
            zCView.GetStartscreen(Process).RemoveItem(pView);
            //TODO: Löschen! 
            pView.Dispose();
            pView = null;
            pShow = false;
            Show(Process);
        }

        public static void Show(Process Process)
        {
            if (pShow)
                return;
            pShow = true;
            if (pView == null)
            {
                pView = zCView.Create(Process, 0, 0, 150, 200);
                zString tex = zString.Create(Process, "Cursor.tga");
                pView.InsertBack(tex);
                tex.Dispose();
            }
            zCView.GetStartscreen(Process).InsertItem(pView, 0);
            noHandle = true;
            //new zCInput_Win32(Process, zCInput.GetInput(Process).Address).SetDeviceEnabled(2, 1);
        }

        public static void Hide(Process Process)
        {
            if (!pShow)
                return;
            zCView.GetStartscreen(Process).RemoveItem(pView);
            noHandle = false;
        }

        public static bool isShow()
        {
            return pShow;
        }

        static float[] keystats = new float[3];
        static bool wasHooked;
        
        public static Int32 Update(String message)
        {
            cursor_fX += Process.ReadInt(9246300) * 40f * Process.ReadFloat(9019720);
            cursor_fY += Process.ReadInt(9246304) * 40f * Process.ReadFloat(9019724);



            if (cursor_fX < 0)
                cursor_fX = 0;
            if (cursor_fX > 7950)
                cursor_fX = 7950;
            if (cursor_fY > 7950)
                cursor_fY = 7950;
            if (cursor_fY < 0)
                cursor_fY = 0;

            InputHooked.WheelChanged(Process.ReadInt(9246300 + 8));

            int keyLeft = Process.ReadInt(9246300 + 12);
            int keyMid = Process.ReadInt(9246300 + 16);
            int keyRight = Process.ReadInt(9246300 + 20);

            if (keystats[0] == 1 && keyLeft == 0)
                InputHooked.SendKeyReleased((byte)VirtualKeys.LeftButton);
            if (keystats[0] == 0 && keyLeft == 1)
                InputHooked.SendKeyPressed((byte)VirtualKeys.LeftButton);
            keystats[0] = keyLeft;


            if (noHandle)
            {
                Process.Write(new byte[24], 9246300);
            }

            if (pView == null)
                return 0;

            pView.Top();
            pView.SetPos((int)cursor_fX, (int)cursor_fY);
            return 0;
        }



    }
}
