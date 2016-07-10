using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.IO;

namespace GUC
{
    public static class TCPStatus
    {
        static TcpListener tcpListener;
        static Thread listenThread;

        public static void Start()
        {
            try
            {
                tcpListener = new TcpListener(IPAddress.Any, Network.GameServer.Options.Port);
                listenThread = new Thread(new ThreadStart(ListenForClients));
                listenThread.Start();
            }
            catch (Exception e)
            {
                Log.Logger.Log("TCP-Listener for Status-Message could not be setup.<br>"+e);
            }
        }

        static void ListenForClients()
        {
            try
            {
                tcpListener.Start();

                while (true)
                {
                    TcpClient client = tcpListener.AcceptTcpClient();

                    ThreadPool.QueueUserWorkItem(HandleClient, client);

                    Thread.Sleep(50);//We only accept 20 Clients per second!
                }
            }
            catch { }
        }

        static void HandleClient(Object state)
        {
            NetworkStream clientStream = null;
            StreamReader sr = null;
            StreamWriter sw = null;
            try
            {
                TcpClient client = (TcpClient)state;
                clientStream = client.GetStream();

                sr = new StreamReader(clientStream);
                sw = new StreamWriter(clientStream);
                sw.AutoFlush = true;

                string type = sr.ReadLine().Trim().ToLower();

                if (type.Equals("getstatus"))
                {
                    sw.WriteLine("giveStatus");
                    sw.WriteLine(Network.GameServer.Options.ServerName);
                    sw.WriteLine(Network.GameServer.Options.Slots);
                }
            }
            catch
            {
            }
            finally
            {
                if (sr != null)
                {
                    sr.Close();
                }
                if (sw != null)
                {
                    sw.Close();
                }
                if (clientStream != null)
                {
                    clientStream.Close();
                }
            }
        }
    }
}
