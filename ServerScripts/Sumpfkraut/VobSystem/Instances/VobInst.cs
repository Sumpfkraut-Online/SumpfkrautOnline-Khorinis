using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GUC.Server.Scripting.Objects;
using GUC.Server.Scripts.Sumpfkraut.VobSystem.Definitions;
using GUC.Types;

namespace GUC.Server.Scripts.Sumpfkraut.VobSystem.Instances
{
    class VobInst
    {

        // TO DO: set ids in constructors
        protected int id = -1;
        public int getID () { return this.id; }
        public void setID (int id) { this.id = id; }

        // definition on which basis the item was created
        protected VobDef vobDef;
        public VobDef getVobDef () { return this.vobDef; }
        public void setVobDef (VobDef vobDef) { this.vobDef = vobDef; }

        // the ingame-item created by using itemDef
        protected Vob vob;
        public Vob getVob () { return this.vob; }
        public void setVob (Vob vob) 
        { 
            this.vob = vob;
            if (getIsSpawned())
            {
                SpawnVob();
            }
        }

        // is the vob spawned or not? --> for vobs which should be ready to be spawned 
        // without them changing the ingame-world
        protected bool isSpawned = false;
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

        // current world where the instance is in
        // (always set inWorld and position at the same time)
        protected WorldInst inWorld;
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
        protected Vec3f position;
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

        protected Vec3f direction;
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

        protected DateTime changeDate;
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

        protected DateTime creationDate;
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




        public abstract void CreateVob();
        //public abstract void DeleteVob();
        public abstract void SpawnVob();
        public abstract void DespawnVob();

    }
}
