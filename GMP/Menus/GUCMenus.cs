using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Client.GUI;
using Gothic.mClasses;
using WinApi.User.Enumeration;
using GUC.Client.Menus.MainMenus;

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
                InputHooked.deactivateFullControl(Program.Process);
            }
            activeMenus.Insert(0, menu);
        }

        public static void DeactivateMenu(GUCMenu menu)
        {
            activeMenus.Remove(menu);
            if (activeMenus.Count == 0)
            {
                InputHooked.activateFullControl(Program.Process);
            }
        }

        public static void CloseActiveMenus()
        {
            for (int i = 0; i < activeMenus.Count; i++)
                activeMenus[i].Close();

            InputHooked.activateFullControl(Program.Process);
        }

        private static GUCVisual background;
        public static GUCVisual _Background
        {
            get
            {
                if (background == null)
                {
                    background = new GUCVisual();
                    background.SetBackTexture("STARTSCREEN.TGA");
                }
                return background;
            }
        }

        //main menus
        public static MainMenu Main = new MainMenu();
        public static ExitMenu Exit = new ExitMenu();

        public static LoginMenu Login = new LoginMenu();
        public static RegisterMenu Register = new RegisterMenu();
        public static CharSelMenu CharSelection = new CharSelMenu();
        public static CharCreationMenu CharCreation = new CharCreationMenu();
        public static PlayerlistMenu Playerlist = new PlayerlistMenu();

        public static HelpMenu Help = new HelpMenu();
        public static HelpRPMenu HelpRP = new HelpRPMenu();
        public static HelpChatMenu HelpChat = new HelpChatMenu();

        //ingame menus
        public static PlayerInventory Inventory = new PlayerInventory();
        public static AnimationMenu Animation = new AnimationMenu();
        public static StatusMenu Status = new StatusMenu();
        public static TradeMenu Trade = new TradeMenu();

        public static DropItemMenu InputNumber = new DropItemMenu();
    }
}
