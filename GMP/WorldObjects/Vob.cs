using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.zClasses;
using GUC.Enumeration;
using GUC.Types;
using GUC.Client.WorldObjects.Instances;

namespace GUC.Client.WorldObjects
{
    public class Vob : Collections.IVobObj<uint>
    {
        public uint ID { get; private set; }
        public VobInstance Instance { get; protected set; }

        public zCVob gVob { get; protected set; }

        public VobType VobType { get { return Instance.VobType; } }

        public string Visual { get { return Instance.Visual; } }

        public bool CDDyn { get { return Instance.CDDyn; } }
        public bool CDStatic { get { return Instance.CDStatic; } }

        public Vob(uint id, ushort instanceid) : this(id, instanceid, null)
        {
        }

        public Vob(uint id, ushort instanceid, zCVob vob)
        {
            this.ID = id;
            this.Instance = VobInstance.Instances.Get(instanceid);
            this.gVob = vob;
            this.Spawned = false;
        }

        #region Position
        protected Vec3f pos = new Vec3f(0, 0, 0);
        protected Vec3f dir = new Vec3f(0, 0, 1);

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
                pos = value;

                if (Spawned)
                {
                    float[] arr = pos.ToArray();
                    gVob.TrafoObjToWorld.setPosition(arr);
                    gVob.SetPositionWorld(arr);
                    gVob.TrafoObjToWorld.setPosition(arr);
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
                dir = value.IsNull() ? new Vec3f(0, 0, 1) : value;

                if (Spawned)
                {
                    Vec3f zAxis = dir.Normalise();
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

                    Vec3f xAxis = up.Cross(zAxis).Normalise();
                    Vec3f yAxis = zAxis.Cross(xAxis).Normalise();

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

        public virtual void Spawn(Vec3f position, Vec3f direction, bool drop)
        {
            if (gVob == null)
            {
                CreateVob();
            }

            oCGame.Game(Program.Process).World.AddVob(gVob);

            if (drop)
            {
                Player.Hero.gVob.DoDropVob(gVob);
            }

            if (!Spawned)
            {
                World.Vobs.Add(this);
            }

            Spawned = true;

            this.Position = position;
            this.Direction = direction;

            SetProperties();
        }

        public void Despawn()
        {
            if (!Spawned)
                return;

            if (Player.VobControlledList.Contains(this))
            {
                Player.VobControlledList.Remove(this);
            }

            oCGame.Game(Program.Process).World.RemoveVob(gVob);
            World.Vobs.Remove(this);
            gVob = null;
            Spawned = false;
        }
        #endregion

        protected virtual void CreateVob()
        {
            this.gVob = zCVob.Create(Program.Process);
        }

        protected virtual void SetProperties()
        {
            gVob.BitField1 |= (int)zCVob.BitFlag0.staticVob;
            gVob.SetVisual(Visual);

            if (CDDyn) gVob.BitField1 |= (int)zCVob.BitFlag0.collDetectionDynamic;
            else gVob.BitField1 &= ~(int)zCVob.BitFlag0.collDetectionDynamic;

            if (CDStatic) gVob.BitField1 |= (int)zCVob.BitFlag0.collDetectionStatic;
            else gVob.BitField1 &= ~(int)zCVob.BitFlag0.collDetectionStatic;
        }

        public virtual void Update(long now)
        {
        }
    }
}
