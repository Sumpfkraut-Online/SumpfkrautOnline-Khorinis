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
using GUC.Server.Scripts.Sumpfkraut.CommandConsole.InfoObjects;
using GUC.Server.WorldObjects;

namespace GUC.Server.Scripts.Sumpfkraut.Web.WS
{
    public class WSServer : GUC.Utilities.ExtendedObject
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

            try
            {
                String json = context.DataFrame.ToString();
                String returnJson;
                WSChatProtocol protocol = JsonConvert.DeserializeObject<WSChatProtocol>(json);
                protocol.context = context;

                String cmd;
                String[] param;
                CommandConsole.CommandConsole.ProcessCommand processCmd = null;
                Dictionary<String, object> returnData = null;

                // group commands and parameters and process them
                bool firstCmd = true;
                int cmdIndex = -1;
                int lastCmdIndex  = -1;
                int tempIndex = -1;
                int paramLength = -1;
                for (int i = 0; i < protocol.cmds.Length; i++)
                {
                    tempIndex = protocol.cmds[i].IndexOf('/');
                    paramLength++;

                    if (i < (protocol.cmds.Length - 1))
                    {
                        // if there are still components to iterate over
                        if (tempIndex != 0)
                        {
                            // not a command -> continue to count further till
                            // next command or end of array is reached
                            continue;
                        }

                        if (firstCmd)
                        {
                            // very first command
                            continue;
                        }
                        else
                        {
                            // n-th command
                            // process previous command with full range of its parameters
                            param = new String[paramLength];
                            firstCmd = false;
                            lastCmdIndex = cmdIndex;
                            cmdIndex = i;
                        }
                    }
                    else
                    {
                        // no components left to iterate over
                        // process current command with or without parameters
                        if (firstCmd)
                        {
                            // only very first command, no parameters
                            lastCmdIndex = cmdIndex;
                            param = new String[paramLength];
                        }
                        else
                        {
                            // n-th command
                            param = new String[paramLength];
                        }
                    }

                    if (lastCmdIndex < 0)
                    {
                        lastCmdIndex = 0;
                    }

                    // pick and format command-string itself
                    cmd = protocol.cmds[lastCmdIndex];
                    cmd = cmd.ToUpper();
                    if (paramLength > 0)
                    {
                        // grab all relevant parameters if there are any for this command
                        Array.Copy(protocol.cmds, lastCmdIndex + 1, param, 0, paramLength);
                    }
                    
                    if (!CommandConsole.CommandConsole.CmdToProcessFunc.TryGetValue(
                        cmd, out processCmd))
                    {
                        // command does not map to processing subroutine
                        continue;
                    }

                    // serialize json-string from provided command and parameters
                    // and send everything to the client
                    processCmd(null, cmd, param, out returnData);
                    returnJson = JsonConvert.SerializeObject(returnData);
                    if (returnJson != null)
                    {
                        context.Send(returnJson);
                    }
                }
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
