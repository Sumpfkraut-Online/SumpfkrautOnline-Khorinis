using System;
using System.Collections.Generic;
using System.Text;

namespace GMP_Server.Scripting.Listener
{
    public interface IServerStartup
    {
        void OnServerInit();
    }
}
