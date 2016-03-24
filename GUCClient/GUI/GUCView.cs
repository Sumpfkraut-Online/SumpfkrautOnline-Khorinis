using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.zClasses;

namespace GUC.Client.GUI
{
    interface InputReceiver
    {
        void KeyPressed(WinApi.User.Enumeration.VirtualKeys key);
    }

    abstract class GUCView
    {
        public abstract void Show();
        public abstract void Hide();

        #region Fonts
        public enum Fonts
        {
            Default,
            Default_Hi,
            Menu,
            Menu_Hi
        }

        public const int FontsizeDefault = 18;
        public const int FontsizeMenu = 32;

        protected static Dictionary<Fonts, string> fontDict = new Dictionary<Fonts, string>
        {
            { Fonts.Default, "Font_Old_10_White.tga"},
            { Fonts.Default_Hi, "Font_Old_10_White_Hi.tga"},
            { Fonts.Menu, "Font_Old_20_White.tga"},
            { Fonts.Menu_Hi, "Font_Old_20_White_Hi.tga"}
        };
        #endregion

        #region pixel virtual conversion
        public static int[] GetScreenSize()
        {
            //return new int[2] { Program.Process.ReadInt(0x08DFC8), Program.Process.ReadInt(0x08DFCC) };
            return new int[] { 
                Convert.ToInt32(zCOption.GetOption(Program.Process).getEntryValue("VIDEO", "zVidResFullscreenX")), 
                Convert.ToInt32(zCOption.GetOption(Program.Process).getEntryValue("VIDEO", "zVidResFullscreenY")) 
            };
        }

        public static int[] PixelToVirtual(int x, int y)
        {
            return new int[] 
            { 
                x * 0x2000 / GetScreenSize()[0],
                y * 0x2000 / GetScreenSize()[1] 
            };
        }

        public static int PixelToVirtualX(int x)
        {
            return x * 0x2000 / GetScreenSize()[0];
        }

        public static int PixelToVirtualY(int y)
        {
            return y * 0x2000 / GetScreenSize()[1];
        }

        public static int StringPixelWidth(string str)
        {
            float size = 0;
            float add;
            for (int i = 0; i < str.Length; i++)
            {
                add = 0;
                InputHandler.AllChars.TryGetValue(str[i], out add);
                size += add;
            }
            return (int)size;
        }
        #endregion

        static private GUI.GUCVisualText debugText;
        static public GUI.GUCVisualText DebugText
        {
            get
            {
                if (debugText == null)
                {
                    var vis = new GUI.GUCVisual();
                    debugText = vis.CreateTextCenterX("", 5);
                }
                debugText.Parent.Show();
                return debugText;
            }
        }
    }
}
