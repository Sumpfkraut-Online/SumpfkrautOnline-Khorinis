using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;

namespace GUC.Mod.Ingame
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
            IngameInput.Init();

            if (!Program.client.messageListener.ContainsKey((byte)NetworkID.ChatMessage))
                Program.client.messageListener.Add((byte)NetworkID.ChatMessage, new ChatMessage());

            if (!Program.client.messageListener.ContainsKey((byte)NetworkID.TradeMessage))
                Program.client.messageListener.Add((byte)NetworkID.TradeMessage, new TradeMessage());
        }
    }
}
