using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.Network;
using System.Security.Cryptography;
using GUC.Client.WorldObjects.Collections;

namespace GUC.Client.WorldObjects.Instances
{
    public class VobInstance : IVobObj<ushort>
    {
        public readonly static VobType sVobType = VobType.Vob;
        public readonly static InstanceCollection Instances = new InstanceCollection();

        public ushort ID { get; internal set; }

        public VobType VobType { get; protected set; }

        #region Properties

        protected string visual = "";
        public virtual string Visual
        {
            get { return visual; }
            set { visual = value.Trim().ToUpper(); }
        }

        protected bool cddyn = true;
        public virtual bool CDDyn
        {
            get { return cddyn; }
            set { cddyn = value; }
        }

        protected bool cdstatic = true;
        public virtual bool CDStatic
        {
            get { return cdstatic; }
            set { cdstatic = value; }
        }

        #endregion
        
        public VobInstance()
        {
            this.VobType = sVobType;
        }

        internal virtual void WriteProperties(PacketWriter stream)
        {
            stream.Write(this.ID);
            stream.Write(this.Visual);
            stream.Write(this.CDDyn);
            stream.Write(this.CDStatic);

            //...
        }

        internal virtual void ReadProperties(PacketReader stream)
        {
            this.ID = stream.ReadUShort();
            this.Visual = stream.ReadString();
            this.CDDyn = stream.ReadBit();
            this.CDStatic = stream.ReadBit();
            
            // ... 
        }

        public static VobInstance CreateByType(VobType type)
        {
            switch (type)
            {
                case VobType.Vob:
                    return new VobInstance();
                case VobType.NPC:
                    return new NPCInstance();
                case VobType.Item:
                    return new ItemInstance();

                case VobType.Mob:
                    return new MobInstance();
                case VobType.MobInter:
                    return new MobInterInstance();
                case VobType.MobFire:
                    return new MobFireInstance();
                case VobType.MobLadder:
                    return new MobLadderInstance();
                case VobType.MobSwitch:
                    return new MobSwitchInstance();
                case VobType.MobWheel:
                    return new MobWheelInstance();
                case VobType.MobContainer:
                    return new MobContainerInstance();
                case VobType.MobDoor:
                    return new MobDoorInstance();

                default:
                    return null;
            }
        }
    }
}
