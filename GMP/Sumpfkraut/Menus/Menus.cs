using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Sumpfkraut.GUI;

namespace GUC.Sumpfkraut.Menus
{
    partial class MainMenus
    {
        public abstract class AbstractMenu
        {
            protected GUCMainMenu thisMenu = null;

            public virtual void Open()
            {
                Open(null, null);
            }

            public virtual void Open(object sender, EventArgs e)
            {
                if (ActiveMenu != null)
                    ActiveMenu.Close();
                if (thisMenu == null)
                {
                    CreateMenu();
                }
                thisMenu.Show();
                ActiveMenu = this;
            }

            public virtual void Close()
            {
                Close(null, null);
            }

            public virtual void Close(object sender, EventArgs e)
            {
                if (this != ActiveMenu)
                    return;
                if (thisMenu != null)
                    thisMenu.Hide();
                ActiveMenu = null;
            }

            protected abstract void CreateMenu();
        }

        public static AbstractMenu ActiveMenu = null;
        public static AbstractMenu Main = new MainMenu();
        public static AbstractMenu Exit = new ExitMenu();
        public static AbstractMenu Login = new LoginMenu();
        public static AbstractMenu Register = new RegisterMenu();
        public static AbstractMenu PlayerList = new PlayerListMenu();
        public static AbstractMenu RPGuide = new RPGuideMenu();
        public static AbstractMenu CharList = new CharListMenu();
        public static AbstractMenu CharCreation = new CharCreationMenu();
        public static AbstractMenu Help = new HelpMenu();

        public static GUCMenuTexture _Background = null;
    }
}
