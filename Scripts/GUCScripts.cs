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
        public bool OnClientConnection(GameClient client)
        {
            ScriptClient sc = new ScriptClient(client);
            return true;
        }
    }
}
