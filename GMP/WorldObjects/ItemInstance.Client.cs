using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RakNet;
using GUC.Network;
using Gothic.zClasses;
using Gothic.zTypes;
using WinApi;

namespace GUC.WorldObjects
{
    internal partial class ItemInstance
    {

        public static int getIndex(String instance)
        {
            try
            {
                return Convert.ToInt32(instance.Substring("ITGUC_".Length));
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public void Read(BitStream stream)
        {
            stream.Read(out this.id);
            stream.Read(out this.name);

            ulong param = 0;
            BitStreamExtension.Read(stream, out param);
            //stream.Read(out param);
            ItemInstanceParameters paramI = (ItemInstanceParameters)param;
            
            if (paramI.HasFlag(ItemInstanceParameters.scemeName))
                stream.Read(out this.scemeName);
            for (int i = 0; i < 8; i++)
                if (paramI.HasFlag((ItemInstanceParameters)((ulong)ItemInstanceParameters.protection0 << i)))
                    stream.Read(out protection[i]);
            if (paramI.HasFlag(ItemInstanceParameters.damageType))
            {
                byte dT = 0;
                stream.Read(out dT);
                DamageType = (Enumeration.DamageType)dT;
            }
            if (paramI.HasFlag(ItemInstanceParameters.totalDamage))
                stream.Read(out this.totalDamage);
            for (int i = 0; i < 8; i++)
                if (paramI.HasFlag((ItemInstanceParameters)((ulong)ItemInstanceParameters.damages0 << i)))
                    stream.Read(out damages[i]);

            if (paramI.HasFlag(ItemInstanceParameters.range))
                stream.Read(out this.range);

            for (int i = 0; i < conditionAttributes.Length; i++)
                if (paramI.HasFlag((ItemInstanceParameters)((ulong)ItemInstanceParameters.conditionAttributes0 << i)))
                    stream.Read(out conditionAttributes[i]);
            for (int i = 0; i < conditionValues.Length; i++)
                if (paramI.HasFlag((ItemInstanceParameters)((ulong)ItemInstanceParameters.conditionValues0 << i)))
                    stream.Read(out conditionValues[i]);

            if (paramI.HasFlag(ItemInstanceParameters.value))
                stream.Read(out this.value);

            if (paramI.HasFlag(ItemInstanceParameters.mainFlags))
            {
                int dT = 0;
                stream.Read(out dT);
                MainFlags = (Enumeration.MainFlags)dT;
            }

            if (paramI.HasFlag(ItemInstanceParameters.flags))
            {
                int dT = 0;
                stream.Read(out dT);
                Flags = (Enumeration.Flags)dT;
            }

            if (paramI.HasFlag(ItemInstanceParameters.wear))
            {
                byte dT = 0;
                stream.Read(out dT);
                Wear = (Enumeration.ArmorFlags)dT;
            }

            if (paramI.HasFlag(ItemInstanceParameters.materials))
            {
                byte dT = 0;
                stream.Read(out dT);
                Materials = (Enumeration.MaterialTypes)dT;
            }

            if (paramI.HasFlag(ItemInstanceParameters.description))
                stream.Read(out this.description);


            for (int i = 0; i < text.Length; i++)
                if (paramI.HasFlag((ItemInstanceParameters)((ulong)ItemInstanceParameters.text0 << i)))
                    stream.Read(out text[i]);
            for (int i = 0; i < count.Length; i++)
                if (paramI.HasFlag((ItemInstanceParameters)((ulong)ItemInstanceParameters.count0 << i)))
                    stream.Read(out count[i]);

            if (paramI.HasFlag(ItemInstanceParameters.visual))
                stream.Read(out this.visual);
            if (paramI.HasFlag(ItemInstanceParameters.visual_Change))
                stream.Read(out this.visual_Change);
            if (paramI.HasFlag(ItemInstanceParameters.effect))
                stream.Read(out this.effect);

            if (paramI.HasFlag(ItemInstanceParameters.visual_skin))
                stream.Read(out this.visual_skin);
            if (paramI.HasFlag(ItemInstanceParameters.munition))
            {
                int iiID = 0;
                stream.Read(out iiID);
                this.munition = ItemInstance.ItemInstanceDict[iiID];
            }
            if (paramI.HasFlag(ItemInstanceParameters.isKeyInstance))
            {
                stream.Read(out isKeyInstance);

                if (isKeyInstance)
                {
                    String instanceString = "ITGUC_" + ID;
                    char[] cArr = instanceString.ToCharArray();
                    byte[] i = new byte[13];
                    Process.ThisProcess().Write(i, 0x008B7CB0);

                    System.Text.Encoding enc = System.Text.Encoding.Default;
                    byte[] arr = enc.GetBytes(instanceString);
                    Process.ThisProcess().Write(arr, 0x008B7CB0);

                }

            }

            if (paramI.HasFlag(ItemInstanceParameters.Spell))
            {
                int spellID = 0;
                stream.Read(out spellID);

                Spell spell = null;
                Spell.SpellDict.TryGetValue(spellID, out spell);

                if (spell == null)
                    throw new Exception("Spell was not found: "+spellID);
                this.Spell = spell;
            }
        }





        public void toItem(oCItem item)
        {
            ItemInstanceParameters p = getParams();


            //Setting up defaults:
            //item.VTBL = 8636420;
            //item.ObjectName.Set("ITGUC_" + ID);



            item.Name.Set(this.Name);
            

            if (p.HasFlag(ItemInstanceParameters.scemeName))
                item.ScemeName.Set(this.ScemeName);
            for (int i = 0; i < 8; i++)
                if (p.HasFlag((ItemInstanceParameters)((ulong)ItemInstanceParameters.protection0 << i)))
                    item.setProtection(i, Protection[i]);
            if (p.HasFlag(ItemInstanceParameters.damageType))
                item.DamageType = (int)DamageType;
            if (p.HasFlag(ItemInstanceParameters.totalDamage))
                item.DamageTotal = TotalDamage;
            for (int i = 0; i < 8; i++)
                if (p.HasFlag((ItemInstanceParameters)((ulong)ItemInstanceParameters.damages0 << i)))
                    item.setDamage(i, Damages[i]);

            if (p.HasFlag(ItemInstanceParameters.range))
                item.Range = Range;

            for (int i = 0; i < conditionAttributes.Length; i++)
                if (p.HasFlag((ItemInstanceParameters)((ulong)ItemInstanceParameters.conditionAttributes0 << i)))
                    item.setConditionalAttribute(i, ConditionAttributes[i]);
            for (int i = 0; i < conditionValues.Length; i++)
                if (p.HasFlag((ItemInstanceParameters)((ulong)ItemInstanceParameters.conditionValues0 << i)))
                    item.setConditionalValue(i, ConditionValues[i]);

            if (p.HasFlag(ItemInstanceParameters.value))
                item.Value = Value;

            if (p.HasFlag(ItemInstanceParameters.mainFlags))
                item.MainFlag = (int)MainFlags;

            if (p.HasFlag(ItemInstanceParameters.flags) || p.HasFlag(ItemInstanceParameters.mainFlags))
                item.Flags = (int)Flags | (int)MainFlags;

            if (p.HasFlag(ItemInstanceParameters.wear))
                item.Wear = (int)Wear;

            if (p.HasFlag(ItemInstanceParameters.materials))
                item.Material = (int)Materials;

            if (p.HasFlag(ItemInstanceParameters.description))
                item.Description.Set(Description);


            for (int i = 0; i < text.Length; i++)
                if (p.HasFlag((ItemInstanceParameters)((ulong)ItemInstanceParameters.text0 << i)))
                    item.setText(i, Text[i]);
            for (int i = 0; i < count.Length; i++)
                if (p.HasFlag((ItemInstanceParameters)((ulong)ItemInstanceParameters.count0 << i)))
                    item.setCount(i, Count[i]);

            if (p.HasFlag(ItemInstanceParameters.visual)){
                item.Visual.Set(Visual.ToUpper());
            }
            if (p.HasFlag(ItemInstanceParameters.visual_Change))
                item.VisualChange.Set(Visual_Change.Trim());
            
            
            if (p.HasFlag(ItemInstanceParameters.effect))
                item.Effect.Set(Effect.ToUpper().Trim());

            if (p.HasFlag(ItemInstanceParameters.visual_skin))
                item.VisualSkin = Visual_skin;
            if (p.HasFlag(ItemInstanceParameters.munition))
            {
                Process process = Process.ThisProcess();
                item.Munition = zCParser.getParser(process).GetIndex("ITGUC_" + munition.ID);////item.Munition =
                //zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', "Set munition: " + munition.ID + " | " + item.Munition + "||" + munition.Visual + " " + this.Visual, 0, "Program.cs", 0);
            }

            if (p.HasFlag(ItemInstanceParameters.Spell))
                item.Spell = Spell.ID;
            item.CreateVisual();
        }
    }
}
