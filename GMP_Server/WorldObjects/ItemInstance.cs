using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.Server.Network;
using RakNet;
using GUC.Network;
using System.IO;

namespace GUC.Server.WorldObjects
{
    public class ItemInstance : VobInstance
    {
        new public readonly static InstanceManager<ItemInstance> Table = new InstanceManager<ItemInstance>();

        #region Properties

        public const int TextAndCountLen = 6;

        /// <summary>The standard name of this item.</summary>
        public String Name = "";

        /// <summary>The type of this item.</summary>
        public ItemType Type = ItemType.Misc;

        /// <summary>The material of this item. Controls the dropping sound.</summary>
        public ItemMaterial Material = ItemMaterial.Wood;

        /// <summary>Text lines shown on the left side of the description box in the inventory.</summary>
        public readonly string[] Text = new string[TextAndCountLen] { "", "", "", "", "", "" };
        /// <summary>Values shown on the right side of the description box in the inventory.</summary>
        public readonly ushort[] Count = new ushort[TextAndCountLen] { 0, 0, 0, 0, 0, 0 };

        /// <summary>The name shown in the description box in the inventory. If it's the same as 'Name', just leave this empty.</summary>
        public string Description = "";

        string visualChange = "";
        /// <summary>The ASC-Mesh for armors which is put over the NPC's character model.</summary>
        public String VisualChange
        {
            get { return visualChange; }
            set { visualChange = value.Trim().ToUpper(); }
        }

        string effect = "";
        /// <summary>Magic Effect. See Scripts/System/VisualFX/VisualFxInst.d</summary>
        public String Effect
        {
            get { return effect; }
            set { effect = value.Trim().ToUpper(); }
        }

        /// <summary>The ItemInstance which is used for ammunition.</summary>
        public ItemInstance Munition = null;

        public override bool CDDyn { get { return true; } }
        public override bool CDStatic { get { return true; } }

        #endregion

        public ItemInstance(string instanceName, object scriptObject)
            : base(instanceName, scriptObject)
        {
        }

        public ItemInstance(ushort ID, string instanceName, object scriptObject)
            : base(ID, instanceName, scriptObject)
        {
        }

        /// <summary> Use this to send additional information to the clients. The base properties are already written! </summary>
        new public static Action<ItemInstance, PacketWriter> OnWriteProperties;
        internal override void WriteProperties(PacketWriter stream)
        {
            base.WriteProperties(stream);

            stream.Write(Name);
            stream.Write((byte)Type);
            stream.Write((byte)Material);
            for (int i = 0; i < TextAndCountLen; i++)
            {
                stream.Write(Text[i]);
                stream.Write(Count[i]);
            }
            stream.Write(Description);
            stream.Write(VisualChange);
            stream.Write(Effect);
            stream.Write(Munition == null ? ushort.MinValue : Munition.ID);

            if (OnWriteProperties != null)
            {
                OnWriteProperties(this, stream);
            }
        }
    }
}