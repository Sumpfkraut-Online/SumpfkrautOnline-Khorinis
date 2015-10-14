using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.Client.Menus.MainMenus;

namespace GUC.Client.GUI.MainMenu
{
    class MainMenuCharSlot : MainMenuItem
    {
        GUCVisual vis;
        GUCVisualText visText { get { return vis.Texts[0]; } }
 
        AccCharInfo info;
        int slotNum; //extra in case of info == null
        public int SlotNum { get { return slotNum; } }

        Action CharSelect;
        Action EmptySelect;

        MainMenuCharacter Character;

        public MainMenuCharSlot(int num, int x, int y, MainMenuCharacter character, Action OnCharSelect, Action OnEmptySelect)
        {
            slotNum = num;
            HelpText = string.Format("Slot {0} - Drücke ENTER um den Charakter auszuwählen.", num + 1);
            this.Character = character;
            this.CharSelect = OnCharSelect;
            this.EmptySelect = OnEmptySelect;

            vis = GUCVisualText.Create("---", x, y);
        }

        public void SetInfo(AccCharInfo newInfo)
        {
            OnActivate = newInfo == null ? EmptySelect : CharSelect;
            visText.Text = newInfo == null ? "---" : newInfo.Name;
            info = newInfo;
        }

        public override void Show()
        {
            vis.Show();
        }

        public override void Hide()
        {
            vis.Hide();
        }

        public override void Select()
        {
            vis.Font = Fonts.Default_Hi;
            Character.Info = info;
        }

        public override void Deselect()
        {
            vis.Font = Fonts.Default;
            Character.Info = null;
        }
    }
}
