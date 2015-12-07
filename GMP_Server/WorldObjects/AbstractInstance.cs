using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.Network;
using GUC.Server.Log;

namespace GUC.Server.WorldObjects
{
    public class VobInstance : ServerObject
    {
        #region Instance Management
        static Dictionary<string, VobInstance> instanceDict = new Dictionary<string, VobInstance>();
        static Dictionary<ushort, VobInstance> instanceList = new Dictionary<ushort, VobInstance>();
        public static IEnumerable<VobInstance> GetAll() { return instanceDict.Values; }

        public static VobInstance Get(string name)
        {
            VobInstance instance;
            instanceDict.TryGetValue(name, out instance);
            return instance;
        }

        public static VobInstance Get(ushort id)
        {
            VobInstance instance;
            instanceList.TryGetValue(id, out instance);
            return instance;
        }

        public static void Remove(string name)
        {
            Remove(Get(name));
        }

        public static void Remove(ushort id)
        {
            Remove(Get(id));
        }

        public static void Remove(VobInstance instance)
        {
            if (instance != null)
            {
                instanceList.Remove(instance.ID);
                instanceDict.Remove(instance.InstanceName);
            }
        }

        static ushort idCount = 1;
        public void Add(VobInstance instance)
        {
            if (instance == null)
            {
                Logger.logError("Adding {0}instance");
                return;
            }

            if (String.IsNullOrWhiteSpace(instance.InstanceName))
            {
                return;
            }

            if (instanceDict.ContainsKey(instance.InstanceName))
            {
                Log.Logger.log(String.Format("ERR: {0} creation failed: {1} is already existing.", instance.GetType(), instance.InstanceName));
                return;
            }

            if (instance.ID == 0) //seek a new ID for this instance
            {
                for (int i = 0; i < ushort.MaxValue-1; i++)
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
        #endregion

        public ushort ID { get; protected set; }
        public VobType Type { get; protected set; }
        public string InstanceName { get; protected set; }

        #region Properties

        string visual = "";
        public string Visual
        {
            get { return visual; }
            set { visual = value.Trim().ToUpper(); }
        }

        bool cddyn = true;
        public virtual bool CDDyn
        {
            get { return cddyn; }
            set { cddyn = value; }
        }

        bool cdstatic = true;
        public virtual bool CDStatic
        {
            get { return cdstatic; }
            set { cdstatic = value; }
        }

        #endregion

        public VobInstance(string instanceName, object scriptObject)
            : this(0, instanceName, scriptObject)
        {
        }

        public VobInstance(ushort ID, string instanceName, object scriptObject) : base (scriptObject)
        {
            this.ID = ID;
            this.Type = VobType.Vob;
            this.InstanceName = instanceName.ToUpper();
        }

        public override void Delete()
        {
            throw new NotImplementedException();
        }

        /// <summary> Use this to send additional information to the clients. The base properties are already written! </summary>
        public static Action<VobInstance, PacketWriter> OnWriteProperties;
        internal virtual void WriteProperties(PacketWriter stream)
        {
            stream.Write(visual);
            stream.Write(cddyn);
            stream.Write(cdstatic);

            if (OnWriteProperties != null)
                OnWriteProperties(this, stream);
        }
    }
}
