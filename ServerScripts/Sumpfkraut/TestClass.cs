using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Alchemy;
using Alchemy.Classes;

namespace GUC.Server.Scripts.Sumpfkraut
{
    class TestClass : ScriptObject
    {
        new static String _objName = "TestClass (default)";

        public TestClass ()
        {
            SetObjName(_objName);
        }



        public void Init ()
        {
            //var server = new WebSocketListener(new IPEndPoint(IPAddress.Any, 8006));
            //var rfc6455 = new vtortola.WebSockets.Rfc6455.WebSocketFactoryRfc6455(server);
            ////var rfc6455 = new vtortola.WebSockets.WebSocketFactory(server);
            //server.Standards.RegisterStandard(rfc6455);
            //server.Start();


            
        }
    }
}
