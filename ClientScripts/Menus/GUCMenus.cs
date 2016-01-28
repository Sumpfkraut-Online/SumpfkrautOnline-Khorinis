using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Client.GUI;
using WinApi.User.Enumeration;
using GUC.Client;

namespace GUC.Client.Scripts.Menus
{
    abstract class GUCMenu
    {
        private static readonly List<GUCMenu> activeMenus = new List<GUCMenu>();
        public static IEnumerable<GUCMenu> GetActiveMenus() { return activeMenus; }

        public static void CloseActiveMenus()
        {
            for (int i = activeMenus.Count-1; i >= 0; i--)
                activeMenus[i].Close();
        }

        public virtual void Open()
        {
            if (activeMenus.Count == 0)
            {
                InputHandler.DeactivateGothicControl();
            }
            activeMenus.Insert(0, this);
        }
        public virtual void Close()
        {
            activeMenus.Remove(this);
            if (activeMenus.Count == 0)
            {
                InputHandler.ActivateGothicControl();
            }
        }

        public abstract void Update(long now);
        public abstract void KeyDown(VirtualKeys key, long now);
        public abstract void KeyUp(VirtualKeys key, long now);
    }
}
