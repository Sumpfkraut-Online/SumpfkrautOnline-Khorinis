using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.Network;

namespace GUC.WorldObjects.Instances
{
    public abstract partial class BaseVobInstance : GameObject
    {
        public abstract VobTypes VobType { get; }

        #region ScriptObject

        public partial interface IScriptBaseVobInstance : IScriptGameObject
        {
        }

        public new IScriptBaseVobInstance ScriptObject
        {
            get { return (IScriptBaseVobInstance)base.ScriptObject; }
            set { base.ScriptObject = value; }
        }

        #endregion

        #region Properties

        bool isStatic = false;
        public bool IsStatic { get { return isStatic; } }

        public bool Added { get; internal set; }

        public override int ID
        {
            get { return base.ID; } 
            set
            {
                if (this.Added)
                    throw new Exception("Instance is added to InstanceCollection. ID can't be changed!");
                base.ID = value;
            }
        }

        #endregion

        #region Write & Read

        protected override void WriteProperties(PacketWriter stream)
        {
            stream.Write((ushort)ID);
        }

        protected override void ReadProperties(PacketReader stream)
        {
            this.ID = stream.ReadUShort();
        }

        #endregion
    }
}
