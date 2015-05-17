using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;
using GUC.Enumeration;

namespace GUC.Server.Scripts.Sumpfkraut.VobSystem.Definitions
{
    /**
     *   Class from which all items are instatiated (which are handled by the serverscript).
     */
    class ItemDef : ItemInstance
    {

        // read only in GUC and may conflict with ID of ItemDef in database 
        // --> must test the GUC-interal assignment
        public int getID () { return this.ID; }
        //public void setID (int ID) { this.ID = ID; }

        // ID exclusively for the VobSystem (VobSys) representing the ID in database
        // simply ID as attribute name was already occupied by the GUC internal
        protected int VobSysID;
        public int getVobSysID () { return this.VobSysID; }
        public void setVobSysID (int VobSysID) { this.VobSysID = VobSysID; }

        public string getInstanceName () { return this.InstanceName; }
        // read only in GUC
        //public void setInstanceName (string InstanceName) { this.InstanceName = InstanceName; }

        public string getName () { return this.Name; }
        public void setName (string Name) { this.Name = Name; }

        public string getScemeName () { return this.ScemeName; }
        public void setScemeName (string ScemeName) { this.ScemeName = ScemeName; }

        // setProtection(int[] Protection) is already defined in ItemInstance-class
        public int[] getProtections ()
        {
            DamageTypeIndex[] damageTypeIndices = Enum.GetValues(typeof(DamageTypeIndex)).Cast<DamageTypeIndex>().ToArray();
            int[] protections = new int[damageTypeIndices.Length - 1];
            foreach (DamageTypeIndex dti in damageTypeIndices)
            {
                if ((int) dti == 0)
                {
                    continue;
                }
                protections[(int) dti - 1] = this.getProtection(dti);
            }
            return protections;
        }

        // another get-method already exists (see public int getDamage(DamageTypeIndex index))
        // no set-method due to missing scope in GMP_Server
        // ignores damage type barrier (index 0 in enum DamageTypeIndex) as does the GUC itself mostly
        public int[] getDamages ()
        {
            DamageTypeIndex[] damageTypeIndices = Enum.GetValues(typeof(DamageTypeIndex)).Cast<DamageTypeIndex>().ToArray();
            int[] damages = new int[damageTypeIndices.Length - 1];
            foreach (DamageTypeIndex dti in damageTypeIndices)
            {
                if ((int) dti == 0)
                {
                    continue;
                }
                damages[(int) dti - 1] = this.getDamage(dti);
            }
            return damages;
        }

        public int getValue () { return this.Value; }
        public void setValue (int Value) { this.Value = Value; }

        public Enumeration.MainFlags getMainFlag () { return this.MainFlags; }
        public void setMainFlag (Enumeration.MainFlags MainFlag) { this.MainFlags = MainFlag; }

        public Enumeration.Flags getFlag () { return this.Flags; }
        public void setFlag (Enumeration.Flags Flag) { this.Flags = Flag; }

        public Enumeration.ArmorFlags getArmorFlag () { return this.Wear; }
        public void setArmorFlag (Enumeration.ArmorFlags ArmorFlag) { this.Wear = ArmorFlag; }

        public Enumeration.DamageTypes getDamageType () { return this.DamageType; }
        public void setDamageType (Enumeration.DamageTypes DamageType) { this.DamageType = DamageType; }
        
        public int getTotalDamage () { return this.TotalDamage; }
        public void setTotalDamage (int TotalDamage) { this.TotalDamage = TotalDamage; }

        public int getRange() { return this.Range; }
        public void setRange(int Range) { this.Range = Range; }

        public string getVisual () { return this.Visual; }
        public void setVisual (string Visual) { this.Visual = Visual; }

        public string getVisualChange () { return this.Visual_Change; }
        public void setVisualChange (string VisualChange) { this.Visual_Change = VisualChange; }

        public string getEffect () { return this.Effect; }
        public void setEffect (string Effect) { this.Effect = Effect; }

        public int getVisualSkin () { return this.Visual_skin; }
        public void setVisualSkin (int VisualSkin) { this.Visual_skin = VisualSkin; }

        public Enumeration.MaterialType getMaterial () { return this.Materials; }
        public void setMaterial (Enumeration.MaterialType Material) { this.Materials = Material; }

        // no access to Munition in ItemInstance of the GUC (only by passing it in a constructor)
        //public ItemInstance getMunition ()
        //{
        //    return this.Munition;
        //}
        //public void setMunition (ItemInstance Munition)
        //{
        //    this.Munition = Munition;
        //}

        // no access to IsKeyInstance in ItemInstance of the GUC (only by passing it in a constructor)
        //public bool getIsKeyInstance()
        //{
        //    return this.IsKeyInstance;
        //}
        //public void setIsKeyInstance(bool IsKeyInstance)
        //{
        //    this.IsKeyInstance = IsKeyInstance;
        //}

        //public bool getIsLockPick()
        //{
        //    return this.IsKeyInstance;
        //}
        //public void setIsKeyInstance(bool IsKeyInstance)
        //{
        //    this.IsKeyInstance = IsKeyInstance;
        //}

        public bool getIsTorch () { return this.IsTorch; }
        public void setIsTorch (bool IsTorch) { this.IsTorch = IsTorch; }

        public bool getIsTorchBurning () { return this.IsTorchBurning; }
        public void setIsTorchBurning (bool IsTorchBurning) { this.IsTorchBurning = IsTorchBurning; }

        public bool getIsTorchBurned () { return this.IsTorchBurned; }
        public void setIsTorchBurned (bool IsTorchBurned) { this.IsTorchBurned = IsTorchBurned; }

        public bool getIsGold (){ return this.IsGold; }
        public void setIsGold (bool IsGold) { this.IsGold = IsGold; }



        // -----------------------------------------------------------
        // not part of general ItemDef- and ItemInstance-constructors
        // -----------------------------------------------------------


        public Spell getSpell () { return this.Spell; }
        public void setSpell (Spell Spell) { this.Spell = Spell; }

  
        // descriptive texts and values (appear ingame in the item information panel)

        public string getDescription () { return this.Description; }
        public void setDescription (string Description) { this.Description = Description; }

        public string getText0 () { return this.Text0; }
        public void setText0 (string Text0) { this.Text0 = Text0; }

        public string getText1 () { return this.Text1; }
        public void setText1 (string Text1) { this.Text1 = Text1; }

        public string getText2 () { return this.Text2; }
        public void setText2 (string Text2) { this.Text2 = Text2; }

        public string getText3 () { return this.Text3; }
        public void setText3 (string Text3) { this.Text3 = Text3; }

        public string getText4 () { return this.Text4; }
        public void setText4 (string Text4) { this.Text4 = Text4; }

        public string getText5 () { return this.Text5; }
        public void setText5 (string Text5) { this.Text5 = Text5; }
        
        public int getCount0 () { return this.Count0; }
        public void setCount0 (int Count0) { this.Count0 = Count0; }

        public int getCount1 () { return this.Count1; }
        public void setCount1 (int Count1) { this.Count1 = Count1; }

        public int getCount2 () { return this.Count2; }
        public void setCount2 (int Count2) { this.Count2 = Count2; }

        public int getCount3 () { return this.Count3; }
        public void setCount3 (int Count3) { this.Count3 = Count3; }

        public int getCount4 () { return this.Count4; }
        public void setCount4 (int Count4) { this.Count4 = Count4; }

        public int getCount5 () { return this.Count5; }
        public void setCount5 (int Count5) { this.Count5 = Count5; }
        

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

        

        // potions
        public ItemDef (String instanceName, String name, String scemeName, int value, String visual, String effect)
            : base (instanceName, name, scemeName, value, visual, effect)
        { }

        // weapons
        public ItemDef (String instanceName, String name, DamageTypes dmgType, MainFlags mainFlags, Flags flags, int totalDamage, int range, int value, String visual)
            : base (instanceName, name, dmgType, mainFlags, flags, totalDamage, range, value, visual)
        { }

        // armor
        public ItemDef (String instanceName, String name, int[] protection, int value, String visual, String visual_Change)
            : base (instanceName, name, protection, value, visual, visual_Change)
        { }

        

        public ItemDef (String instanceName, String name, String scemeName, int value, 
            MainFlags mainFlags, Flags flags, String visual)
            : this (instanceName, name, scemeName, null, null, value, 
                mainFlags, flags, 0, 0, 0, 0, visual, null)
        { }

        public ItemDef (String instanceName, String name, String scemeName, int value, 
            MainFlags mainFlags, Flags flags, String visual, String visual_Change, String effect)
            : this (instanceName, name, scemeName, null, null, value, 
                mainFlags, flags, 0, 0, 0, 0, visual, visual_Change, effect)
        { }

        public ItemDef (String instanceName, String name, int[] protection, int[] damages, 
            int value, MainFlags mainFlags, Flags flags, ArmorFlags armorFlags, 
            DamageTypes dmgType, int totalDamage, int range, String visual)
            : this (instanceName, name, protection, damages, 
                value, mainFlags, flags, armorFlags, 
                dmgType, totalDamage, range, visual, null)
        { }
        public ItemDef (String instanceName, String name, String scemeName, int[] protection, 
            int[] damages, int value, MainFlags mainFlags, Flags flags, ArmorFlags armorFlags, 
            DamageTypes dmgType, int totalDamage, int range, String visual)
            : this (instanceName, name, scemeName, protection, 
                damages, value, mainFlags, flags, armorFlags, 
                dmgType, totalDamage, range, visual, null)
        { }
        public ItemDef (String instanceName, String name, int[] protection, int[] damages, 
            int value, MainFlags mainFlags, Flags flags, ArmorFlags armorFlags, DamageTypes dmgType, 
            int totalDamage, int range, String visual, String visual_Change)
            : this (instanceName, name, null, protection, damages, 
                value, mainFlags, flags, armorFlags, dmgType, 
                totalDamage, range, visual, visual_Change, null, 0)
        { }

        public ItemDef (String instanceName, String name, int[] protection, int[] damages, 
            int value, MainFlags mainFlags, Flags flags, ArmorFlags armorFlags, DamageTypes dmgType, 
            int totalDamage, int range, String visual, String visual_Change, String effect)
            : this (instanceName, name, null, protection, damages, 
                value, mainFlags, flags, armorFlags, dmgType, 
                totalDamage, range, visual, visual_Change, effect, 0)
        { }

        public ItemDef (String instanceName, String name, String scemeName, int[] protection, int[] damages, 
            int value, MainFlags mainFlags, Flags flags, ArmorFlags armorFlags, DamageTypes dmgType, 
            int totalDamage, int range, String visual, String visual_Change)
            : this (instanceName, name, scemeName, protection, damages, 
                value, mainFlags, flags, armorFlags, dmgType, 
                totalDamage, range, visual, visual_Change, "", 0)
        { }

        public ItemDef (String instanceName, String name, String scemeName, int[] protection, int[] damages, 
            int value, MainFlags mainFlags, Flags flags, ArmorFlags armorFlags, DamageTypes dmgType, 
            int totalDamage, int range, String visual, String visual_Change, String effect)
            : this (instanceName, name, scemeName, protection, damages, 
                value, mainFlags, flags, armorFlags, dmgType, 
                totalDamage, range, visual, visual_Change, effect, 0, 0, null)
        { }

        public ItemDef (String instanceName, String name, String scemeName, int[] protection, int[] damages, 
            int value, MainFlags mainFlags, Flags flags, ArmorFlags armorFlags, DamageTypes dmgType, 
            int totalDamage, int range, String visual, String visual_Change, String effect, MaterialType types)
            : this (instanceName, name, scemeName, protection, damages, 
                value, mainFlags, flags, armorFlags, dmgType, 
                totalDamage, range, visual, visual_Change, effect, 0, types, null)
        { }

        public ItemDef (String instanceName, String name, String scemeName, int[] protection, int[] damages, 
            int value, MainFlags mainFlags, Flags flags, ArmorFlags armorFlags, DamageTypes dmgType, 
            int totalDamage, int range, String visual, String visual_Change, String effect, MaterialType types, 
            ItemInstance munition)
            : this (instanceName, name, scemeName, protection, damages, 
                value, mainFlags, flags, armorFlags, dmgType, 
                totalDamage, range, visual, visual_Change, effect, 0, types, 
                munition)
        { }

        public ItemDef (String instanceName, String name, String scemeName, int[] protection, int[] damages, 
            int value, MainFlags mainFlags, Flags flags, ArmorFlags armorFlags, DamageTypes dmgType, 
            int totalDamage, int range, String visual, String visual_Change, String effect, int visualSkin, 
            MaterialType types, ItemInstance munition)
            : this (instanceName, name, scemeName, protection, damages, 
                value, mainFlags, flags, armorFlags, dmgType, 
                totalDamage, range, visual, visual_Change, effect, visualSkin, 
                types, munition, false, false, false, false, false)
        { }

        public ItemDef (String instanceName, String name, String scemeName, int[] protection, 
            int[] damages, int value, MainFlags mainFlags, Flags flags, ArmorFlags armorFlags, 
            DamageTypes dmgType, int totalDamage, int range, String visual, String visual_Change, 
            String effect, int visualSkin, MaterialType types, ItemInstance munition, bool keyInstance, 
            bool torch, bool torchBurning, bool torchBurned, bool gold)
            : base (instanceName, name, scemeName, protection, 
                damages, value, mainFlags, flags, armorFlags, 
                dmgType, totalDamage, range, visual, visual_Change, 
                effect, visualSkin, types, munition, keyInstance, 
                torch, torchBurning, torchBurned, gold)
        {
            this.OnUse += new Scripting.Events.UseItemEventHandler(this.UseItem);
            this.OnEquip += new Scripting.Events.NPCEquipEventHandler(this.EquipItem);
            this.OnUnEquip += new Scripting.Events.NPCEquipEventHandler(this.UnequipItem);
        }



        protected void EquipItem (NPCProto npc, Item item)
        {
            //npc.HP      = this.getOnEquip_HPChange();
            //npc.HPMax   = this.getOnEquip_HPMaxChange();
            //npc.MP      = this.getOnEquip_MPChange();
            //npc.MPMax   = this.getOnEquip_MPMaxChange();
        }

        protected void UnequipItem (NPCProto npc, Item item)
        {
            //npc.HP      = this.getOnUnEquip_HPChange();
            //npc.HPMax   = this.getOnUnEquip_HPMaxChange();
            //npc.MP      = this.getOnUnEquip_MPChange();
            //npc.MPMax   = this.getOnUnEquip_MPMaxChange();
        }

        protected void UseItem (NPCProto npc, Item item, short state, short targetState)
        {
            //if (!(state == -1 && targetState == 0))
            //{
            //    return;
            //}

            //if ((npc.HP + this.getOnUse_HPChange()) >= this.getOnUse_HP_Min())
            //{
            //    // 
            //    //if ((npc.HP + this.getOnUse_HPChange()))
            //    npc.HP =+ this.getOnUse_HPChange();
            //}
            //else
            //{
            //    //if (this.getHP_Min <= this.getHPMax_min)
            //    npc.HP = this.getOnUse_HP_Min();
            //}

            //if ((npc.HPMax + this.getOnUse_HPMaxChange()) >= this.getOnUse_HPMax_Min())
            //{
            //    npc.HPMax =+ this.getOnUse_HPMaxChange();
            //}
            //else
            //{
            //    npc.HPMax = this.getOnUse_HPMax_Min();
            //}


            //if ((npc.MP + this.getOnUse_MPChange()) >= this.getOnUse_MP_Min())
            //{
            //    npc.MP =+ this.getOnUse_MPChange();
            //}

            //if ((npc.MPMax + this.getOnUse_MPMaxChange()) >= this.getOnUse_MPMax_Min())
            //{
            //    npc.MPMax =+ this.getOnUse_MPMaxChange();
            //}
            

        }


    }
}
