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
        const float defaultSpeed = 1.0f;
        const float fastSpeed = 3.0f;
        static float speedMultiplier = defaultSpeed;

        KeyDictionary spectatorControls = new KeyDictionary()
        {
            { VirtualKeys.Shift, down => speedMultiplier = !down ? defaultSpeed : fastSpeed },
            { KeyBind.OpenAllChat, d => { ChatMenu.Menu.OpenAllChat(); } },
            { KeyBind.OpenTeamChat, d => { ChatMenu.Menu.OpenTeamChat(); } },
            { KeyBind.Inventory, ToggleScoreBoard }, 
        };

        long lastUpdate = 0;
        void SpectatorUpdate()
        {
            if (InputHandler.MouseDistY != 0)
            {
                float angle = InputHandler.MouseDistY * 0.2f;
                Gothic.oCGame.GetCameraVob().RotateLocalX(angle > 20 ? 20 : angle);
            }
            if (InputHandler.MouseDistX != 0)
            {
                float angle = InputHandler.MouseDistX * 0.2f;
                Gothic.oCGame.GetCameraVob().RotateWorldY(angle > 20 ? 20 : angle);
            }

            long diff = GameTime.Ticks - lastUpdate;
            lastUpdate = GameTime.Ticks;
            if (diff > TimeSpan.TicksPerMillisecond * 50)
                diff = TimeSpan.TicksPerMillisecond * 50;

            float speed = (float)diff / 18000.0f * speedMultiplier;
            if (InputHandler.IsPressed(VirtualKeys.Up) || InputHandler.IsPressed(VirtualKeys.W))
            {
                var cam = Gothic.oCGame.GetCameraVob();
                var dir = new Vec3f(cam.Direction) * speed;
                cam.MoveWorld(dir.X, dir.Y, dir.Z);
            }
            else if (InputHandler.IsPressed(VirtualKeys.Down) || InputHandler.IsPressed(VirtualKeys.S))
            {
                var cam = Gothic.oCGame.GetCameraVob();
                var dir = new Vec3f(cam.Direction) * -speed;
                cam.MoveWorld(dir.X, dir.Y, dir.Z);
            }

            if (InputHandler.IsPressed(VirtualKeys.A))
            {
                var cam = Gothic.oCGame.GetCameraVob();
                var dir = new Vec3f(cam.Direction).Cross(new Vec3f(0, 1, 0));
                dir *= speed;
                cam.MoveWorld(dir.X, dir.Y, dir.Z);
            }
            else if (InputHandler.IsPressed(VirtualKeys.D))
            {
                var cam = Gothic.oCGame.GetCameraVob();
                var dir = new Vec3f(cam.Direction).Cross(new Vec3f(0, 1, 0));
                dir *= -speed;
                cam.MoveWorld(dir.X, dir.Y, dir.Z);
            }

            if (InputHandler.IsPressed(VirtualKeys.Space))
            {
                var cam = Gothic.oCGame.GetCameraVob();
                var dir = new Vec3f(0, 1, 0);
                dir *= speed;
                cam.MoveWorld(dir.X, dir.Y, dir.Z);
            }
            else if (InputHandler.IsPressed(VirtualKeys.Control))
            {
                var cam = Gothic.oCGame.GetCameraVob();
                var dir = new Vec3f(0, 1, 0);
                dir *= -speed;
                cam.MoveWorld(dir.X, dir.Y, dir.Z);
            }
        }
    }
}
