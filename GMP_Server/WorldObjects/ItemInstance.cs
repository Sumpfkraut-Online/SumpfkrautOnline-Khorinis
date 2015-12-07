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
        #region Client fields

        /// <summary>The standard name of this item.</summary>
        public String Name = "";

        /// <summary>The starter condition value of this item. The higher the condition, the longer it lasts before breaking. Default value is 1000.</summary>
        public ushort Condition = 1000;

        ///<summary>The grammatical gender of the name. F.e. "Rostig'er' Zweihänder"</summary>
        public Gender Gender = Gender.Neuter;

        /// <summary>The weight of this item.</summary>
        public ushort Weight = 1;

        /// <summary>The type of this item.</summary>
        public ItemType Type = ItemType.Misc;

        /// <summary>The material of this item. Controls the dropping sound.</summary>
        public ItemMaterial Material = ItemMaterial.Wood;

        /// <summary>Text lines shown on the left side of the description box in the inventory.</summary>
        public string[] Text = new string[4] { "", "", "", "" };
        /// <summary>Values shown on the right side of the description box in the inventory.</summary>
        public ushort[] Count = new ushort[4];

        /// <summary>The name shown in the description box in the inventory. If it's the same as 'Name', just leave this empty.</summary>
        public string Description = "";

        /// <summary>The 3DS-Model shown in the world and inventory.</summary>
        public String Visual
        {
            get { return visual; }
            set { visual = value.Trim().ToUpper(); }
        }
        string visual = "";

        /// <summary>The ASC-Mesh for armors which is put over the NPC's character model.</summary>
        public String VisualChange
        {
            get { return visualChange; }
            set { visualChange = value.Trim().ToUpper(); }
        }
        string visualChange = "";

        /// <summary>Magic Effect. See Scripts/System/VisualFX/VisualFxInst.d</summary>
        public String Effect
        {
            get { return effect; }
            set { effect = value.Trim().ToUpper(); }
        }
        string effect = "";

        /// <summary>The ItemInstance which is used for ammunition.</summary>
        public ItemInstance Munition = null;

        internal override void Write(BinaryWriter bw)
        {
            bw.Write(ID);
            bw.Write(Name);
            bw.Write(Weight);
            bw.Write((byte)Type);
            bw.Write((byte)Material);
            for (int i = 0; i < 4; i++)
            {
                bw.Write(Text[i]);
                bw.Write(Count[i]);
            }
            bw.Write(Description);
            bw.Write(visual);
            bw.Write(visualChange);
            bw.Write(effect);
            bw.Write(Munition == null ? (ushort)0 : Munition.ID);
            bw.Write((byte)Gender);
            bw.Write(Condition);
        }

        #endregion

        public float Range = 100;

        #region Constructors

        public ItemInstance(string instanceName, object scriptObject)
            : base(instanceName, scriptObject)
        {
        }

        public ItemInstance(ushort ID, string instanceName, object scriptObject)
            : base(ID, instanceName, scriptObject)
        {
        }

        #endregion
        
        public static InstanceManager<ItemInstance> Table = new InstanceManager<ItemInstance>();
    }
}