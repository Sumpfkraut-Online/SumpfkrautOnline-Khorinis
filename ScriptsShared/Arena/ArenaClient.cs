using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUC.Scripts.Sumpfkraut.Networking;

namespace GUC.Scripts.Arena
{
    partial class ArenaClient : ScriptClient
    {
        partial void pOnConnect();
        public override void OnConnection()
        {
            pOnConnect();
        }

    }
}
