using Alchemy.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Server.Scripts.Sumpfkraut.Web.WS.Protocols
{

    public abstract class AbstractWSProtocol : ScriptObject
    {

        public  WSProtocolType protocolType;
        public UserContext context;

    }

}
