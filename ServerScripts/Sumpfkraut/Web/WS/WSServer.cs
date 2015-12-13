using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alchemy;
using Alchemy.Classes;
using System.Net;
using Newtonsoft.Json;
using Alchemy.Handlers.WebSocket.rfc6455;

namespace GUC.Server.Scripts.Sumpfkraut.Web.WS
{
    public class WSServer : ScriptObject
    {

        new public static readonly String _staticName = "WSServer (static)";

        protected WebSocketServer wsServer;
        protected Dictionary<EndPoint, WSUser> onlineUsersByEndPoint = 
            new Dictionary<EndPoint, WSUser>();



        public WSServer ()
        {
            SetObjName("WSServer (default)");
        }



        public void Init ()
        {
            wsServer = new WebSocketServer(81, IPAddress.Any);
            wsServer.OnConnect = OnConnect;
            wsServer.OnConnected = OnConnected;
            wsServer.OnDisconnect = OnDisconnect;
            wsServer.OnReceive = OnReceive;
            wsServer.OnSend = OnSend;
            wsServer.TimeOut = new TimeSpan(0, 5, 0);
            wsServer.Start();
        }

        

        private void OnConnect (UserContext context)
        {
            MakeLog("Client tries to connect from: " +
                context.ClientAddress.ToString());

            WSUser user = new WSUser();
            user.context = context;      
            onlineUsersByEndPoint.Add(user.context.ClientAddress, user);
        }

        protected void OnConnected (UserContext context)
        {
            MakeLog("New client connection from: " +
                context.ClientAddress.ToString());
        }

        private void OnDisconnect (UserContext context)
        {
            onlineUsersByEndPoint.Remove(context.ClientAddress);
        }

        protected void OnReceive (UserContext context)
        {
            Print("Received data from: " + context.ClientAddress);

            //object data = context.Data;
            //Alchemy.Handlers.WebSocket.rfc6455.DataFrame dataFrame = 
            //    (Alchemy.Handlers.WebSocket.rfc6455.DataFrame) context.DataFrame;

            String json = context.DataFrame.ToString();

            try
            {
                dynamic obj = JsonConvert.DeserializeObject(json);

                foreach (var field in obj)
                {
                    Print(field);
                }

                context.Send("Got it, pal!");
            }
            catch (Exception ex)
            {
                MakeLogError(ex);
                context.Send(ex.ToString());
            }

            //try
            //{

            //    String jsonText = context.DataFrame.ToString();
            //    Print(jsonText);

            //    //// <3 dynamics
            //    //dynamic obj = JsonConvert.DeserializeObject(json);

            //    //switch ((int)obj.Type)
            //    //{
            //    //    case (int)CommandType.Register:
            //    //        Register(obj.Name.Value, context);
            //    //        break;
            //    //    case (int)CommandType.Message:
            //    //        ChatMessage(obj.Message.Value, context);
            //    //        break;
            //    //    case (int)CommandType.NameChange:
            //    //        NameChange(obj.Name.Value, context);
            //    //        break;
            //    //}
            //}
            //catch (Exception ex) // Bad JSON! For shame.
            //{
            //    //var r = new Response { Type = ResponseType.Error, Data = new { e.Message } };

            //    //context.Send(JsonConvert.SerializeObject(r));
            //}
        }

        protected void OnSend (UserContext context)
        {
            
        }

    }
}
