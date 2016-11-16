using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.Scripts.Sumpfkraut.Networking;

namespace GUC.Scripts.Left4Gothic
{
    partial class L4Client
    {
        new public static L4Client Client { get { return (L4Client)ScriptClient.Client; } }

        public static void SendCharCreationMessage(CharacterInfo info)
        {
            var stream = GetScriptMessageStream();
            stream.Write((byte)ScriptMessageIDs.CharCreation);
            info.Write(stream);
            SendScriptMessage(stream, PktPriority.High, PktReliability.Reliable);
        }
    }
}
