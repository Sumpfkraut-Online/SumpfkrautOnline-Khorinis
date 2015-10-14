using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Client.GUI;
using GUC.Client.GUI.MainMenu;
using Gothic.mClasses;
using WinApi.User.Enumeration;
using GUC.Types;
using GUC.Network;
using Gothic.zClasses;
using Gothic.zTypes;

namespace GUC.Client.Menus.MainMenus
{
    abstract class GUCMainMenu : GUCMenu
    {
        /* Classic main menu
         * 
         * Please always add at least one menu item
         * 
         */

        protected GUCVisual Back;
        protected GUCVisual helpVis;
        protected GUCVisualText helpText { get { return helpVis.Texts[0]; } }
        protected List<MainMenuItem> items = new List<MainMenuItem>();
        protected int cursor = 0;
        protected int[] pos;
        protected int preferredCursorItem = 0;

        protected Action OnEscape = null;

        #region Init
        public GUCMainMenu()
        {
            int[] screenSize = GUCView.GetScreenSize();
            pos = new int[] { (screenSize[0] - 640) / 2, (screenSize[1] - 480) / 2 };
            Back = new GUCVisual(pos[0], pos[1], 640, 480);
            Back.SetBackTexture("Menu_Ingame.tga");
            Back.Font = GUCVisual.Fonts.Menu;

            helpVis = GUCVisualText.Create("", 0, pos[1] + 455);
            helpText.CenteredX = true;
        }

        private bool init = false;
        protected abstract void OnCreate();
        #endregion

        #region Open & Close

        public override void Open()
        {
            GUCMenus.CloseActiveMenus(); //main menus never overlap

            if (!init)
            {   //create items on first opening, otherwise pointers to 'Open()-Methods' of other menus, which are yet not constructed, will be used => crash
                //could also be solved with static Open()-Methods
                OnCreate();
                init = true;
            }

            base.Open();
            Back.Show();
            helpVis.Show();
            for (int i = 0; i < items.Count; i++)
                items[i].Show();

            cursor = preferredCursorItem;
            if (!items[cursor].Enabled)
            {
                sndEnabled = false; //sound would be played on opening
                MoveCursor();
                sndEnabled = true;
            }
            items[cursor].Select();
            UpdateHelpText();
        }

        public override void Close()
        {
            base.Close();
            Back.Hide();
            helpVis.Hide();
            for (int i = 0; i < items.Count; i++)
                items[i].Hide();

            items[cursor].Deselect();
        }

        #endregion

        #region Add item methods

        #region Standard Button
        protected MainMenuButton AddButton(string text, string help, int y, Action OnActivate)
        {   // X-centered version
            var b = new MainMenuButton(text, help, pos[1] + y, OnActivate);
            items.Add(b);
            return b;
        }

        protected MainMenuButton AddButton(string text, string help, int x, int y, Action OnActivate)
        {
            var b = new MainMenuButton(text, help, pos[0] + x, pos[1] + y, OnActivate);
            items.Add(b);
            return b;
        }
        #endregion

        #region Textbox
        protected MainMenuTextBox AddTextBox(string title, string help, int y, int width, Action OnActivate)
        { //centered version
            const int borderOffset = 70;
            var tb = new MainMenuTextBox(title, help, pos[0] + 640 - width - borderOffset, pos[1] + y, width, pos[0] + borderOffset, OnActivate);
            items.Add(tb);
            return tb;
        }

        protected MainMenuTextBox AddTextBox(string title, string help, int x, int y, int width, int titleX, Action OnActivate)
        {
            var tb = new MainMenuTextBox(title, help, pos[0] + x, pos[1] + y, width, pos[0] + titleX, OnActivate);
            items.Add(tb);
            return tb;
        }

        protected MainMenuTextBox AddTextBox(string title, string help, int x, int y, int width, int titleX, int titleY, Action OnActivate)
        {
            var tb = new MainMenuTextBox(title, help, pos[0] + x, pos[1] + y, width, pos[0] + titleX, pos[1] + titleY, OnActivate);
            items.Add(tb);
            return tb;
        }
        #endregion

        #region Character & Character list

        protected MainMenuCharacter AddCharacter(int x, int y, int w, int h)
        {
            var c = new MainMenuCharacter("", pos[0] + x, pos[1] + y, w, h);
            c.Enabled = false;
            items.Add(c);
            return c;
        }

        protected MainMenuCharacter AddCharacter(string help, int x, int y, int w, int h)
        {
            var c = new MainMenuCharacter(help, pos[0] + x, pos[1] + y, w, h);
            items.Add(c);
            return c;
        }

        protected CharListHandle AddCharList(int x, int y, int lines, MainMenuCharacter character, Action OnCharSelect, Action OnEmptySelect)
        {
            MainMenuCharSlot[] arr = new MainMenuCharSlot[lines];
            CharListHandle handle = new CharListHandle(arr);
            for (int i = 0; i < lines; i++)
            {
                arr[i] = new MainMenuCharSlot(i, pos[0] + x, pos[1] + y + GUCView.FontsizeDefault * i, character, OnCharSelect, OnEmptySelect);
            }
            items.AddRange(arr);
            return handle;
        }

        protected class CharListHandle
        {
            MainMenuCharSlot[] slots;
            public CharListHandle(MainMenuCharSlot[] items)
            {
                this.slots = items;
            }

            public void Fill(AccCharInfo[] infos)
            {
                for (int i = 0; i < slots.Length; i++)
                    slots[i].SetInfo(Array.Find(infos, info => info.SlotNum == i));
            }

            public int GetSlotNum(MainMenuItem slot)
            {
                if (slot is MainMenuCharSlot)
                {
                    return ((MainMenuCharSlot)slot).SlotNum;
                }
                return -1;
            }
        }

        #endregion

        #region Choices
        protected MainMenuChoice AddChoice(string title, string help, int x, int y, Dictionary<int, string> choices, bool sorted, Action OnActivate, Action OnChange)
        {
            var c = new MainMenuChoice(title, help, pos[0] + x, pos[1] + y, choices, sorted, OnActivate, OnChange);
            items.Add(c);
            return c;
        }
        #endregion

        #endregion

        #region Help Text
        protected void UpdateHelpText()
        {
            if (DateTime.Now.Ticks > HelpTextNextUpdateTime)
            {
                helpText.SetColor(ColorRGBA.White);
                helpText.Text = items[cursor].HelpText;
            }
        }

        protected long HelpTextNextUpdateTime = 0;
        public void SetHelpText(String Text)
        {
            helpText.SetColor(ColorRGBA.Red);
            helpText.Text = Text;
            HelpTextNextUpdateTime = DateTime.Now.Ticks + 2 * TimeSpan.TicksPerSecond;
        }
        #endregion

        #region Navigation
        protected void SetCursor(MainMenuItem item)
        {
            if (items.Contains(item) && item.Enabled)
            {
                items[cursor].Deselect();
                cursor = items.IndexOf(item);
                items[cursor].Select();
                UpdateHelpText();
            }
        }

        protected void SetCursor(int i)
        {
            if (i >= 0 && i < items.Count && items[i].Enabled)
            {
                items[cursor].Deselect();
                cursor = i;
                items[cursor].Select();
                UpdateHelpText();
            }
        }

        protected void MoveCursor()
        {
            MoveCursor(false);
        }

        protected void MoveCursor(bool up)
        {
            items[cursor].Deselect();
            if (up)
            {
                cursor--;
                if (cursor < 0)
                    cursor = items.Count - 1;
            }
            else
            {
                cursor++;
                if (cursor >= items.Count)
                    cursor = 0;
            }

            if (items[cursor].Enabled)
            {
                items[cursor].Select();
                UpdateHelpText();
            }
            else
            {
                MoveCursor(up);
                return;
            }
            PlaySound(SndBrowse);
        }

        public override void KeyPressed(VirtualKeys key)
        {
            if (key == VirtualKeys.Return)
            {
                if (items[cursor].OnActivate != null)
                {
                    items[cursor].OnActivate();
                }
                PlaySound(SndSelect);
                return;
            }
            else if (key == VirtualKeys.Up)
            {
                MoveCursor(true);
            }
            else if (key == VirtualKeys.Down || key == VirtualKeys.Tab)
            {
                MoveCursor(false);
            }
            else if (key == VirtualKeys.Escape)
            {
                this.Close();
                if (OnEscape != null)
                {
                    OnEscape();
                }
                PlaySound(SndEscape);
                return;
            }
            else if (items[cursor] is InputReceiver)
            {
                ((InputReceiver)items[cursor]).KeyPressed(key);
                PlaySound(SndBrowse);
            }
        }
        #endregion

        public override void Update(long now)
        {
            if (items[cursor] is MainMenuTextBox)
            {
                ((MainMenuTextBox)items[cursor]).Update(now);
            }
        }

        bool sndEnabled = true;
        void PlaySound(zCSoundFX snd)
        {
            if (sndEnabled)
            {
                zCSndSys_MSS.SoundSystem(Program.Process).PlaySound(snd, 0, 0, 1.3f * Program.GetSoundVol());
            }
        }

        static zCSoundFX sndBrowse = null;
        protected zCSoundFX SndBrowse
        {
            get
            {
                if (sndBrowse == null)
                {
                    using (zString z = zString.Create(Program.Process, "MENU_BROWSE"))
                    {
                        sndBrowse = zCSndSys_MSS.SoundSystem(Program.Process).LoadSoundFXScript(z);
                    }
                }
                return sndBrowse;
            }
        }

        static zCSoundFX sndSelect = null;
        protected zCSoundFX SndSelect
        {
            get
            {
                if (sndSelect == null)
                {
                    using (zString z = zString.Create(Program.Process, "MENU_SELECT"))
                    {
                        sndSelect = zCSndSys_MSS.SoundSystem(Program.Process).LoadSoundFXScript(z);
                    }
                }
                return sndSelect;
            }
        }

        static zCSoundFX sndEscape = null;
        protected zCSoundFX SndEscape
        {
            get
            {
                if (sndEscape == null)
                {
                    using (zString z = zString.Create(Program.Process, "MENU_ESC"))
                    {
                        sndEscape = zCSndSys_MSS.SoundSystem(Program.Process).LoadSoundFXScript(z);
                    }
                }
                return sndEscape;
            }
        }
    }
}
