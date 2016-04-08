using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;

namespace GUC.Scripts.Sumpfkraut.Networking
{
    public partial class ScriptClient : ScriptObject, GameClient.IScriptClient
    {
        public void ReadScriptMsg(PacketReader stream) { }

        public static ScriptClient Client { get { return (ScriptClient)GameClient.Client.ScriptObject; } }
    }
}
