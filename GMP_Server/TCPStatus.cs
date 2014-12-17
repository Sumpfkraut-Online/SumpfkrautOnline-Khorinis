using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.IO;

namespace GUC.Server
{
    public class TCPStatus
    {
        static TCPStatus status;
        public static TCPStatus getTCPStatus()
        {
            if (status == null)
            {
                status = new TCPStatus();
                status.start();
            }
            
            return status;
        }
        protected TCPStatus()
        {

        }

        protected Dictionary<String, String> info = new Dictionary<string, string>();

        private TcpListener tcpListener;
        private Thread listenThread;

        public void addInfo(String key, String value)
        {
            lock (info)
            {
                key = key.ToLower().Trim();
                value = value.Trim();
                if (info.ContainsKey(key))
                    info[key] = value;
                else
                    info.Add(key, value);
            }
        }

        protected void start()
        {
            try
            {
                this.tcpListener = new TcpListener(IPAddress.Any, Program.serverOptions.Port);
                this.listenThread = new Thread(new ThreadStart(ListenForClients));
                this.listenThread.Start();

            }
            catch (Exception ex)
            {
                Log.Logger.log(Log.Logger.LogLevel.WARNING, "TCP-Listener for Status-Message could not be setup.<br>"+ex.ToString());
            }
        }

        private void ListenForClients()
        {
            try
            {
                this.tcpListener.Start();

                while (true)
                {
                    TcpClient client = this.tcpListener.AcceptTcpClient();

                    ThreadPool.QueueUserWorkItem(HandleClient, client);

                    Thread.Sleep(50);//We only accept 20 Clients per second!
                }
            }
            catch (Exception ex) { }
        }

        private void HandleClient(Object state)
        {
            try
            {
                TcpClient client = (TcpClient)state;
                NetworkStream clientStream = client.GetStream();

                StreamReader sr = new StreamReader(clientStream);
                StreamWriter sw = new StreamWriter(clientStream);
                sw.AutoFlush = true;

                string type = sr.ReadLine().Trim().ToLower();

                if (type.Equals("getstatus"))
                {
                    sw.WriteLine("giveStatus");
                    lock (info)
                    {
                        sw.WriteLine("" + info.Count);
                        foreach (KeyValuePair<String, String> pair in info)
                        {
                            sw.WriteLine(pair.Key.Trim().ToLower() + ":" + pair.Value.Trim());
                        }
                    }
                    sw.WriteLine("servername:TestName!");
                    sw.WriteLine("serverlanguage:English");
                    sw.WriteLine("maxslots:15");
                    sw.WriteLine("players:10");

                }

                sr.Close();
                sw.Close();
                clientStream.Close();

            }
            catch (Exception ex)
            {

            }
        }
    }
}
