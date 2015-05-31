using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using WinApi;
using GUC.Sumpfkraut.GUI;
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
            Chat.GetChat(); //Init
            Trade.GetTrade(); //Init
            AnimationMenu.GetMenu(); //Init
        }
    }
}
