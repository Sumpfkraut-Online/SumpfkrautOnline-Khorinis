using Alchemy.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Server.Scripts.Sumpfkraut.Web.WS.Protocols
{

    public class WSChatProtocol : AbstractWSProtocol
    {

        // mainly used when sending messages to the server 
        // (standarized commandish communication)
        public String[] cmds;
        // mainly used to send text messages back to the client
        // (no specific text-formatting and -forwarding)
        public String rawText;

    }

}
