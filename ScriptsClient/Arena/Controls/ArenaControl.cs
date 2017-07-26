using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripts.Sumpfkraut.Controls;
using WinApi.User.Enumeration;
using GUC.Scripts.Sumpfkraut.Networking;

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
    }
}
