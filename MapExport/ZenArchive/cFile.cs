using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace MapExport.ZenArchive
{
    public class cFile
    {
        private FileStream fileStream;
        protected String Filename { get; set; }
        protected FileMode FileMode { get; set; }
        public FileStream FileS
        {
            get { lock (fileStream) { return fileStream; } }
            set { fileStream = value; }
        }

        public System.Text.ASCIIEncoding Encoding { get; set; }

        public cFile()
        {
            Encoding = new System.Text.ASCIIEncoding();
        }

        public void open(String filename, FileMode mode)
        {
            Filename = filename;
            FileMode = mode;
            FileS = new FileStream(filename, mode);
        }

        public Boolean EOF()
        {
            lock (FileS)
            {
                return FileS.Position >= FileS.Length;
            }
        }

        public UInt32 readUInt()
        {
            lock (FileS)
            {
                return Convert.ToUInt32(FileS.ReadByte());
            }
        }

        public int readInt()
        {
            lock (FileS)
            {
                return FileS.ReadByte();
            }
        }


        public String readLine()
        {
            StringBuilder sb = new StringBuilder();
            while (!EOF())
            {
                int value = readInt();
                if (value != 0x0A)
                    sb.Append(Convert.ToString(value, 16).PadLeft(2, '0'));
                else
                    return Encoding.GetString(ByteHelper.GetStringToBytes(sb.ToString()));
            }
            return "";
        }

        public String readLineB()
        {
            StringBuilder sb = new StringBuilder();
            while (!EOF())
            {
                int value = readInt();
                if (value != 0x0A)
                    sb.Append(Convert.ToString(value, 16).PadLeft(2, '0'));
                else
                    return sb.ToString().ToUpper();
            }
            return "";
        }

        public UInt16 readChunkID()
        {
            byte[] arr = new byte[2];
            lock (FileS)
            {
                FileS.Read(arr, 0, 2);
            }
            return BitConverter.ToUInt16(arr, 0);
        }
        public UInt32 readChunkLength()
        {
            byte[] arr = new byte[4];
            lock (FileS)
            {
                FileS.Read(arr, 0, 4);
            }
            return BitConverter.ToUInt32(arr, 0);
        }

        public void Write(String value)
        {
            byte[] arr = ByteHelper.GetStringToBytes(value);
            lock (FileS)
            {
                FileS.Write(arr, 0, arr.Length);
            }
        }

        public void close()
        {

        }
    }
}
