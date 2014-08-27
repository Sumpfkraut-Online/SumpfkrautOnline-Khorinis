using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;
using Gothic.zClasses;
using GUC.Types;
using Gothic.zTypes;

namespace GUC.Network.Messages.CameraCommands
{
    public class CamToPlayerFront : IMessage
    {
        public static zCVob playerVob = null;
        public void Read(RakNet.BitStream stream, RakNet.Packet packet, Client client)
        {
            destroyPlayerVob();

            Process process = Process.ThisProcess();


            playerVob = zCVob.Create(process);
            playerVob.VobType = zCVob.VobTypes.Vob;
            

            Vec3f pos = (Vec3f)oCNpc.Player(process).GetPositionArray();
            pos.Y -= oCNpc.Player(process).BBox3D.Height/2.0f;
            setPosition(pos);
            setDirection((Vec3f)oCNpc.Player(process).TrafoObjToWorld.getDirection() * -1.0f);

            oCGame.Game(process).World.AddVob(playerVob);

            
            oCGame.Game(process).AICamera.SetTarget(playerVob);
        }

        public static void destroyPlayerVob()
        {
            if (playerVob == null)
                return;
            


            oCGame.Game(Process.ThisProcess()).World.RemoveVob(playerVob);

            playerVob = null;
        }


        public static void setPosition(Vec3f pos)
        {
            if (playerVob == null)
                return;

            Process process = Process.ThisProcess();
            zCVob vob = playerVob;
            vob.TrafoObjToWorld.setPosition(pos.Data);
            vob.SetPositionWorld(pos.Data);
            vob.TrafoObjToWorld.setPosition(pos.Data);
        }

        public static void setDirection(Vec3f dir)
        {
            if (playerVob == null)
                return;

            dir = dir.normalise();

            Process process = Process.ThisProcess();
            zCVob vob = playerVob;


            Vec3f zAxis = dir;
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
    }
}
