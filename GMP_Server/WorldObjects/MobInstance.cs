using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using System.IO;

namespace GUC.Server.WorldObjects
{
    class MobInstance : AbstractInstance
    {
        MobType type = MobType.Vob;

        //Vob:
        public string visual = "ITFO_APPLE.3DS";
        public bool cdDyn = true;
        public bool cdStatic = true;

        //Mob:
        public string focusName = "";

        //MobInter:
        public string onTriggerFunc = "";
        public string onTriggerClientFunc = "";
        public string useWithItem = "";

        //MobFire:
        public string fireVobTreeName = "";

        //MobContainer:
        public ushort capacity = 0;

        protected override void Write(BinaryWriter bw)
        {
            bw.Write(ID);

            bw.Write((byte)type);
            bw.Write(visual);
            bw.Write(cdDyn);
            bw.Write(cdStatic);
            bw.Write(focusName);
            bw.Write(fireVobTreeName);
            bw.Write(capacity);
        }

        // actually unnecessary but now we don't have to cast every time
        public static new MobInstance Get(string instanceName)
        {
            if (instanceName == null)
                return null;

            AbstractInstance inst = null;
            instanceDict.TryGetValue(instanceName, out inst);
            return (MobInstance)inst;
        }

        public static new MobInstance Get(ushort id)
        {
            if (id == 0)
                return null;

            AbstractInstance inst = null;
            instanceList.TryGetValue(id, out inst);
            return (MobInstance)inst;
        }
    }
}
