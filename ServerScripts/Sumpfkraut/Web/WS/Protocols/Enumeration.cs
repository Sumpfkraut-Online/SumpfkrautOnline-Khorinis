using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Server.Scripts.Sumpfkraut.Web.WS.Protocols
{

    public enum WSProtocolType
    {
        Unknown             = 0,
        UserData            = Unknown + 1,
        ChatData            = UserData + 1,
        VobData             = ChatData + 1,
    }

}
