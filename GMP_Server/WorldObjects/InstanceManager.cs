using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.Enumeration;
using System.Security.Cryptography;
using GUC.Server.WorldObjects.Mobs;

namespace GUC.Server.WorldObjects
{
    public abstract class InstanceManager
    {
        static void Init()
        {
            try
            {
                managers = new List<InstanceManager>();
                managers.Add(VobInstance.Table);
                managers.Add(ItemInstance.Table);
                managers.Add(NPCInstance.Table);

                managers.Add(MobInstance.Table);
                managers.Add(MobInterInstance.Table);
                managers.Add(MobFireInstance.Table);
                managers.Add(MobLadderInstance.Table);
                managers.Add(MobSwitchInstance.Table);
                managers.Add(MobWheelInstance.Table);
                managers.Add(MobContainerInstance.Table);
                managers.Add(MobDoorInstance.Table);
            }
            catch (Exception e)
            {
                Log.Logger.logError(e.InnerException.Source);
                Log.Logger.logError(e.InnerException.Message);
                Log.Logger.logError(e.InnerException.StackTrace);
            }
        }

        internal static byte[] instanceData;
        internal static byte[] instanceHash;
        static List<InstanceManager> managers = null;

        protected abstract void WriteTable(PacketWriter stream);

        public void NetUpdate()
        {
            if (InstanceManager.managers == null)
                Init();
            
            PacketWriter stream = Program.server.SetupStream(NetworkID.ConnectionMessage);

            stream.StartCompressing();
            for (int i = 0; i < managers.Count; i++)
                managers[i].WriteTable(stream);
            stream.StopCompressing();

            InstanceManager.instanceData = new byte[stream.GetLength()];
            Buffer.BlockCopy(stream.GetData(), 0, InstanceManager.instanceData, 0, stream.GetLength());

            using (MD5 md5 = new MD5CryptoServiceProvider())
            {
                md5.TransformFinalBlock(InstanceManager.instanceData, 0, InstanceManager.instanceData.Length);
                InstanceManager.instanceHash = md5.Hash;
            }

            //FIXME: Send to all clients

            StringBuilder sb = new StringBuilder();
            foreach (byte b in InstanceManager.instanceHash)
                sb.Append(b.ToString("X2"));
            System.IO.File.WriteAllBytes(sb.ToString(), InstanceManager.instanceData);
        }
    }

    public class InstanceManager<T> : InstanceManager where T : VobInstance
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

        public IEnumerable<T> GetAll()
        {
            return instanceDict.Values;
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
                instanceDict.Remove(instance.InstanceName);
            }
        }

        public void Add(T instance)
        {
            if (instance == null || instance.InstanceName == null || instance.InstanceName.Length == 0)
                return;

            if (instanceDict.ContainsKey(instance.InstanceName))
            {
                Log.Logger.log(String.Format("ERR: {0} creation failed: {1} is already existing.", instance.GetType(), instance.InstanceName));
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
            instanceDict.Add(instance.InstanceName, instance);
        }

        protected override void WriteTable(PacketWriter stream)
        {
            foreach (VobInstance inst in this.GetAll().OrderBy(n => n.ID)) //ordered by IDs
            {
                inst.WriteProperties(stream);
            }
        }
    }
}
