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

        public bool IsGold 
        {
            get { return itemInstances.isGold; } 
            set { itemInstances.isGold = value; } 
        }
        public bool IsKeyInstance 
        {
            get { return itemInstances.isKeyInstance; } 
            set { itemInstances.isKeyInstance = value; } 
        }
        public bool IsLockPick 
        { 
            get { return itemInstances.isKeyInstance; } 
            set { itemInstances.isKeyInstance = value; } 
        }
        public bool IsTorch 
        { 
            get { return itemInstances.isTorch; } 
            set { itemInstances.isTorch = value; } 
        }
        public bool IsTorchBurning { 
            get { return itemInstances.isTorchBurning; }
            set { itemInstances.isTorchBurning = value; } 
        }
        public bool IsTorchBurned { 
            get { return itemInstances.isTorchBurned; }
            set { itemInstances.isTorchBurned = value; } 
        }

        public String Effect { 
            get { return itemInstances.Effect; }
            set { itemInstances.Effect = value; } 
        }
        public Spell Spell { 
            get { return itemInstances.Spell.ScriptingProto; }
            set { itemInstances.Spell = value.spell; } 
        }

        public Enumeration.ArmorFlags Wear { 
            get { return itemInstances.Wear; }
            set { itemInstances.Wear = value; } 
        }
        public Enumeration.DamageType DamageType { 
            get { return itemInstances.DamageType; }
            set { itemInstances.DamageType = value; } 
        }
        public int Range { 
            get { return itemInstances.Range; }
            set { itemInstances.Range = value; } 
        }
        public int TotalDamage { 
            get { return itemInstances.TotalDamage; }
            set { itemInstances.TotalDamage = value; } 
        }
        // !!! TO DO: Damages !!!
        public ItemInstance Munition
        {
            get { return itemInstances.munition; }
            set { itemInstances.munition = value; }
        }

        public int HPChange;
        public int HPMaxChange;
        public int MPChange;
        public int MPMaxChange;

        protected ItemDef(String instanceName)
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

            //this.Effect;

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
