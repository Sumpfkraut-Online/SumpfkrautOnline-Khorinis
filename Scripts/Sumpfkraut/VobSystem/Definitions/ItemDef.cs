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
        new public ItemInstance baseDef { get { return (ItemInstance)base.baseDef; } }

        /// <summary>The standard name of this item.</summary>
        public string Name { get { return baseDef.Name; } set { baseDef.Name = value; } }

        /// <summary>The material of this item. Controls the dropping sound.</summary>
        public ItemMaterials Material { get { return baseDef.Material; } set { baseDef.Material = value; } }

        /// <summary>Magic effect when laying in the world. See Scripts/System/VisualFX/VisualFxInst.d</summary>
        public string Effect { get { return baseDef.Effect; } set { baseDef.Effect = value; } }
        #endregion

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