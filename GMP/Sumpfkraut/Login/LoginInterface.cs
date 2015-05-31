using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using WinApi;
using GUC.Sumpfkraut.GUI;
using GUC.States;

using Gothic.zClasses;
using GUC.WorldObjects;
using GUC.WorldObjects.Character;
using GUC.Types;
using Gothic.zTypes;

using GUC.Sumpfkraut.Menus;

namespace GUC.Sumpfkraut.Login
{
    class LoginInterface
    {
        /*==============================
        Initialized in StartupState.Init
        ==============================*/

        private static LoginInterface inter = null;
        public static void Init()
        {
            if (inter == null)
            {
                inter = new LoginInterface();
            }
        }

        private LoginInterface()
        {
            Process proc = Process.ThisProcess();

            proc.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(GUI.ItemRenderer).GetMethod("OnRender"), (int)0x00704B90, (int)7, 0);
            //Process.ThisProcess().Hook("UntoldChapter\\DLL\\GUC.dll", typeof(GUI.ItemRenderer).GetMethod("ItemBiasHook"), (int)0x713AAC, (int)6, 0);

            proc.Write(new byte[] { 233, 229, 2, 0, 0, 0 }, 0x42AE7E); //disable ingame ESC menu

            if (MainMenus._Background == null)
            {
                MainMenus._Background = new GUCMenuTexture("StartScreen.tga");
            }
            MainMenus._Background.Show();

            LoginMessage.GetMsg(); //init
            MainMenus.Main.Open();
        }
    }
}
