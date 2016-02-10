using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Network;
using GUC.Network;

namespace GUC.Scripting
{
    public partial interface ScriptInterface
    {
        /// <summary>
        /// Is called on new connection to check whether the client is banned.
        /// </summary>
        bool OnClientValidation(Client client);

        /// <summary>
        /// Is called when a Menu-Message from a client is received.
        /// </summary>
        void OnReadMenuMsg(Client client, PacketReader stream);

        /// <summary>
        /// Is called when an Ingame-Message from a client is received.
        /// </summary>
        void OnReadIngameMsg(Client client, PacketReader stream);
    }
}
