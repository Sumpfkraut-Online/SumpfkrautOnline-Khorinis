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

        public VobType VobType
        {
            get
            {
                return (VobType)gVob.VobType;
            }
            set
            {
                gVob.VobType = (zCObject.VobTypes)value;
            }
        }

        public zCVob gVob { get; protected set; }

        public Vob(uint id) : this(id, zCVob.Create(Program.Process))
        {
        }

        public Vob(uint id, zCVob vob)
        {
            ID = id;
            gVob = vob;
            Spawned = false;
        }

        #region Position
        public float[] Posf { get { return Position.Data; } set { Position = (Vec3f)value; } }
        public float[] Dirf { get { return Direction.Data; } set { Direction = (Vec3f)value; } }

        public virtual Vec3f Position
        {
            get { return (Vec3f)gVob.TrafoObjToWorld.getPosition(); }
            set
            {
                Vec3f pos;
                if (value != null)
                {
                    pos = value;
                }
                else
                {
                    pos = new Vec3f(0,0,0);
                }

                gVob.TrafoObjToWorld.setPosition(pos.Data);
                gVob.SetPositionWorld(pos.Data);
                gVob.TrafoObjToWorld.setPosition(pos.Data);
            }
        }

        public virtual Vec3f Direction
        {
            get { return (Vec3f)gVob.TrafoObjToWorld.getDirection(); }
            set
            {
                Vec3f dir;
                if (value != null && !value.isNull())
                {
                    dir = value;
                }
                else
                {
                    dir = new Vec3f(0, 0, 1);
                }

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
        #endregion

        #region Visual
        protected string visual = "HUMANS.MDS";
        public string Visual
        {
            get
            {
                return visual;
            }
            set
            {
                visual = value;
                gVob.SetVisual(visual);
            }
        }
        #endregion

        #region Collision
        public bool CDDyn
        {
            get { return (gVob.BitField1 & (int)zCVob.BitFlag0.collDetectionDynamic) != 0; }
            set
            {
                if (value)
                    gVob.BitField1 |= (int)zCVob.BitFlag0.collDetectionDynamic;
                else
                    gVob.BitField1 &= ~(int)zCVob.BitFlag0.collDetectionDynamic;
            }
        }
        public bool CDStatic
        {
            get { return (gVob.BitField1 & (int)zCVob.BitFlag0.collDetectionStatic) != 0; }
            set
            {
                if (value)
                    gVob.BitField1 |= (int)zCVob.BitFlag0.collDetectionStatic;
                else
                    gVob.BitField1 &= ~(int)zCVob.BitFlag0.collDetectionStatic;
            }
        }
        #endregion
        
        #region Spawn
        public bool Spawned { get; protected set; }

        public void Spawn()
        {
            Spawn(Position, Direction, false);
        }

        public void Spawn(Vec3f position, Vec3f direction)
        {
            Spawn(position, direction, false);
        }

        public void Spawn(Vec3f position, Vec3f direction, bool drop)
        {
            if (Spawned)
                return;

            if (this is NPC)
            {
                ((NPC)this).gNpc.InitHumanAI();
                ((NPC)this).gNpc.HP = int.MaxValue;
                ((NPC)this).gNpc.HPMax = int.MaxValue;
            }

            oCGame.Game(Program.Process).World.AddVob(gVob);
            if (drop)
                Player.Hero.gNpc.DoDropVob(gVob);

            Position = position;
            Direction = direction;

            World.AddVob(this);
            Spawned = true;
        }

        public void Despawn()
        {
            if (!Spawned)
                return;

            oCGame.Game(Program.Process).World.RemoveVob(gVob);
            World.RemoveVob(this);
            Spawned = false;
        }

        public virtual void Update(long now)
        {
        }
        #endregion
    }
}
