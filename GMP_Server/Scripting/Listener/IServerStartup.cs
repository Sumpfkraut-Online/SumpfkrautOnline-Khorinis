using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Server.Scripting.Listener
{
    public interface IServerStartup
    {
        void OnServerInit();
    }
}
