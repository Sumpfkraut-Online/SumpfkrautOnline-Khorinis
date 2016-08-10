using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.IO;
using GUC.Network;
using GUC.Options;
using GUC.Log;

namespace GUC
{
    static class TCPListener
    {
        #region NetworkStream extensions

        // cause we want to keep the server .NET 4.0

        const int TimeOutSpan = 1000;

        static bool ReadTimeOut(this NetworkStream stream, byte[] buffer)
        {
            return stream.ReadTimeOut(buffer, 0, buffer.Length);
        }

        static bool ReadTimeOut(this NetworkStream stream, byte[] buffer, int offset, int size)
        {
            IAsyncResult result = stream.BeginRead(buffer, offset, size, null, null);
            bool ret = result.AsyncWaitHandle.WaitOne(TimeOutSpan);
            stream.EndRead(result);
            return ret;
        }

        static bool WriteTimeOut(this NetworkStream stream, byte[] buffer)
        {
            return stream.WriteTimeOut(buffer, 0, buffer.Length);
        }

        static bool WriteTimeOut(this NetworkStream stream, byte[] buffer, int offset, int size)
        {
            IAsyncResult result = stream.BeginWrite(buffer, offset, size, null, null);
            bool ret = result.AsyncWaitHandle.WaitOne(TimeOutSpan);
            stream.EndWrite(result);
            return ret;
        }

        #endregion

        #region Prewritten Infos

        static byte[] statusInfo = new byte[byte.MaxValue + 4];
        static int statusInfoLen = 0;
        public static void UpdateStatusInfo()
        {
            try
            {
                lock (statusInfo)
                {
                    statusInfo[0] = 0;
                    statusInfo[1] = ServerOptions.Slots;
                    statusInfo[2] = ServerOptions.Password == null ? (byte)0 : (byte)1;

                    string name = ServerOptions.ServerName;
                    statusInfo[3] = (byte)name.Length;
                    Encoding.UTF8.GetBytes(name, 0, name.Length, statusInfo, 4);
                    statusInfoLen = 4 + name.Length;
                }
            }
            catch (Exception e)
            {
                Logger.LogError(e);
            }
        }

        static byte[] downloadInfo = new byte[byte.MaxValue + 1];
        static int downloadInfoLen = 0;
        public static void UpdateDownloadInfo()
        {
            try
            {
                lock (downloadInfo)
                {
                    string link = ServerOptions.DataLink;
                    if (link == null)
                    {
                        downloadInfo[0] = 0;
                        downloadInfoLen = 1;
                    }
                    else
                    {
                        downloadInfo[0] = (byte)link.Length;
                        Encoding.UTF8.GetBytes(link, 0, link.Length, downloadInfo, 1);
                        downloadInfoLen = 1 + link.Length;
                    }
                }
            }
            catch (Exception e)
            {
                Logger.LogError(e);
            }
        }

        #endregion

        public static void Run()
        {
            TcpListener listener = new TcpListener(IPAddress.Any, ServerOptions.Port);
            listener.Start();

            UpdateStatusInfo();
            UpdateDownloadInfo();

            byte[] buf = new byte[byte.MaxValue + 2];
            while (true)
            {
                var client = listener.AcceptTcpClient();
                try
                {
                    NetworkStream stream = client.GetStream();

                    if (stream.ReadTimeOut(buf, 0, 1))
                    {
                        switch (buf[0])
                        {
                            case 0:
                                statusInfo[0] = (byte)GameClient.GetCount();
                                stream.WriteTimeOut(statusInfo, 0, statusInfoLen);
                                break;
                            case 1:
                                if (ServerOptions.Password != null)
                                {
                                    if (!stream.ReadTimeOut(buf, 0, 16))
                                        break;

                                    bool same = true;
                                    for (int i = 0; i < 16; i++)
                                        if (buf[i] != ServerOptions.Password[i])
                                        {
                                            same = false;
                                            break;
                                        }

                                    if (!same)
                                    {
                                        buf[0] = 0;
                                        stream.WriteTimeOut(buf, 0, 1);
                                        break;
                                    }
                                }
                                buf[0] = 1;
                                if (ServerOptions.DataLink != null)
                                {
                                    buf[1] = (byte)Encoding.UTF8.GetBytes(ServerOptions.DataLink, 0, ServerOptions.DataLink.Length, buf, 2);
                                    stream.WriteTimeOut(buf, 0, buf[1] + 2);
                                }
                                else
                                {
                                    stream.WriteTimeOut(buf, 0, 1);
                                }
                                break;
                        }
                    }
                }
                catch (Exception e)
                {
                    Logger.LogError(e);
                }
                client.Close();

                Thread.Sleep(50);
            }
        }
    }
}
