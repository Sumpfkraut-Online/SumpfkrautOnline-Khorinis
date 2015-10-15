using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;

namespace GUC.Server.WorldObjects
{
    public class InstanceManager<T> where T : AbstractInstance
    {
        ushort idCount = 0;

        Dictionary<string, T> instanceDict = new Dictionary<string, T>();
        Dictionary<ushort, T> instanceList = new Dictionary<ushort, T>();

        public T Get(string instanceName)
        {
            T instance;
            instanceDict.TryGetValue(instanceName.ToUpper(), out instance);
            return instance;
        }

        public T Get(ushort id)
        {
            T instance;
            instanceList.TryGetValue(id, out instance);
            return instance;
        }

        public void Remove(string instanceName)
        {
            Remove(Get(instanceName));
        }

        public void Remove(ushort id)
        {
            Remove(Get(id));
        }

        public void Remove(T instance)
        {
            if (instance != null && instance.ID != 0)
            {
                instanceList.Remove(instance.ID);
                instanceDict.Remove(instance.instanceName);
            }
        }

        public void Add(T instance)
        {
            if (instance == null || instance.instanceName == null || instance.instanceName.Length == 0)
                return;

            if (instanceDict.ContainsKey(instance.instanceName))
            {
                Log.Logger.log(String.Format("ERR: {0} creation failed: {1} is already existing.", instance.GetType(), instance.instanceName));
                return;
            }

            if (instance.ID == 0) //seek a new ID for this instance
            {
                for (int i = 0; i < ushort.MaxValue; i++)
                {
                    if (idCount != 0)
                    {
                        if (!instanceList.ContainsKey(idCount))
                        {
                            instance.ID = idCount++;
                            break;
                        }
                    }
                    idCount++;
                }

                if (instance.ID == 0) //no free id found
                {
                    Log.Logger.log(String.Format("ERR: {0} creation failed: Maximum reached: {1}", instance.GetType(), ushort.MaxValue));
                    return;
                }
            }
            else
            {
                if (instanceList.ContainsKey(idCount))
                {
                    Log.Logger.log(String.Format("ERR: {0} creation failed: ID {1} is already existing.", instance.GetType(), instance.ID));
                    return;
                }
            }

            instanceList.Add(instance.ID, instance);
            instanceDict.Add(instance.instanceName, instance);
        }


        internal byte[] data; // instance table
        internal byte[] hash; // MD5 of instance table

        public void NetUpdate()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (GZipStream gz = new GZipStream(ms, CompressionMode.Compress))
                using (BinaryWriter bw = new BinaryWriter(gz, Encoding.UTF8))
                {
                    bw.Write((ushort)instanceList.Count);

                    foreach (AbstractInstance inst in instanceList.Values.OrderBy(n => n.ID)) //ordered by IDs
                    {
                        inst.Write(bw);
                    }
                }
                data = ms.ToArray();
            }

            using (MD5 md5 = new MD5CryptoServiceProvider())
            {
                md5.TransformFinalBlock(data, 0, data.Length);
                hash = md5.Hash;
            }

            StringBuilder sb = new StringBuilder();
            foreach (byte b in hash)
                sb.Append(b.ToString("X2"));
            System.IO.File.WriteAllBytes(sb.ToString(), data);
        }
    }
}
