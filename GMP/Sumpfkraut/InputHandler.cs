using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.mClasses;
using WinApi;
using Gothic.zClasses;
using Gothic.zTypes;
using GUC.Sumpfkraut.GUI;
using Gothic.zStruct;
using WinApi.User.Enumeration;
using GUC.Sumpfkraut.Menus;

namespace GUC.Sumpfkraut
{
    class InputHandler
    {
        /*public static Dictionary<string, double> AllChars = new Dictionary<string, double>() { { "A", 14.998571428571428571428571428571d }, { "B", 11.998857142857142857142857142857d }, { "C", 10.998857142857142857142857142857d }, { "D", 11.998857142857142857142857142857d }, { "E", 11.998857142857142857142857142857d }, { "F", 10.999142857142857142857142857143d }, { "G", 12.998857142857142857142857142857d }, { "H", 11.998857142857142857142857142857d }, { "I", 4.9997142857142857142857142857143d }, { "J", 10.999142857142857142857142857143d }, { "K", 12.998857142857142857142857142857d },
                                                                                         { "L", 10.999142857142857142857142857143d }, { "M", 14.998571428571428571428571428571d }, { "N", 10.999142857142857142857142857143d }, { "O", 12.998857142857142857142857142857d }, { "P", 11.998857142857142857142857142857d }, { "Q", 12.998857142857142857142857142857d }, { "R", 11.998571428571428571428571428571d }, { "S", 11.998857142857142857142857142857d }, { "T", 11.998857142857142857142857142857d }, { "U", 11.998857142857142857142857142857d }, { "V", 11.998857142857142857142857142857d },
                                                                                         { "W", 14.998571428571428571428571428571d }, { "X", 13.998571428571428571428571428571d }, { "Y", 10.999142857142857142857142857143d }, { "Z", 10.999142857142857142857142857143d }, { "a", 10.999142857142857142857142857143d }, { "b", 10.999142857142857142857142857143d }, { "c", 9.9991428571428571428571428571429d }, { "d", 10.999142857142857142857142857143d }, { "e", 10.999142857142857142857142857143d }, { "f", 8.9991428571428571428571428571429d }, { "g", 10.999142857142857142857142857143d },
                                                                                         { "h", 10.999142857142857142857142857143d }, { "i", 5.9994285714285714285714285714286d }, { "j", 4.9997142857142857142857142857143d }, { "k", 9.9991428571428571428571428571429d }, { "l", 4.9997142857142857142857142857143d }, { "m", 12.998857142857142857142857142857d }, { "n", 10.999142857142857142857142857143d }, { "o", 10.999142857142857142857142857143d }, { "p", 10.999142857142857142857142857143d }, { "q", 10.999142857142857142857142857143d }, { "r", 8.9991428571428571428571428571429d },
                                                                                         { "s", 10.999142857142857142857142857143d }, { "t", 7.9994285714285714285714285714286d }, { "u", 10.999142857142857142857142857143d }, { "v", 10.999142857142857142857142857143d }, { "w", 13.998571428571428571428571428571d }, { "x", 10.999142857142857142857142857143d }, { "y", 10.999142857142857142857142857143d }, { "z", 9.9991428571428571428571428571429d }, { "Ä", 14.998571428571428571428571428571d }, { "Ü", 11.998857142857142857142857142857d }, { "Ö", 12.998857142857142857142857142857d },
                                                                                         { "ä", 10.999142857142857142857142857143d }, { "ü", 10.999142857142857142857142857143d }, { "ö", 10.999142857142857142857142857143d }, { "ß", 11.998857142857142857142857142857d }, { "1", 5.9994285714285714285714285714286d }, { "2", 8.9991428571428571428571428571429d }, { "3", 8.9991428571428571428571428571429d }, { "4", 10.999142857142857142857142857143d }, { "5", 7.9994285714285714285714285714286d }, { "6", 7.9994285714285714285714285714286d }, { "7", 8.9991428571428571428571428571429d }, 
                                                                                         { "8", 7.9994285714285714285714285714286d }, { "9", 7.9994285714285714285714285714286d }, { "0", 8.9991428571428571428571428571429d }, { "!", 3.9997142857142857142857142857143d }, { "\"", 6.9994285714285714285714285714286d }, { "§", 7.9994285714285714285714285714286d }, { "$", 7.9994285714285714285714285714286d }, { "%", 10.999142857142857142857142857143d }, { "&", 11.998857142857142857142857142857d }, { "/", 7.9994285714285714285714285714286d }, { "(", 5.9994285714285714285714285714286d }, 
                                                                                         { ")", 4.9997142857142857142857142857143d }, { "=", 7.9994285714285714285714285714286d }, { "?", 7.9994285714285714285714285714286d }, { "_", 10.999142857142857142857142857143d }, { "-", 6.9994285714285714285714285714286d }, { ".", 4.9997142857142857142857142857143d }, { ":", 3.9997142857142857142857142857143d }, { ",", 4.9997142857142857142857142857143d }, { ";", 3.9997142857142857142857142857143d }, { "<", 8.9991428571428571428571428571429d }, { ">", 8.9991428571428571428571428571429d },
                                                                                         { "|", 2.9997142857142857142857142857143d }, { "#", 12.998857142857142857142857142857d }, { "'", 4.9997142857142857142857142857143d }, { "+", 8.9991428571428571428571428571429d }, { "~", 9.9991428571428571428571428571429d }, { "{", 5.9994285714285714285714285714286d }, { "}", 5.9994285714285714285714285714286d }, { "@", 13.998571428571428571428571428571d }, {" ", 9.9991428571428571428571428571429d } };*/

        public static Dictionary<char, float> AllChars = new Dictionary<char, float>() { { 'A', 14.99857f }, { 'B', 11.99886f }, { 'C', 10.99886f }, { 'D', 11.99886f }, { 'E', 11.99886f }, { 'F', 10.99914f }, { 'G', 12.99886f }, { 'H', 11.99886f }, { 'I', 4.99971f }, { 'J', 10.99914f }, { 'K', 12.99886f },
                                                                                         { 'L', 10.99914f }, { 'M', 14.99857f }, { 'N', 10.99914f }, { 'O', 12.99886f }, { 'P', 11.99886f }, { 'Q', 12.99886f }, { 'R', 11.99857f }, { 'S', 11.99886f }, { 'T', 11.99886f }, { 'U', 11.99886f }, { 'V', 11.99886f },
                                                                                         { 'W', 14.99857f }, { 'X', 13.99857f }, { 'Y', 10.99914f }, { 'Z', 10.99914f }, { 'a', 10.99914f }, { 'b', 10.99914f }, { 'c', 9.99914f  }, { 'd', 10.99914f }, { 'e', 10.99914f }, { 'f', 8.99914f }, { 'g', 10.99914f },
                                                                                         { 'h', 10.99914f }, { 'i', 5.99943f }, { 'j', 4.99971f }, { 'k', 9.99914f }, { 'l', 4.99971f }, { 'm', 12.99886f }, { 'n', 10.99914f }, { 'o', 10.99914f }, { 'p', 10.99914f }, { 'q', 10.99914f }, { 'r', 8.99914f },
                                                                                         { 's', 10.99914f }, { 't', 7.99943f }, { 'u', 10.99914f }, { 'v', 10.99914f }, { 'w', 13.99857f }, { 'x', 10.99914f }, { 'y', 10.99914f }, { 'z', 9.99914f }, { 'Ä', 14.99857f }, { 'Ü', 11.99886f }, { 'Ö', 12.99886f },
                                                                                         { 'ä', 10.99914f }, { 'ü', 10.99914f }, { 'ö', 10.99914f }, { 'ß', 11.99886f }, { '1', 5.99943f }, { '2', 8.99914f }, { '3', 8.99914f }, { '4', 10.99914f }, { '5', 7.99943f }, { '6', 7.99943f }, { '7', 8.99914f }, 
                                                                                         { '8', 7.99943f }, { '9', 7.99943f }, { '0', 8.99914f }, { '!', 3.99971f }, { '"', 6.99943f }, { '§', 7.99943f }, { '$', 7.99943f }, { '%', 10.99914f }, { '&', 11.99886f }, { '/', 7.99943f }, { '(', 5.99943f }, 
                                                                                         { ')', 4.99971f }, { '=', 7.99943f }, { '?', 7.99943f }, { '_', 10.99914f }, { '-', 6.99943f }, { '.', 4.99971f }, { ':', 3.99971f }, { ',', 4.99971f }, { ';', 3.99971f }, { '<', 8.99914f }, { '>', 8.99914f },
                                                                                         { '|', 2.99971f }, { '#', 12.99886f }, { '\'', 4.99971f }, { '+', 8.99914f }, { '~', 9.99914f }, { '{', 5.99943f }, { '}', 5.99943f }, { '@', 13.99857f }, {' ', 9.99914f } };

        public const int DefaultFontYPixels = 18;

        public static int[] PixelToVirtual(int x, int y)
        {
            int[] screenSize = InputHooked.GetScreenSize(Program.process);
            return new int[] { x*0x2000/screenSize[0], y*0x2000/screenSize[1] };
        }

        public static int StringPixelWidth(string str)
        {
            double size = 0;
            foreach (char c in str)
            {
                if (!InputHandler.AllChars.ContainsKey(c))
                    continue;

                size += (double)InputHandler.AllChars[c];
            }

            return (int)size;
        }

        public InputHandler()
        {
        }

        private static zTSound3DParams param;
        private static zCSndSys_MSS ss = null;
        private static zCVob soundvob;
        public static void PlaySound(string name)
        {
            if (ss == null)
            {
                param = zTSound3DParams.Create(Process.ThisProcess());
                ss = zCSndSys_MSS.SoundSystem(Process.ThisProcess());
                soundvob = zCVob.Create(Process.ThisProcess());
            }

            using (zString z = zString.Create(Process.ThisProcess(), name))
            {
                ss.PlaySound3D(z, soundvob, 0, param);
            }

            /*using (zString z = zString.Create(Process.ThisProcess(),name))
            {
                Process.ThisProcess().CDECLCALL<NullReturnCall>((uint)0x6CBFD0, new CallValue[] { z });
            }*/
        }

        public delegate void ShortCutHandler();
        public static Dictionary<VirtualKeys, ShortCutHandler> shortCuts = new Dictionary<VirtualKeys, ShortCutHandler>()
        {
            { VirtualKeys.Escape, MainMenus.Main.Open },
            { Ingame.Chat.GetChat().ActivationKey, Ingame.Chat.GetChat().Open },
            { Ingame.Chat.GetChat().ToggleKey, Ingame.Chat.GetChat().ToggleChat },
            { Ingame.Trade.GetTrade().ActivationKey, Ingame.Trade.GetTrade().Activate },
            { Ingame.AnimationMenu.GetMenu().ActivationKey, Ingame.AnimationMenu.GetMenu().Open }
        };

        private static List<GUCInputReceiver> receiver = new List<GUCInputReceiver>();

        public static void activateFullControl(GUCInputReceiver menu)
        { // when one or more menus overlap. When the last is closed, the one before gets the control back
            receiver.Insert(0, menu);
            InputHooked.deaktivateFullControl(Process.ThisProcess());
        }

        public static void deactivateFullControl(GUCInputReceiver menu)
        {
            receiver.Remove(menu);
            if (receiver.Count == 0)
            {
                InputHooked.activateFullControl(Process.ThisProcess());
            }
        }
        
        private static void SendKeyPressed(int key)
        {
            if (receiver.Count == 0) 
            { //check for shortcuts
                foreach (KeyValuePair<VirtualKeys,ShortCutHandler> sc in shortCuts)
                    if (key == (int)sc.Key) sc.Value();
            }
            else //a menu is open
            {
                receiver[0].KeyPressed(key);
            }
        }

        private static long[] keys = new long[0xFF];
        public static void Update()
        {
            if (WinApi.User.Window.GetWindowThreadProcessId(WinApi.User.Window.GetForegroundWindow()) == Process.ThisProcess().ProcessID)
            {
                long ticks = DateTime.Now.Ticks;
                for (int i = 1; i < keys.Length; i++)
                {
                    if (InputHooked.IsPressed(i))
                    {
                        if (keys[i] == 0) //newly pressed
                        {
                            keys[i] = ticks + 5500000;
                            SendKeyPressed(i);
                        }
                        else //hold
                        {
                            if (ticks > keys[i])
                            {
                                keys[i] = ticks + 200000;
                                SendKeyPressed(i);
                            }
                        }
                    }
                    else
                    {
                        if (keys[i] > 0) //release
                        {
                            keys[i] = 0;
                            //SendKeyReleased(i);
                        }
                    }
                    
                }

                if (receiver.Count != 0)
                    receiver[0].Update(ticks);
            }
            else
            {
                Array.Clear(keys, 0, keys.Length);
            }
        }
    }
}
