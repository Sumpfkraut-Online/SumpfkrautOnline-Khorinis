using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Security.Cryptography;
using System.IO;

namespace GUCLauncher
{
    class ServerListItem : INotifyPropertyChanged
    {
        string featured;
        public string Featured
        {
            get { return this.featured; }
            set { this.featured = value; Refresh("Featured"); }
        }

        string name;
        public string Name
        {
            get { return this.name; }
            set { this.name = value; Refresh("Name"); }
        }

        string ip;
        public string IP
        {
            get { return this.ip; }
            set { this.ip = value; Refresh("IP"); }
        }

        ushort port;
        public ushort Port
        {
            get { return this.port; }
            set { this.port = value; Refresh("Port"); }
        }

        public string GetFolder() { return this.ip + "-" + this.port; }

        string ping;
        public string Ping
        {
            get { return this.ping; }
            set { this.ping = value; Refresh("Ping"); }
        }

        string players;
        public string Players
        {
            get { return this.players; }
            set { this.players = value; Refresh("Players"); }
        }

        bool hasPW;
        public bool HasPW
        {
            get { return this.hasPW; }
            set { this.hasPW = value; Refresh("Locked"); }
        }

        byte[] password;
        public byte[] Password
        {
            get { return this.password; }
            set
            {
                if (value != null && value.Length != 16)
                    throw new ArgumentException("value.Length");
                this.password = value;
            }
        }

        public string Locked { get { return HasPW ? "Resources/lock.png" : string.Empty; } }

        public ServerListItem(string ip, ushort port)
        {
            this.ip = ip;
            this.port = port;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void Refresh(string val)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(val));
        }

        public void Write(StreamWriter sw)
        {
            sw.WriteLine(this.ip + ":" + this.port);
            sw.WriteLine(this.password == null ? "" : Convert.ToBase64String(this.password));
        }

        public static bool TryGetAddress(string str, out string address, out ushort port)
        {
            if (!string.IsNullOrWhiteSpace(str))
            {
                string[] strs = str.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                if (strs.Length == 2)
                {
                    address = strs[0].Trim();
                    if (!string.IsNullOrWhiteSpace(address) && ushort.TryParse(strs[1].Trim(), out port) && port > 1024)
                    {
                        return true;
                    }
                }
            }
            address = null;
            port = 0;
            return false;
        }

        public static ServerListItem ReadNew(StreamReader sr)
        {
            string addressLine = sr.ReadLine();
            string passwordLine = sr.ReadLine();

            string address;
            ushort port;
            if (TryGetAddress(addressLine, out address, out port))
            {
                ServerListItem item = new ServerListItem(address, port);
                item.Password = string.IsNullOrEmpty(passwordLine) ? null : Convert.FromBase64String(passwordLine);
                return item;
            }
            return null;
        }
    }
}
