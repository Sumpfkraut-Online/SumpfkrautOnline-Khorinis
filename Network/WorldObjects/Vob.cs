using System;
using System.Collections.Generic;
using System.Text;
using GUC.Types;
using GUC.Enumeration;
using GUC.WorldObjects.Character;
using GUC.WorldObjects.Mobs;

namespace GUC.WorldObjects
{
    /**
     * A class to handle Vobs in the client.
     * The WorldOjects.Vob class is used to deal with Vobs that are in the client's process. It partly mirrors the type zCVob
     * and serves as an interface between the Gothic process and the server.
     */
    internal partial class Vob
    {
        /**
         * An enumeration for what information will be sent between client and server.
         * These flags are important either when sending or receiving information. They will decide which data will be put
         * into the stream or which data is in the stream, ready to be read. All write/read methods will make sure you only send relevant data according
         * to the vobType.
         */
        [Flags]
        public enum VobSendFlags : int
        {
            //Vob
            Visual = 1 << 0, /**< Vob's visual (string) */
            CDDyn = Visual << 1, /**< Dynamic Collision (bool) */
            CDStatic = CDDyn << 1, /**< Static Collision (bool) */
            VobEnd = CDStatic, /**< Just a flag to visualize the end of general Vob attributes */

            //Vob=>NPCProto:

            //Vob=>Item:
            Amount = VobEnd << 1, /**< The amount of an item (int) */

            //Vob => MobInter
            FocusName = VobEnd << 1, /**< Focusname of a mob (string??) */
            State = FocusName << 1, /**< Current State of a mob (S0, S1, ...) (type???) */
            UseWithItem = State << 1, /**< The item that is needed to use this mob (type???) */
            TriggerTarget = UseWithItem << 1, /**< Which target (zCMover, zCTriggerScript, ...) will be triggered by usage of this mob (string???) */
            EndMobInter = TriggerTarget, /**< Just a flag to visualize the end of general MobInter attributes */

            //Vob => MobInter => MobLockable
            IsLocked = EndMobInter << 1, /**< Whether this MobLockable (Doors, Chests, ...) is locked (bool) */
            KeyInstance = IsLocked << 1, /**< Instance of the key to unlock this MobLockable (type???) */
            PickLockStr = KeyInstance << 1, /**< String of picklock combination to unlock this (e.g. "RLLLRLLRRL") (string???) */
            EndMobLockAble = PickLockStr, /**< Just a flag to visualize the end of general MobLockable attributes */

            //Vob => MobInter => MobLockable => MobContainer
            MCItemList = EndMobLockAble << 1, /**< List of items contained in a container (e.g. chest) (updated???) (type???) */
        }


        protected int _id = 0; /**< Same as ID. @see ID*/
        protected int type = 0; /**< Same as VobType. @see VobType @see VobTypes */
        protected String map = null; /**< Internal world of this vob. */
        protected bool spawned = false; /**< Is this vob spawned. */
        protected bool created = false; /**< Same as Created. @see Created */

        public bool Created { get { return created; } set { created = value; } } /**< Has this vob been created. Vobs may be created but not yet spawned into a world. */

        #region Collision
        
        protected bool _CDDyn = true; /**< Same as CDDyn. @see CDDyn */
        protected bool _CDStatic = true; /**< Same as CDStatic. @see CDStatic */

        public bool CDDyn { get { return _CDDyn; } set { _CDDyn = value; } } /**< Dynamic Collision. Decides whether this vob will collide with other vobs. */
        public bool CDStatic { get { return _CDStatic; } set { _CDStatic = value; } } /**< Static Collision. Decides whether this vob will collide with the mesh. */
        #endregion

        protected String visual = "HUMANS.MDS"; /**< Same as Visual. @see Visual */
        public String Visual { get { return visual; } set { visual = value.ToUpper(); } } /**< Vob's visual. Usually an .ASC/.MDS/.3DS; changing this will probably require some more client-side code. */

        public int ID { get { return this._id; } set { if (_id != 0) throw new Exception("ID can be only set if 0"); _id = value; } } /**< Internal ID of this vob. It's the primary key to identify vobs and can't be set more than once. */

        public VobTypes VobType { get { return (VobTypes)this.type; } set { type = (int)value; } } /**< Internal Type of this vob. @see VobTypes */

        public bool IsSpawned { get { return spawned; } set { spawned = value; } } /**< Check whether this vob is spawned. Will return true if this vob has been spawned into the world. @see Spawn() */

        /**
         * The world this vob is in.
         * When setting this to null, the vob will be deleted from the world.
         * */
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

        public Vec3f Position { get { return (Vec3f)this.pos; } set { this.Pos = value.Data; } } /**< Same as Pos. @see Pos */

        /**
         * Regular (X|Y|Z) position vector.
         * When this is set to an invalid value, it will default to (0|0|0).
         * The position in the world will be updated automatically (D_SERVER???).
         */
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


        public Vec3f Direction { get { return (Vec3f)this.dir; } /**< Same as Dir. @see Dir */
            set { 
                this.Dir = value.Data; 
                if (this.Direction.isNull())
                    this.dir[2] = 1; 
            } 
        }
        
        /**
         * Regular (X|Y|Z) direction vector.
         * When this is set to an invalid value, it will default to (0|0|1).
         * No idea whether this will be applied automatically or at all.
         */

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

        /**
         * Create a vob of a certain type.
         * Will do nothing if vt is null.
         * @param vt Type of the vob
         * @see VobTypes
         */
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
