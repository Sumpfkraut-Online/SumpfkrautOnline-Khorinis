using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi.User.Enumeration;
using GUC.Types;
using GUC.Scripts.Sumpfkraut.Controls;

namespace GUC.Scripts.Arena.Controls
{
    partial class ArenaControl
    {
        const float defaultSpeed = 12.5f;
        const float fastSpeed = 45.0f;
        static float speedMultiplier = defaultSpeed;

        KeyDictionary spectatorControls = new KeyDictionary()
        {
            { VirtualKeys.Shift, down => speedMultiplier = !down ? defaultSpeed : fastSpeed },
        };

        void SpectatorUpdate()
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
