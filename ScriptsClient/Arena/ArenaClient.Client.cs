using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripts.Sumpfkraut.Networking;
using GUC.Network;

namespace GUC.Scripts.Arena
{
    partial class ArenaClient
    {
        new public static ArenaClient Client { get { return (ArenaClient)ScriptClient.Client; } }

        public static void SendCharCreationMessage(CharCreationInfo info)
        {
            var stream = GetScriptMessageStream();
            stream.Write((byte)ScriptMessages.CharCreation);
            info.Write(stream);
            SendScriptMessage(stream, PktPriority.High, PktReliability.Reliable);
        }
    }
}
