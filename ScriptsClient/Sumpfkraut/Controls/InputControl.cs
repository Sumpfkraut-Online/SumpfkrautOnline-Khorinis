using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi.User.Enumeration;
using GUC.Scripts.Sumpfkraut.Menus;

namespace GUC.Scripts.Sumpfkraut.Controls
{
    abstract class InputControl
    {
        public static InputControl Active;
        
        static InputControl()
        {
            InputHandler.OnKeyDown += sKeyDown;
            InputHandler.OnKeyUp += sKeyUp;
        }
        
        protected abstract void KeyDown(VirtualKeys key);
        protected abstract void KeyUp(VirtualKeys key);
        protected abstract void Update(long now);

        static void sKeyDown(VirtualKeys key)
        {
            if (!GUCMenu.KeyDownUpdateMenus(key) && Active != null)
                Active.KeyDown(key);
        }

        static void sKeyUp(VirtualKeys key)
        {
            if (!GUCMenu.KeyUpUpdateMenus(key) && Active != null)
                Active.KeyUp(key);
        }

        public static void UpdateControls(long now)
        {
            if (!GUCMenu.IsAMenuActive && Active != null)
                Active.Update(now);
        }
    }
}
