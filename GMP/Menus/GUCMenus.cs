using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Client.GUI;
using WinApi.User.Enumeration;

namespace GUC.Client.Menus
{
    abstract class GUCMenu
    {
        public virtual void Open()
        {
            GUCMenus.ActivateMenu(this);
        }
        public virtual void Close()
        {
            GUCMenus.DeactivateMenu(this);
        }
        
        public abstract void KeyPressed(VirtualKeys key);
        public virtual void Update(long now)
        {
        }
    }

    static class GUCMenus
    {
        private static List<GUCMenu> activeMenus = new List<GUCMenu>();
        public static List<GUCMenu> _ActiveMenus { get { return activeMenus; } }

        public static void ActivateMenu(GUCMenu menu)
        {
            if (activeMenus.Count == 0)
            {
                InputHandler.DeactivateGothicControl();
            }
            activeMenus.Insert(0, menu);
        }

        public static void DeactivateMenu(GUCMenu menu)
        {
            activeMenus.Remove(menu);
            if (activeMenus.Count == 0)
            {
                InputHandler.ActivateGothicControl();
            }
        }

        public static void CloseActiveMenus()
        {
            for (int i = activeMenus.Count-1; i >= 0; i--)
            {
                activeMenus[i].Close();
            }

            InputHandler.ActivateGothicControl();
        }
    }
}
