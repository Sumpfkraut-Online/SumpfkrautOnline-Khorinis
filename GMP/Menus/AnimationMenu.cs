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

        bool isOpen = false;
        GUCVisual back;

        public AnimationMenu()
        {
            const int backWidth = 300;
            const int backHeight = 400;
            back = new GUCVisual(200, 200, backWidth, backHeight);
            back.SetBackTexture("Menu_Ingame.tga");
        }

        public override void Open()
        {
            zERROR.GetZErr(Program.Process).Report(2, 'G', "Open THIS SHIT", 0, "hGame.cs", 0);
            back.Show();
            isOpen = true;
            base.Open();
        }

        public override void Close()
        {
            back.Hide();
            isOpen = false;
            base.Close();
        }

        public override void KeyPressed(VirtualKeys key)
        {
            if (!isOpen)
            {
                if(key == VirtualKeys.X)
                {
                    Open();
                }
            }
            else if( !ActivateItem(key) )
            {
                Close();
            }
        }

        abstract class AniItem
        {
            public string Text;
        }

        class Animation : AniItem
        {
            public Animations StartAni = Animations.INVALID;
            public Animations StopAni = Animations.INVALID;
            public Animation(string text, Animations StartAni)
            {
                Text = text;
                this.StartAni = StartAni;
            }
            public Animation(string text, Animations StartAni, Animations StopAni) : this(text, StartAni)
            {
                this.StopAni = StopAni;
            }
        }

        class AniPage : AniItem
        {
            public AniPage Parent;
            public Animation PageAnimation;
            public List<AniItem> ItemList;
            public AniPage(string text)
            {
                Text = text;
                ItemList = new List<AniItem>();
            }

            public void AddItem(AniItem item)
            {
                ItemList.Add(item);
                if (item is AniPage)
                {
                    ((AniPage)item).Parent = this;
                }
            }
        }

        AniPage current;

        void SetMenu(AniPage newList)
        {
            current = newList;
            for (int i = 0; i < current.ItemList.Count; i++)
            {
                //VisualTexts[i].SetText(curList.list[i].Text);
            }
        }

        int KeyToNumber(VirtualKeys key)
        {
            return key - VirtualKeys.N0;
        }

        bool ActivateItem(VirtualKeys key)
        {
            int num =  KeyToNumber(key);
            if (key < VirtualKeys.N0 || key > VirtualKeys.N9 || num >= current.ItemList.Count)
                return false;

            if(num == 0 && current.Parent != null)
            {
                if(current.Parent.PageAnimation != null)
                {
                    StopAnimation(current.Parent.PageAnimation);
                }
                SetMenu(current.Parent);
                return true;
            }
            else if (current.ItemList[num] is AniPage)
            {
                if (((AniPage)current.ItemList[num]).PageAnimation != null)
                {
                    PlayAnimation(((Animation)current.ItemList[num]).StartAni);
                }
                SetMenu((AniPage)current.ItemList[num]);
                return true;
            }
            else
            {
                // Animation starten
                PlayAnimation(((Animation)current.ItemList[num]).StartAni);
                return true;
            }
        }

        public void StopAnimation(Animations StopAni)
        {

        }
        public void PlayAnimation(Animations StartAni)
        {

        }

    }
}
