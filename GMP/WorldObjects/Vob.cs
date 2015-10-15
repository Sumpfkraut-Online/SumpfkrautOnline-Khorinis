using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.zClasses;
using GUC.Enumeration;
using GUC.Types;
using Gothic.zTypes;
using RakNet;
using GUC.Network;

namespace GUC.Client.WorldObjects
{
    class Vob
    {
        public uint ID { get; private set; }

        public zCVob gVob { get; protected set; }

        public Vob(uint id)
        {
            ID = id;
            gVob = null;
            spawned = false;
        }

        public Vob(uint id, zCVob vob)
        {
            ID = id;
            gVob = vob;
            spawned = false;
        }

        #region Position
        public float[] posf { get { return position.Data; } set { position = (Vec3f)value; } }
        public float[] dirf { get { return direction.Data; } set { direction = (Vec3f)value; } }

        protected Vec3f iPos = new Vec3f();
        protected Vec3f iDir = new Vec3f();

        public Vec3f position
        {
            get
            {
                if (spawned)
                {
                    return (Vec3f)gVob.TrafoObjToWorld.getPosition();
                }
                else
                {
                    return iPos;
                }
            }
            set
            {
                iPos = value == null ? new Vec3f(0, 0, 0) : value;

                if (spawned)
                {
                    gVob.TrafoObjToWorld.setPosition(iPos.Data);
                    gVob.SetPositionWorld(iPos.Data);
                    gVob.TrafoObjToWorld.setPosition(iPos.Data);
                }
            }
        }

        public Vec3f direction
        {
            get
            {
                if (spawned)
                {
                    return (Vec3f)gVob.TrafoObjToWorld.getDirection();
                }
                else
                {
                    return iDir;
                }
            }
            set
            {
                iDir = (value == null || value.isNull()) ? new Vec3f(0, 0, 1) : value;

                if (spawned)
                {
                    Vec3f zAxis = iDir.normalise();
                    Vec3f up = new Vec3f(0.0f, 0.0f, 0.0f);

                    if (Math.Abs(zAxis.Y) > 0.5)
                    {
                        if (zAxis.Y > 0)
                            up.Z = -1.0f;
                        else
                            up.Z = 1.0f;
                    }
                    else if (Math.Abs(zAxis.X) < 0.0001 && Math.Abs(zAxis.Y) < 0.0001)
                    {
                        if (zAxis.Y > -0.0001)
                        {
                            up.Y = 1.0f;
                        }
                        else
                        {
                            up.Y = -1.0f;
                        }
                    }
                    else
                    {
                        up.Y = 1.0f;
                    }

                    Vec3f xAxis = up.cross(zAxis).normalise();
                    Vec3f yAxis = zAxis.cross(xAxis).normalise();

                    Matrix4 trafo = gVob.TrafoObjToWorld;

                    trafo.set(12, 0);
                    trafo.set(13, 0);
                    trafo.set(14, 0);
                    trafo.set(15, 1);

                    trafo.set(0, xAxis.X);
                    trafo.set(4, xAxis.Y);
                    trafo.set(8, xAxis.Z);

                    trafo.set(1, yAxis.X);
                    trafo.set(5, yAxis.Y);
                    trafo.set(9, yAxis.Z);

                    trafo.set(2, zAxis.X);
                    trafo.set(6, zAxis.Y);
                    trafo.set(10, zAxis.Z);
                }
            }
        }
        #endregion

        #region Visual
        protected string iVisual = "ITFO_APPLE.3DS";
        public string visual
        {
            get
            {
                return iVisual;
            }
            set
            {
                iVisual = value;
                if (spawned)
                {
                    gVob.SetVisual(value);
                }
            }
        }
        #endregion

        #region Collision
        protected bool icdDyn = false;

        public bool cdDyn
        {
            get
            {
                return icdDyn;
            }
            set
            {
                icdDyn = value;
                if (spawned)
                {
                    if (value)
                        gVob.BitField1 |= (int)zCVob.BitFlag0.collDetectionDynamic;
                    else
                        gVob.BitField1 &= ~(int)zCVob.BitFlag0.collDetectionDynamic;
                }
            }
        }

        protected bool icdStatic = false;
        public bool cdStatic
        {
            get { return icdStatic; }
            set
            {
                if (spawned)
                {
                    if (value)
                        gVob.BitField1 |= (int)zCVob.BitFlag0.collDetectionStatic;
                    else
                        gVob.BitField1 &= ~(int)zCVob.BitFlag0.collDetectionStatic;
                }
            }
        }
        #endregion

        #region Spawn
        public bool spawned { get; protected set; }

        public void Spawn()
        {
            Spawn(iPos, iDir, false);
        }

        public void Spawn(Vec3f position, Vec3f direction)
        {
            Spawn(position, direction, false);
        }

        public void Spawn(Vec3f position, Vec3f direction, bool drop)
        {
            CreateVob(gVob == null);

            oCGame.Game(Program.Process).World.AddVob(gVob);

            if (drop)
            {
                Player.Hero.gNpc.DoDropVob(gVob);
            }

            this.position = position;
            this.direction = direction;

            if (!spawned)
                World.AddVob(this);

            spawned = true;
        }

        protected virtual void CreateVob(bool createNew)
        {
            if (createNew)
            {
                gVob = zCVob.Create(Program.Process);
            }
            gVob.BitField1 |= (int)zCVob.BitFlag0.staticVob;
            visual = iVisual;
            cdDyn = icdDyn;
            cdStatic = icdStatic;
        }

        public void Despawn()
        {
            if (!spawned)
                return;

            oCGame.Game(Program.Process).World.RemoveVob(gVob);
            World.RemoveVob(this);
            gVob = null;
            spawned = false;
        }

        public virtual void Update(long now)
        {
        }
        #endregion
    }
}
