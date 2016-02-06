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
        public partial interface IScriptItemInstance : IScriptVobInstance
        {
        }

        new public const VobTypes sVobType = VobTypes.Item;
        public static readonly InstanceDictionary ItemInstances = VobInstance.AllInstances.GetDict(sVobType);

        public ItemInstance(PacketReader stream, IScriptItemInstance scriptObj) : base(stream, scriptObj)
        {
        }

        #region Properties

        /// <summary>The name of this item.</summary>
        public String Name = "";

        /// <summary>The material of this item. Controls the dropping sound.</summary>
        public ItemMaterials Material = ItemMaterials.Wood;

        string effect = "";
        /// <summary>Magic Effect. See Scripts/System/VisualFX/VisualFxInst.d</summary>
        public String Effect
        {
            get { return effect; }
            set { effect = value.Trim().ToUpper(); }
        }

        #endregion

        new public IScriptItemInstance ScriptObj { get; protected set; }

        internal override void ReadProperties(PacketReader stream)
        {
            base.ReadProperties(stream);

            this.Name = stream.ReadString();
            this.Material = (ItemMaterials)stream.ReadByte();
            this.Effect = stream.ReadString();
        }

        internal override void WriteProperties(PacketWriter stream)
        {
            base.WriteProperties(stream);

            stream.Write(Name);
            stream.Write((byte)Material);
            stream.Write(Effect);
        }
    }
}