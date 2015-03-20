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

        public bool getIsGold (){ return this.IsGold; }
        public void setIsGold (bool IsGold) { this.IsGold = IsGold; }

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



        public string getEffect () { return this.Effect; }
        public void setEffect (string Effect) { this.Effect = Effect; }



        public Spell getSpell () { return this.Spell; }
        public void setSpell (Spell Spell) { this.Spell = Spell; }

        public Enumeration.ArmorFlags getWear () { return this.Wear; }
        public void setWear (Enumeration.ArmorFlags Wear) { this.Wear = Wear; }

        //public Enumeration.DamageType DamageType
        //{
        //    get { return this.DamageType; }
        //    set { this.DamageType = value; }
        //}
        public Enumeration.DamageTypes getDamageType () { return this.DamageType; }
        public void setDamageType (Enumeration.DamageTypes DamageType) { this.DamageType = DamageType; }
        
        public int getRange() { return this.Range; }
        public void setRange(int Range) { this.Range = Range; }
        
        public int getTotalDamage () { return this.TotalDamage; }
        public void setTotalDamage (int TotalDamage) { this.TotalDamage = TotalDamage; }



        // get-method already exists (see public int getDamage(DamageTypeIndex index))
        //public int[] getDamages()
        //{
        //    return this.getDamages();
        //    //return this.Damages;
        //}
        //public void setDamages(int[] Damages)
        //{
        //    this.Damages = Damages;
        //}

        //public ItemInstance getMunition ()
        //{
        //    return this.Munition;
        //}
        //public void setMunition (ItemInstance Munition)
        //{
        //    this.Munition = Munition;
        //}

        
        
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

        
        //static ItemDef ii;
        //static ItemDef get ()
        //{
        //    if (ii == null)
        //        ii = new ItemDef();
        //    return ii;
        //}

        //protected ItemDef (String instanceName)
        //    : base (instanceName)
        //{
        //    //this.InstanceName;

        //    //this.Name;
        //    //this.Value;
        //    //this.Visual;
        //    //this.Visual_Change;
        //    //this.Visual_skin;
        //    //this.MainFlags;
        //    //this.Flags;

        //    //this.Materials;
        //    //this.Description;

        //    //this.Effect = "";

        //    //this.IsGold;
        //    //this.IsLockPick;
        //    //this.IsTorch;
        //    //this.IsTorchBurned;
        //    //this.IsTorchBurning;

        //    //this.Range;
        //    //this.Spell;

        //    //this.Wear;
        //    //this.ScemeName;

        //    this.OnUse += new Scripting.Events.UseItemEventHandler(this.UseItem);
        //    this.OnEquip += new Scripting.Events.NPCEquipEventHandler(this.EquipItem);
        //    this.OnUnEquip += new Scripting.Events.NPCEquipEventHandler(this.UnequipItem);

        //    CreateItemInstance();
        //}

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

        

        public ItemDef (String instanceName, String name, String scemeName, int value, MainFlags mainFlags, Flags flags, String visual)
            : this (instanceName, name, scemeName, null, null, value, mainFlags, flags, 0, 0, 0, 0, visual, null)
        { }

        public ItemDef (String instanceName, String name, String scemeName, int value, MainFlags mainFlags, Flags flags, String visual, String visual_Change, String effect)
            : this (instanceName, name, scemeName, null, null, value, mainFlags, flags, 0, 0, 0, 0, visual, visual_Change, effect)
        { }

        public ItemDef (String instanceName, String name, int[] protection, int[] damages, int value, MainFlags mainFlags, Flags flags, ArmorFlags armorFlags, DamageTypes dmgType, int totalDamage, int range, String visual)
            : this (instanceName, name, protection, damages, value, mainFlags, flags, armorFlags, dmgType, totalDamage, range, visual, null)
        { }
        public ItemDef (String instanceName, String name, String scemeName, int[] protection, int[] damages, int value, MainFlags mainFlags, Flags flags, ArmorFlags armorFlags, DamageTypes dmgType, int totalDamage, int range, String visual)
            : this (instanceName, name, scemeName, protection, damages, value, mainFlags, flags, armorFlags, dmgType, totalDamage, range, visual, null)
        { }
        public ItemDef (String instanceName, String name, int[] protection, int[] damages, int value, MainFlags mainFlags, Flags flags, ArmorFlags armorFlags, DamageTypes dmgType, int totalDamage, int range, String visual, String visual_Change)
            : this (instanceName, name, null, protection, damages, value, mainFlags, flags, armorFlags, dmgType, totalDamage, range, visual, visual_Change, null, 0)
        { }

        public ItemDef (String instanceName, String name, int[] protection, int[] damages, int value, MainFlags mainFlags, Flags flags, ArmorFlags armorFlags, DamageTypes dmgType, int totalDamage, int range, String visual, String visual_Change, String effect)
            : this (instanceName, name, null, protection, damages, value, mainFlags, flags, armorFlags, dmgType, totalDamage, range, visual, visual_Change, effect, 0)
        { }

        public ItemDef (String instanceName, String name, String scemeName, int[] protection, int[] damages, int value, MainFlags mainFlags, Flags flags, ArmorFlags armorFlags, DamageTypes dmgType, int totalDamage, int range, String visual, String visual_Change)
            : this (instanceName, name, scemeName, protection, damages, value, mainFlags, flags, armorFlags, dmgType, totalDamage, range, visual, visual_Change, "", 0)
        { }

        public ItemDef (String instanceName, String name, String scemeName, int[] protection, int[] damages, int value, MainFlags mainFlags, Flags flags, ArmorFlags armorFlags, DamageTypes dmgType, int totalDamage, int range, String visual, String visual_Change, String effect)
            : this (instanceName, name, scemeName, protection, damages, value, mainFlags, flags, armorFlags, dmgType, totalDamage, range, visual, visual_Change, effect, 0, 0, null)
        { }

        public ItemDef (String instanceName, String name, String scemeName, int[] protection, int[] damages, int value, MainFlags mainFlags, Flags flags, ArmorFlags armorFlags, DamageTypes dmgType, int totalDamage, int range, String visual, String visual_Change, String effect, MaterialType types)
            : this (instanceName, name, scemeName, protection, damages, value, mainFlags, flags, armorFlags, dmgType, totalDamage, range, visual, visual_Change, effect, 0, types, null)
        { }

        public ItemDef (String instanceName, String name, String scemeName, int[] protection, int[] damages, int value, MainFlags mainFlags, Flags flags, ArmorFlags armorFlags, DamageTypes dmgType, int totalDamage, int range, String visual, String visual_Change, String effect, MaterialType types, ItemInstance munition)
            : this (instanceName, name, scemeName, protection, damages, value, mainFlags, flags, armorFlags, dmgType, totalDamage, range, visual, visual_Change, effect, 0, types, munition)
        { }

        public ItemDef (String instanceName, String name, String scemeName, int[] protection, int[] damages, int value, MainFlags mainFlags, Flags flags, ArmorFlags armorFlags, DamageTypes dmgType, int totalDamage, int range, String visual, String visual_Change, String effect, int visualSkin, MaterialType types, ItemInstance munition)
            : this (instanceName, name, scemeName, protection, damages, value, mainFlags, flags, armorFlags, dmgType, totalDamage, range, visual, visual_Change, effect, visualSkin, types, munition, false, false, false, false, false)
        { }

        public ItemDef (String instanceName, String name, String scemeName, int[] protection, int[] damages, int value, MainFlags mainFlags, Flags flags, ArmorFlags armorFlags, DamageTypes dmgType, int totalDamage, int range, String visual, String visual_Change, String effect, int visualSkin, MaterialType types, ItemInstance munition, bool keyInstance, bool torch, bool torchBurning, bool torchBurned, bool gold)
            : base (instanceName, name, scemeName, protection, damages, value, mainFlags, flags, armorFlags, dmgType, totalDamage, range, visual, visual_Change, effect, visualSkin, types, munition, keyInstance, torch, torchBurning, torchBurned, gold)
            //: base (instanceName)
        {
            //this.itemInstances.Name = name;
            //this.itemInstances.Protection = protection;
            //this.itemInstances.Damages = damages;
            //this.itemInstances.Value = value;
            //this.itemInstances.MainFlags = mainFlags;
            //this.itemInstances.Flags = flags;
            //this.itemInstances.Wear = armorFlags;
            //this.itemInstances.Visual = visual;
            //this.itemInstances.Visual_Change = visual_Change;
            //this.itemInstances.Visual_skin = visualSkin;
            //this.itemInstances.ScemeName = scemeName;
            //this.itemInstances.Effect = effect;
            //if(munition != null)
            //    this.itemInstances.Munition = munition.itemInstances;

            //this.itemInstances.DamageType = dmgType;
            //this.itemInstances.Range = range;
            //this.itemInstances.TotalDamage = totalDamage;
            //this.itemInstances.isKeyInstance = keyInstance;
            //this.IsTorch = torch;
            //this.IsTorchBurned = torchBurned;
            //this.IsTorchBurning = torchBurning;
            //this.IsGold = gold;

            this.OnUse += new Scripting.Events.UseItemEventHandler(this.UseItem);
            this.OnEquip += new Scripting.Events.NPCEquipEventHandler(this.EquipItem);
            this.OnUnEquip += new Scripting.Events.NPCEquipEventHandler(this.UnequipItem);

            CreateItemInstance();
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
