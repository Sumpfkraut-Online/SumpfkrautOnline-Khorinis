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

        //public bool IsGold
        //{
        //    get { return this.IsGold; }
        //    set { this.IsGold = value; }
        //}
        //public bool IsKeyInstance
        //{
        //    get { return this.isKeyInstance; }
        //    set { this.isKeyInstance = value; }
        //}
        //public bool IsLockPick
        //{
        //    get { return this.isKeyInstance; }
        //    set { this.isKeyInstance = value; }
        //}
        //public bool IsTorch
        //{
        //    get { return this.isTorch; }
        //    set { this.isTorch = value; }
        //}
        //public bool IsTorchBurning
        //{
        //    get { return this.isTorchBurning; }
        //    set { this.isTorchBurning = value; }
        //}
        //public bool IsTorchBurned
        //{
        //    get { return this.isTorchBurned; }
        //    set { this.isTorchBurned = value; }
        //}

        //public String Effect
        //{
        //    get { return this.Effect; }
        //    set { this.Effect = value; }
        //}
        //public Spell Spell
        //{
        //    get { return this.Spell.ScriptingProto; }
        //    set { this.Spell = value.spell; }
        //}

        //public Enumeration.ArmorFlags Wear
        //{
        //    get { return this.Wear; }
        //    set { this.Wear = value; }
        //}
        //public Enumeration.DamageType DamageType
        //{
        //    get { return this.DamageType; }
        //    set { this.DamageType = value; }
        //}
        //public int Range
        //{
        //    get { return this.Range; }
        //    set { this.Range = value; }
        //}
        //public int TotalDamage
        //{
        //    get { return this.TotalDamage; }
        //    set { this.TotalDamage = value; }
        //}
        //// !!! TO DO: Damages !!!
        //public ItemInstance Munition
        //{
        //    get { return this.munition; }
        //    set { this.munition = value; }
        //}

        protected int HPChange = 0;
        protected int HPMaxChange = 0;
        protected int MPChange = 0;
        protected int MPMaxChange = 0;

        public bool getIsGold ()
        {
            return this.IsGold;
        }
        public void setIsGold (bool IsGold)
        {
            this.IsGold = IsGold;
        }

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

        public bool getIsTorch ()
        {
            return this.IsTorch;
        }
        public void setIsTorch (bool IsTorch)
        {
            this.IsTorch = IsTorch;
        }

        public bool getIsTorchBurning ()
        {
            return this.IsTorchBurning;
        }
        public void setIsTorchBurning (bool IsTorchBurning)
        {
            this.IsTorchBurning = IsTorchBurning;
        }

        public bool getIsTorchBurned ()
        {
            return this.IsTorchBurned;
        }
        public void setIsTorchBurned (bool IsTorchBurned)
        {
            this.IsTorchBurned = IsTorchBurned;
        }



        public string getEffect ()
        {
            return this.Effect;
        }
        public void setEffect (string Effect)
        {
            this.Effect = Effect;
        }



        public Spell getSpell ()
        {
            return this.Spell;
        }
        public void setSpell (Spell Spell)
        {
            this.Spell = Spell;
        }

        public Enumeration.ArmorFlags getWear ()
        {
            return this.Wear;
        }
        public void setWear (Enumeration.ArmorFlags Wear)
        {
            this.Wear = Wear;
        }

        //public Enumeration.DamageType DamageType
        //{
        //    get { return this.DamageType; }
        //    set { this.DamageType = value; }
        //}
        public Enumeration.DamageType getDamageType ()
        {
            return this.DamageType;
        }
        public void setDamageType (Enumeration.DamageType DamageType)
        {
            this.DamageType = DamageType;
        }
        
        public int getRange()
        {
            return this.Range;
        }
        public void setRange(int Range)
        {
            this.Range = Range;
        }
        
        public int getTotalDamage ()
        {
            return this.TotalDamage;
        }
        public void setTotalDamage (int TotalDamage)
        {
            this.TotalDamage = TotalDamage;
        }



        public int getHPChange ()
        {
            return this.HPChange;
        }
        public void setHPChange (int HPChange)
        {
            this.HPChange = HPChange;
        }

        public int getHPMaxChange ()
        {
            return this.HPMaxChange;
        }
        public void setHPMaxChange (int HPMaxChange)
        {
            this.HPMaxChange = HPMaxChange;
        }

        public int getMPChange ()
        {
            return this.MPChange;
        }
        public void setMPChange (int MPChange)
        {
            this.MPChange = MPChange;
        }

        public int getMPMaxChange ()
        {
            return this.MPMaxChange;
        }
        public void setMPMaxChange (int MPMaxChange)
        {
            this.MPMaxChange = MPMaxChange;
        }

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




        protected ItemDef (String instanceName)
            : base (instanceName)
        {
            //this.InstanceName;

            //this.Name;
            //this.Value;
            //this.Visual;
            //this.Visual_Change;
            //this.Visual_skin;
            //this.MainFlags;
            //this.Flags;

            //this.Materials;
            //this.Description;

            //this.Effect = "";

            //this.IsGold;
            //this.IsLockPick;
            //this.IsTorch;
            //this.IsTorchBurned;
            //this.IsTorchBurning;

            //this.Range;
            //this.Spell;

            //this.Wear;
            //this.ScemeName;

            this.OnUse += new Scripting.Events.UseItemEventHandler(this.useItem);
            //this.OnEquip += new Scripting.Events.UseItemEventHandler(useItem);
            //this.OnUnEquip += new Scripting.Events.UseItemEventHandler(useItem);
            CreateItemInstance();
        }

        // potions
        public ItemDef (String instanceName, String name, String scemeName, int value, String visual, String effect)
            : base (instanceName, name, scemeName, value, visual, effect)
        { }

        // weapons
        public ItemDef (String instanceName, String name, DamageType dmgType, MainFlags mainFlags, Flags flags, int totalDamage, int range, int value, String visual)
            : base (instanceName, name, dmgType, mainFlags, flags, totalDamage, range, value, visual)
        { }

        // armor
        public ItemDef (String instanceName, String name, int[] protection, int value, String visual, String visual_Change)
            : base (instanceName, name, protection, value, visual, visual_Change)
        { }

        

        public ItemDef (String instanceName, String name, String scemeName, int value, MainFlags mainFlags, Flags flags, String visual)
            : base (instanceName, name, scemeName, null, null, value, mainFlags, flags, 0, 0, 0, 0, visual, null)
        { }

        public ItemDef (String instanceName, String name, String scemeName, int value, MainFlags mainFlags, Flags flags, String visual, String visual_Change, String effect)
            : base (instanceName, name, scemeName, null, null, value, mainFlags, flags, 0, 0, 0, 0, visual, visual_Change, effect)
        { }

        public ItemDef (String instanceName, String name, int[] protection, int[] damages, int value, MainFlags mainFlags, Flags flags, ArmorFlags armorFlags, DamageType dmgType, int totalDamage, int range, String visual)
            : base (instanceName, name, protection, damages, value, mainFlags, flags, armorFlags, dmgType, totalDamage, range, visual, null)
        { }
        public ItemDef (String instanceName, String name, String scemeName, int[] protection, int[] damages, int value, MainFlags mainFlags, Flags flags, ArmorFlags armorFlags, DamageType dmgType, int totalDamage, int range, String visual)
            : base (instanceName, name, scemeName, protection, damages, value, mainFlags, flags, armorFlags, dmgType, totalDamage, range, visual, null)
        { }
        public ItemDef (String instanceName, String name, int[] protection, int[] damages, int value, MainFlags mainFlags, Flags flags, ArmorFlags armorFlags, DamageType dmgType, int totalDamage, int range, String visual, String visual_Change)
            : base (instanceName, name, null, protection, damages, value, mainFlags, flags, armorFlags, dmgType, totalDamage, range, visual, visual_Change, null, 0)
        { }

        public ItemDef (String instanceName, String name, int[] protection, int[] damages, int value, MainFlags mainFlags, Flags flags, ArmorFlags armorFlags, DamageType dmgType, int totalDamage, int range, String visual, String visual_Change, String effect)
            : base (instanceName, name, null, protection, damages, value, mainFlags, flags, armorFlags, dmgType, totalDamage, range, visual, visual_Change, effect, 0)
        { }

        public ItemDef (String instanceName, String name, String scemeName, int[] protection, int[] damages, int value, MainFlags mainFlags, Flags flags, ArmorFlags armorFlags, DamageType dmgType, int totalDamage, int range, String visual, String visual_Change)
            : base (instanceName, name, scemeName, protection, damages, value, mainFlags, flags, armorFlags, dmgType, totalDamage, range, visual, visual_Change, "", 0)
        { }

        public ItemDef (String instanceName, String name, String scemeName, int[] protection, int[] damages, int value, MainFlags mainFlags, Flags flags, ArmorFlags armorFlags, DamageType dmgType, int totalDamage, int range, String visual, String visual_Change, String effect)
            : base (instanceName, name, scemeName, protection, damages, value, mainFlags, flags, armorFlags, dmgType, totalDamage, range, visual, visual_Change, effect, 0, 0, null)
        { }

        public ItemDef (String instanceName, String name, String scemeName, int[] protection, int[] damages, int value, MainFlags mainFlags, Flags flags, ArmorFlags armorFlags, DamageType dmgType, int totalDamage, int range, String visual, String visual_Change, String effect, MaterialTypes types)
            : base (instanceName, name, scemeName, protection, damages, value, mainFlags, flags, armorFlags, dmgType, totalDamage, range, visual, visual_Change, effect, 0, types, null)
        { }

        public ItemDef (String instanceName, String name, String scemeName, int[] protection, int[] damages, int value, MainFlags mainFlags, Flags flags, ArmorFlags armorFlags, DamageType dmgType, int totalDamage, int range, String visual, String visual_Change, String effect, MaterialTypes types, ItemInstance munition)
            : base (instanceName, name, scemeName, protection, damages, value, mainFlags, flags, armorFlags, dmgType, totalDamage, range, visual, visual_Change, effect, 0, types, munition)
        { }

        public ItemDef (String instanceName, String name, String scemeName, int[] protection, int[] damages, int value, MainFlags mainFlags, Flags flags, ArmorFlags armorFlags, DamageType dmgType, int totalDamage, int range, String visual, String visual_Change, String effect, int visualSkin, MaterialTypes types, ItemInstance munition)
            : base (instanceName, name, scemeName, protection, damages, value, mainFlags, flags, armorFlags, dmgType, totalDamage, range, visual, visual_Change, effect, visualSkin, types, munition, false, false, false, false, false)
        { }
        public ItemDef (String instanceName, String name, String scemeName, int[] protection, int[] damages, int value, MainFlags mainFlags, Flags flags, ArmorFlags armorFlags, DamageType dmgType, int totalDamage, int range, String visual, String visual_Change, String effect, int visualSkin, MaterialTypes types, ItemInstance munition, bool keyInstance, bool torch, bool torchBurning, bool torchBurned, bool gold)
            : base (instanceName)
        { }





        protected void equip (NPCProto npc, Item item)
        {

        }

        protected void unequip (NPCProto npc, Item item)
        {

        }

        protected void useItem (NPCProto npc, Item item, short state, short targetState)
        {
            if (!(state == -1 && targetState == 0))
            {
                return;
            }

            //npc.HP      = this.HPChange;
            //npc.HPMax   = this.HPMaxChange;
            //npc.MP      = this.MPChange;
            //npc.MPMax   = this.MPMaxChange;
        }


    }
}
