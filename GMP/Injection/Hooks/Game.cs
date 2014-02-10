using System;
using System.Collections.Generic;
using System.Text;
using Injection;

namespace GMP.Injection.Hooks
{
    public class Game
    {
        public static Int32 ExitGame(String message)
        {
            Program.client.Disconnect();
            return 0;
        }
    }
}
