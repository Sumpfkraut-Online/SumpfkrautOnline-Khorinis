using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.zClasses;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;

namespace GUC.Client.WorldObjects
{
    abstract class AbstractInstance : IDisposable
    {
        public ushort ID { get; protected set; }
        public abstract zCVob CreateVob();

        protected static Dictionary<ushort, AbstractInstance> instanceList;
        protected static string fileName = "Data.pak";

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

        protected abstract void Read(BinaryReader br);
        protected abstract void Write(BinaryWriter bw);

        public static void ReadData(byte[] data)
        {
            /*using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(data, 0, data.Length);
                ms.Position = 0;

                using (GZipStream gz = new GZipStream(ms, CompressionMode.Decompress))
                using (BinaryReader br = new BinaryReader(gz, Encoding.UTF8))
                {
                    // dispose old instances
                    if (instanceList != null)
                        for (int i = 0; i < instanceList.Count; i++)
                        {
                            instanceList.ElementAt(i).Value.Dispose();
                        }

                    //read new instances
                    AbstractInstance inst;
                    ushort num = br.ReadUInt16();
                    instanceList = new Dictionary<ushort, AbstractInstance>(num);
                    for (int i = 0; i < num; i++)
                    {
                        inst = new AbstractInstance();
                        inst.Read(br);
                        instanceList.Add(inst.ID, inst);
                    }
                }
            }*/
        }

        public static void WriteFile()
        {
            string path = States.StartupState.getDaedalusPath() + fileName;
            using (FileStream fs = new FileStream(path, FileMode.Create))
            using (GZipStream gz = new GZipStream(fs, CompressionMode.Compress))
            using (BinaryWriter bw = new BinaryWriter(gz, Encoding.UTF8))
            {
                bw.Write((ushort)instanceList.Count);
                //ordered by IDs
                foreach (AbstractInstance inst in instanceList.Values.OrderBy(n => n.ID))
                {
                    inst.Write(bw);
                }
            }
        }

        public static AbstractInstance Get(ushort id)
        {
            AbstractInstance result = null;
            instanceList.TryGetValue(id, out result);
            return result;
        }

        private bool disposed = false;
        public void Dispose()
        {
            if (!this.disposed)
            {
                DisposeInstance();
                disposed = true;
            }
        }

        protected virtual void DisposeInstance()
        {
        }
    }
}
