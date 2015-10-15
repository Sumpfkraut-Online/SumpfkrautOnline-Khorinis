using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;

namespace GUC.Client.WorldObjects
{
    abstract class AbstractInstance
    {
        protected static Dictionary<ushort, AbstractInstance> InstanceList;
        protected static string fileName = "data.pak";
        protected static void ReadNew(BinaryReader br) { }
        protected abstract void Write(BinaryWriter bw);
        
        public ushort ID;

        public static byte[] ReadFile()
        {
            try
            {
                string path = States.StartupState.getDaedalusPath() + fileName;

                if (File.Exists(path))
                {
                    byte[] data = File.ReadAllBytes(path);
                    ReadData(data);

                    byte[] hash;
                    using (MD5 md5 = new MD5CryptoServiceProvider())
                    {
                        md5.TransformFinalBlock(data, 0, data.Length);
                        hash = md5.Hash;
                    }

                    return hash;
                }
            }
            catch { }
            return new byte[16];
        }

        public static void ReadData(byte[] data)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(data, 0, data.Length);
                ms.Position = 0;

                using (GZipStream gz = new GZipStream(ms, CompressionMode.Decompress))
                using (BinaryReader br = new BinaryReader(gz, Encoding.UTF8))
                {
                    // read new instances
                    ushort num = br.ReadUInt16();
                    InstanceList = new Dictionary<ushort, AbstractInstance>(num);
                    for (int i = 0; i < num; i++)
                    {
                        ReadNew(br);
                    }
                }
            }
        }

        public static void WriteFile()
        {
            using (FileStream fs = new FileStream(States.StartupState.getDaedalusPath() + fileName, FileMode.Create))
            using (GZipStream gz = new GZipStream(fs, CompressionMode.Compress))
            using (BufferedStream bs = new BufferedStream(gz))
            using (BinaryWriter bw = new BinaryWriter(bs, Encoding.UTF8))
            {
                bw.Write((ushort)InstanceList.Count);
                //ordered by IDs
                foreach (AbstractInstance inst in InstanceList.Values.OrderBy(n => n.ID))
                {
                    inst.Write(bw);
                }
            }
        }
    }
}
