using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Mob;
using GUC.Server.Scripts.Sumpfkraut.VobSystem.Definitions;
using GUC.Server.Scripts.Sumpfkraut.WorldSystem;
using GUC.Types;

namespace GUC.Server.Scripts.Sumpfkraut.VobSystem.Instances
{

    /**
     *   Class which handles mob creation.
     */
    class MobInst : VobInst
    {

        // definition on which basis the item was created
        private MobDef vobDef;
        public MobDef getVobDef () { return this.vobDef; }
        public void setVobDef (MobDef vobDef) { this.vobDef = vobDef; }

        // the ingame-item created by using itemDef
        private MobInter vob;
        public MobInter getVob () { return this.vob; }
        public void setVob (MobInter vob) { this.vob = vob; }
       
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

        private List<ItemInst> inventoryItems = new List<ItemInst>();
        public List<ItemInst> getInventoryItems () { return this.inventoryItems; }
        public void setInventoryItems (List<ItemInst> inventoryItems) 
        { 
            this.inventoryItems = inventoryItems; 
        }


    
        public MobInst ()
            : this(null)
        { }

        public MobInst (MobDef def)
            : this(def, null)
        { }

        public MobInst (MobDef def, WorldInst inWorld)
            : this(def, inWorld, new Vec3f(0, 0, 0))
        { }

        public MobInst (MobDef def, WorldInst inWorld, Vec3f position)
            : this(def, inWorld, position, new Vec3f(0, 0, 0))
        { }

        public MobInst (MobDef def, WorldInst inWorld, Vec3f position, Vec3f direction)
        {
            if (def != null)
            {
                this.setVobDef(def);
            }

            if (inWorld != null)
            {
                this.setInWorld(inWorld);
            }
            
            this.setPosition(position);
            this.setDirection(direction);

            CreateVob();
        }


        public void CreateVob ()
        {
            CreateVobFromDef(this.getVobDef());
            setInWorld(getInWorld());
            setPosition(getPosition());
            setDirection(getDirection());
        }

        private void CreateVobFromDef (MobDef def)
        {
            if (def == null)
            {
                Log.Logger.logError("CreateVobFromDef: The MobDef-object is invalid/null!");
                return;
            }

            DeleteVob();

            MobInter newVob = null;
            MobInterType mobType = def.GetMobInterType();

            // need to despawn newly created vobs because, maybe, they shouldn not be spawned at
            // this point, however, the GUC does not allow so at the moment
            switch (mobType)
            {
                case MobInterType.MobBed:
                    newVob = new MobBed(def.GetVisual(), def.GetFocusName(), def.GetUseWithItem(), 
                        def.GetTriggerTarget(), def.GetCDDyn(), def.GetCDStatic());
                    newVob.Despawn();
                    break;
                case MobInterType.MobContainer:
                    newVob = new MobContainer(def.GetVisual(), def.GetFocusName(), new ItemInstance[0], 
                        new int[0], def.GetIsLocked(), def.GetKeyInstance(), def.GetPicklockString(), 
                        def.GetUseWithItem(), def.GetTriggerTarget(), def.GetCDDyn(), def.GetCDStatic());
                    newVob.Despawn();
                    break;
                case MobInterType.MobDoor:
                    newVob = new MobDoor(def.GetVisual(), def.GetFocusName(), def.GetIsLocked(), 
                        def.GetKeyInstance(), def.GetPicklockString(), def.GetUseWithItem(), 
                        def.GetTriggerTarget(), def.GetCDDyn(), def.GetCDStatic());
                    newVob.Despawn();
                    break;
                case MobInterType.MobInter:
                    newVob = new MobInter(def.GetVisual(), def.GetFocusName(), def.GetUseWithItem(), 
                        def.GetTriggerTarget(), def.GetCDDyn(), def.GetCDStatic());
                    newVob.Despawn();
                    break;
                case MobInterType.MobSwitch:
                    newVob = new MobSwitch(def.GetVisual(), def.GetFocusName(), def.GetUseWithItem(), 
                        def.GetTriggerTarget(), def.GetCDDyn(), def.GetCDStatic());
                    newVob.Despawn();
                    break;
                case MobInterType.None:
                    break;
                default:
                    Log.Logger.logWarning("MobInst (constr): No valid MobInterType was provided on "
                        + "Mob-instantiation.");
                    break;
            }


            if (newVob != null)
            {
                this.setVob(newVob);
            }
        }

        public void DeleteVob ()
        {
            MobInter vob = this.vob;
            vob = null;
        }

        public void SpawnVob ()
        {
            if (this.vob != null)
            {
                this.vob.Spawn(this.getInWorld().getName(), this.getPosition(), this.getDirection());
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
