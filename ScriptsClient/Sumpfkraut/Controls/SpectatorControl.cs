using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi.User.Enumeration;
using GUC.Types;

namespace GUC.Scripts.Sumpfkraut.Controls
{
    static class SpectatorControl
    {
        const float defaultSpeed = 12.5f;
        const float fastSpeed = 45.0f;

        public static void KeyDown(VirtualKeys key, long now)
        {
            if (key == VirtualKeys.Shift)
            {
                speedMultiplier = fastSpeed;
            }
        }

        public static void KeyUp(VirtualKeys key, long now)
        {
            if (key == VirtualKeys.Shift)
            {
                speedMultiplier = defaultSpeed;
            }
        }

        static float speedMultiplier = defaultSpeed;

        public static void Update(long now)
        {
            if (InputHandler.MouseDistY != 0)
            {
                Gothic.oCGame.GetCameraVob().RotateLocalX(InputHandler.MouseDistY * 0.2f);
            }
            if (InputHandler.MouseDistX != 0)
            {
                Gothic.oCGame.GetCameraVob().RotateWorldY(InputHandler.MouseDistX * 0.2f);
            }

            if (InputHandler.IsPressed(VirtualKeys.Up) || InputHandler.IsPressed(VirtualKeys.W))
            {
                var cam = Gothic.oCGame.GetCameraVob();
                var dir = new Vec3f(cam.Direction) * speedMultiplier;
                cam.MoveWorld(dir.X, dir.Y, dir.Z);
            }
            else if (InputHandler.IsPressed(VirtualKeys.Down) || InputHandler.IsPressed(VirtualKeys.S))
            {
                var cam = Gothic.oCGame.GetCameraVob();
                var dir = new Vec3f(cam.Direction) * -speedMultiplier;
                cam.MoveWorld(dir.X, dir.Y, dir.Z);
            }

            if (InputHandler.IsPressed(VirtualKeys.A))
            {
                var cam = Gothic.oCGame.GetCameraVob();
                var dir = new Vec3f(cam.Direction).Cross(new Vec3f(0, 1, 0));
                dir *= speedMultiplier;
                cam.MoveWorld(dir.X, dir.Y, dir.Z);
            }
            else if (InputHandler.IsPressed(VirtualKeys.D))
            {
                var cam = Gothic.oCGame.GetCameraVob();
                var dir = new Vec3f(cam.Direction).Cross(new Vec3f(0, 1, 0));
                dir *= -speedMultiplier;
                cam.MoveWorld(dir.X, dir.Y, dir.Z);
            }

            if (InputHandler.IsPressed(VirtualKeys.Space))
            {
                var cam = Gothic.oCGame.GetCameraVob();
                var dir = new Vec3f(0, 1, 0);
                dir *= speedMultiplier;
                cam.MoveWorld(dir.X, dir.Y, dir.Z);
            }
            else if (InputHandler.IsPressed(VirtualKeys.Control))
            {
                var cam = Gothic.oCGame.GetCameraVob();
                var dir = new Vec3f(0, 1, 0);
                dir *= -speedMultiplier;
                cam.MoveWorld(dir.X, dir.Y, dir.Z);
            }
        }
    }
}
