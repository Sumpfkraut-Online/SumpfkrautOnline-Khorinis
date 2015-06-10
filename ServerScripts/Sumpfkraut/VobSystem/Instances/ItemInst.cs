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

        // TO DO: if items are already in a world or inventory, they must be removed from this location before
        // adding it to another + split up into two ItemInst if only a partial amount is moved
        private NPCInst inInventoryNPC;
        public NPCInst getInInventoryNPC () { return inInventoryNPC; }
        public void setInInventoryNPC (NPCInst inInventoryNPC) { this.inInventoryNPC = inInventoryNPC; }

        private WorldInst inWorld;
        public WorldInst getInWorld () { return inWorld; }
        public void setInWorld (WorldInst inWorld) { this.inWorld = inWorld; }


        // does not automatically spawn the item in a world or inventory
        public ItemInst (ItemDef def)
            : this(def, 1)
        { }

        // does not automatically spawn the item in a world or inventory
        public ItemInst (ItemDef def, int amount)
        {
            this.itemDef = def;
            this.item = new Item(def, amount);
        }

        // constructor which also spawns the item in an npcs inventory
        public ItemInst (ItemDef def, int amount, NPCInst inInventoryNPC)
            : this(def, amount)
        {
            // TO DO
        }

        // constructor which also spawns the item in a world
        public ItemInst (ItemDef def, int amount, WorldInst inWorld, Vec3f position)
            : this(def, amount)
        {
            // TO DO
        }

    }
}
