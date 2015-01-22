using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RakNet;
using GUC.Network;
using GUC.Types;
using Gothic.zClasses;
using WinApi;
using Gothic.zTypes;
using GUC.WorldObjects.Character;


namespace GUC.WorldObjects
{
    internal partial class Vob
    {
        int _address = 0; /**< Same as Address. @see Address */

        public int Address { get { return _address; } set { _address = value; } } /**< Client-side address of this vob in Gothic's process. */

        /**
         * Despawns a vob. General vobs can't be despawned,
         * thus this method has to be re-implemented.
         */
        public virtual void Despawn()
        {
            spawned = false;
        }

        /**
         * Spawns a vob in the client's process.
         * The caller has to take care of enacting this change to other clients (???)
         * @param map The world to spawn in.
         * @param position A simple (X|Y|Z) position vector.
         * @param direction A simple (X|Y|Z) direction vector.
         */
        public virtual void Spawn(String map, Vec3f position, Vec3f direction)
        {
            this.Map = map;
            this.Position = position;
            this.Direction = direction;

            spawned = true;

            if (this.Address != 0)
                return;

            if (this.Map != Player.Hero.Map)
                return;

            Process process = Process.ThisProcess();

            zCVob gVob = zCVob.Create(process);
            gVob.VobType = zCVob.VobTypes.Vob;

            this.Address = gVob.Address;
            sWorld.SpawnedVobDict.Add(this.Address, this);

            setVobData(process, gVob);

            oCGame.Game(process).World.AddVob(gVob);

            
        }

        /** 
         * Applies the Vob's settings.
         * This will apply the Vob's settings from the client to
         * Gothic's process. In particular it will change visual,
         * position, direction, dynamic collision and static
         * collision.
         * @param process Gothic's process
         * @param vob The client vob
         */
        protected void setVobData(Process process, zCVob vob)
        {
            if (this.Visual != null && this.Visual.Length != 0)
                vob.SetVisual(this.Visual);


            setPosition(this.Position);
            setDirection(this.Direction);

            if (CDDyn)
                vob.BitField1 |= (int)zCVob.BitFlag0.collDetectionDynamic;
            else
                vob.BitField1 &= ~(int)zCVob.BitFlag0.collDetectionDynamic;
            if (CDStatic)
                vob.BitField1 |= (int)zCVob.BitFlag0.collDetectionStatic;
            else
                vob.BitField1 &= ~(int)zCVob.BitFlag0.collDetectionStatic;
        }

        /**
         * This will apply the client vob's position to
         * Gothic's process.
         * @param pos A simple (X|Y|Z) position vector.
         */
        public void setPosition(Vec3f pos)
        {
            this.Position = pos;

            if (this.Address == 0)
                return;

            Process process = Process.ThisProcess();
            zCVob vob = new zCVob(process, this.Address);
            vob.TrafoObjToWorld.setPosition(this.Position.Data);
            vob.SetPositionWorld(this.Position.Data);
            vob.TrafoObjToWorld.setPosition(this.Position.Data);
        }

        /**
         * This will apply the client vob's direction to
         * Gothic's process.
         * @param pos A simple (X|Y|Z) direction vector.
         */
        public void setDirection(Vec3f dir)
        {
            dir = dir.normalise();
            this.Direction = dir;

            if (this.Address == 0)
                return;

            Process process = Process.ThisProcess();
            zCVob vob = new zCVob(process, this.Address);


            Vec3f zAxis = this.Direction;
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

            Matrix4 trafo = vob.TrafoObjToWorld;

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


            zVec3 p = vob.GetPosition();
            vob.SetPositionWorld(p);
            
            p.Dispose();
        }

        /**
         * Reads important vob information from the server.
         * Which information has been sent is determined by the second int in the stream.
         * Information that may be in the stream (in this order): visual, dynamic collision,
         * static collision.
         * @param stream The information has been sent via this stream.
         */
        public virtual VobSendFlags Read(BitStream stream)
        {
            int sendInfo = 0;
            VobSendFlags sendInfoFlag = 0;
            stream.Read(out _id);
            stream.Read(out sendInfo);

            sendInfoFlag = (VobSendFlags)sendInfo;

            if (sendInfoFlag.HasFlag(VobSendFlags.Visual))
                stream.Read(out visual);
            if (sendInfoFlag.HasFlag(VobSendFlags.CDDyn))
                stream.Read(out _CDDyn);
            if (sendInfoFlag.HasFlag(VobSendFlags.CDStatic))
                stream.Read(out _CDStatic);
            return sendInfoFlag;
        }
    }
}
