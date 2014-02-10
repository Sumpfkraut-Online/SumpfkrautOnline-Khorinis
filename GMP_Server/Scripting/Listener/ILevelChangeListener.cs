using System;
using System.Collections.Generic;
using System.Text;

namespace GMP_Server.Scripting.Listener
{
    public interface ILevelChangeListener
    {
        void OnLevelChanged(Player pl, String world, String oldWorld);
    }
}
