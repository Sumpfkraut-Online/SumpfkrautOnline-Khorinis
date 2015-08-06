using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi.User.Enumeration;
using GUC.Client.GUI;
using GUC.Client.WorldObjects;
using Gothic.zClasses;
using GUC.Enumeration;

namespace GUC.Client.Menus
{
    class AnimationMenu : GUCMenu
    {
        public VirtualKeys Hotkey = VirtualKeys.X;

        GUCVisual back;
        GUCVisualText[] TextLines;
        AniPage current;

        public AnimationMenu()
        {
            const int backWidth = 300;
            const int backHeight = 400;
            const int numLines = 10;

            back = new GUCVisual(200, 200, backWidth, backHeight);
            back.SetBackTexture("Menu_Ingame.tga");     

            TextLines = new GUCVisualText[numLines];
            for (int i = 0; i < numLines; i++)
            {
                TextLines[i] = back.CreateText("hello baby", 20, 20 + i*25);
            }

            InitAniPages();
        }

        public override void Open()
        {
            zERROR.GetZErr(Program.Process).Report(2, 'G', "Open Ani Menu", 0, "hGame.cs", 0);
            back.Show();
            base.Open();
        }

        public override void Close()
        {
            zERROR.GetZErr(Program.Process).Report(2, 'G', "Close Ani Menu", 0, "hGame.cs", 0);
            back.Hide();
            base.Close();
        }

        public override void KeyPressed(VirtualKeys key)
        {
            if( !ActivateItem(key) )
            {
                Close();
            }
        }

        class AniPage
        {
            public string Text;
            public Animations StartAni;
            public Animations StopAni;
            public AniPage Parent;

            public List<AniPage> ItemList;

            public AniPage()
            {
                StartAni = Animations.INVALID;
                StopAni = Animations.INVALID;
                ItemList = new List<AniPage>();
            }

            public AniPage(string text) : this()
            {
                Text = text;
            }

            public AniPage(string text, Animations startAni) : this(text)
            {
                Text = text;
                StartAni = startAni;
            }

            public AniPage(string text, Animations startAni, Animations stopAni) : this(text, startAni)
            {
                StopAni = stopAni;
            }

            public void AddItem(AniPage item)
            {
                ItemList.Add(item);
                item.Parent = this;
            }
        }

        void SetMenu(AniPage newList)
        {
            current = newList;
            for (int i = 0; i < TextLines.Length; i++)
            {
                if (i < current.ItemList.Count)
                {
                    TextLines[i].Text = current.ItemList[i].Text;
                }
                else
                {
                    TextLines[i].Text = "";
                }
            }
        }

        int KeyToNumber(VirtualKeys key)
        {
            //so N1 = 0, N2 = 1, ... , N9 = 8, N0 = 9
            //int num = key - VirtualKeys.N0 - 1;
            //return num >= 0 ? num : 10;
            return key - VirtualKeys.N0;
        }

        bool ActivateItem(VirtualKeys key)
        {
            if (key < VirtualKeys.N0 || key > VirtualKeys.N9)
                return false;

            int num = KeyToNumber(key);
            if (num == 0 && current.Parent != null)
            {
                if(current.StopAni != Animations.INVALID)
                {
                    if(current.StartAni == current.StopAni)
                    {
                        StopAnimation(current.StopAni);
                    }
                    else
                    {
                        PlayAnimation(current.StopAni);
                    }
                }
                SetMenu(current.Parent);
            }
            else
            {
                if(num < current.ItemList.Count)
                {
                    if(current.ItemList[num].ItemList.Count > 0)
                    {
                        SetMenu(current.ItemList[num]);
                    }
                    if(current.ItemList[num].StartAni != Animations.INVALID)
                    {
                        PlayAnimation(current.ItemList[num].StartAni);
                    }
                }
            }
            return true;
        }

        public void InitAniPages()
        {

            AniPage MainPage = new AniPage();
            MainPage.AddItem(new AniPage("test",Animations.R_CHAIR_RANDOM_1, Animations.R_CHAIR_RANDOM_1));

            SetMenu(MainPage);
        }

        void PlayAnimation(Animations StartAni)
        {
            Player.Hero.AnimationStart(StartAni);
            Network.Messages.NPCMessage.WriteAnimationStart(StartAni);
            zERROR.GetZErr(Program.Process).Report(2, 'G', "Play Animation " + StartAni, 0, "hGame.cs", 0);
        }

        void FadeAnimation(Animations StopAni)
        {
            Player.Hero.AnimationFade(StopAni);
            Network.Messages.NPCMessage.WriteAnimationStop(StopAni, true);
            zERROR.GetZErr(Program.Process).Report(2, 'G', "Fade Animation " + StopAni, 0, "hGame.cs", 0);
        }

        void StopAnimation(Animations StopAni)
        {
            Player.Hero.AnimationStop(StopAni);
            Network.Messages.NPCMessage.WriteAnimationStop(StopAni, false);
            zERROR.GetZErr(Program.Process).Report(2, 'G', "Stop Animation " + StopAni, 0, "hGame.cs", 0);
        }

    }
}
