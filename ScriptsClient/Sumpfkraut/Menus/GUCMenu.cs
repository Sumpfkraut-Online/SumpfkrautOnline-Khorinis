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
        public static bool IsAMenuActive { get { return activeMenus.Count > 0; } }

        // for button holding
        static long holdInitTime, holdLastTime;
        static VirtualKeys lastKey;

        public static bool KeyDownUpdateMenus(VirtualKeys key)
        {
            if (activeMenus.Count > 0)
            {
                if (holdInitTime == 0)
                {
                    holdInitTime = holdLastTime = GameTime.Ticks;
                    lastKey = key;
                }

                activeMenus[0].KeyPress(key, false);
                return true;
            }
            return false;
        }

        public static bool KeyUpUpdateMenus(VirtualKeys key)
        {
            holdInitTime = holdLastTime = 0;
            return activeMenus.Count > 0;
        }

        public static void UpdateMenus(long now)
        {
            if (activeMenus.Count == 0)
                return;

            // button holding
            if (holdInitTime > 0)
            {
                if (now - holdInitTime > 600 * TimeSpan.TicksPerMillisecond
                 && now - holdLastTime > 100 * TimeSpan.TicksPerMillisecond)
                {
                    activeMenus[0].KeyPress(lastKey, true);
                    holdLastTime = now;
                }
            }

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

        protected virtual void KeyPress(VirtualKeys key, bool hold) { }
        protected virtual void Update(long now) { }
    }
}
