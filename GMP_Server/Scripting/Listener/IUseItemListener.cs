using System;
using System.Collections.Generic;
using System.Text;

namespace GMP_Server.Scripting.Listener
{
    public interface IUseItemListener
    {
        void OnUseItem(Player player, string instance);
    }
}
