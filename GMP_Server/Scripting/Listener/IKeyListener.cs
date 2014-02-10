using System;
using System.Collections.Generic;
using System.Text;

namespace GMP_Server.Scripting.Listener
{
    public interface IKeyListener
    {
        void OnKeyAction(Player player, int key, bool pressed, byte times);
    }
}
