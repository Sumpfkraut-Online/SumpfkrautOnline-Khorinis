using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.Network;
using GUC.WorldObjects.Collections;

namespace GUC.WorldObjects.Instances
{
    public partial interface IScriptVobInstance : IScriptWorldObject
    {
        void OnWriteProperties(PacketWriter stream);
        void OnReadProperties(PacketReader stream);
    }

    public partial class VobInstance : WorldObject, IVobObj<ushort>
    {
        public readonly static InstanceCollection AllInstances = new InstanceCollection();
        public readonly static InstanceDictionary VobInstances = AllInstances.GetDict(VobTypes.Vob);

        public ushort ID { get; protected set; }
        public VobTypes VobType { get; protected set; }
        new public IScriptVobInstance ScriptObj { get; protected set; }

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
        
        internal VobInstance()
        {
            this.VobType = VobTypes.Vob;
        }

        internal virtual void WriteProperties(PacketWriter stream)
        {
            stream.Write(this.ID);
            stream.Write(this.Visual);
            stream.Write(this.CDDyn);
            stream.Write(this.CDStatic);

            this.ScriptObj.OnWriteProperties(stream);
        }

        internal virtual void ReadProperties(PacketReader stream)
        {
            this.ID = stream.ReadUShort();
            this.Visual = stream.ReadString();
            this.CDDyn = stream.ReadBit();
            this.CDStatic = stream.ReadBit();

            this.ScriptObj.OnReadProperties(stream);
        }

        internal static VobInstance CreateInstanceByType(VobTypes type)
        {
            VobInstance result;
            switch (type)
            {
                case VobTypes.Vob:
                    result = new VobInstance();
                    break;
                case VobTypes.Item:
                    result = new ItemInstance();
                    break;
                case VobTypes.NPC:
                    result = new NPCInstance();
                    break;
                case VobTypes.Mob:
                    result = new MobInstance();
                    break;
                case VobTypes.MobInter:
                    result = new MobInterInstance();
                    break;
                case VobTypes.MobFire:
                    result = new MobFireInstance();
                    break;
                case VobTypes.MobLadder:
                    result = new MobLadderInstance();
                    break;
                case VobTypes.MobSwitch:
                    result = new MobSwitchInstance();
                    break;
                case VobTypes.MobWheel:
                    result = new MobWheelInstance();
                    break;
                case VobTypes.MobContainer:
                    result = new MobContainerInstance();
                    break;
                case VobTypes.MobDoor:
                    result = new MobDoorInstance();
                    break;
                default:
                    return null;
            }

            //result.ScriptObj = Scripting.ScriptManager.si.CreateScriptInstance(type);

            return result;
        }
    }
}
