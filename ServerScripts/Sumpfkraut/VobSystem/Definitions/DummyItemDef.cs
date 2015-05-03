using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.Server.Scripting.Objects;


namespace GUC.Server.Scripts.Sumpfkraut.VobSystem.Definitions
{
    class DummyItemDef
    {

        protected int ID;
        public int getID () { return this.ID; }
        public void setID (int ID) { this.ID = ID; }

        protected string InstanceName;
        public string getInstanceName () { return this.InstanceName; }
        // read only in GUC
        public void setInstanceName (string InstanceName) { this.InstanceName = InstanceName; }

        protected string Name;
        public string getName () { return this.Name; }
        public void setName (string Name) { this.Name = Name; }

        protected string ScemeName;
        public string getScemeName () { return this.ScemeName; }
        public void setScemeName (string ScemeName) { this.ScemeName = ScemeName; }

        protected int[] Protections = new int[Enum.GetValues(typeof(DamageTypeIndex)).Length];
        public int[] getProtections () { return this.Protections; }
        public int getProtections (DamageTypeIndex index) 
        {
            if (index == DamageTypeIndex.DAM_INDEX_BARRIER)
                throw new Exception("Don't use Protectiontype Barrier!");
            return this.Protections[(int)index - 1];
        }
        public void setProtections (int[] Protections) { this.Protections = Protections; }
        public void setProtections (DamageTypeIndex index, int value)
        {
            if (index == DamageTypeIndex.DAM_INDEX_BARRIER)
                throw new Exception("Don't use Protectiontype Barrier!");
            this.Protections[(int)index - 1] = value;
        }

        // get-method already exists (see public int getDamage(DamageTypeIndex index))
        protected int[] Damages;
        public int[] getDamages()
        {
            return this.Damages;
        }
        public void setDamages(int[] Damages)
        {
            this.Damages = Damages;
        }

        protected int Value;
        public int getValue () { return this.Value; }
        public void setValue (int Value) { this.Value = Value; }

        protected Enumeration.MainFlags MainFlag;
        public Enumeration.MainFlags getMainFlag () { return this.MainFlag; }
        public void setMainFlag (Enumeration.MainFlags MainFlag) { this.MainFlag = MainFlag; }

        protected Enumeration.Flags Flag;
        public Enumeration.Flags getFlag () { return this.Flag; }
        public void setFlag (Enumeration.Flags Flag) { this.Flag = Flag; }

        protected Enumeration.ArmorFlags ArmorFlag;
        public Enumeration.ArmorFlags getArmorFlag () { return this.ArmorFlag; }
        public void setArmorFlag (Enumeration.ArmorFlags ArmorFlag) { this.ArmorFlag = ArmorFlag; }

        protected Enumeration.DamageTypes DamageType;
        public Enumeration.DamageTypes getDamageType () { return this.DamageType; }
        public void setDamageType (Enumeration.DamageTypes DamageType) { this.DamageType = DamageType; }

        protected int TotalDamage;
        public int getTotalDamage () { return this.TotalDamage; }
        public void setTotalDamage (int TotalDamage) { this.TotalDamage = TotalDamage; }

        protected int Range;
        public int getRange() { return this.Range; }
        public void setRange(int Range) { this.Range = Range; }

        protected string Visual;
        public string getVisual () { return this.Visual; }
        public void setVisual (string Visual) { this.Visual = Visual; }

        protected string VisualChange;
        public string getVisualChange () { return this.VisualChange; }
        public void setVisualChange (string VisualChange) { this.VisualChange = VisualChange; }

        protected string Effect;
        public string getEffect () { return this.Effect; }
        public void setEffect (string Effect) { this.Effect = Effect; }
        
        protected int VisualSkin;
        public int getVisualSkin () { return this.VisualSkin; }
        public void setVisualSkin (int VisualSkin) { this.VisualSkin = VisualSkin; }

        protected Enumeration.MaterialType Material;
        public Enumeration.MaterialType getMaterial () { return this.Material; }
        public void setMaterial (Enumeration.MaterialType Material) { this.Material = Material; }

        protected ItemDef Munition;
        public ItemDef getMunition () { return this.Munition; }
        public void setMunition (ItemDef Munition) { this.Munition = Munition; }

        protected bool IsKeyInstance;
        public bool getIsKeyInstance (){ return this.IsKeyInstance; }
        public void setIsKeyInstance (bool IsKeyInstance) { this.IsKeyInstance = IsKeyInstance; }

        protected bool IsTorch;
        public bool getIsTorch () { return this.IsTorch; }
        public void setIsTorch (bool IsTorch) { this.IsTorch = IsTorch; }

        protected bool IsTorchBurning;
        public bool getIsTorchBurning () { return this.IsTorchBurning; }
        public void setIsTorchBurning (bool IsTorchBurning) { this.IsTorchBurning = IsTorchBurning; }

        protected bool IsTorchBurned;
        public bool getIsTorchBurned () { return this.IsTorchBurned; }
        public void setIsTorchBurned (bool IsTorchBurned) { this.IsTorchBurned = IsTorchBurned; }

        protected bool IsGold;
        public bool getIsGold (){ return this.IsGold; }
        public void setIsGold (bool IsGold) { this.IsGold = IsGold; }



        // only attributes which aren't used in constructor from here on after

        protected Spell Spell;
        public Spell getSpell () { return this.Spell; }
        public void setSpell (Spell Spell) { this.Spell = Spell; }



        // descriptive texts and values (appear ingame in the item information panel)

        protected string Description;
        public string getDescription () { return this.Description; }
        public void setDescription (string Description) { this.Description = Description; }

        protected string Text0;
        public string getText0 () { return this.Text0; }
        public void setText0 (string Text0) { this.Text0 = Text0; }

        protected string Text1;
        public string getText1 () { return this.Text1; }
        public void setText1 (string Text0) { this.Text0 = Text1; }

        protected string Text2;
        public string getText2 () { return this.Text2; }
        public void setText2 (string Text0) { this.Text0 = Text2; }

        protected string Text3;
        public string getText3 () { return this.Text3; }
        public void setText3 (string Text0) { this.Text0 = Text3; }

        protected string Text4;
        public string getText4 () { return this.Text4; }
        public void setText4 (string Text0) { this.Text0 = Text4; }

        protected string Text5;
        public string getText5 () { return this.Text5; }
        public void setText5 (string Text0) { this.Text0 = Text5; }

        protected int Count0;
        public int getCount0 () { return this.Count0; }
        public void setCount0 (int Count0) { this.Count0 = Count0; }

        protected int Count1;
        public int getCount1 () { return this.Count1; }
        public void setCount1 (int Count0) { this.Count0 = Count1; }

        protected int Count2;
        public int getCount2 () { return this.Count2; }
        public void setCount2 (int Count0) { this.Count0 = Count2; }

        protected int Count3;
        public int getCount3 () { return this.Count3; }
        public void setCount3 (int Count0) { this.Count0 = Count3; }

        protected int Count4;
        public int getCount4 () { return this.Count4; }
        public void setCount4 (int Count0) { this.Count0 = Count4; }

        protected int Count5;
        public int getCount5 () { return this.Count5; }
        public void setCount5 (int Count0) { this.Count0 = Count5; }

   

        // attributes which are part of events
        
        // triggered with OnUse

        protected int OnUse_HPChange = 0;
        public int getOnUse_HPChange () { return this.OnUse_HPChange; }
        public void setOnUse_HPChange (int HPChange) { this.OnUse_HPChange = HPChange; }

        protected int OnUse_HPMaxChange = 0;
        public int getOnUse_HPMaxChange () { return this.OnUse_HPMaxChange; }
        public void setOnUse_HPMaxChange (int HPMaxChange) { this.OnUse_HPMaxChange = HPMaxChange; }

        protected int OnUse_MPChange = 0;
        public int getOnUse_MPChange () { return this.OnUse_MPChange; }
        public void setOnUse_MPChange (int MPChange) { this.OnUse_MPChange = MPChange; }

        protected int OnUse_MPMaxChange = 0;
        public int getOnUse_MPMaxChange () { return this.OnUse_MPMaxChange; }
        public void setOnUse_MPMaxChange (int MPMaxChange) { this.OnUse_MPMaxChange = MPMaxChange; }

        protected int OnUse_HP_Min = 1;
        public int getOnUse_HP_Min () { return this.OnUse_HP_Min; }
        public void setOnUse_HP_Min (int HP_Min) { this.OnUse_HP_Min = HP_Min; }

        protected int OnUse_HPMax_Min = 1;
        public int getOnUse_HPMax_Min () { return this.OnUse_HPMax_Min; }
        public void setOnUse_HPMax_Min (int HPMax_Min) { this.OnUse_HPMax_Min = HPMax_Min; }

        protected int OnUse_MP_Min = 0;
        public int getOnUse_MP_Min () { return this.OnUse_MP_Min; }
        public void setOnUse_MP_Min (int MP_Min) { this.OnUse_MP_Min = MP_Min; }

        protected int OnUse_MPMax_Min = 0;
        public int getOnUse_MPMax_Min () { return this.OnUse_MPMax_Min; }
        public void setOnUse_MPMax_Min (int MPMax_Min) { this.OnUse_MPMax_Min = MPMax_Min; }



        // triggered with OnEquip

        protected int OnEquip_HPChange = 0;
        public int getOnEquip_HPChange () { return this.OnEquip_HPChange; }
        public void setOnEquip_HPChange (int HPChange) { this.OnEquip_HPChange = HPChange; }

        protected int OnEquip_HPMaxChange = 0;
        public int getOnEquip_HPMaxChange () { return this.OnEquip_HPMaxChange; }
        public void setOnEquip_HPMaxChange (int HPMaxChange) { this.OnEquip_HPMaxChange = HPMaxChange; }

        protected int OnEquip_MPChange = 0;
        public int getOnEquip_MPChange () { return this.OnEquip_MPChange; }
        public void setOnEquip_MPChange (int MPChange) { this.OnEquip_MPChange = MPChange; }

        protected int OnEquip_MPMaxChange = 0;
        public int getOnEquip_MPMaxChange () { return this.OnEquip_MPMaxChange; }
        public void setOnEquip_MPMaxChange (int MPMaxChange) { this.OnEquip_MPMaxChange = MPMaxChange; }

        protected int OnEquip_HP_Min = 1;
        public int getOnEquip_HP_Min () { return this.OnEquip_HP_Min; }
        public void setOnEquip_HP_Min (int HP_Min) { this.OnEquip_HP_Min = HP_Min; }

        protected int OnEquip_HPMax_Min = 1;
        public int getOnEquip_HPMax_Min () { return this.OnEquip_HPMax_Min; }
        public void setOnEquip_HPMax_Min (int HPMax_Min) { this.OnEquip_HPMax_Min = HPMax_Min; }

        protected int OnEquip_MP_Min = 0;
        public int getOnEquip_MP_Min () { return this.OnEquip_MP_Min; }
        public void setOnEquip_MP_Min (int MP_Min) { this.OnEquip_MP_Min = MP_Min; }

        protected int OnEquip_MPMax_Min = 0;
        public int getOnEquip_MPMax_Min () { return this.OnEquip_MPMax_Min; }
        public void setOnEquip_MPMax_Min (int MPMax_Min) { this.OnEquip_MPMax_Min = MPMax_Min; }



        // triggered with OnUnEquip

        protected int OnUnEquip_HPChange = 0;
        public int getOnUnEquip_HPChange () { return this.OnUnEquip_HPChange; }
        public void setOnUnEquip_HPChange (int HPChange) { this.OnUnEquip_HPChange = HPChange; }

        protected int OnUnEquip_HPMaxChange = 0;
        public int getOnUnEquip_HPMaxChange () { return this.OnUnEquip_HPMaxChange; }
        public void setOnUnEquip_HPMaxChange (int HPMaxChange) { this.OnUnEquip_HPMaxChange = HPMaxChange; }

        protected int OnUnEquip_MPChange = 0;
        public int getOnUnEquip_MPChange () { return this.OnUnEquip_MPChange; }
        public void setOnUnEquip_MPChange (int MPChange) { this.OnUnEquip_MPChange = MPChange; }

        protected int OnUnEquip_MPMaxChange = 0;
        public int getOnUnEquip_MPMaxChange () { return this.OnUnEquip_MPMaxChange; }
        public void setOnUnEquip_MPMaxChange (int MPMaxChange) { this.OnUnEquip_MPMaxChange = MPMaxChange; }

        protected int OnUnEquip_HP_Min = 1;
        public int getOnUnEquip_HP_Min () { return this.OnUnEquip_HP_Min; }
        public void setOnUnEquip_HP_Min (int HP_Min) { this.OnUnEquip_HP_Min = HP_Min; }

        protected int OnUnEquip_HPMax_Min = 1;
        public int getOnUnEquip_HPMax_Min () { return this.OnUnEquip_HPMax_Min; }
        public void setOnUnEquip_HPMax_Min (int HPMax_Min) { this.OnUnEquip_HPMax_Min = HPMax_Min; }

        protected int OnUnEquip_MP_Min = 0;
        public int getOnUnEquip_MP_Min () { return this.OnUnEquip_MP_Min; }
        public void setOnUnEquip_MP_Min (int MP_Min) { this.OnUnEquip_MP_Min = MP_Min; }

        protected int OnUnEquip_MPMax_Min = 0;
        public int getOnUnEquip_MPMax_Min () { return this.OnUnEquip_MPMax_Min; }
        public void setOnUnEquip_MPMax_Min (int MPMax_Min) { this.OnUnEquip_MPMax_Min = MPMax_Min; }



        public DummyItemDef ()
        {
            
        }
        
    }
}
