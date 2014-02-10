using System;
using System.Collections.Generic;
using System.Text;
using Network;

namespace GMP_Server.Helper
{
    public class Kick
    {
        public static void kick(Player pl)
        {
            Program.server.server.CloseConnection(pl.guid, true);
        }

        public static void ban(Player pl)
        {
            kick(pl);

            
        }

        public static bool isBanned(String x, String y, String z)
        {

            return false;
        }
    }
}
