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
        private enum VobSendFlags
        {
            Visual,
            Position,
            Direction,
            CDDyn,
            CDStatic,
            MAX_FLAGS
        }

        protected int _id = 0;
        protected int type = 0;
        protected bool spawned = false;

        public bool IsSpawned { get { return spawned; } set { spawned = value; } }

        public int ID { get { return this._id; } set { if (_id != 0) throw new Exception("ID can be only set if 0"); _id = value; } }

        public VobType VobType { get { return (VobType)this.type; } set { type = (int)value; } }

        #region Collision
        protected bool _CDDyn = true;
        protected bool _CDStatic = true;

        public bool CDDyn
        {
            get { return _CDDyn; }
            set
            {
                _CDDyn = value;
            }
        }
        public bool CDStatic
        {
            get { return _CDStatic; }
            set
            {
                _CDStatic = value;
            }
        }
        #endregion

        #region Visual
        protected String visual = "HUMANS.MDS";
        public String Visual
        {
            get { return visual; }
            set
            {
                visual = value;
            }
        }
        #endregion

        #region Position
        protected float[] pos = new float[3] { 0, 0, 0 };
        protected float[] dir = new float[3] { 0, 0, 1 };

        public Vec3f PosVec { get { return (Vec3f)this.pos; } set { Position = value.Data; } }
        public Vec3f DirVec { get { return (Vec3f)this.dir; } set { Direction = value.Data; } }

        public float[] Position
        {
            get { return pos; }
            set
            {
                if (value != null && value.Length == 3)
                {
                    pos = value;
                }
                else
                {
                    pos[0] = 0;
                    pos[1] = 0;
                    pos[2] = 0;
                }
            }
        }

        public float[] Direction
        {
            get { return dir; }
            set
            {
                if (value != null && value.Length == 3)
                {
                    dir = value;
                }
                else
                {
                    dir[0] = 0;
                    dir[1] = 0;
                    dir[2] = 1;
                }
            }
        }
        #endregion
    }
}
