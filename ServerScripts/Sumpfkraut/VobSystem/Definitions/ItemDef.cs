using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.Server.WorldObjects;

namespace GUC.Server.Scripts.Sumpfkraut.VobSystem.Definitions
{
    /**
     *   Class from which all items are instatiated (which are handled by the serverscript).
     *   Not, that ItemDef is the only vob-definition-class which does not inherited from VobDef.
     *   This is due to C# being unable to multiple inheritance, while inheriting from ItemInstance
     *   is necessary to obtain all the convenient functionality.
     */
    class ItemDef : VobDef
    {

        #region dictionaries

        protected static Dictionary<int, ItemDef> defById = new Dictionary<int, ItemDef>();
        protected static Dictionary<string, ItemDef> defByName = new Dictionary<string, ItemDef>();

        #endregion



        #region standard attributes

        new public static readonly String _staticName = "ItemDef (static)";
        new protected String _objName = "ItemDef (default)";

        protected ItemInstance itemInstance;
        public ItemInstance getItemInstance () { return this.itemInstance; }
        public void setItemInstance (ItemInstance itemInstance) { this.itemInstance = itemInstance; }

        protected string instanceName;
        public string getInstanceName () { return this.instanceName; }
        public void setInstanceName (string instanceName) 
        { 
            this.instanceName = instanceName; 
        }

        protected string name;
        public string getName () { return this.name; }
        public void setName (string name) 
        { 
            this.name = name; 
        }

        protected string scemeName;
        public string getScemeName () { return this.scemeName; }
        public void setScemeName (string scemeName) 
        { 
            this.scemeName = scemeName;
        }

        protected int[] protections;
        public int[] getProtections () { return this.protections; }
        public void setProtections (int[] protections) 
        {
            this.protections = protections;
        }

        protected int[] damages;
        public int[] getDamages ()
        {
            return this.damages;
        }
        public void setDamages (int[] damages)
        {
            this.damages = damages;
        }
        // another get-method already exists (see public int getDamage(DamageTypeIndex index))
        // no set-method due to missing scope in GMP_Server
        // ignores damage type barrier (index 0 in enum DamageTypeIndex) as does the GUC itself mostly
        //public int[] getDamages ()
        //{
        //    DamageTypeIndex[] damageTypeIndices = Enum.GetValues(typeof(DamageTypeIndex)).Cast<DamageTypeIndex>().ToArray();
        //    int[] damages = new int[damageTypeIndices.Length - 1];
        //    foreach (DamageTypeIndex dti in damageTypeIndices)
        //    {
        //        if ((int) dti == 0)
        //        {
        //            continue;
        //        }
        //        damages[(int) dti - 1] = this.getDamage(dti);
        //    }
        //    return damages;
        //}

        protected int value;
        public int getValue () { return this.value; }
        public void setValue (int value) 
        {
            this.value = value;
        }

        protected ItemType itemType;
        public ItemType getItemType () { return this.itemType; }
        public void setItemType (ItemType itemType)
        {
            this.itemType = itemType;
        }

        //protected Enumeration.MainFlags mainFlag;
        //public Enumeration.MainFlags getMainFlag () { return this.mainFlag; }
        //public void setMainFlag (Enumeration.MainFlags mainFlag) 
        //{ 
        //    this.mainFlag = mainFlag; 
        //}

        //protected Enumeration.Flags flag;
        //public Enumeration.Flags getFlag () { return this.flag; }
        //public void setFlag (Enumeration.Flags flag) 
        //{ 
        //    this.flag = flag; 
        //}

        //protected Enumeration.ArmorFlags armorFlag;
        //public Enumeration.ArmorFlags getArmorFlag () { return this.armorFlag; }
        //public void setArmorFlag (Enumeration.ArmorFlags armorFlag) 
        //{ 
        //    this.armorFlag = armorFlag; 
        //}

        //protected Enumeration.DamageTypes damageType;
        //public Enumeration.DamageTypes getDamageType () { return this.damageType; }
        //public void setDamageType (Enumeration.DamageTypes damageType) 
        //{ 
        //    this.damageType = damageType; 
        //}

        //protected int totalDamage;
        //public int getTotalDamage () { return this.totalDamage; }
        //public void setTotalDamage (int totalDamage) 
        //{ 
        //    this.totalDamage = totalDamage; 
        //}

        protected int range;
        public int getRange() { return this.range; }
        public void setRange(int range) 
        { 
            this.range = range; 
        }

        protected string visual;
        public string getVisual () { return this.visual; }
        public void setVisual (string visual) 
        { 
            this.visual = visual; 
        }

        protected string visualChange;
        public string getVisualChange () { return this.visualChange; }
        public void setVisualChange (string visualChange) 
        { 
            this.visualChange = visualChange; 
        }

        protected string effect;
        public string getEffect () { return this.effect; }
        public void setEffect (string effect) 
        { 
            this.effect = effect; 
        }

        protected int visualSkin;
        public int getVisualSkin () { return this.visualSkin; }
        public void setVisualSkin (int visualSkin) 
        { 
            this.visualSkin = visualSkin; 
        }

        protected ItemMaterial material;
        public ItemMaterial getMaterial () { return this.material; }
        public void setMaterial (ItemMaterial material) 
        { 
            this.material = material; 
        }

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

        protected bool isTorch;
        public bool getIsTorch () { return this.isTorch; }
        public void setIsTorch (bool isTorch) 
        { 
            this.isTorch = isTorch; 
        }

        protected bool isTorchBurning;
        public bool getIsTorchBurning () { return this.isTorchBurning; }
        public void setIsTorchBurning (bool isTorchBurning) 
        { 
            this.isTorchBurning = isTorchBurning; 
        }

        protected bool isTorchBurned;
        public bool getIsTorchBurned () { return this.isTorchBurned; }
        public void setIsTorchBurned (bool isTorchBurned) 
        { 
            this.isTorchBurned = isTorchBurned; 
        }

        protected bool isGold;
        public bool getIsGold (){ return this.isGold; }
        public void setIsGold (bool isGold) 
        { 
            this.isGold = isGold; 
        }



        // -----------------------------------------------------------
        // not part of general ItemDef- and ItemInstance-constructors
        // -----------------------------------------------------------


        protected SpellDef spell;
        public SpellDef getSpell () { return this.spell; }
        public void setSpell (SpellDef spell) 
        { 
            this.spell = spell; 
        }

  
        // descriptive texts and values (appear ingame in the item information panel)

        protected string description;
        public string getDescription () { return this.description; }
        public void setDescription (string description) 
        { 
            this.description = description; 
        }

        protected string[] text;
        public string[] getText () { return this.text; }
        public void setText (string[] text)
        {
            this.text = text;
        }
        //public string getText0 () { return this.Text0; }
        //public void setText0 (string Text0) { this.Text0 = Text0; }

        //public string getText1 () { return this.Text1; }
        //public void setText1 (string Text1) { this.Text1 = Text1; }

        //public string getText2 () { return this.Text2; }
        //public void setText2 (string Text2) { this.Text2 = Text2; }

        //public string getText3 () { return this.Text3; }
        //public void setText3 (string Text3) { this.Text3 = Text3; }

        //public string getText4 () { return this.Text4; }
        //public void setText4 (string Text4) { this.Text4 = Text4; }

        //public string getText5 () { return this.Text5; }
        //public void setText5 (string Text5) { this.Text5 = Text5; }

        protected int[] count;
        public int[] getCount () { return this.count; }
        public void setCount (int[] count) 
        { 
            this.count = count; 
        }
        //public int getCount0 () { return this.Count0; }
        //public void setCount0 (int Count0) { this.Count0 = Count0; }

        //public int getCount1 () { return this.Count1; }
        //public void setCount1 (int Count1) { this.Count1 = Count1; }

        //public int getCount2 () { return this.Count2; }
        //public void setCount2 (int Count2) { this.Count2 = Count2; }

        //public int getCount3 () { return this.Count3; }
        //public void setCount3 (int Count3) { this.Count3 = Count3; }

        //public int getCount4 () { return this.Count4; }
        //public void setCount4 (int Count4) { this.Count4 = Count4; }

        //public int getCount5 () { return this.Count5; }
        //public void setCount5 (int Count5) { this.Count5 = Count5; }

        #endregion



        #region OnUse attributes
        // triggered with OnUse

        protected int onUse_HPChange = 0;
        public int getOnUse_HPChange () { return this.onUse_HPChange; }
        public void setOnUse_HPChange (int HPChange) { this.onUse_HPChange = HPChange; }

        protected int onUse_HPMaxChange = 0;
        public int getOnUse_HPMaxChange () { return this.onUse_HPMaxChange; }
        public void setOnUse_HPMaxChange (int HPMaxChange) { this.onUse_HPMaxChange = HPMaxChange; }

        protected int onUse_MPChange = 0;
        public int getOnUse_MPChange () { return this.onUse_MPChange; }
        public void setOnUse_MPChange (int MPChange) { this.onUse_MPChange = MPChange; }

        protected int onUse_MPMaxChange = 0;
        public int getOnUse_MPMaxChange () { return this.onUse_MPMaxChange; }
        public void setOnUse_MPMaxChange (int MPMaxChange) { this.onUse_MPMaxChange = MPMaxChange; }

        protected int onUse_HP_Min = 1;
        public int getOnUse_HP_Min () { return this.onUse_HP_Min; }
        public void setOnUse_HP_Min (int HP_Min) { this.onUse_HP_Min = HP_Min; }

        protected int onUse_HPMax_Min = 1;
        public int getOnUse_HPMax_Min () { return this.onUse_HPMax_Min; }
        public void setOnUse_HPMax_Min (int HPMax_Min) { this.onUse_HPMax_Min = HPMax_Min; }

        protected int onUse_MP_Min = 0;
        public int getOnUse_MP_Min () { return this.onUse_MP_Min; }
        public void setOnUse_MP_Min (int MP_Min) { this.onUse_MP_Min = MP_Min; }

        protected int onUse_MPMax_Min = 0;
        public int getOnUse_MPMax_Min () { return this.onUse_MPMax_Min; }
        public void setOnUse_MPMax_Min (int MPMax_Min) { this.onUse_MPMax_Min = MPMax_Min; }

        #endregion
        
        #region OnEquip attributes
        // triggered with OnEquip

        protected int onEquip_HPChange = 0;
        public int getOnEquip_HPChange () { return this.onEquip_HPChange; }
        public void setOnEquip_HPChange (int HPChange) { this.onEquip_HPChange = HPChange; }

        protected int onEquip_HPMaxChange = 0;
        public int getOnEquip_HPMaxChange () { return this.onEquip_HPMaxChange; }
        public void setOnEquip_HPMaxChange (int HPMaxChange) { this.onEquip_HPMaxChange = HPMaxChange; }

        protected int onEquip_MPChange = 0;
        public int getOnEquip_MPChange () { return this.onEquip_MPChange; }
        public void setOnEquip_MPChange (int MPChange) { this.onEquip_MPChange = MPChange; }

        protected int onEquip_MPMaxChange = 0;
        public int getOnEquip_MPMaxChange () { return this.onEquip_MPMaxChange; }
        public void setOnEquip_MPMaxChange (int MPMaxChange) { this.onEquip_MPMaxChange = MPMaxChange; }

        protected int onEquip_HP_Min = 1;
        public int getOnEquip_HP_Min () { return this.onEquip_HP_Min; }
        public void setOnEquip_HP_Min (int HP_Min) { this.onEquip_HP_Min = HP_Min; }

        protected int onEquip_HPMax_Min = 1;
        public int getOnEquip_HPMax_Min () { return this.onEquip_HPMax_Min; }
        public void setOnEquip_HPMax_Min (int HPMax_Min) { this.onEquip_HPMax_Min = HPMax_Min; }

        protected int onEquip_MP_Min = 0;
        public int getOnEquip_MP_Min () { return this.onEquip_MP_Min; }
        public void setOnEquip_MP_Min (int MP_Min) { this.onEquip_MP_Min = MP_Min; }

        protected int onEquip_MPMax_Min = 0;
        public int getOnEquip_MPMax_Min () { return this.onEquip_MPMax_Min; }
        public void setOnEquip_MPMax_Min (int MPMax_Min) { this.onEquip_MPMax_Min = MPMax_Min; }

        #endregion
        
        #region OnUnEquip attributes
        // triggered with OnUnEquip

        protected int onUnEquip_HPChange = 0;
        public int getOnUnEquip_HPChange () { return this.onUnEquip_HPChange; }
        public void setOnUnEquip_HPChange (int HPChange) { this.onUnEquip_HPChange = HPChange; }

        protected int onUnEquip_HPMaxChange = 0;
        public int getOnUnEquip_HPMaxChange () { return this.onUnEquip_HPMaxChange; }
        public void setOnUnEquip_HPMaxChange (int HPMaxChange) { this.onUnEquip_HPMaxChange = HPMaxChange; }

        protected int onUnEquip_MPChange = 0;
        public int getOnUnEquip_MPChange () { return this.onUnEquip_MPChange; }
        public void setOnUnEquip_MPChange (int MPChange) { this.onUnEquip_MPChange = MPChange; }

        protected int onUnEquip_MPMaxChange = 0;
        public int getOnUnEquip_MPMaxChange () { return this.onUnEquip_MPMaxChange; }
        public void setOnUnEquip_MPMaxChange (int MPMaxChange) { this.onUnEquip_MPMaxChange = MPMaxChange; }

        protected int onUnEquip_HP_Min = 1;
        public int getOnUnEquip_HP_Min () { return this.onUnEquip_HP_Min; }
        public void setOnUnEquip_HP_Min (int HP_Min) { this.onUnEquip_HP_Min = HP_Min; }

        protected int onUnEquip_HPMax_Min = 1;
        public int getOnUnEquip_HPMax_Min () { return this.onUnEquip_HPMax_Min; }
        public void setOnUnEquip_HPMax_Min (int HPMax_Min) { this.onUnEquip_HPMax_Min = HPMax_Min; }

        protected int onUnEquip_MP_Min = 0;
        public int getOnUnEquip_MP_Min () { return this.onUnEquip_MP_Min; }
        public void setOnUnEquip_MP_Min (int MP_Min) { this.onUnEquip_MP_Min = MP_Min; }

        protected int onUnEquip_MPMax_Min = 0;
        public int getOnUnEquip_MPMax_Min () { return this.onUnEquip_MPMax_Min; }
        public void setOnUnEquip_MPMax_Min (int MPMax_Min) { this.onUnEquip_MPMax_Min = MPMax_Min; }

        #endregion
        


        #region constructors

        // potions
        //public ItemDef (String instanceName, String name, String scemeName, int value, String visual, String effect)
        //    : base (instanceName, name, scemeName, value, visual, effect)
        //{ }

        //// weapons
        //public ItemDef (String instanceName, String name, DamageTypes dmgType, MainFlags mainFlags, Flags flags, int totalDamage, int range, int value, String visual)
        //    : base (instanceName, name, dmgType, mainFlags, flags, totalDamage, range, value, visual)
        //{ }

        //// armor
        //public ItemDef (String instanceName, String name, int[] protection, int value, String visual, String visual_Change)
        //    : base (instanceName, name, protection, value, visual, visual_Change)
        //{ }

        

        //public ItemDef (String instanceName, String name, String scemeName, int value, 
        //    MainFlags mainFlags, Flags flags, String visual)
        //    : this (instanceName, name, scemeName, null, null, value, 
        //        mainFlags, flags, 0, 0, 0, 0, visual, null)
        //{ }

        //public ItemDef (String instanceName, String name, String scemeName, int value, 
        //    MainFlags mainFlags, Flags flags, String visual, String visual_Change, String effect)
        //    : this (instanceName, name, scemeName, null, null, value, 
        //        mainFlags, flags, 0, 0, 0, 0, visual, visual_Change, effect)
        //{ }

        //public ItemDef (String instanceName, String name, int[] protection, int[] damages, 
        //    int value, MainFlags mainFlags, Flags flags, ArmorFlags armorFlags, 
        //    DamageTypes dmgType, int totalDamage, int range, String visual)
        //    : this (instanceName, name, protection, damages, 
        //        value, mainFlags, flags, armorFlags, 
        //        dmgType, totalDamage, range, visual, null)
        //{ }
        //public ItemDef (String instanceName, String name, String scemeName, int[] protection, 
        //    int[] damages, int value, MainFlags mainFlags, Flags flags, ArmorFlags armorFlags, 
        //    DamageTypes dmgType, int totalDamage, int range, String visual)
        //    : this (instanceName, name, scemeName, protection, 
        //        damages, value, mainFlags, flags, armorFlags, 
        //        dmgType, totalDamage, range, visual, null)
        //{ }
        //public ItemDef (String instanceName, String name, int[] protection, int[] damages, 
        //    int value, MainFlags mainFlags, Flags flags, ArmorFlags armorFlags, DamageTypes dmgType, 
        //    int totalDamage, int range, String visual, String visual_Change)
        //    : this (instanceName, name, null, protection, damages, 
        //        value, mainFlags, flags, armorFlags, dmgType, 
        //        totalDamage, range, visual, visual_Change, null, 0)
        //{ }

        //public ItemDef (String instanceName, String name, int[] protection, int[] damages, 
        //    int value, MainFlags mainFlags, Flags flags, ArmorFlags armorFlags, DamageTypes dmgType, 
        //    int totalDamage, int range, String visual, String visual_Change, String effect)
        //    : this (instanceName, name, null, protection, damages, 
        //        value, mainFlags, flags, armorFlags, dmgType, 
        //        totalDamage, range, visual, visual_Change, effect, 0)
        //{ }

        //public ItemDef (String instanceName, String name, String scemeName, int[] protection, int[] damages, 
        //    int value, MainFlags mainFlags, Flags flags, ArmorFlags armorFlags, DamageTypes dmgType, 
        //    int totalDamage, int range, String visual, String visual_Change)
        //    : this (instanceName, name, scemeName, protection, damages, 
        //        value, mainFlags, flags, armorFlags, dmgType, 
        //        totalDamage, range, visual, visual_Change, "", 0)
        //{ }

        //public ItemDef (String instanceName, String name, String scemeName, int[] protection, int[] damages, 
        //    int value, MainFlags mainFlags, Flags flags, ArmorFlags armorFlags, DamageTypes dmgType, 
        //    int totalDamage, int range, String visual, String visual_Change, String effect)
        //    : this (instanceName, name, scemeName, protection, damages, 
        //        value, mainFlags, flags, armorFlags, dmgType, 
        //        totalDamage, range, visual, visual_Change, effect, 0, 0, null)
        //{ }

        //public ItemDef (String instanceName, String name, String scemeName, int[] protection, int[] damages, 
        //    int value, MainFlags mainFlags, Flags flags, ArmorFlags armorFlags, DamageTypes dmgType, 
        //    int totalDamage, int range, String visual, String visual_Change, String effect, MaterialType types)
        //    : this (instanceName, name, scemeName, protection, damages, 
        //        value, mainFlags, flags, armorFlags, dmgType, 
        //        totalDamage, range, visual, visual_Change, effect, 0, types, null)
        //{ }

        //public ItemDef (String instanceName, String name, String scemeName, int[] protection, int[] damages, 
        //    int value, MainFlags mainFlags, Flags flags, ArmorFlags armorFlags, DamageTypes dmgType, 
        //    int totalDamage, int range, String visual, String visual_Change, String effect, MaterialType types, 
        //    ItemInstance munition)
        //    : this (instanceName, name, scemeName, protection, damages, 
        //        value, mainFlags, flags, armorFlags, dmgType, 
        //        totalDamage, range, visual, visual_Change, effect, 0, types, 
        //        munition)
        //{ }

        //public ItemDef (String instanceName, String name, String scemeName, int[] protection, int[] damages, 
        //    int value, MainFlags mainFlags, Flags flags, ArmorFlags armorFlags, DamageTypes dmgType, 
        //    int totalDamage, int range, String visual, String visual_Change, String effect, int visualSkin, 
        //    MaterialType types, ItemInstance munition)
        //    : this (instanceName, name, scemeName, protection, damages, 
        //        value, mainFlags, flags, armorFlags, dmgType, 
        //        totalDamage, range, visual, visual_Change, effect, visualSkin, 
        //        types, munition, false, false, false, false, false)
        //{ }

        //public ItemDef (String instanceName, String name, String scemeName, int[] protection, 
        //    int[] damages, int value, MainFlags mainFlags, Flags flags, ArmorFlags armorFlags, 
        //    DamageTypes dmgType, int totalDamage, int range, String visual, String visual_Change, 
        //    String effect, int visualSkin, MaterialType types, ItemInstance munition, bool keyInstance, 
        //    bool torch, bool torchBurning, bool torchBurned, bool gold)
        //    : base (instanceName, name, scemeName, protection, 
        //        damages, value, mainFlags, flags, armorFlags, 
        //        dmgType, totalDamage, range, visual, visual_Change, 
        //        effect, visualSkin, types, munition, keyInstance, 
        //        torch, torchBurning, torchBurned, gold)
        //{
        //    this.OnUse += new Scripting.Events.UseItemEventHandler(this.UseItem);
        //    this.OnEquip += new Scripting.Events.NPCEquipEventHandler(this.EquipItem);
        //    this.OnUnEquip += new Scripting.Events.NPCEquipEventHandler(this.UnequipItem);
        //}

        #endregion



        #region dictionary-methods

        static bool AddToDict (ItemDef obj)
        {
            int id = obj.getId();

            if (id < 1)
            {
                
            }

            return true;
        }

        #endregion



        #region methods called by EventHandlers

        //protected void EquipItem (NPCProto npc, Item item)
        //{
        //    //npc.HP      = this.getOnEquip_HPChange();
        //    //npc.HPMax   = this.getOnEquip_HPMaxChange();
        //    //npc.MP      = this.getOnEquip_MPChange();
        //    //npc.MPMax   = this.getOnEquip_MPMaxChange();
        //}

        //protected void UnequipItem (NPCProto npc, Item item)
        //{
        //    //npc.HP      = this.getOnUnEquip_HPChange();
        //    //npc.HPMax   = this.getOnUnEquip_HPMaxChange();
        //    //npc.MP      = this.getOnUnEquip_MPChange();
        //    //npc.MPMax   = this.getOnUnEquip_MPMaxChange();
        //}

        //protected void UseItem (NPCProto npc, Item item, short state, short targetState)
        //{
        //    //if (!(state == -1 && targetState == 0))
        //    //{
        //    //    return;
        //    //}

        //    //if ((npc.HP + this.getOnUse_HPChange()) >= this.getOnUse_HP_Min())
        //    //{
        //    //    // 
        //    //    //if ((npc.HP + this.getOnUse_HPChange()))
        //    //    npc.HP =+ this.getOnUse_HPChange();
        //    //}
        //    //else
        //    //{
        //    //    //if (this.getHP_Min <= this.getHPMax_min)
        //    //    npc.HP = this.getOnUse_HP_Min();
        //    //}

        //    //if ((npc.HPMax + this.getOnUse_HPMaxChange()) >= this.getOnUse_HPMax_Min())
        //    //{
        //    //    npc.HPMax =+ this.getOnUse_HPMaxChange();
        //    //}
        //    //else
        //    //{
        //    //    npc.HPMax = this.getOnUse_HPMax_Min();
        //    //}


        //    //if ((npc.MP + this.getOnUse_MPChange()) >= this.getOnUse_MP_Min())
        //    //{
        //    //    npc.MP =+ this.getOnUse_MPChange();
        //    //}

        //    //if ((npc.MPMax + this.getOnUse_MPMaxChange()) >= this.getOnUse_MPMax_Min())
        //    //{
        //    //    npc.MPMax =+ this.getOnUse_MPMaxChange();
        //    //}


        //}

        #endregion
        
    }
}
