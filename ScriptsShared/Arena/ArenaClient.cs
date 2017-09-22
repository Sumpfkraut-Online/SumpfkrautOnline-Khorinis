using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUC.Scripts.Sumpfkraut.Networking;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;

namespace GUC.Scripts.Arena
{
    partial class ArenaClient : ScriptClient
    {
        public TOClassDef ClassDef;

        partial void pOnConnect();
        public override void OnConnection()
        {
            pOnConnect();
        }

        partial void pOnDisconnect();
        public override void OnDisconnection()
        {
            pOnDisconnect();
        }

    }
}
