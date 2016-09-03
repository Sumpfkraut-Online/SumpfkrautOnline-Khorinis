using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using GUC.Log;
using System.Security.Cryptography;
using System.Collections.ObjectModel;

namespace GUC.Options
{
    public static class ServerOptions
    {
        #region Server name

        static string serverName = "ServerName";
        public static string ServerName { get { return serverName; } }
        public static void SetServerName(string newName, bool save = true)
        {
            if (string.IsNullOrWhiteSpace(newName))
                return;

            int byteCount = Encoding.UTF8.GetByteCount(newName);
            if (byteCount > byte.MaxValue)
            {
                Logger.LogError("Server name is too long! Max 255 bytes. Has " + byteCount);
                return;
            }
            
            serverName = newName;
            TCPListener.UpdateStatusInfo();
            Console.Title = serverName;
            if (save) Save();
            Logger.Log("Server name is set to '{0}'", serverName);
        }

        #endregion

        #region Port

        static ushort port = 9054;
        public static ushort Port { get { return port; } }
        public static void SetPort(ushort newPort, bool save = true)
        {
            port = newPort;
            if (save) Save();

            Logger.Log("Server port is set to '{0}'", port);
        }

        public static void SetPort(string newPort, bool save = true)
        {
            ushort res;
            if (ushort.TryParse(newPort, out res))
                SetPort(res, save);
        }

        #endregion

        #region Slots

        static byte slots = 100;
        public static byte Slots { get { return slots; } }
        public static void SetSlots(byte newSlots, bool save = true)
        {
            if (newSlots == 0)
                return;

            slots = newSlots;
            if (save) Save();

            Logger.Log("Slot count is set to '{0}'", slots);
        }

        public static void SetSlots(string newSlots, bool save = true)
        {
            byte res;
            if (byte.TryParse(newSlots, out res))
                SetSlots(res, save);
        }

        #endregion

        #region Password

        static byte[] password = null;
        public static ReadOnlyCollection<byte> Password { get; private set; }

        public static void SetPassword(string newPassword, bool save = true)
        {
            byte[] newHash;
            if (newPassword == null)
            {
                newHash = null;
            }
            else
            {
                using (MD5 md5 = new MD5CryptoServiceProvider())
                {
                    newHash = md5.ComputeHash(Encoding.Unicode.GetBytes(newPassword));
                }
            }
            SetPassword(newHash, save);
        }

        static void SetPassword(byte[] newHash, bool save)
        {
            if (newHash == null || newHash.Length != 16)
            {
                password = null;
                Password = null;
                Network.GameServer.ServerInterface.SetIncomingPassword(default(string), 0);
                Logger.Log("Password removed.");
            }
            else
            {
                password = newHash;
                Password = new ReadOnlyCollection<byte>(password);
                string pwStr = Convert.ToBase64String(password);
                Network.GameServer.ServerInterface.SetIncomingPassword(pwStr, pwStr.Length);
                Logger.Log("Password is set.");
            }

            TCPListener.UpdateStatusInfo();
            if (save) Save();

        }

        #endregion

        #region Data link

        static string dataLink = null;
        public static string DataLink { get { return dataLink; } }
        public static void SetDataLink(string newLink, bool save = true)
        {
            if (string.IsNullOrWhiteSpace(newLink))
            {
                dataLink = null;
            }
            else
            {
                int byteCount = Encoding.UTF8.GetByteCount(newLink);
                if (byteCount > byte.MaxValue)
                {
                    Logger.LogError("Data link is too long! Max 255 bytes! Has " + byteCount);
                    return;
                }
                dataLink = newLink;
            }
            TCPListener.UpdateDownloadInfo();
            if (save) Save();

            Logger.Log("Data link is set to '{0}'", dataLink);
        }

        #endregion

        #region Save & Load

        const string ConfigFile = "Config/Server.cfg";

        public static void Save()
        {
            if (!Directory.Exists("Config"))
                Directory.CreateDirectory("Config");

            using (StreamWriter sw = new StreamWriter(ConfigFile))
            {
                sw.Write("ServerName="); sw.WriteLine(serverName);
                sw.Write("Port="); sw.WriteLine(port);
                sw.Write("Slots="); sw.WriteLine(slots);
                if (password != null)
                {
                    sw.Write("Password="); sw.WriteLine(Convert.ToBase64String(password));
                }
                if (dataLink != null)
                {
                    sw.Write("DataLink="); sw.WriteLine(dataLink);
                }
            }
        }

        public static void Load()
        {
            if (File.Exists(ConfigFile))
            {
                using (StreamReader sr = new StreamReader(ConfigFile))
                {
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        if (line.StartsWith("ServerName="))
                        {
                            SetServerName(line.Substring(11), false);
                        }
                        else if (line.StartsWith("Port="))
                        {
                            ushort newPort;
                            if (ushort.TryParse(line.Substring(5), out newPort))
                            {
                                SetPort(newPort, false);
                            }
                        }
                        else if (line.StartsWith("Slots="))
                        {
                            byte newSlots;
                            if (byte.TryParse(line.Substring(6), out newSlots))
                            {
                                SetSlots(newSlots, false);
                            }
                        }
                        else if (line.StartsWith("Password="))
                        {
                            SetPassword(Convert.FromBase64String(line.Substring(9)), false);
                        }
                        else if (line.StartsWith("DataLink="))
                        {
                            SetDataLink(line.Substring(9), false);
                        }
                    }
                }
            }
            Save(); // save corrected version or create file
        }

        #endregion

        #region Commands

        static readonly Dictionary<string, Action<string>> cmdArgList = new Dictionary<string, Action<string>>()
        {
            { "SetServerName".ToUpper(), arg => SetServerName(arg) },
            { "SetPort".ToUpper(), arg => SetPort(arg) },
            { "SetSlots".ToUpper(), arg => SetSlots(arg) },
            { "SetPassword".ToUpper(), arg => SetPassword(arg) },
            { "SetDatalink".ToUpper(), arg => SetDataLink(arg) },
        };

        static readonly Dictionary<string, Action> cmdList = new Dictionary<string, Action>()
        {
            { "GetServerName".ToUpper(), () => Logger.Log(serverName) },
            { "GetPort".ToUpper(), () => Logger.Log(port) },
            { "GetSlots".ToUpper(), () => Logger.Log(slots) },
            { "GetDatalink".ToUpper(), () => Logger.Log(dataLink == null ? "" : dataLink) },
            { "SetPassword".ToUpper(), () => SetPassword(null) },
            { "RemovePassword".ToUpper(), () => SetPassword(null) },
            { "GetPassword".ToUpper(), () => Logger.Log(password == null ? "No password is set." : "A password is set.") }
        };

        internal static bool ProcessCmd(string line)
        {
            int index = line.IndexOf(' ');
            if (index == -1)
            {
                Action action;
                if (cmdList.TryGetValue(line.ToUpper(), out action))
                {
                    action();
                    return true;
                }
            }
            else
            {
                Action<string> action;
                if (cmdArgList.TryGetValue(line.Remove(index).ToUpper(), out action))
                {
                    action(line.Substring(index + 1));
                    return true;
                }
            }

            return false;
        }

        #endregion
    }
}
