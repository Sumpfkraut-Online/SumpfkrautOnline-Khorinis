using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUC.Network;
using GUC.Scripting;
using GUC.Scripts.Sumpfkraut.Networking;

namespace GUC.Scripts
{
    public partial class GUCScripts : ScriptInterface
    {
        public void OnClientConnection(GameClient client)
        {
            new ScriptClient(client);
        }
    }
}
