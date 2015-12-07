using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using System.IO;

namespace GUC.Server.WorldObjects
{
    public class MobInstance : VobInstance
    {
        public MobType type = MobType.Vob;

        //Vob:
        public string Visual
        {
            get { return visual; }
            set { visual = value.Trim().ToUpper(); }
        }
        string visual = "";
        public bool cdDyn = true;
        public bool cdStatic = true;

        //Mob:
        public string focusName = "";

        //MobInter:
        public string onTriggerFunc = "";
        public string onTriggerClientFunc = "";
        public bool isMulti = false;

        //MobFire:
        public string fireVobTreeName = "";

        //MobLockable
        public string onTryOpenClientFunc = "";

        internal override void Write(BinaryWriter bw)
        {
            bw.Write(ID);
            bw.Write((byte)type);
            bw.Write(visual);
            bw.Write(cdDyn);
            bw.Write(cdStatic);
            bw.Write(focusName);
            bw.Write(onTriggerClientFunc);
            bw.Write(fireVobTreeName);
            bw.Write(onTryOpenClientFunc);
        }

        #region Constructors

        public MobInstance(string instanceName, object scriptObject)
            : base(instanceName, scriptObject)
        {
        }

        public MobInstance(ushort ID, string instanceName, object scriptObject)
            : base(ID, instanceName, scriptObject)
        {
        }

        #endregion

        public static InstanceManager<MobInstance> Table = new InstanceManager<MobInstance>();
    }
}
