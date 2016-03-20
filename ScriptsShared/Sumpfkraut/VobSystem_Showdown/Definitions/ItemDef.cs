using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.Network;
using GUC.WorldObjects.Instances;

namespace GUC.Scripts.Sumpfkraut.VobSystem.Definitions
{
    public partial class ItemDef : VobDef, ItemInstance.IScriptItemInstance
    {
        #region BaseDef
        new public ItemInstance BaseDef { get { return (ItemInstance)base.BaseDef; } }

        string name = "";
        /// <summary>The standard name of this item.</summary>
        public string Name
        {
            get { return this.name; }
            set { this.name = value == null ? "" : value; }
        }

        /// <summary>The material of this item. Controls the dropping sound.</summary>
        public ItemMaterials Material = ItemMaterials.Wood;

        string effect = "";
        /// <summary>Magic effect when laying in the world. (case insensitive) See _work/Data/Scripts/System/VisualFX/VisualFxInst.d</summary>
        public string Effect
        {
            get { return this.effect; }
            set { this.effect = value == null ? "" : value.ToUpper(); }
        }
        #endregion
        
        public ItemDef(PacketReader stream) : base(new ItemInstance(), stream)
        {
        }

        public override void OnReadProperties(PacketReader stream)
        {
            base.OnReadProperties(stream);
        }

        public override void OnWriteProperties(PacketWriter stream)
        {
            base.OnWriteProperties(stream);
        }
    }
}