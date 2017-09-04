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
        public NPCInst Enemy { get; private set; }

        TOClassDef classDef;
        public TOClassDef ClassDef { get { return classDef; } }

        partial void pOnConnect();
        public override void OnConnection()
        {
            pOnConnect();
        }

    }
}
