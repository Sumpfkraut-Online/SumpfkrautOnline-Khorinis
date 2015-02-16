using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

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

        public int HPChange;
        public int HPMaxChange;
        public int MPChange;
        public int MPMaxChange;

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

        protected void useItem(NPCProto npc, Item item, short state, short targetState)
        {
            if (!(state == -1 && targetState == 0))
            {
                return;
            }

            npc.HP      = this.HPChange;
            npc.HPMax   = this.HPMaxChange;
            npc.MP      = this.MPChange;
            npc.MPMax   = this.MPMaxChange;
        }


    }
}
