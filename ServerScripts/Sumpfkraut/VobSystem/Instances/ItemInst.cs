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

        // definition on which basis the item was created
        private ItemDef itemDef;
        public ItemDef getItemDef () { return this.itemDef; }
        public void setItemDef (ItemDef itemDef) { this.itemDef = itemDef; }

        // the ingame-item created by using itemDef
        private Item item;
        public Item getItem () { return this.item; }
        public void setItem (Item item) { this.item = item; }


        // TO DO: must update the database-entry, too
        private int amount;
        public int getAmount () { return this.amount; }
        public void setAmount (int amount) 
        { 
            this.amount = amount;
            this.item.setAmount(amount);
        }

        // TO DO: if items are already in a world or inventory, they must be removed from this location before
        // adding it to another + split up into two ItemInst if only a partial amount is moved
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
        public void setInWorld (WorldInst inWorld) { this.inWorld = inWorld; }

        // cartesian position in current world 
        // (always set inWorld and position at the same time)
        private Vec3f position;
        public Vec3f getPosition () { return this.position; }
        public void setPosition (Vec3f position) { this.position = position; }


        // does not automatically spawn the item in a world or inventory
        public ItemInst (ItemDef def)
            : this(def, 1)
        { }

        // does not automatically spawn the item in a world or inventory
        public ItemInst (ItemDef def, int amount)
        {
            this.itemDef = def;
            this.setAmount(amount);
            this.item = new Item(def, amount);
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



        public void CreateItem ()
        {
            if (this.item != null)
            {
                this.DeleteItem();
            }
            if (this.getItemDef() != null)
            {
                this.item = new Item(this.getItemDef(), this.getAmount());
            }
        }

        public void DeleteItem ()
        {
            this.item.Delete();
        }

        public void SpawnItem ()
        {
            this.item.Spawn();
        }

        public void DespawnItem ()
        {
            this.item.Despawn();
        }

        


    }
}
