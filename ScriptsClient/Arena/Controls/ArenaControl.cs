using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripts.Sumpfkraut.Controls;
using WinApi.User.Enumeration;
using GUC.Scripts.Sumpfkraut.Networking;
using Gothic.Objects;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;

namespace GUC.Scripts.Arena.Controls
{
    partial class ArenaControl : InputControl
    {
        protected override void KeyDown(VirtualKeys key)
        {
            if (key == VirtualKeys.Escape)
            {
                Menus.MainMenu.Menu.Open();
                return;
            }

            if (ScriptClient.Client.IsCharacter)
                playerControls.TryCall(key, true);
            else if (ScriptClient.Client.IsSpecating)
                spectatorControls.TryCall(key, true);
        }

        protected override void KeyUp(VirtualKeys key)
        {
            if (ScriptClient.Client.IsCharacter)
                playerControls.TryCall(key, false);
            else if (ScriptClient.Client.IsSpecating)
                spectatorControls.TryCall(key, false);
        }

        protected override void Update(long now)
        {
            if (ScriptClient.Client.IsCharacter)
                PlayerUpdate();
            if (ScriptClient.Client.IsSpecating)
                SpectatorUpdate();

            UpdateFOV(now);
        }

        static bool showScreenInfo = true;
        static void ToggleScreenInfo(bool down)
        {
            if (!down) return;

            showScreenInfo = !showScreenInfo;
            Network.GameClient.ShowInfo = showScreenInfo;
            Menus.TOInfoScreen.Shown = showScreenInfo;
        }


        static bool g1 = false;
        static void ToggleG1Camera(bool down)
        {
            zCAICamera cam;
            if (!down || (cam = zCAICamera.CurrentCam).IsNull)
                return;

            if (Gothic.System.zCParser.GetCameraParser().LoadDat(g1 ? "CAMERA.DAT" : "CAMERA.DAT.G1"))
            {
                cam.CreateInstance(cam.CurrentMode); // update camera
                g1 = !g1;
                SetFOV(currentFOV); // update fov
            }
        }

        static float startFOV = 90.0f;
        static long startFOVTime = 0;
        static float currentFOV = 90.0f;
        static float endFOV = 90.0f;
        static long endFOVTime = 0;

        static void FOVTransition(float fov, long intermission = 0)
        {
            startFOV = currentFOV;
            endFOV = Alg.Clamp(10, fov, 160);

            startFOVTime = GameTime.Ticks;
            endFOVTime = startFOVTime + intermission;
        }

        static void UpdateFOV(long now)
        {
            if (currentFOV == endFOV)
                return;

            long elapsed = now - startFOVTime;
            long duration = endFOVTime - startFOVTime;

            float fov;
            if (elapsed >= duration || duration <= 0)
            {
                fov = endFOV;
            }
            else
            {
                fov = startFOV + (endFOV - startFOV) * elapsed / duration;
            }

            SetFOV(fov);
        }

        static void SetFOV(float fov)
        {
            var cam = zCCamera.ActiveCamera;
            if (cam.IsNull)
                return;

            float ratio = 0.75f;
            if (g1)
            {
                var screen = GUI.GUCView.GetScreenSize();
                if (screen.X != 0 && screen.Y != 0)
                    ratio = (float)screen.Y / screen.X;
            }

            cam.SetFOV(fov, fov * ratio);
            currentFOV = fov;
        }
    }
}
