using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi.User.Enumeration;

namespace GUC.Client.Scripts.Sumpfkraut.Menus
{

    //TODO: public static ChatMenu Chat = ChatMenu.GetChat();
    //https://github.com/Sumpfkraut-Online/SumpfkrautOnline-Khorinis/blob/492c6700c5f260f94bc9c61655e3b28b80bbbb7e/GMP/Menus/GUCMenus.cs
    abstract class GUCMenu
    {
        private static readonly List<GUCMenu> activeMenus = new List<GUCMenu>();
        public static IEnumerable<GUCMenu> GetActiveMenus() { return activeMenus; }

        public static void UpdateMenus(long now)
        {
            if (activeMenus.Count > 0)
            {
                GUCMenu activeMenu = activeMenus[0];
                if (activeMenu != null)
                    activeMenu.Update(now);
            }
        }

        public static void CloseActiveMenus()
        {
            for (int i = activeMenus.Count-1; i >= 0; i--)
                activeMenus[i].Close();
        }

        public virtual void Open()
        {
            activeMenus.Insert(0, this);
        }

        public virtual void Close()
        {
            activeMenus.Remove(this);
        }

        public abstract void Update(long now);
        public abstract void KeyDown(VirtualKeys key, long now);
        public abstract void KeyUp(VirtualKeys key, long now);
    }
}
