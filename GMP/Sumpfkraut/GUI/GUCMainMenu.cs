using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.mClasses;
using Gothic.zClasses;
using Gothic.zTypes;
using WinApi;
using WinApi.User.Enumeration;
using GUC.Types;
using GUC.Sumpfkraut;

namespace GUC.Sumpfkraut.GUI
{
    class GUCMainMenu : GUCInputReceiver, GUCMVisual
    {
        GUCMenuTexture backTex;
        GUCMenuText helpText;

        List<GUCMainMenuItem> menuItems;
        int cursor;
        bool shown;

        List<GUCMenuText> texts;
        public GUCMainMenuCharacter Character;

        public EventHandler OnEscape;

        int[] pos;

        public GUCMainMenu(int background)
        {
            menuItems = new List<GUCMainMenuItem>();
            texts = new List<GUCMenuText>();
            Character = null;

            cursor = 0;
            shown = false;

            int[] screenSize = InputHooked.GetScreenSize(Process.ThisProcess());
            pos = new int[] { (screenSize[0] - 640) / 2, (screenSize[1] - 480) / 2 };

            if (background == 1)
            {
                backTex = new GUCMenuTexture("Menu_CharacterCreation.tga", pos[0], pos[1], 640, 480);
            }
            else if (background == 2)
            {
                backTex = new GUCMenuTexture("Menu_SaveLoad_Back.tga", pos[0], pos[1], 640, 480);
            }
            else
            {
                backTex = new GUCMenuTexture("Menu_Ingame.tga", pos[0], pos[1], 640, 480);
            }

            helpText = new GUCMenuText("", pos[0], pos[1] + 455, false);
        }

        public void Show()
        {
            if (!shown)
            {
                UpdateHelpText();
                backTex.Show();
                helpText.Show();
                foreach (GUCMainMenuItem m in menuItems)
                    m.Show();

                foreach (GUCMenuText t in texts)
                    t.Show();

                if (Character != null) Character.Show();
                UpdateCharacter();

                menuItems[cursor].Enable();
                InputHandler.activateFullControl(this);
                shown = true;
            }
        }

        public void Hide()
        {
            if (shown)
            {
                menuItems[cursor].Disable();
                cursor = 0;
                backTex.Hide();
                helpText.Hide();
                foreach (GUCMainMenuItem m in menuItems)
                    m.Hide();

                foreach (GUCMenuText t in texts)
                    t.Hide();

                if (Character != null) Character.Hide();

                InputHandler.deactivateFullControl(this);
                shown = false;
            }
        }

        public void AddText(string text, int y)
        {
            AddText(text, 0, y);
            texts[texts.Count - 1].CenterText();
        }

        public void AddText(string text, int x, int y)
        {
            GUCMenuText t = new GUCMenuText(text, pos[0] + x, pos[1] + y, true);
            if (shown) t.Show();
            texts.Add(t);
        }

        public GUCMainMenuButton AddMenuButton(string text, string helpText, EventHandler func, int y)
        {
            GUCMainMenuButton b = AddMenuButton(text, helpText, func, 0, y);
            menuItems[menuItems.Count - 1].Center();
            return b;
        }

        public GUCMainMenuButton AddMenuButton(string text, string helpText, EventHandler func, int x, int y)
        {
            GUCMainMenuButton b = new GUCMainMenuButton(text, helpText, pos[0] + x, pos[1] + y, true);
            b.func = func;
            if (shown) b.Show();
            menuItems.Add(b);
            return b;
        }

        public GUCMainMenuTextBox AddMenuTextBox(string text, string helpText, EventHandler func, int y, int width)
        {
            GUCMainMenuTextBox tb = AddMenuTextBox(text, helpText, func, 0, y, width);
            menuItems[menuItems.Count - 1].Center();
            return tb;
        }

        public GUCMainMenuTextBox AddMenuTextBox(string text, string helpText, EventHandler func, int x, int y, int width)
        {
            GUCMainMenuTextBox tb = new GUCMainMenuTextBox(text, helpText, pos[0] + x, pos[1] + y, width);
            tb.func = func;
            if (shown) tb.Show();
            menuItems.Add(tb);
            return tb;
        }

        public GUCMainMenuChoice AddMenuChoice(string title, string helpText, EventHandler func, int x, int y, Dictionary<int, string> choices, EventHandler OnChange)
        {
            GUCMainMenuChoice c = new GUCMainMenuChoice(title, helpText, pos[0] + x, pos[1] + y, choices);
            c.func = func;
            c.OnChange += OnChange;
            if (shown) c.Show();
            menuItems.Add(c);
            return c;
        }

        public void SetCursor(int index)
        {
            menuItems[cursor].Disable();
            cursor = index;
            menuItems[cursor].Enable();
            UpdateHelpText();
            UpdateCharacter();
        }

        private void UpdateHelpText()
        {
            helpText.SetColor(ColorRGBA.White);
            helpText.Text = menuItems[cursor].GetHelpText();
            helpText.CenterText();
        }

        public void SetHelpText(string text)
        {
            helpText.SetColor(ColorRGBA.Red);
            helpText.Text = text;
            helpText.CenterText();
        }

        private void MoveCursor(bool up)
        {
            menuItems[cursor].Disable();
            if (up)
            {
                cursor--;
                if (cursor < 0)
                {
                    cursor = menuItems.Count - 1;
                }
            }
            else
            {
                cursor++;
                if (cursor >= menuItems.Count)
                {
                    cursor = 0;
                }
            }
            menuItems[cursor].Enable();

            if (menuItems[cursor] is GUCMainMenuButton && ((GUCMainMenuButton)menuItems[cursor]).ignore)
            {
                MoveCursor(up);
            }

            UpdateCharacter();

            InputHandler.PlaySound("INV_CHANGE.WAV");
        }

        private void UpdateCharacter()
        {
            if (Character != null && cursor >= charStartIndex && cursor < charEndIndex)
            {
                if (((GUCMainMenuButton)menuItems[cursor]).charInfo != null)
                {
                    Character.BodyMesh = ((GUCMainMenuButton)menuItems[cursor]).charInfo.BodyMesh;
                    Character.BodyTex = ((GUCMainMenuButton)menuItems[cursor]).charInfo.BodyTex;
                    Character.HeadMesh = ((GUCMainMenuButton)menuItems[cursor]).charInfo.HeadMesh;
                    Character.HeadTex = ((GUCMainMenuButton)menuItems[cursor]).charInfo.HeadTex;
                    Character.Fatness = ((GUCMainMenuButton)menuItems[cursor]).charInfo.Fatness;
                    Character.BodyHeight = ((GUCMainMenuButton)menuItems[cursor]).charInfo.BodyHeight;
                    Character.BodyWidth = ((GUCMainMenuButton)menuItems[cursor]).charInfo.BodyWidth;
                    Character.Show();
                }
                else
                {
                    Character.Hide();
                }
            }
        }

        public void KeyPressed(int key)
        {
            if (key == (int)VirtualKeys.Return)
            {
                InputHandler.PlaySound("INV_OPEN.WAV");
                menuItems[cursor].Func();
                return;
            }
            else if (key == (int)VirtualKeys.Up)
            {
                MoveCursor(true);
            }
            else if (key == (int)VirtualKeys.Down || key == (int)VirtualKeys.Tab)
            {
                MoveCursor(false);
            }
            else if (key == (int)VirtualKeys.Escape)
            {
                InputHandler.PlaySound("INV_CLOSE.WAV");
                if (OnEscape != null)
                {
                    OnEscape(null, null);
                    return;
                }
            }
            else if (key == (int)VirtualKeys.Right)
            {
                if (menuItems[cursor] is GUCMainMenuChoice)
                {
                    ((GUCMainMenuChoice)menuItems[cursor]).ChangeChoice(true);
                }
                else if (menuItems[cursor] == Character)
                {
                    Character.Rotate(true);
                }
            }
            else if (key == (int)VirtualKeys.Left)
            {
                if (menuItems[cursor] is GUCMainMenuChoice)
                {
                    ((GUCMainMenuChoice)menuItems[cursor]).ChangeChoice(false);
                }
                else if (menuItems[cursor] == Character)
                {
                    Character.Rotate(false);
                }
            }
            else
            {
                if (menuItems[cursor] is GUCMainMenuTextBox)
                {
                    ((GUCMainMenuTextBox)menuItems[cursor]).TextBox.KeyPressed(key);
                }
            }
            UpdateHelpText();
        }

        public void Update(long ticks)
        {
            if (menuItems[cursor] is GUCMainMenuTextBox)
            {
                ((GUCMainMenuTextBox)menuItems[cursor]).TextBox.Update(ticks);
            }
        }

        public void AddCharacter(int x, int y, int w, int h)
        {
            AddCharacter(x, y, w, h, null, false);
        }

        public void AddCharacter(int x, int y, int w, int h, EventHandler func, bool selectable)
        {
            Character = new GUCMainMenuCharacter(pos[0] + x, pos[1] + y, w, h);
            if (selectable)
            {
                Character.func = func;
                menuItems.Add(Character);
            }
        }

        private int charStartIndex = -1;
        private int charEndIndex = -1;
        private EventHandler OnCreateNewChar;
        private EventHandler OnSelectChar;
        public void AddCharList(int num, int x, int y, EventHandler CreateNewCharacter, EventHandler SelectCharacter)
        {
            charStartIndex = menuItems.Count;
            for (int i = 0; i < num; i++)
            {
                GUCMainMenuButton b = new GUCMainMenuButton("---", string.Format("Slot {0} - Drücke ENTER um den Charakter auszuwählen.", i + 1), pos[0] + x, pos[1] + y + 18*i, false);
                if (shown) b.Show();
                menuItems.Add(b);
            }
            charEndIndex = menuItems.Count;

            OnCreateNewChar = CreateNewCharacter;
            OnSelectChar = SelectCharacter;
        }

        public void FillCharList(List<Login.LoginMessage.CharInfo> chars)
        {
            for (int i = charStartIndex;  i < charEndIndex; i++)
            {
                    ((GUCMainMenuButton)menuItems[i]).charInfo = null;
                    ((GUCMainMenuButton)menuItems[i]).SetText("---");
                    ((GUCMainMenuButton)menuItems[i]).func = OnCreateNewChar;
            }

            foreach(Login.LoginMessage.CharInfo ci in chars)
            {
                ((GUCMainMenuButton)menuItems[charStartIndex + ci.SlotNum]).charInfo = ci;
                ((GUCMainMenuButton)menuItems[charStartIndex + ci.SlotNum]).SetText(ci.Name);
                ((GUCMainMenuButton)menuItems[charStartIndex + ci.SlotNum]).func = OnSelectChar;
            }
            UpdateCharacter();
        }

        public int GetCharIndex()
        {
            return cursor - charStartIndex;
        }
    }
}
