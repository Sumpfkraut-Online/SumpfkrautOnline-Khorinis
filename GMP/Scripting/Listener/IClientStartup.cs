using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripting.Listener
{
    public interface IClientStartup
    {
        void OnClientInit();
    }
}
