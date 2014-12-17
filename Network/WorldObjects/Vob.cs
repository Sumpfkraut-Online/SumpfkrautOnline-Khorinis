using System;
using System.Collections.Generic;
using System.Text;
using GUC.Types;
using GUC.Enumeration;
using GUC.WorldObjects.Character;
using GUC.WorldObjects.Mobs;

namespace GUC.WorldObjects
{
    internal partial class Vob
    {
        [Flags]
        public enum VobSendFlags : int
        {
            //Vob
            Visual = 1 << 0,
            CDDyn = Visual << 1,
            CDStatic = CDDyn << 1,
            VobEnd = CDStatic,

            //Vob=>NPCProto:

            //Vob=>Item:
            Amount = VobEnd << 1,

            //Vob => MobInter
            FocusName = VobEnd << 1,
            State = FocusName << 1,
            UseWithItem = State << 1,
            TriggerTarget = UseWithItem << 1,
            EndMobInter = TriggerTarget,

            //Vob => MobInter => MobLockable
            IsLocked = EndMobInter << 1,
            KeyInstance = IsLocked << 1,
            PickLockStr = KeyInstance << 1,
            EndMobLockAble = PickLockStr,

            //Vob => MobInter => MobLockable => MobContainer
            MCItemList = EndMobLockAble << 1,
        }


        protected int _id = 0;
        protected int type = 0;
        protected String map = null;
        protected bool spawned = false;
        protected bool created = false;

        public bool Created { get { return created; } set { created = value; } }

        #region Collision
        protected bool _CDDyn = true;
        protected bool _CDStatic = true;

        public bool CDDyn { get { return _CDDyn; } set { _CDDyn = value; } }
        public bool CDStatic { get { return _CDStatic; } set { _CDStatic = value; } }
        #endregion

        protected String visual = "HUMANS.MDS";
        public String Visual { get { return visual; } set { visual = value.ToUpper(); } }

        public int ID { get { return this._id; } set { if (_id != 0) throw new Exception("ID can be only set if 0"); _id = value; } }

        public VobTypes VobType { get { return (VobTypes)this.type; } set { type = (int)value; } }

        public bool IsSpawned { get { return spawned; } set { spawned = value; } }

        public String Map {
            get { return this.map; }
            set
            {
                if (value == null)
                {
#if D_SERVER
                    if (this.Map != null)
                    {
                        sWorld.getWorld(this.Map).setVobPosition(this.pos, null, this);
                    }
#endif
                    this.map = null;
                    return;
                }

                

                String t = sWorld.getMapName(value);
                if (t.Length == 0)
                    throw new ArgumentException("The map is not valid!");
                if (t == this.Map)
                    return;

#if D_SERVER
                if (this.Map != null)
                {
                    sWorld.getWorld(this.Map).setVobPosition(this.pos, null, this);
                }
#endif
                map = t;

#if D_SERVER
                if (this.Map != null)
                {
                    sWorld.getWorld(this.Map).setVobPosition(null, this.pos, this);
                }
#endif
            }
        }


        
        #region Position
        protected float[] pos = null;
        protected float[] dir = new float[3] { 0, 0, 1};

        public Vec3f Position { get { return (Vec3f)this.pos; } set { this.Pos = value.Data; } }

        public float[] Pos
        {
            get { return this.pos; }
            set
            {
                

                if (value == null || value.Length < 3)
                {
                    value = new float[3];
                }

#if D_SERVER
                if (this.Map != null)
                {
                    sWorld.getWorld(this.Map).setVobPosition(this.pos, value, this);
                }
#endif
                if (this.pos == null)
                    this.pos = new float[3];

                this.pos[0] = value[0];
                this.pos[1] = value[1];
                this.pos[2] = value[2];
            }
        }

        public Vec3f Direction { get { return (Vec3f)this.dir; } 
            set { 
                this.Dir = value.Data; 
                if (this.Direction.isNull())
                    this.dir[2] = 1; 
            } 
        }
        public float[] Dir
        {
            get { return this.dir; }
            set
            {
                if (value == null || value.Length < 3)
                {
                    this.dir[0] = 0;
                    this.dir[1] = 0;
                    this.dir[2] = 1;

                    return;
                }
                this.dir[0] = value[0];
                this.dir[1] = value[1];
                this.dir[2] = value[2];

                if (this.Direction.isNull())
                    this.dir[2] = 1.0f;
            }
        }

        #endregion


        public static Vob createVob(VobTypes vt)
        {
            Vob v = null;
            if (vt == VobTypes.Npc)
                v = new NPC();
            else if (vt == VobTypes.Player)
                v = new Player();
            else if (vt == VobTypes.Item)
                v = new Item();
            else if (vt == VobTypes.Vob)
                v = new Vob();
            else if (vt == VobTypes.MobInter)
                v = new MobInter();
            else if (vt == VobTypes.MobBed)
                v = new MobBed();
            else if (vt == VobTypes.MobContainer)
                v = new MobContainer();
            else if (vt == VobTypes.MobDoor)
                v = new MobDoor();
            else if (vt == VobTypes.MobSwitch)
                v = new MobSwitch();


            if (v != null)
                v.VobType = vt;

            return v;
        }
    }
}
