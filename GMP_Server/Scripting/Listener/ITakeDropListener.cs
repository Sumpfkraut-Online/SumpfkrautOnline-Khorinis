using System;
using System.Collections.Generic;
using System.Text;

namespace GMP_Server.Scripting.Listener
{
    public interface ITakeDropListener
    {
        void OnDropItem(Player pl, String itmCode, int amount);
        void OnTakeItem(Player pl, String itmCode, int amount);
    }
}
