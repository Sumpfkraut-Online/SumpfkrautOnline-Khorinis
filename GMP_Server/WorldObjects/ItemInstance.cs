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
    public class ItemInstance : AbstractInstance
    {
        new protected static ushort idCount = 0;

        new protected static Dictionary<string, AbstractInstance> instanceDict = new Dictionary<string, AbstractInstance>();
        new protected static Dictionary<ushort, AbstractInstance> instanceList = new Dictionary<ushort, AbstractInstance>();

        #region Client fields

        /// <summary>The standard name of this item.</summary>
        public String name = "";

        /// <summary>The starter condition value of this item. The higher the condition, the longer it lasts before breaking. Default value is 1000.</summary>
        public ushort condition = 1000;

        ///<summary>The grammatical gender of the name. F.e. "Rostig'er' Zweihänder"</summary>
        public Gender gender = Gender.Neuter;

        /// <summary>The weight of this item.</summary>
        public ushort weight = 1;

        /// <summary>The type of this item.</summary>
        public ItemType type = ItemType.Misc;

        /// <summary>The material of this item. Controls the dropping sound.</summary>
        public ItemMaterial material = ItemMaterial.Wood;
        
        /// <summary>Text lines shown on the left side of the description box in the inventory.</summary>
        public string[] text = new string[4] { "", "", "", "" };
        /// <summary>Values shown on the right side of the description box in the inventory.</summary>
        public ushort[] count = new ushort[4];

        /// <summary>The name shown in the description box in the inventory.</summary>
        public string description = "";

        /// <summary>The 3DS-Model shown in the world and inventory.</summary>
        public String visual = "";

        /// <summary>The ASC-Mesh for armors which is put over the NPC's character model.</summary>
        public String visualChange = "";

        /// <summary>Magic Effect. See Scripts/System/VisualFX/VisualFxInst.d</summary>
        public String effect = "";

        /// <summary>The ItemInstance which is used for ammunition.</summary>
        public ItemInstance munition = null;

        #endregion

        #region Constructors
        public ItemInstance(string instanceName) : base(instanceName)
        {
        }

        public ItemInstance(ushort ID, string instanceName) : base(ID, instanceName)
        {
        }
        #endregion

        protected override void Write(BinaryWriter bw)
        {
            bw.Write(ID);
            bw.Write(name);
            bw.Write(weight);
            bw.Write((byte)type);
            bw.Write((byte)material);
            for (int i = 0; i < 4; i++)
            {
                bw.Write(text[i]);
                bw.Write(count[i]);
            }
            bw.Write(description);
            bw.Write(visual);
            bw.Write(visualChange);
            bw.Write(effect);
            bw.Write(munition == null ? 0 : munition.ID);
            bw.Write((byte)gender);
            bw.Write(condition);
        }

        //meh
        public static ItemInstance Get(string instanceName)
        {
            return (ItemInstance)UncastedGet(instanceName);
        }

        public static ItemInstance Get(ushort id)
        {
            return (ItemInstance)UncastedGet(id);
        }
    }
}