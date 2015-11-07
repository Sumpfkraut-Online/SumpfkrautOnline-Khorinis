using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using GUC.Enumeration;

namespace GUC.Client.WorldObjects
{
    public class MobInstance : AbstractInstance
    {
        public static InstanceManager<MobInstance> Table = new InstanceManager<MobInstance>("mobs.pak");

        public MobType type;

        //Vob:
        public string visual;
        public bool cdDyn;
        public bool cdStatic;

        //Mob:
        public string focusName;

        //MobInter:
        public string onTriggerClientFunc;

        //MobFire:
        public string fireVobTreeName;

        internal override void Read(BinaryReader br)
        {
            ID = br.ReadUInt16();
            type = (MobType)br.ReadByte();
            visual = br.ReadString();
            cdDyn = br.ReadBoolean();
            cdStatic = br.ReadBoolean();
            focusName = br.ReadString();
            onTriggerClientFunc = br.ReadString();
            fireVobTreeName = br.ReadString();
        }

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
        }
    }
}
