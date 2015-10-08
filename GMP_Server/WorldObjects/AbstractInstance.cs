using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;

namespace GUC.Server.WorldObjects
{
    public abstract class AbstractInstance
    {
        protected static ushort idCount = 0;

        protected static Dictionary<string, AbstractInstance> instanceDict = new Dictionary<string, AbstractInstance>();
        protected static Dictionary<ushort, AbstractInstance> instanceList = new Dictionary<ushort, AbstractInstance>();

        public ushort ID { get; protected set; }
        public string instanceName { get; protected set; }

        protected AbstractInstance()
        {
        }

        public AbstractInstance(string instanceName)
        {
            this.ID = 0;
            this.instanceName = instanceName.ToUpper();
        }

        public AbstractInstance(ushort ID, string codeName)
        {
            this.ID = ID;
            this.instanceName = instanceName.ToUpper();
        }

        public static void Add(AbstractInstance inst)
        {
            if (instanceDict.ContainsKey(inst.instanceName))
            {
                Log.Logger.log(String.Format("ERR: {0} creation failed: {1} is already existing.", inst.GetType(), inst.instanceName));
                return;
            }

            if (inst.ID == 0) //seek a new ID for this instance
            {
                for (int i = 0; i < ushort.MaxValue; i++)
                {
                    if (idCount != 0)
                    {
                        if (!instanceList.ContainsKey(idCount))
                        {
                            inst.ID = idCount++;
                            break;
                        }
                    }
                    idCount++;
                }

                if (inst.ID == 0) //no free id found
                {
                    Log.Logger.log(String.Format("ERR: {0} creation failed: Maximum reached: {1}", inst.GetType(), ushort.MaxValue));
                    return;
                }
            }
            else
            {
                if (instanceList.ContainsKey(idCount))
                {
                    Log.Logger.log(String.Format("ERR: {0} creation failed: ID {1} is already existing.", inst.GetType(), inst.ID));
                    return;
                }
            }

            instanceList.Add(inst.ID, inst);
            instanceDict.Add(inst.instanceName, inst);
        }

        public static void Remove(string instanceName)
        {
            Remove(Get(instanceName));
        }

        public static void Remove(ushort id)
        {
            Remove(Get(id));
        }

        public static void Remove(AbstractInstance inst)
        {
            if (inst == null)
                return;

            instanceList.Remove(inst.ID);
            instanceDict.Remove(inst.instanceName);
        }

        public static AbstractInstance Get(string instanceName)
        {
            if (instanceName == null)
                return null;

            AbstractInstance inst = null;
            instanceDict.TryGetValue(instanceName, out inst);
            return inst;
        }

        public static AbstractInstance Get(ushort id)
        {
            if (id == 0)
                return null;

            AbstractInstance inst = null;
            instanceList.TryGetValue(id, out inst);
            return inst;
        }

        #region Networking

        internal static byte[] data; // instance table
        internal static byte[] hash; // MD5 of instance table

        protected abstract void Write(BinaryWriter bw);

        public static void NetUpdate()
        {
            using (MemoryStream ms = new MemoryStream())
            {   //SO MANY STREAMS
                using (GZipStream gz = new GZipStream(ms, CompressionMode.Compress))
                using (BufferedStream bs = new BufferedStream(gz))
                using (BinaryWriter bw = new BinaryWriter(bs, Encoding.UTF8))
                {
                    bw.Write((ushort)instanceList.Count);
                    //ordered by IDs
                    foreach (NPCInstance inst in instanceList.Values.OrderBy(n => n.ID))
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
        }

        #endregion
    }
}
