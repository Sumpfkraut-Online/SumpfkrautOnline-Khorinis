using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi.User.Enumeration;

namespace GUC.Scripts.Sumpfkraut.Menus
{
    abstract class GUCMenu
    {
        static readonly List<GUCMenu> activeMenus = new List<GUCMenu>();
        public static IEnumerable<GUCMenu> GetActiveMenus() { return activeMenus; }
        public static bool IsMenuActive { get { return activeMenus.Count > 0; } }

        public static bool KeyDownUpdateMenus(VirtualKeys key)
        {
            if (activeMenus.Count > 0)
            {
                activeMenus[0].KeyDown(key);
                return true;
            }
            return false;
        }

        public static bool KeyUpUpdateMenus(VirtualKeys key)
        {
            if (activeMenus.Count > 0)
            {
                activeMenus[0].KeyUp(key);
                return true;
            }
            return false;
        }

        public static void UpdateMenus(long now)
        {
            activeMenus.ForEach(m => m.Update(now));
        }

        public static void CloseActiveMenus()
        {
            for (int i = activeMenus.Count - 1; i >= 0; i--)
                activeMenus[i].Close();
        }

        bool opened = false;
        public bool Opened { get { return opened; } }

        public virtual void Open()
        {
            if (opened)
            {
                activeMenus.Remove(this); // to the top
            }
            activeMenus.Insert(0, this);
            opened = true;
        }

        public virtual void Close()
        {
            if (opened)
            {
                activeMenus.Remove(this);
                opened = false;
            }
        }

        protected virtual void KeyDown(VirtualKeys key) { }
        protected virtual void KeyUp(VirtualKeys key) { }
        protected virtual void Update(long now) { }
    }
}
