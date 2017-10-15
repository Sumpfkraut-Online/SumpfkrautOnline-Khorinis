using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.GUI;
using GUC.Scripts.Sumpfkraut.GUI.MainMenu;
using WinApi.User.Enumeration;
using GUC.Types;
using GUC.Scripts.Sumpfkraut.Controls;


namespace GUC.Scripts.Sumpfkraut.Menus.MainMenus
{
    /// <summary>
    /// Recreation of the classic Gothic main menu.
    /// </summary>
    abstract class GUCMainMenu : GUCMenu
    {
        protected GUCVisual Back;
        protected GUCVisual helpVis;
        protected GUCVisualText helpText { get { return helpVis.Texts[0]; } }
        protected List<MainMenuItem> items = new List<MainMenuItem>();
        protected int cursor = 0;
        protected int[] pos;
        protected int preferredCursorItem = 0;

        protected Action OnEscape = null;

        public MainMenuItem CurrentItem { get; private set; }

        KeyHoldHelper scrollHelper;

        #region Init
        public GUCMainMenu()
        {
            var screenSize = GUCView.GetScreenSize();
            pos = new int[] { (screenSize.X - 640) / 2, (screenSize.Y - 480) / 2 };
            Back = new GUCVisual(pos[0], pos[1], 640, 480);
            Back.SetBackTexture("Menu_Ingame.tga");
            Back.Font = GUCVisual.Fonts.Menu;

            helpVis = GUCVisualText.Create("", 0, pos[1] + 455);
            helpText.CenteredX = true;

            scrollHelper = new KeyHoldHelper()
            {
                { () => MoveCursor(true), VirtualKeys.Up },
                { () => MoveCursor(false), VirtualKeys.Down },
                { () => MoveCursor(false), VirtualKeys.Tab },
            };
        }

        private bool init = false;
        protected abstract void OnCreate();
        #endregion

        #region Open & Close

        protected bool isOpen = false;

        public override void Open()
        {
            CloseActiveMenus(); //main menus never overlap

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
            CurrentItem = items[cursor];

            if (!CurrentItem.Enabled)
            {
                sndEnabled = false; //sound would be played on opening
                MoveCursor();
                sndEnabled = true;
            }
            CurrentItem.Select();
            UpdateHelpText();

            isOpen = true;
        }

        public override void Close()
        {
            base.Close();
            Back.Hide();
            helpVis.Hide();
            for (int i = 0; i < items.Count; i++)
                items[i].Hide();

            CurrentItem.Deselect();

            isOpen = false;
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
        /*
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
        */
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
            if (DateTime.UtcNow.Ticks > helpTextNextUpdateTime)
            {
                helpText.SetColor(ColorRGBA.White);
                helpText.Text = CurrentItem.HelpText;
            }
        }

        protected long helpTextNextUpdateTime = 0;
        public void SetHelpText(String Text)
        {
            helpText.SetColor(ColorRGBA.Red);
            helpText.Text = Text;
            helpTextNextUpdateTime = DateTime.UtcNow.Ticks + 2 * TimeSpan.TicksPerSecond;
        }
        #endregion

        #endregion

        #region Navigation
        protected void SetCursor(MainMenuItem item)
        {
            SetCursor(items.IndexOf(item));
        }

        protected void SetCursor(int i)
        {
            if (i >= 0 && i < items.Count)
            {
                MainMenuItem newItem = items[i];
                if (newItem.Enabled)
                {
                    CurrentItem.Deselect();
                    cursor = i;
                    CurrentItem = newItem;
                    CurrentItem.Select();
                    UpdateHelpText();
                }
            }
        }

        protected void MoveCursor()
        {
            MoveCursor(false);
        }

        protected void MoveCursor(bool up)
        {
            CurrentItem.Deselect();
            for (int i = 0; i < items.Count; i++)
            {
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

                CurrentItem = items[cursor];
                if (CurrentItem.Enabled)
                {
                    CurrentItem.Select();
                    UpdateHelpText();
                    break;
                }
            }
            //PlaySound(SndBrowse);
        }
        
        protected override void KeyDown(VirtualKeys key)
        {
            long now = GameTime.Ticks;
            switch (key)
            {
                case VirtualKeys.Return:
                    if (items[cursor].OnActivate != null)
                    {
                        items[cursor].OnActivate();
                    }
                    //PlaySound(SndSelect);
                    break;
                case VirtualKeys.Escape:
                    this.Close();
                    if (OnEscape != null)
                    {
                        OnEscape();
                    }
                    //PlaySound(SndEscape);
                    break;
                default:
                    if (CurrentItem is InputReceiver)
                    {
                        ((InputReceiver)CurrentItem).KeyPressed(key);
                    }
                    break;
            }
        }

        #endregion

        protected override void Update(long now)
        {
            scrollHelper.Update(now);
            if (items[cursor] is MainMenuTextBox)
            {
                ((MainMenuTextBox)items[cursor]).Update(now);
            }
        }

        bool sndEnabled = true;
        /*void PlaySound(zCSoundFX snd)
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
        }*/
    }
}
