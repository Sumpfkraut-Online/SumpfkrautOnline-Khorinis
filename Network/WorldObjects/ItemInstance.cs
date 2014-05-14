using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;

namespace GUC.WorldObjects
{
    internal partial class ItemInstance
    {
        



        protected int id = 0;
        protected String name = "";
        protected String scemeName = "";

        protected int[] protection = new int[8];

        protected DamageType damageType = DamageType.DAM_INVALID;
        protected int totalDamage = 0;
        protected int[] damages = new int[8];
        protected int range = 0;

        protected int[] conditionAttributes = new int[3];
        protected int[] conditionValues = new int[3];

        protected int value = 0;

        protected MainFlags mainFlags = 0;
        protected Flags flags = 0;
        protected ArmorFlags wear = 0;

        protected MaterialTypes materials = 0;

        protected String description = "";
        protected String[] text = new String[6] {"", "", "", "", "", ""};
        protected int[] count = new int[6];

        //Visuals:
        protected String visual = "";
        protected String visual_Change = "";
        protected String effect = "";

        protected int visual_skin = 0;
        protected ItemInstance munition = null;

        public bool isKeyInstance = false;

        public Spell Spell = null;


        public int ID { get { return id; } }

        public String Name { get { return name; } set { this.name = value; } }
        public String ScemeName { get { return scemeName; } set { this.scemeName = value; } }
        public int[] Protection { get { return protection; } 
            set {
                if (value == null)
                    value = new int[protection.Length];
                for(int i = 0; i < protection.Length; i++)
                    this.protection[i] = value[i]; 
            } 
        }


        public DamageType DamageType { get { return damageType; } set { this.damageType = value; } }
        public int TotalDamage { get { return totalDamage; } set { this.totalDamage = value; } }
        public int[] Damages { get { return damages; }
            set
            {
                if (value == null)
                    value = new int[damages.Length];
                for (int i = 0; i < damages.Length; i++)
                    this.damages[i] = value[i];
            }
        }
        public int Range { get { return range; } set { this.range = value; } }

        public int[] ConditionAttributes
        {
            get { return conditionAttributes; }
            set
            {
                if (value == null)
                    value = new int[conditionAttributes.Length];
                for (int i = 0; i < conditionAttributes.Length; i++)
                    this.conditionAttributes[i] = value[i];
            }
        }
        public int[] ConditionValues
        {
            get { return conditionValues; }
            set
            {
                if (value == null)
                    value = new int[conditionValues.Length];
                for (int i = 0; i < conditionValues.Length; i++)
                    this.conditionValues[i] = value[i];
            }
        }

        public int Value { get { return value; } set { this.value = value; } }

        public MainFlags MainFlags { get { return mainFlags; } set { this.mainFlags = value; } }
        public Flags Flags { get { return flags; } set { this.flags = value; } }
        public ArmorFlags Wear { get { return wear; } set { this.wear = value; } }

        public MaterialTypes Materials { get { return materials; } set { this.materials = value; } }

        public String Description { get { return description; } set { this.description = value; } }
        public String[] Text
        {
            get { return text; }
            set
            {
                if (value == null)
                    value = new String[text.Length];
                for (int i = 0; i < text.Length; i++)
                    this.text[i] = value[i];
            }
        }
        public int[] Count
        {
            get { return count; }
            set
            {
                if (value == null)
                    value = new int[count.Length];
                for (int i = 0; i < count.Length; i++)
                    this.count[i] = value[i];
            }
        }

        //Visuals:
        public String Visual { get { return visual; } set { this.visual = value; } }
        public String Visual_Change { get { return visual_Change; } set { this.visual_Change = value; } }
        public String Effect { get { return effect; } set { this.effect = value; } }

        public int Visual_skin { get { return visual_skin; } set { this.visual_skin = value; } }

        public ItemInstance Munition { get { return munition; } set { this.munition = value; } }


        public String getDeadalusScript()
        {
            StringBuilder script = new StringBuilder();

            ItemInstanceParameters p = getParams();

            script.AppendLine("INSTANCE ITGUC_" + ID + " (C_Item)");
            script.AppendLine("{");

            script.AppendLine("name = \""+Name+"\";");

            if (p.HasFlag(ItemInstanceParameters.scemeName))
                script.AppendLine("scemeName = \"" + ScemeName + "\";");
            for (int i = 0; i < 8; i++)
                if (p.HasFlag((ItemInstanceParameters)((ulong)ItemInstanceParameters.protection0 << i)))
                    script.AppendLine("protection["+i+"] = " + Protection[i] + ";");
            if (p.HasFlag(ItemInstanceParameters.damageType))
                script.AppendLine("damagetype = " + (byte)DamageType + ";");
            if (p.HasFlag(ItemInstanceParameters.totalDamage))
                script.AppendLine("damageTotal = " + TotalDamage + ";");
            for (int i = 0; i < 8; i++)
                if (p.HasFlag((ItemInstanceParameters)((ulong)ItemInstanceParameters.damages0 << i)))
                    script.AppendLine("damage[" + i + "] = " + Damages[i] + ";");

            if (p.HasFlag(ItemInstanceParameters.range))
                script.AppendLine("range = " + Range + ";");

            for (int i = 0; i < conditionAttributes.Length; i++)
                if (p.HasFlag((ItemInstanceParameters)((ulong)ItemInstanceParameters.conditionAttributes0 << i)))
                    script.AppendLine("cond_atr[" + i + "] = " + conditionAttributes[i] + ";");
            for (int i = 0; i < conditionValues.Length; i++)
                if (p.HasFlag((ItemInstanceParameters)((ulong)ItemInstanceParameters.conditionValues0 << i)))
                    script.AppendLine("cond_value[" + i + "] = " + conditionValues[i] + ";");

            if (p.HasFlag(ItemInstanceParameters.value))
                script.AppendLine("value = " + (int)Value + ";");

            if (p.HasFlag(ItemInstanceParameters.mainFlags))
                script.AppendLine("mainflag = " + (int)MainFlags + ";");

            if (p.HasFlag(ItemInstanceParameters.flags))
                script.AppendLine("flags = " + (int)Flags + ";");

            if (p.HasFlag(ItemInstanceParameters.wear))
                script.AppendLine("wear = " + (int)Wear + ";");

            if (p.HasFlag(ItemInstanceParameters.materials))
                script.AppendLine("material = " + (int)Materials + ";");

            if (p.HasFlag(ItemInstanceParameters.description))
                script.AppendLine("description = \"" + Description + "\";");


            for (int i = 0; i < text.Length; i++)
                if (p.HasFlag((ItemInstanceParameters)((ulong)ItemInstanceParameters.text0 << i)))
                    script.AppendLine("TEXT[" + i + "] = \"" + text[i] + "\";");
            for (int i = 0; i < count.Length; i++)
                if (p.HasFlag((ItemInstanceParameters)((ulong)ItemInstanceParameters.count0 << i)))
                    script.AppendLine("COUNT[" + i + "] = " + count[i] + ";");

            if (p.HasFlag(ItemInstanceParameters.visual))
                script.AppendLine("visual = \"" + Visual + "\";");
            if (p.HasFlag(ItemInstanceParameters.visual_Change))
                script.AppendLine("visual_change = \"" + Visual_Change + "\";");
            if (p.HasFlag(ItemInstanceParameters.effect))
                script.AppendLine("effect = \"" + Effect + "\";");

            if (p.HasFlag(ItemInstanceParameters.visual_skin))
                script.AppendLine("visual_skin = " + visual_skin + ";");



            script.AppendLine("};");


            return script.ToString();
        }

        protected ItemInstanceParameters getParams()
        {
            ItemInstanceParameters param = 0;

            if (ScemeName.Length > 0)
                param |= ItemInstanceParameters.scemeName;

            for (int i = 0; i < protection.Length; i++)
                if (Protection[i] != 0)
                    param |= (ItemInstanceParameters)((ulong)ItemInstanceParameters.protection0 << i);

            if (DamageType != Enumeration.DamageType.DAM_INVALID)
                param |= ItemInstanceParameters.damageType;
            if (TotalDamage != 0)
                param |= ItemInstanceParameters.totalDamage;

            for (int i = 0; i < Damages.Length; i++)
                if (Damages[i] != 0)
                    param |= (ItemInstanceParameters)((ulong)ItemInstanceParameters.damages0 << i);

            if (Range != 0)
                param |= ItemInstanceParameters.range;

            for (int i = 0; i < this.ConditionAttributes.Length; i++)
                if (ConditionAttributes[i] != 0)
                    param |= (ItemInstanceParameters)((ulong)ItemInstanceParameters.conditionAttributes0 << i);
            for (int i = 0; i < this.ConditionValues.Length; i++)
                if (ConditionValues[i] != 0)
                    param |= (ItemInstanceParameters)((ulong)ItemInstanceParameters.conditionValues0 << i);

            if (Value != 0)
                param |= ItemInstanceParameters.value;

            if (MainFlags != 0)
                param |= ItemInstanceParameters.mainFlags;

            if (Flags != 0)
                param |= ItemInstanceParameters.flags;

            if (Wear != 0)
                param |= ItemInstanceParameters.wear;
            if (Materials != 0)
                param |= ItemInstanceParameters.materials;

            if (Description.Length > 0)
                param |= ItemInstanceParameters.description;

            for (int i = 0; i < this.Text.Length; i++)
                if (Text[i].Length > 0)
                    param |= (ItemInstanceParameters)((ulong)ItemInstanceParameters.text0 << i);
            for (int i = 0; i < this.Count.Length; i++)
                if (Count[i] != 0)
                    param |= (ItemInstanceParameters)((ulong)ItemInstanceParameters.count0 << i);

            if (Visual.Length > 0)
                param |= ItemInstanceParameters.visual;
            if (Visual_Change.Length > 0)
                param |= ItemInstanceParameters.visual_Change;
            if (Effect.Length > 0)
                param |= ItemInstanceParameters.effect;

            if (Visual_skin != 0)
                param |= ItemInstanceParameters.visual_skin;
            if (munition != null)
                param |= ItemInstanceParameters.munition;
            if (isKeyInstance)
                param |= ItemInstanceParameters.isKeyInstance;
            if (Spell != null)
                param |= ItemInstanceParameters.Spell;
            return param;
        }


        #region Statics

        protected static Dictionary<int, ItemInstance> itemInstanceDict = new Dictionary<int, ItemInstance>();
        protected static List<ItemInstance> itemInstanceList = new List<ItemInstance>();

        public static Dictionary<int, ItemInstance> ItemInstanceDict { get { return itemInstanceDict; } }
        public static List<ItemInstance> ItemInstanceList { get { return itemInstanceList; } }


        public static void addItemInstance(ItemInstance instance)
        {
            ItemInstanceDict.Add(instance.ID, instance);
            ItemInstanceList.Add(instance);
        }
        #endregion

        
        protected enum ItemInstanceParameters : ulong
        {
            scemeName = 1 << 0,
            protection0 = scemeName << 1,
            protection1 = protection0 << 1,
            protection2 = protection1 << 1,
            protection3 = protection2 << 1,
            protection4 = protection3 << 1,
            protection5 = protection4 << 1,
            protection6 = protection5 << 1,
            protection7 = protection6 << 1,
            damageType = protection7 << 1,
            totalDamage = damageType << 1,
            damages0 = totalDamage << 1,
            damages1 = damages0 << 1,
            damages2 = damages1 << 1,
            damages3 = damages2 << 1,
            damages4 = damages3 << 1,
            damages5 = damages4 << 1,
            damages6 = damages5 << 1,
            damages7 = damages6 << 1,
            range = damages7 << 1,
            conditionAttributes0 = range << 1,
            conditionAttributes1 = conditionAttributes0 << 1,
            conditionAttributes2 = conditionAttributes1 << 1,
            conditionValues0 = conditionAttributes2 << 1,
            conditionValues1 = conditionValues0 << 1,
            conditionValues2 = conditionValues1 << 1,
            value = conditionValues2 << 1,
            mainFlags = value << 1,
            flags = mainFlags << 1,
            wear = flags << 1,
            materials = wear << 1,
            description = materials << 1,
            text0 = description << 1,
            text1 = text0 << 1,
            text2 = text1 << 1,
            text3 = text2 << 1,
            text4 = text3 << 1,
            text5 = text4 << 1,
            count0 = text5 << 1,
            count1 = count0 << 1,
            count2 = count1 << 1,
            count3 = count2 << 1,
            count4 = count3 << 1,
            count5 = count4 << 1,
            visual = count5 << 1,
            visual_Change = visual << 1,
            effect = visual_Change << 1,
            visual_skin = effect << 1,
            munition = visual_skin << 1,
            isKeyInstance = munition << 1,
            Spell = isKeyInstance << 1
        }
    }
}
