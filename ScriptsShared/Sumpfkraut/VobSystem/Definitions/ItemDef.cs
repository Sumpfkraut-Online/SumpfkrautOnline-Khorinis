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
        #region properties 
        new public static readonly String _staticName = "ItemDef (static)";

        new public ItemInstance BaseDef { get { return (ItemInstance)base.BaseDef; } }

        /// <summary>The standard name of this item.</summary>
        public string Name { get { return BaseDef.Name; } set { BaseDef.Name = value; } }

        /// <summary>The material of this item. Controls the dropping sound.</summary>
        public ItemMaterials Material { get { return BaseDef.Material; } set { BaseDef.Material = value; } }

        /// <summary>Magic effect when laying in the world. See Scripts/System/VisualFX/VisualFxInst.d</summary>
        public string Effect { get { return BaseDef.Effect; } set { BaseDef.Effect = value; } }
        #endregion
        
        public ItemDef (PacketReader stream) : base(new ItemInstance(), stream)
        { }

        public override void OnReadProperties (PacketReader stream)
        {
            base.OnReadProperties(stream);
        }

        public override void OnWriteProperties (PacketWriter stream)
        {
            base.OnWriteProperties(stream);
        }
    }
}