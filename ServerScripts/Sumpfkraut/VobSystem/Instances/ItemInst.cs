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
    class ItemInst
    {

        // TO DO: set ids in constructors
        private int id = -1;
        public int getID () { return this.id; }
        public void setID (int id) { this.id = id; }

        // is the vob spawned or not? --> for vobs which should be ready to be spawned 
        // without them changing the ingame-world
        private bool isSpawned = false;
        public bool getIsSpawned () { return this.isSpawned; }
        public void setIsSpawned (bool isSpawned) 
        {
            if ((isSpawned) && (!this.isSpawned))
            {
                this.SpawnVob();
            }
            else if ((!isSpawned) && (this.isSpawned))
            {
                this.DespawnVob();
            }
            this.isSpawned = isSpawned; 
        }

        // definition on which basis the item was created
        private ItemDef vobDef;
        public ItemDef getVobDef () { return this.vobDef; }
        public void setVobDef (ItemDef vobDef) { this.vobDef = vobDef; }

        // the ingame-item created by using itemDef
        private Item vob;
        public Item getVob () { return this.vob; }
        public void setVob (Item vob) { this.vob = vob; }

        private int amount;
        public int getAmount () { return this.amount; }
        public void setAmount (int amount) 
        { 
            this.amount = amount;
            if (this.vob != null)
            {
                this.vob.setAmount(amount);
            }
        }

        private DateTime changeDate;
        public DateTime getChangeDate () { return this.changeDate; }
        public void setChangeDate (DateTime changeDate) { this.changeDate = changeDate; }
        public void setChangeDate (string changeDate)
        {
            DateTime dt;
            if (Sumpfkraut.Utilities.DateTimeUtil.TryStringToDateTime(changeDate, out dt))
            {
                this.changeDate = dt;
            }
        }

        private DateTime creationDate;
        public DateTime getCreationDate () { return this.creationDate; }
        public void setCreationDate (DateTime creationDate) { this.creationDate = creationDate; }
        public void setCreationDate (string creationDate)
        {
            DateTime dt;
            if (Sumpfkraut.Utilities.DateTimeUtil.TryStringToDateTime(creationDate, out dt))
            {
                this.creationDate = dt;
            }
        }

        // TO DO: if items are already in a world or inventory, they must be removed from this location before
        // adding it to another + split up into two ItemInst if only a partial amount is moved
        // --> maybe doing this in handler-methods would be better than in this container-class
        private NPCInst inInventoryNPC;
        public NPCInst getInInventoryNPC () { return inInventoryNPC; }
        public void setInInventoryNPC (NPCInst inInventoryNPC) { this.inInventoryNPC = inInventoryNPC; }

        private MobInst inInventoryMob;
        public MobInst getInInventoryMob () { return inInventoryMob; }
        public void setInInventoryMob (MobInst inInventoryMob) { this.inInventoryMob = inInventoryMob; }

        // current world where the instance is in
        // (always set inWorld and position at the same time)
        private WorldInst inWorld;
        public WorldInst getInWorld () { return inWorld; }
        public void setInWorld (WorldInst inWorld) 
        { 
            this.inWorld = inWorld;
            // despawn and spawn again to swtich worlds ingame
            this.DespawnVob();
            this.SpawnVob();
        }

        // cartesian position in current world 
        // (always set inWorld and position at the same time)
        private Vec3f position;
        public Vec3f getPosition () { return this.position; }
        public void setPosition (Vec3f position) 
        { 
            this.position = position;
            if (this.vob != null)
            {
                Vob vob = this.vob;
                vob.setPosition(position);
            }          
        }

        private Vec3f direction;
        public Vec3f getDirection () { return this.direction; }
        public void setDirection (Vec3f direction) 
        {
            this.direction = direction;
            if (this.vob != null)
            {
                Vob vob = this.vob;
                vob.setDirection(direction);
            }   
        }



        // does not automatically spawn the item in a world or inventory
        public ItemInst (ItemDef def)
            : this(def, 1)
        { }

        // does not automatically spawn the item in a world or inventory
        public ItemInst (ItemDef def, int amount)
        {
            this.vobDef = def;
            this.setAmount(amount);
            this.vob = new Item(def, amount);
        }

        // constructor which also spawns the item in an npcs inventory
        public ItemInst (ItemDef def, int amount, NPCInst inInventoryNPC)
            : this(def, amount)
        {
            // TO DO
        }

        // constructor which also spawns the item in a container-mob
        public ItemInst (ItemDef def, int amount, MobInst inContainer)
            : this(def, amount)
        {
            ItemInst newItemInst = new ItemInst(def, amount);
            newItemInst.setInInventoryMob(inContainer);
        }

        // constructor which also places the item at a certain position into a world
        public ItemInst (ItemDef def, int amount, WorldInst inWorld, Vec3f position)
            : this(def, amount)
        {
            ItemInst newItemInst = new ItemInst(def, amount);
            newItemInst.setInWorld(inWorld);
            newItemInst.setPosition(position);
        }



        public void CreateVob ()
        {
            if (this.vob != null)
            {
                this.DeleteVob();
            }
            if (this.getVobDef() != null)
            {
                this.vob = new Item(this.getVobDef(), this.getAmount());
            }
        }

        public void DeleteVob ()
        {
            if (this.vob != null)
            {
                this.vob.Delete();
            } 
        }

        public void SpawnVob ()
        {
            if (this.vob != null)
            {
                this.vob.Spawn(this.getInWorld().getWorldName(), this.getPosition(), this.getDirection());
            }  
        }

        public void DespawnVob ()
        {
            if (this.vob != null)
            {
                this.vob.Despawn();
            }  
        }

        


    }
}
