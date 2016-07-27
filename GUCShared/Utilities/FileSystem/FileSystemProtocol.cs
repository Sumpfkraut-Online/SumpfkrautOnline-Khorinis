using GUC.Utilities.FileSystem.Enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Utilities.FileSystem
{

    public struct FileSystemProtocol
    {

        private FileSystemManipulation manipulation;
        public FileSystemManipulation Manipulation { get{ return this.manipulation; } }

        private string path;
        public string Path { get { return this.path; } }

        private DateTime timestamp;
        public DateTime Timestamp { get { return this.timestamp; } }

        private List<object> options;
        public List<object> Options { get { return this.options; } }

        private int maxTries;
        public int MaxTries { get { return this.maxTries; } }

        public int Tries;

        public delegate void ProtocollApplicationEventHandler (ref FileSystemProtocol protocol, 
            ProtocolStatus status);
        public event ProtocollApplicationEventHandler OnProtocolApplication;



        public FileSystemProtocol (FileSystemManipulation manipulation, string path, DateTime timestamp, 
            int maxTries, List<object> options, ProtocollApplicationEventHandler handler)
        {
            this.manipulation = manipulation;
            this.path = path;
            this.timestamp = timestamp;
            this.options = options;
            this.maxTries = maxTries;
            this.Tries = 0;
            this.OnProtocolApplication = handler;
        }



        public void InvokeProtocollApplication (ProtocolStatus status)
        {
            if (OnProtocolApplication != null)
            {
                OnProtocolApplication.Invoke(ref this, status);
            }
        }



        public override string ToString ()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("FileSystemProtocol: Manipulation = {0} | Path = {1} | Timestamp = {2} | " 
                + "MaxTries = {3}", manipulation, path, timestamp, maxTries);

            if (options != null)
            {
                sb.Append(" | Options = [ ");
                foreach (object option in options) { sb.Append(option + ", "); }
                sb.Append(" ]");
            }

            return sb.ToString();
        }

    }

}
