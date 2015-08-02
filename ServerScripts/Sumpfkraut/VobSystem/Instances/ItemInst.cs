using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Types;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripts.Sumpfkraut.VobSystem.Definitions;

namespace GUC.Server.Scripts.Sumpfkraut.VobSystem.Instances
{
    /**
     *   Class which handles item creation.
     */
    class ItemInst : VobInst
    {

        // must provide an alternative vobDef and corresponding get-set-methods
        // due to C# being unable to inherit from more than 1 baseclass
        // (ItemDef would be a childclass of VobDef, if not for the necessary inheritance from ItemInstance)
        protected ItemDef vobDef;
        public ItemDef getVobDef () { return this.vobDef; }
        public void setVobDef (ItemDef vobDef) { this.vobDef = vobDef; }

        private int amount;
        public int getAmount () { return this.amount; }
        public void setAmount (int amount) 
        {
            Item vob = (Item) this.vob;
            this.amount = amount;
            if (vob != null)
            {
                vob.setAmount(amount);
            }
        }

        private NPCInst inInventoryNPC;
        public NPCInst getInInventoryNPC () { return inInventoryNPC; }
        public void setInInventoryNPC (NPCInst inInventoryNPC) { this.inInventoryNPC = inInventoryNPC; }

        private MobInst inInventoryMob;
        public MobInst getInInventoryMob () { return inInventoryMob; }
        public void setInInventoryMob (MobInst inInventoryMob) { this.inInventoryMob = inInventoryMob; }




        // does not automatically spawn the item in a world or inventory
        public ItemInst (ItemDef def)
            : this(def, 1)
        { }

        // does not automatically spawn the item in a world or inventory
        public ItemInst (ItemDef def, int amount)
        {
            this.vobDef = def;
            this.setAmount(amount);
            this.setVob(new Item(def, amount));
        }

        // constructor which also spawns the item in an npcs inventory
        public ItemInst (ItemDef def, int amount, NPCInst inInventoryNPC)
            : this(def, amount)
        {
            this.setInInventoryNPC(inInventoryNPC);
        }

        // constructor which also spawns the item in a container-mob
        public ItemInst (ItemDef def, int amount, MobInst inInventoryMob)
            : this(def, amount)
        {
            this.setInInventoryMob(inInventoryMob);
        }

        // constructor which also places the item at a certain position into a world
        public ItemInst (ItemDef def, int amount, WorldInst inWorld, Vec3f position)
            : this(def, amount)
        {
            this.setInWorld(inWorld);
            this.setPosition(position);
        }



        public void CreateVob ()
        {
            CreateVobFromDef(getVobDef());
            setAmount(getAmount());
            
            MobInst inInventoryMob = getInInventoryMob();
            NPCInst inInventoryNPC = getInInventoryNPC();
            WorldInst inWorld = getInWorld();

            if (inInventoryMob != null)
            {
                setInInventoryMob(inInventoryMob);
            }
            else if (inInventoryNPC != null)
            {
                setInInventoryNPC(inInventoryNPC);
            }
            else if (inWorld != null)
            {
                setInWorld(inWorld);
                setPosition(getPosition());
                setDirection(getDirection());
            }
            else
            {
                Log.Logger.logError("CreateVob: Cannot create vob because ItemInst does belong to"
                    + "neither NPCInst, MobInst or WorldInst!");
            }
        }

        public void CreateVobFromDef (ItemDef def) 
        {
            if (def == null)
            {
                Log.Logger.logError("CreateVobFromDef: The ItemDef-object is invalid/null!");
                return;
            }
            this.DeleteVob();
            this.vob = new Item(this.getVobDef(), this.getAmount());
        }

        public void DeleteVob ()
        {
            Item vob = (Item) this.vob;
            if (vob != null)
            {
                vob.Delete();
                vob = null;
            } 
        }

        public void SpawnVob ()
        {
            if (this.vob != null)
            {
                this.vob.Spawn(this.getInWorld().getWorldName(), this.getPosition(), this.getDirection());
                setIsSpawned(true);
            }  
        }

        public void DespawnVob ()
        {
            if (this.vob != null)
            {
                this.vob.Despawn();
                setIsSpawned(false);
            }  
        }

        


    }
}
