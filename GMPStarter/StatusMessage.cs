using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Net.NetworkInformation;

namespace GMPStarter
{
    class StatusMessage
    {
        private System.Object lockThis = new System.Object();
        bool loaded = false;

        String ip;
        ushort port;

        String servername = "";
        String serverlanguage = "";

        long ping = 0;
        String maxSlots = "";
        String players = "";
        String[] files = null;


        public StatusMessage(String _ip, ushort _port)
        {
            ip = _ip;
            port = _port;
        }

        public void start()
        {
            //run(null);
            ThreadPool.QueueUserWorkItem(run);
        }

        public void startGetFiles()
        {
            //run(null);
            ThreadPool.QueueUserWorkItem(getFiles);
        }


        public bool isLoaded
        {
            get { lock(lockThis){ return loaded;} }
        }

        public String IP { get { return this.ip; } }
        public ushort Port { get { return this.port; } }

        public String ServerName { get {
            if (!isLoaded)
                return null;
            else
                return servername;
        } }

        public String ServerLanguage
        {
            get
            {
                if (!isLoaded)
                    return null;
                else
                    return serverlanguage;
            }
        }

        public String MaxSlots
        {
            get
            {
                if (!isLoaded)
                    return null;
                else
                    return this.maxSlots;
            }
        }

        public String PlayerCount
        {
            get
            {
                if (!isLoaded)
                    return null;
                else
                    return players;
            }
        }

        public String Ping
        {
            get
            {
                if (!isLoaded)
                    return null;
                else
                    return ""+ping;
            }
        }

        public static IPEndPoint getIPFromIPorHost(String host, int port)
        {
            IPAddress[] hostInfo = System.Net.Dns.GetHostAddresses(host);
            if (hostInfo.Length == 0)
            {
                return new IPEndPoint(IPAddress.Parse(host), port);
            }
            else
            {
                return new IPEndPoint(hostInfo[0], port);
            }

        }

        protected void run(Object state)
        {
            try
            {
                this.ping = new Ping().Send(IP).RoundtripTime;
                TcpClient client = new TcpClient();



                IPEndPoint serverEndPoint = getIPFromIPorHost(ip, port);

                try
                {
                    client.Connect(serverEndPoint);
                }
                catch (Exception ex)
                {
                    return;
                }

                NetworkStream clientStream = client.GetStream();
                StreamReader sr = new StreamReader(clientStream);
                StreamWriter sw = new StreamWriter(clientStream);
                sw.AutoFlush = true;

                sw.WriteLine("getStatus");

                String type = sr.ReadLine().Trim().ToLower();
                if (type.Equals("givestatus"))
                {
                    uint infoCount = 0;
                    UInt32.TryParse(sr.ReadLine(), out infoCount);
                    for (int i = 0; i < infoCount; i++)
                    {
                        String completeInfo = sr.ReadLine();
                        String[] infoValues = completeInfo.Split(new char[] { ':' }, 2);
                        if (infoValues.Length != 2)
                            continue;
                        if (infoValues[0].Length <= 1)
                            continue;

                        infoValues[0] = infoValues[0].Trim().ToLower();
                        if (infoValues[0].Equals("servername"))
                        {
                            this.servername = infoValues[1].Trim();
                        }
                        else if (infoValues[0].Equals("serverlanguage"))
                        {
                            this.serverlanguage = infoValues[1].Trim();
                        }
                        else if (infoValues[0].Equals("maxslots"))
                        {
                            this.maxSlots = infoValues[1].Trim();
                        }
                        else if (infoValues[0].Equals("players"))
                        {
                            this.players = infoValues[1].Trim();
                        }
                    }
                    


                }
            }
            catch (Exception ex)
            {  }

            lock (lockThis)
            {
                loaded = true;
            }
        }


        Dictionary<String, String> fileList = new Dictionary<string, string>();

        public Dictionary<String, String> Files { get{
            lock(fileList){
                return fileList;
            } } }
        protected void getFiles(Object state)
        {
            try
            {
                this.ping = new Ping().Send(IP).RoundtripTime;
                TcpClient client = new TcpClient();



                IPEndPoint serverEndPoint = getIPFromIPorHost(ip, port);

                client.Connect(serverEndPoint);

                NetworkStream clientStream = client.GetStream();
                StreamReader sr = new StreamReader(clientStream);
                StreamWriter sw = new StreamWriter(clientStream);
                sw.AutoFlush = true;

                sw.WriteLine("getfiles");

                String type = sr.ReadLine().Trim().ToLower();
                if (type.Equals("getfiles"))
                {
                    uint infoCount = 0;
                    UInt32.TryParse(sr.ReadLine(), out infoCount);
                    lock (fileList)
                    {
                        for (int i = 0; i < infoCount; i++)
                        {
                            String completeInfo = sr.ReadLine();
                            String[] infoValues = completeInfo.Split(new char[] { '§' }, 2);
                            if (infoValues.Length != 2)
                                continue;
                            if (infoValues[0].Length <= 1)
                                continue;

                            fileList.Add(infoValues[0], infoValues[1]);

                        }

                    }

                }
            }
            catch (Exception ex)
            { }

            lock (lockThis)
            {
                loaded = true;
            }
        }





    }
}
