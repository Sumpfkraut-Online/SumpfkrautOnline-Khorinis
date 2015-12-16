using Alchemy.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Server.Scripts.Sumpfkraut.Web.WS.Protocols
{

    public abstract class AbstractWSProtocol : ScriptObject
    {

        protected  WSProtocolType protocolType;
        public WSProtocolType GetProtocolType () { return this.protocolType; }

        protected UserContext context;
        public UserContext GetContext () { return this.context; }



        public AbstractWSProtocol ()
            : this (null)
        { }

        public AbstractWSProtocol (UserContext context)
            : this (context, WSProtocolType.Unknown)
        { }

        public AbstractWSProtocol (UserContext context, WSProtocolType protocolType)
        {
            this.protocolType = protocolType;

            if (context != null)
            {
                this.context = context;
            }
        }

    }

}
