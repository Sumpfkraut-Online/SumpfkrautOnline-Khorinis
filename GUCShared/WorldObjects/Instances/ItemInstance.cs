using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.Network;
using GUC.WorldObjects.Collections;

namespace GUC.WorldObjects.Instances
{
    public partial class ItemInstance : VobInstance
    {
        public override VobTypes VobType { get { return VobTypes.Item; } }

        #region ScriptObject

        public partial interface IScriptItemInstance : IScriptVobInstance
        {
        }

        public new IScriptItemInstance ScriptObject
        {
            get { return (IScriptItemInstance)base.ScriptObject; }
            set { base.ScriptObject = value; }
        }

        #endregion

        #region Properties

        /// <summary>The name of this item.</summary>
        public String Name = "";

        /// <summary>The material of this item. Controls the dropping sound.</summary>
        public ItemMaterials Material = ItemMaterials.Wood;

        string effect = "";
        /// <summary>Magic Effect (case insensitive). See Scripts/System/VisualFX/VisualFxInst.d</summary>
        public String Effect
        {
            get { return effect; }
            set { effect = value.ToUpper(); }
        }

        #endregion

        #region Read & Write

        protected override void ReadProperties(PacketReader stream)
        {
            base.ReadProperties(stream);

            this.Name = stream.ReadString();
            this.Material = (ItemMaterials)stream.ReadByte();
            this.Effect = stream.ReadString();
        }

        protected override void WriteProperties(PacketWriter stream)
        {
            base.WriteProperties(stream);

            stream.Write(Name);
            stream.Write((byte)Material);
            stream.Write(Effect);
        }

        #endregion
    }
}