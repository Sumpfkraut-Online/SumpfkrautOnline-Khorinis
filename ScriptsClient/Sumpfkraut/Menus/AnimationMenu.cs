using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi.User.Enumeration;
using GUC.Client.GUI;
using GUC.Enumeration;

namespace GUC.Client.Menus
{
    /*class AnimationMenu : GUCMenu
    {
        public VirtualKeys Hotkey = VirtualKeys.X;

        GUCVisual back;
        GUCVisualText[] TextLines;
        AniPage current;
        Animations lastAnimation;
        int stopModus;

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
                TextLines[i] = back.CreateText("anything right here", 20, 20 + i*25);
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
            for (int i = 1; i < TextLines.Length; i++)
            {
                if (i - 1 < current.ItemList.Count && current.ItemList[i - 1].Text != null && current.ItemList[i - 1].Text.Length > 0)
                {
                    TextLines[i].Text = (i) + " " + current.ItemList[i-1].Text;
                }
                else
                {
                    TextLines[i].Text = "";
                }
            }
            TextLines[0].Text = "0 < Zurück";
        }

        int KeyToNumber(VirtualKeys key)
        {
            //so N1 = 0, N2 = 1, ... , N9 = 8, N0 = 9
            int num = key - VirtualKeys.N0 - 1;
            return num >= 0 ? num : 10;
            //return key - VirtualKeys.N0;
        }

        bool ActivateItem(VirtualKeys key)
        {
            if (key < VirtualKeys.N0 || key > VirtualKeys.N9)
                return false;

            int num = KeyToNumber(key);
            if (num == 10 && current.Parent != null)
            {
                StopLastAnimation(false);
                if(current.StopAni != Animations.INVALID)
                {
                    if(current.StartAni == current.StopAni)
                    {
                        zERROR.GetZErr(Program.Process).Report(2, 'G', "Intern 10 Stop Animation", 0, "hGame.cs", 0);
                        StopAnimation(current.StopAni);
                    }
                    else
                    {
                        zERROR.GetZErr(Program.Process).Report(2, 'G', "Intern 10 Play Animation", 0, "hGame.cs", 0);
                        PlayAnimation(current.StopAni);
                    }
                }
                zERROR.GetZErr(Program.Process).Report(2, 'G', "Intern SetMenu", 0, "hGame.cs", 0);
                SetMenu(current.Parent);
            }
            else
            {
                StopLastAnimation(true);
                stopModus = -1;
                zERROR.GetZErr(Program.Process).Report(2, 'G', "compare count " + current.ItemList.Count + " with num " + num, 0, "hGame.cs", 0);
                if(num < current.ItemList.Count)
                {           
                    if(current.ItemList[num].StartAni != Animations.INVALID)
                    {
                        zERROR.GetZErr(Program.Process).Report(2, 'G', "Intern 20 Play Animation", 0, "hGame.cs", 0);
                        PlayAnimation(current.ItemList[num].StartAni);

                        // Create Stop Modus
                        if (current.ItemList[num].StopAni == Animations.INVALID || current.ItemList[num].StopAni == current.ItemList[num].StartAni)
                        {
                            stopModus = 1;
                            lastAnimation = current.ItemList[num].StartAni;
                        }
                        else
                        {
                            stopModus = 2;
                            lastAnimation = current.ItemList[num].StopAni;
                        }
                    }
                    if (current.ItemList[num].ItemList.Count > 0)
                    {
                        zERROR.GetZErr(Program.Process).Report(2, 'G', "Intern 20 Set Menu", 0, "hGame.cs", 0);
                        SetMenu(current.ItemList[num]);
                    }
                }
            }
            return true;
        }

        public void StopLastAnimation( bool modeOneOnly )
        {
             // bei modeOneOnly = true werden Animationen die eine eigene StopAni haben nicht gestoppt
            if (stopModus == 1)
            {
                zERROR.GetZErr(Program.Process).Report(2, 'G', "HardStop 1 by stop", 0, "hGame.cs", 0);
                StopAnimation(lastAnimation);
            }
            else if(stopModus == 2 && !modeOneOnly )
            {
                zERROR.GetZErr(Program.Process).Report(2, 'G', "HardStop 2 by play", 0, "hGame.cs", 0);
                PlayAnimation(lastAnimation);
            }
        }


        public void InitAniPages()
        {

            AniPage MainPage = new AniPage();

            int MainPageItem = -1;

            // Hände in die Hüfte + alle Unteraminationen
            MainPageItem++;
            MainPage.AddItem(new AniPage("> Hände in die Hüfte", Animations.T_STAND_2_HGUARD, Animations.T_HGUARD_2_STAND));
            MainPage.ItemList[MainPageItem].AddItem( new AniPage("- Umsehen", Animations.T_HGUARD_LOOKAROUND));
            MainPage.ItemList[MainPageItem].AddItem( new AniPage("- Durchgangsverweigerung", Animations.T_HGUARD_NOENTRY));
            MainPage.ItemList[MainPageItem].AddItem( new AniPage("- Gruß", Animations.T_HGUARD_GREET));
            MainPage.ItemList[MainPageItem].AddItem( new AniPage("- Durchgangsgenehmigung", Animations.T_HGUARD_COMEIN));


            // Arme verschränken + alle Unteranimationen
            MainPageItem++;
            MainPage.AddItem(new AniPage("> Arme verschränken", Animations.T_STAND_2_LGUARD, Animations.T_LGUARD_2_STAND));
            MainPage.ItemList[MainPageItem].AddItem( new AniPage("- Umsehen", Animations.T_HGUARD_LOOKAROUND));
            MainPage.ItemList[MainPageItem].AddItem( new AniPage("- Durchgangsverweigerung", Animations.T_LGUARD_NOENTRY));
            MainPage.ItemList[MainPageItem].AddItem( new AniPage("- Gruß", Animations.T_LGUARD_GREET));
            MainPage.ItemList[MainPageItem].AddItem( new AniPage("- Durchgangsgenehmigung", Animations.T_LGUARD_COMEIN));
            MainPage.ItemList[MainPageItem].AddItem( new AniPage("- Nicken", Animations.T_LGUARD_ALLRIGHT));
            MainPage.ItemList[MainPageItem].AddItem( new AniPage("- Gewichtsverlagerung", Animations.T_LGUARD_CHANGELEG));
            MainPage.ItemList[MainPageItem].AddItem( new AniPage("- An der Nase kratzen", Animations.T_LGUARD_SCRATCH));
            MainPage.ItemList[MainPageItem].AddItem( new AniPage("- Schaukeln", Animations.T_LGUARD_STRETCH));

            // Kratzen
            MainPageItem++;
            MainPage.AddItem(new AniPage("> Kratzen"));
            MainPage.ItemList[MainPageItem].AddItem( new AniPage("- Am Sack kratzen",Animations.R_SCRATCHEGG));
            MainPage.ItemList[MainPageItem].AddItem( new AniPage("- Am Kopf kratzen",Animations.R_SCRATCHHEAD));
            MainPage.ItemList[MainPageItem].AddItem( new AniPage("- Linke Schulter kratzen",Animations.R_SCRATCHLSHOULDER));
            MainPage.ItemList[MainPageItem].AddItem( new AniPage("- Rechte Schulter kratzen",Animations.R_SCRATCHRSHOULDER));

            // Gesten
            MainPageItem++;
            MainPage.AddItem(new AniPage("> Gesten"));
            MainPage.ItemList[MainPageItem].AddItem( new AniPage("- Boden treten",Animations.T_BORINGKICK));
            MainPage.ItemList[MainPageItem].AddItem( new AniPage("- Umschauen",Animations.T_SEARCH));
            MainPage.ItemList[MainPageItem].AddItem( new AniPage("- Nicken",Animations.T_YES));
            MainPage.ItemList[MainPageItem].AddItem( new AniPage("- Schulterzucken",Animations.T_DONTKNOW));
            MainPage.ItemList[MainPageItem].AddItem( new AniPage("- Verneinen",Animations.T_NO));

            // Jubel
            MainPageItem++;
            MainPage.AddItem(new AniPage("> Jubeln"));
            MainPage.ItemList[MainPageItem].AddItem( new AniPage("> Jubeln",Animations.T_STAND_2_WATCHFIGHT, Animations.T_WATCHFIGHT_2_STAND));
            // Eigenes Untermenü für Jubeln
            MainPage.ItemList[MainPageItem].ItemList[0].AddItem( new AniPage("- Enttäuschtes Jubeln", Animations.T_WATCHFIGHT_OHNO, Animations.T_WATCHFIGHT_2_STAND));
            MainPage.ItemList[MainPageItem].ItemList[0].AddItem (new AniPage("- Anfeuern", Animations.T_WATCHFIGHT_YEAH, Animations.T_WATCHFIGHT_2_STAND));
            //MainPage.ItemList[MainPageItem].AddItem( new AniPage("Enttäuschtes Jubeln",Animations.T_WATCHFIGHT_OHNO, Animations.T_WATCHFIGHT_2_STAND));
            //MainPage.ItemList[MainPageItem].AddItem( new AniPage("Anfeuern", Animations.T_WATCHFIGHT_YEAH, Animations.T_WATCHFIGHT_2_STAND));

            MainPage.ItemList[MainPageItem].AddItem( new AniPage("- Abwinken",Animations.T_FORGETIT));
            MainPage.ItemList[MainPageItem].AddItem( new AniPage("- Rumfuchteln 1",Animations.T_GETLOST));
            MainPage.ItemList[MainPageItem].AddItem( new AniPage("- Rumfuchteln 2",Animations.T_GETLOST2));
            MainPage.ItemList[MainPageItem].AddItem( new AniPage("- Ich kriege dich noch!",Animations.T_IGETYOU));

            // Grüßen
            MainPageItem++;
            MainPage.AddItem(new AniPage("> Grüßen"));
            MainPage.ItemList[MainPageItem].AddItem( new AniPage("- Gruß mit linkem Arm",Animations.T_GREETCOOL));
            MainPage.ItemList[MainPageItem].AddItem( new AniPage("- Gruß mit rechtem Arm",Animations.T_GREETLEFT));
            MainPage.ItemList[MainPageItem].AddItem( new AniPage("- Herzlicher Gruß",Animations.T_GREETGRD));
            MainPage.ItemList[MainPageItem].AddItem( new AniPage("- Gruß mit Verbeugung",Animations.T_GREETNOV));
            MainPage.ItemList[MainPageItem].AddItem( new AniPage("- Winken mit rechtem Arm",Animations.T_GREETRIGHT));

            // Ambiente
            MainPageItem++;
            MainPage.AddItem(new AniPage("> Ambiente"));
            MainPage.ItemList[MainPageItem].AddItem( new AniPage("- Schwerttraining", Animations.T_1HSFREE, Animations.T_1HSFREE));
            MainPage.ItemList[MainPageItem].AddItem( new AniPage("- Waffe inspizieren", Animations.T_1HSINSPECT));
            MainPage.ItemList[MainPageItem].AddItem( new AniPage("- Horn blasen" ,Animations.T_HORN_S0_2_S1, Animations.T_HORN_S0_2_STAND));
            MainPage.ItemList[MainPageItem].AddItem( new AniPage("- Gitarre spielen", Animations.T_LUTE_S0_2_S1, Animations.T_LUTE_S0_2_STAND));
            MainPage.ItemList[MainPageItem].AddItem( new AniPage("- Waschen", Animations.T_STAND_2_WASH, Animations.T_WASH_2_STAND));
            MainPage.ItemList[MainPageItem].AddItem( new AniPage("- Pinkeln", Animations.T_STAND_2_PEE, Animations.T_PEE_2_STAND));

            // Aktionen
            MainPageItem++;
            MainPage.AddItem(new AniPage("> Aktionen"));
            MainPage.ItemList[MainPageItem].AddItem( new AniPage("> Am Boden sitzen", Animations.T_STAND_2_SIT, Animations.T_SIT_2_STAND));
            MainPage.ItemList[MainPageItem].ItemList[0].AddItem(new AniPage()); // Offenes leeres Menü für Sitzen
            MainPage.ItemList[MainPageItem].AddItem( new AniPage("> Am Boden schlafen", Animations.T_STAND_2_SLEEPGROUND, Animations.T_SLEEPGROUND_2_STAND));
            MainPage.ItemList[MainPageItem].ItemList[1].AddItem(new AniPage()); // Offenes leeres Menü für Schlafen
            MainPage.ItemList[MainPageItem].AddItem( new AniPage("> Anbeten", Animations.T_IDOL_S0_2_S1, Animations.T_IDOL_S0_2_STAND));
            MainPage.ItemList[MainPageItem].ItemList[2].AddItem(new AniPage()); // Offenes leeres Menü für Anbeten
            MainPage.ItemList[MainPageItem].AddItem( new AniPage("> Auf einem Knie Beten", Animations.T_INNOS_S0_2_S1, Animations.T_INNOS_S0_2_STAND));
            MainPage.ItemList[MainPageItem].ItemList[3].AddItem(new AniPage()); // Offenes leeres Menü für Knien1
            MainPage.ItemList[MainPageItem].AddItem( new AniPage("> Auf beiden Knien beten", Animations.T_STAND_2_PRAY, Animations.T_PRAY_2_STAND));
            MainPage.ItemList[MainPageItem].ItemList[4].AddItem(new AniPage()); // Offenes leeres Menü für Knien2
            MainPage.ItemList[MainPageItem].AddItem(new AniPage("- Bein schütteln", Animations.R_LEGSHAKE));
            MainPage.ItemList[MainPageItem].AddItem(new AniPage("- Geben", Animations.T_TRADEITEM));
            MainPage.ItemList[MainPageItem].AddItem(new AniPage("- Plündern", Animations.T_PLUNDER));


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

    }*/
}
