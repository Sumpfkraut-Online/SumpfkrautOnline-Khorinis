using Alchemy.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Server.Scripts.Sumpfkraut.Web.WS.Protocols
{

    public class WSChatProtocol : AbstractWSProtocol
    {

        protected String rawText;
        public String GetRawText () { return this.rawText; }



        public WSChatProtocol (String rawText)
            : this (null, rawText)
        { }

        public WSChatProtocol (UserContext context, String rawText)
            : base (context, WSProtocolType.ChatData)
        {
            this.rawText = rawText;
        }

    }

}
