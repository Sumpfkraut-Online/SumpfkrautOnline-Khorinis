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
    public abstract class AbstractVob
    {
        public uint ID { get; private set; }

        public zCVob gVob { get; protected set; }

        public AbstractVob(uint id)
        {
            ID = id;
            gVob = null;
            Spawned = false;
        }

        public AbstractVob(uint id, zCVob vob)
        {
            ID = id;
            gVob = vob;
            Spawned = false;
        }

        #region Position
        public float[] Posf { get { return Position.Data; } set { Position = (Vec3f)value; } }
        public float[] Dirf { get { return Direction.Data; } set { Direction = (Vec3f)value; } }

        protected Vec3f pos = new Vec3f();
        protected Vec3f dir = new Vec3f();

        public Vec3f Position
        {
            get
            {
                if (Spawned)
                {
                    return (Vec3f)gVob.TrafoObjToWorld.getPosition();
                }
                else
                {
                    return pos;
                }
            }
            set
            {
                pos = value == null ? new Vec3f(0, 0, 0) : value;

                if (Spawned)
                {
                    gVob.TrafoObjToWorld.setPosition(pos.Data);
                    gVob.SetPositionWorld(pos.Data);
                    gVob.TrafoObjToWorld.setPosition(pos.Data);
                }
            }
        }

        public Vec3f Direction
        {
            get
            {
                if (Spawned)
                {
                    return (Vec3f)gVob.TrafoObjToWorld.getDirection();
                }
                else
                {
                    return dir;
                }
            }
            set
            {
                dir = (value == null || value.isNull()) ? new Vec3f(0, 0, 1) : value;

                if (Spawned)
                {
                    Vec3f zAxis = dir.normalise();
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

        #region Spawn
        public bool Spawned { get; protected set; }

        public void Spawn()
        {
            Spawn(pos, dir, false);
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

            if (!Spawned)
                World.AddVob(this);

            Spawned = true;

            this.Position = position;
            this.Direction = direction;
        }

        protected abstract void CreateVob(bool createNew);

        public void Despawn()
        {
            if (!Spawned)
                return;

            oCGame.Game(Program.Process).World.RemoveVob(gVob);
            World.RemoveVob(this);
            gVob = null;
            Spawned = false;
        }

        public virtual void Update(long now)
        {
        }
        #endregion
    }
}
