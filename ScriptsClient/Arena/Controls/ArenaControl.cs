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
            if (down && zCCamera.ActiveCamera.Address != 0)
            {
                var screen = GUI.GUCView.GetScreenSize();
                if (screen.X != 0 && screen.Y != 0)
                {
                    const float FOV = 90.0f;
                    if (g1)
                    {
                        if (Gothic.System.zCParser.GetCameraParser().LoadDat("CAMERA.DAT"))
                        {
                            zCAICamera.CurrentCam.CreateInstance(zCAICamera.CurrentCam.CurrentMode);
                            zCCamera.ActiveCamera.SetFOV(FOV, (float)(FOV * 0.75d));
                            g1 = false;
                        }
                    }
                    else
                    {
                        if (Gothic.System.zCParser.GetCameraParser().LoadDat("CAMERA.DAT.G1"))
                        {
                            zCAICamera.CurrentCam.CreateInstance(zCAICamera.CurrentCam.CurrentMode);
                            double ratio = (double)screen.Y / screen.X;
                            zCCamera.ActiveCamera.SetFOV(FOV, (float)(FOV * ratio));
                            g1 = true;
                        }
                    }
                }
            }
        }
    }
}
