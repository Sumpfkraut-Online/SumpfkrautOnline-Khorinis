using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.Session;
using GUC.Log;

namespace GUC
{
    public static class GothicGlobals
    {
        static oCGame game = oCGame.GetGame();
        public static oCGame Game { get { return game; } }
        internal static void UpdateGameAddress()
        {
            Logger.Log("GothicGlobals.Game is updated!");
            game = oCGame.GetGame();
        }
    }
}
