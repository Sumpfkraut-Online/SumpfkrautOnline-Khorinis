using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alchemy;
using Alchemy.Classes;
using System.Net;
using Newtonsoft.Json;
using Alchemy.Handlers.WebSocket.rfc6455;
using GUC.Server.Scripts.Sumpfkraut.Web.WS.Protocols;

namespace GUC.Server.Scripts.Sumpfkraut.Web.WS
{
    public class WSServer : ScriptObject
    {

        #region attributes

        new public static readonly String _staticName = "WSServer (static)";

        protected WebSocketServer wsServer;

        protected int port;
        public int GetPort () { return this.port; }
        public void SetPort (int port) { this.port = port; }

        protected WSServerState serverState;
        public WSServerState GetServerState () { return this.serverState; }
        protected void SetServerState (WSServerState serverState) { this.serverState = serverState; }

        protected TimeSpan timeout;
        public TimeSpan GetTimeout () { return this.timeout; }
        public void SetTimeout (TimeSpan timeout)
        {
            this.timeout = timeout;
            if (this.wsServer != null)
            {
                this.wsServer.TimeOut = timeout;
            }
        }

        protected Dictionary<UserContext, WSUser> onlineUserByContext = 
            new Dictionary<UserContext, WSUser>();

        public delegate void OnConnectEventHandler (UserContext context);
        protected event OnConnectEventHandler OnConnect;

        public delegate void OnConnectedEventHandler (UserContext context);
        protected event OnConnectedEventHandler OnConnected;

        public delegate void OnDisconnectEventHandler (UserContext context);
        protected event OnDisconnectEventHandler OnDisconnect;

        public delegate void OnReceiveEventHandler (UserContext context);
        protected event OnReceiveEventHandler OnReceive;

        public delegate void OnSendEventHandler (UserContext context);
        protected event OnSendEventHandler OnSend;

        #endregion



        public WSServer ()
        {
            SetObjName("WSServer (default)");
            SetServerState(WSServerState.undefined);
            SetPort(81);
            SetTimeout(new TimeSpan(0, 10, 0));
        }



        #region runtime control

        public bool Abort ()
        {
            try
            {
                if (wsServer == null)
                {
                    MakeLog("Aborting the websocket server isn't necessary"
                        + " because it is uninitialized!");
                }
                else
                {
                    wsServer.Stop();
                    wsServer.Dispose();
                    wsServer = null;
                }
                SetServerState(WSServerState.undefined);
                return true;
            }
            catch (Exception ex)
            {
                MakeLogError("Couldn't abort websocket server: " + ex);
                return false;
            }
        }

        public bool Init ()
        {
            try
            {
                if (wsServer == null)
                {
                    wsServer = new WebSocketServer(port, IPAddress.Any);
                }
                else
                {
                    wsServer.Stop();
                }
                wsServer.OnConnect = UserConnect;
                wsServer.OnConnected = UserConnected;
                wsServer.OnDisconnect = UserDisconnect;
                wsServer.OnReceive = ReceivedData;
                wsServer.OnSend = SentData;
                wsServer.TimeOut = GetTimeout();
                SetServerState(WSServerState.initialized);
                return true;
            }
            catch (Exception ex)
            {
                SetServerState(WSServerState.undefined);
                return false;
            }
        }

        public bool Restart ()
        {
            try
            {
                if (wsServer == null)
                {
                    MakeLogError("Cannot restart an uninitialized server!");
                    return false;
                }
                wsServer.Restart();
                SetServerState(WSServerState.running);
                return true;
            }
            catch (Exception ex)
            {
                MakeLogError("Couldn't restart websocket server: " + ex);
                return false;
            } 
        }

        public bool Start ()
        {
            try
            {
                if (GetServerState() == WSServerState.undefined)
                {
                    return false;
                }
                if ((wsServer == null) && (!Init()))
                {
                    MakeLogError("Couldn't start websocket server!");
                    return false;
                }
                if (GetServerState() == WSServerState.stopped)
                {
                    wsServer.Restart();
                }
                else
                {
                    wsServer.Start();
                }
                SetServerState(WSServerState.running);
                return true;
            }
            catch (Exception ex)
            {
                MakeLogError("Couldn't start websocket server: " + ex);
                return false;
            }
        }

        public bool Stop ()
        {
            try
            {
                if (wsServer == null)
                {
                    MakeLogError("Cannot stop an uninitialized server!");
                    return false;
                }
                wsServer.Stop();
                SetServerState(WSServerState.stopped);
                return true;
            }
            catch (Exception ex)
            {
                MakeLogError("Couldn't stop websocket server: " + ex);
                return false;
            }
        }

        #endregion



        #region event management for exterior subroutines

        public void AddOnConnect (OnConnectEventHandler func)
        {
            this.OnConnect += func;
        }

        public void RemoveOnConnect (OnConnectEventHandler func)
        {
            this.OnConnect -= func;
        }

        public void AddOnConnected (OnConnectedEventHandler func)
        {
            this.OnConnected += func;
        }

        public void RemoveOnConnected (OnConnectedEventHandler func)
        {
            this.OnConnected -= func;
        }

        public void AddOnDisconnect (OnDisconnectEventHandler func)
        {
            this.OnDisconnect += func;
        }

        public void RemoveOnDisconnect (OnDisconnectEventHandler func)
        {
            this.OnDisconnect -= func;
        }
        
        public void AddOnReceive (OnReceiveEventHandler func)
        {
            this.OnReceive += func;
        }

        public void RemoveOnReceive (OnReceiveEventHandler func)
        {
            this.OnReceive -= func;
        }       

        public void AddOnSend (OnSendEventHandler func)
        {
            this.OnSend += func;
        }

        public void RemoveOnSend (OnSendEventHandler func)
        {
            this.OnSend -= func;
        }

        #endregion



        #region internal event management

        private void UserConnect (UserContext context)
        {
            MakeLog("Client tries to connect from: " +
                context.ClientAddress.ToString());
            
            WSUser user = new WSUser();
            user.context = context;
            user.tempId = Guid.NewGuid().ToString("N");

            onlineUserByContext.Add(context, user);
        }

        protected void UserConnected (UserContext context)
        {
            MakeLog("New client connection from: " +
                context.ClientAddress.ToString());
        }

        private void UserDisconnect (UserContext context)
        {
            onlineUserByContext.Remove(context);
        }

        protected void ReceivedData (UserContext context)
        {
            Print("Received data from: " + context.ClientAddress);

            String json = context.DataFrame.ToString();

            try
            {
                WSChatProtocol obj = JsonConvert.DeserializeObject<WSChatProtocol>(json);
                obj.context = context;
                Print(obj.protocolType + ": " + obj.rawText);

                //foreach (var field in obj)
                //{
                //    Print(field);
                //}

                Dictionary<String, object> returnData = new Dictionary<string, object>
                {
                    { "Hans", "Wurst" },
                    { "Eins", 11 },
                    { "Truth", true },
                    { "SomeArray", new int[] { 0, 1, 2, 3, 4, 9001 } },
                };

                String returnJson = JsonConvert.SerializeObject(returnData);
                context.Send("Got it, pal!: " + returnJson);

                //context.Send("Got it, pal!: " + json);
            }
            catch (Exception ex)
            {
                MakeLogError("Invalid websocket input: " + ex);
                context.Send(ex.ToString());
            }
        }

        protected void SentData (UserContext context)
        {
            // do something in the future
        }

        #endregion

    }
}
