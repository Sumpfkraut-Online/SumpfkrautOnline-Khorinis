using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using WinApi;
using GUC.Sumpfkraut.Ingame.GUI;
using GUC.WorldObjects.Character;

namespace GUC.Sumpfkraut.Ingame
{
    class IngameInterface
    {
        /*========================================
        Initialized in the ConnectionMessage-class
        ========================================*/

        private static IngameInterface inter = null;
        public static void Init()
        {
            if (inter == null)
            {
                inter = new IngameInterface();
            }
        }

        
        private IngameInterface()
        {
            Process.ThisProcess().Hook("UntoldChapter\\DLL\\GUC.dll", typeof(GUI.ItemRenderer).GetMethod("OnRender"), (int)0x00704B90, (int)7, 0);

            if (!Program.client.messageListener.ContainsKey((byte)NetworkID.ChatMessage))
                Program.client.messageListener.Add((byte)NetworkID.ChatMessage, new Ingame.Chat());

            new Trade();

            new AnimationMenu();
        }
    }
}
