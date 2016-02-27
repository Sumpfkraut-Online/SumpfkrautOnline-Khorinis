using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;

namespace GUC.Scripts.Sumpfkraut.Networking
{
    public partial class ScriptClient : ScriptObject, GameClient.IScriptClient
    {
        public void OnReadMenuMsg(PacketReader stream) { }
        public void OnReadIngameMsg(PacketReader stream) { }
    }
}
