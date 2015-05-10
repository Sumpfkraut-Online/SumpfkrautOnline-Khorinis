using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GUC.Sumpfkraut.Ingame.GUI;
using GUC.GUI;

using WinApi;
using WinApi.User.Enumeration;
using GUC.Types;
using GUC.Enumeration;
using GUC.WorldObjects.Character;
using GUC.WorldObjects;
using GUC.Network;
using RakNet;
using Gothic.mClasses;
using Gothic.zClasses;

using GUC.Sumpfkraut.Ingame;

using System;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace GUC.Sumpfkraut.Ingame
{

    class AnimationMenu : GUCMInputReceiver
    {

        [DllImport("kernel32.dll",
     EntryPoint = "GetStdHandle",
     SetLastError = true,
     CharSet = CharSet.Auto,
     CallingConvention = CallingConvention.StdCall)]
        private static extern IntPtr GetStdHandle(int nStdHandle);
        [DllImport("kernel32.dll",
            EntryPoint = "AllocConsole",
            SetLastError = true,
            CharSet = CharSet.Auto,
            CallingConvention = CallingConvention.StdCall)]
        private static extern int AllocConsole();

        GUCMenuTexture background;
        GUCMenuText[] TextLines;

        Dictionary<int, int> AnimationKeys = new Dictionary<int, int>()
        {
            {(int)VirtualKeys.N0,0},
            {(int)VirtualKeys.N1,1},
            {(int)VirtualKeys.N2,2},
            {(int)VirtualKeys.N3,3},
            {(int)VirtualKeys.N4,4},
            {(int)VirtualKeys.N5,5},
            {(int)VirtualKeys.N6,6},
            {(int)VirtualKeys.N7,7},
            {(int)VirtualKeys.N8,8},
            {(int)VirtualKeys.N9,9},
        };

        int[] displaySize = InputHooked.GetScreenSize(Process.ThisProcess());
        bool inputEnabled = false;
        // key to open the menu
        int OpenAnimationMenuKey = (int)VirtualKeys.X;

        AnimationMenuMessage AniMessage = new AnimationMenuMessage();
        AnimVoices loopAnimationEnd = AnimVoices.INVALID;
        AnimVoices latestAnimationsUsed = AnimVoices.INVALID;
        // Animation Pages
        AnimationPage currentShownAnimationPage, StartPage, ActionPage, ArmPage, PrayPage, EatPage, GesturePage, GreetPage,
        ScratchPage, MagicPage, DancePage, HGUARDPage, LGUARDPage, SitPage, WorkPage, AmbientPage, PraySpecial;
        // Animation Page Lists
        List<AnimationPage> StartPList, ActionPList, ArmPList, PrayPList, EatPList, GesturePList, GreetPList,
        ScratchPList, MagicPList, DancePList, HGUARDPList, LGUARDPList, SitPList, WorkPList, AmbientPList;


        public AnimationMenu()
        {

            AllocConsole();

            if (!Program.client.messageListener.ContainsKey((byte)NetworkID.AnimationMenuMessage))
            {
                Program.client.messageListener.Add((byte)NetworkID.AnimationMenuMessage, new AnimationMenuMessage());
            }

            int x = 50;
            int y = 50;

            background = new GUCMenuTexture("Menu_Ingame.tga", x, y, 350, 175);

            int offset = 15;

            TextLines = new GUCMenuText[10];
            for (int i = 0; i < 10; i++ )
            {
                if (i == 9)
                    offset = 16;

                TextLines[i] = new GUCMenuText("", x+5, y+5+offset*i);
                TextLines[i].Show();
            }

            IngameInput.menus.Add(this);

            InitAnimationPages();
        }

        public void KeyPressed(int key)
        {
            if (!inputEnabled)
            {
                if (key == OpenAnimationMenuKey) //Open Animation Menu
                {
                    background.Show();
                    inputEnabled = true;
                    ToggleTextLines(true);
                    IngameInput.activateFullControl(this);
                    UpdateTextLines();
                }
            }
            else if (key == OpenAnimationMenuKey || key == (int)VirtualKeys.Escape)
            {
                background.Hide();
                inputEnabled = false;
                ToggleTextLines(false);
                IngameInput.deactivateFullControl();
                ClearTextLines();
            }
            else if (AnimationKeys.ContainsKey(key))
            {
                TryUseAnimation(AnimationKeys[key]);
            }
        }

        public void TryUseAnimation(int key)
        {
            if (key == 0)
            {
                // Letzte ausgeführte Animation beenden
                if (latestAnimationsUsed > AnimVoices.INVALID)
                    AniMessage.StopAnimation(latestAnimationsUsed);

                // Die AnimationLoop beenden falls vorhanden
                if (loopAnimationEnd > AnimVoices.INVALID)
                    AniMessage.StartAnimation(loopAnimationEnd);

                // wenn möglich, eine Seite nach oben gehen
                if (currentShownAnimationPage.prevAniPage != null)
                    currentShownAnimationPage = currentShownAnimationPage.prevAniPage;

                // Zurücksetzen
                latestAnimationsUsed = AnimVoices.INVALID;
                loopAnimationEnd = AnimVoices.INVALID;
            }
            else
            {
                // die position im table an der Gesucht wird
                int position = key - 1;
                if (currentShownAnimationPage.AnimationPages != null)
                {
                    if (position < currentShownAnimationPage.AnimationPages.Count)
                    {
                        // Animation starten
                        if (currentShownAnimationPage.AnimationPages[position].ani > AnimVoices.INVALID)
                            AniMessage.StartAnimation(currentShownAnimationPage.AnimationPages[position].ani);
                        // LoopAnimation? -> Animation für's Beenden abspeichern
                        if (currentShownAnimationPage.AnimationPages[position].stopAni > AnimVoices.INVALID)
                            loopAnimationEnd = currentShownAnimationPage.AnimationPages[position].stopAni;
                        else
                            latestAnimationsUsed = currentShownAnimationPage.AnimationPages[position].ani;

                        // neue Animationsseite setzen, wenn es eine gibt.
                        if (currentShownAnimationPage.AnimationPages[position].AnimationPages != null)
                            currentShownAnimationPage = currentShownAnimationPage.AnimationPages[position];

                        Console.WriteLine(currentShownAnimationPage);
                    }
                }
            }
            // Die Texte der aktuellen AnimationPage anzeigen.
            UpdateTextLines();
        }

        public void UpdateTextLines()
        {
            if (currentShownAnimationPage == null)
            {
                Console.WriteLine("CurrentShownAnimationPage == null ");
                return;
            }

            if (currentShownAnimationPage.AnimationPages != null)
            {
                if (currentShownAnimationPage.AnimationPages.Count > 0)
                {
                    ClearTextLines();
                    int count = 0;
                    foreach (AnimationPage aniPage in currentShownAnimationPage.AnimationPages)
                    {
                        SetLine(count, count + 1, aniPage.text);
                        count++;
                    }
                }
                else
                    Console.WriteLine("Ani Count not > 0");
            }
            else
                Console.WriteLine("AnimationPages are empty!");

            if (currentShownAnimationPage != StartPage)
            {
                if (loopAnimationEnd > AnimVoices.INVALID)
                    SetLine(9, 0, "Animation beenden");
                else
                    SetLine(9, 0, "Zurück");
            }
        }

        public void ClearTextLines()
        {
            foreach (GUCMenuText text in TextLines)
                text.Text = "";
        }

        public void ToggleTextLines(bool mode)
        {
            if (mode)
                foreach (GUCMenuText text in TextLines)
                    text.Show();
            else
                foreach (GUCMenuText text in TextLines)
                    text.Hide();
        }

        public void SetLine(int pos, int nr, string text)
        {
            if (pos < TextLines.Length)
                TextLines[pos].Text = nr + ". " + text;
        }

        public void Update(long ticks)
        {
        }

        public void InitAnimationPages()
        {
            // Initialisierung aller Animation Pages mit Hirachie
            StartPage = new AnimationPage();

            ActionPage = new AnimationPage("Aktionen", StartPage);
            ArmPage = new AnimationPage("Armbewegungen", StartPage);
            PrayPage = new AnimationPage("Beten", StartPage);
            EatPage = new AnimationPage("Essen und Trinken", StartPage);
            GesturePage = new AnimationPage("Gesten", StartPage);
            GreetPage = new AnimationPage("Grüßen", StartPage);
            ScratchPage = new AnimationPage("Kratzen", StartPage);
            MagicPage = new AnimationPage("Magie üben", StartPage);
            DancePage = new AnimationPage("Tanzen", StartPage);

            HGUARDPage = new AnimationPage("Hände in die Hüfte", AnimVoices.T_STAND_2_HGUARD, AnimVoices.T_HGUARD_2_STAND, ActionPage);
            LGUARDPage = new AnimationPage("Arme verschränken", AnimVoices.T_STAND_2_LGUARD, AnimVoices.T_LGUARD_2_STAND, ActionPage);
            SitPage = new AnimationPage("Am Boden sitzen", AnimVoices.T_STAND_2_SIT, AnimVoices.T_SIT_2_STAND, ActionPage);
            WorkPage = new AnimationPage("Arbeiten", ActionPage);
            AmbientPage = new AnimationPage("Ambiente", ActionPage);

            StartPList = new List<AnimationPage>();

            #region Animation Lists

            #region ActionPList
            ActionPList = new List<AnimationPage>()
            {
                HGUARDPage,
                LGUARDPage,
                AmbientPage,
                WorkPage,
                SitPage,
                new AnimationPage("Am Boden schlafen", AnimVoices.T_STAND_2_SLEEPGROUND, AnimVoices.T_SLEEPGROUND_2_STAND, ActionPage),
                new AnimationPage("Bein schütteln", AnimVoices.R_LEGSHAKE, ActionPage),
                new AnimationPage("Geben", AnimVoices.T_TRADEITEM, ActionPage),
                new AnimationPage("Plündern",AnimVoices.T_PLUNDER, ActionPage),
            };


            HGUARDPList = new List<AnimationPage>{
                    new AnimationPage("Umsehen", AnimVoices.T_HGUARD_LOOKAROUND, ActionPage),
                    new AnimationPage("Durchgangsverweigerung", AnimVoices.T_HGUARD_NOENTRY, ActionPage),
                    new AnimationPage("Gruß", AnimVoices.T_HGUARD_GREET, ActionPage),
                    new AnimationPage("Durchgangsgenehmigung", AnimVoices.T_HGUARD_COMEIN, ActionPage),
            };

            LGUARDPList = new List<AnimationPage>(){
                    new AnimationPage("Umsehen", AnimVoices.T_HGUARD_LOOKAROUND, ActionPage),
                    new AnimationPage("Durchgangsverweigerung", AnimVoices.T_LGUARD_NOENTRY, ActionPage),
                    new AnimationPage("Gruß", AnimVoices.T_LGUARD_GREET, ActionPage),
                    new AnimationPage("Durchgangsgenehmigung", AnimVoices.T_LGUARD_COMEIN, ActionPage),
                    new AnimationPage("Nicken", AnimVoices.T_LGUARD_ALLRIGHT, ActionPage),
                    new AnimationPage("Gewichtsverlagerung", AnimVoices.T_LGUARD_CHANGELEG, ActionPage),
                    new AnimationPage("An der Nase kratzen", AnimVoices.T_LGUARD_SCRATCH, ActionPage),
                    new AnimationPage("Schaukeln", AnimVoices.T_LGUARD_STRETCH, ActionPage),
            };

            AmbientPList = new List<AnimationPage>(){
                    new AnimationPage("Schwerttraining", AnimVoices.T_1HSFREE, AnimVoices.T_1HSFREE, ActionPage),
                    new AnimationPage("Waffe inspizieren", AnimVoices.T_1HSINSPECT, ActionPage),
                    new AnimationPage("Horn blasen" ,AnimVoices.T_HORN_S0_2_S1, AnimVoices.T_HORN_S0_2_STAND, ActionPage),
                    new AnimationPage("Gitarre spielen", AnimVoices.T_LUTE_S0_2_S1, AnimVoices.T_LUTE_S0_2_STAND, ActionPage),
                    new AnimationPage("Waschen", AnimVoices.T_STAND_2_WASH, AnimVoices.T_WASH_2_STAND, ActionPage),
                    new AnimationPage("Pinkeln", AnimVoices.T_STAND_2_PEE, AnimVoices.T_PEE_2_STAND, ActionPage)
            };

            WorkPList = new List<AnimationPage>()
            {
                new AnimationPage("Harken", AnimVoices.T_RAKE_S0_2_S1, AnimVoices.T_RAKE_S0_2_STAND, ActionPage),
                new AnimationPage("Hämmern", AnimVoices.T_REPAIR_S0_2_S1, AnimVoices.T_REPAIR_S0_2_STAND, ActionPage),
                new AnimationPage("Fegen", AnimVoices.T_BROOM_S0_2_S1, AnimVoices.T_BROOM_S0_2_STAND, ActionPage),
                new AnimationPage("Wischen", AnimVoices.T_BRUSH_S0_2_S1, AnimVoices.T_BRUSH_S0_2_STAND, ActionPage),
            };

            SitPList = new List<AnimationPage>()
            {
                // Was kann man im Sitzen machen?
            };

            #endregion


            #region ScratchPList
            ScratchPList = new List<AnimationPage>(){
                    new AnimationPage("Am Sack kratzen",AnimVoices.R_SCRATCHEGG, StartPage),
                    new AnimationPage("Am Kopf kratzen",AnimVoices.R_SCRATCHHEAD, StartPage),
                    new AnimationPage("Linke Schulter kratzen",AnimVoices.R_SCRATCHLSHOULDER, StartPage),
                    new AnimationPage("Rechte Schulter kratzen",AnimVoices.R_SCRATCHRSHOULDER, StartPage),
            };
            #endregion

            #region Pray + Special Pray PList
            PraySpecial = new AnimationPage("Auf beiden Knien beten", AnimVoices.T_STAND_2_PRAY, AnimVoices.T_PRAY_2_STAND, StartPage);
            PrayPList = new List<AnimationPage>(){
                    new AnimationPage("Anbeten", AnimVoices.T_IDOL_S0_2_S1, AnimVoices.T_IDOL_S0_2_STAND, StartPage),
                    new AnimationPage("????", AnimVoices.T_IDOL_S0_2_S1, AnimVoices.T_IDOL_S1_2_S0, StartPage),
                    new AnimationPage("Auf einem Knie Beten", AnimVoices.T_INNOS_S0_2_S1, AnimVoices.T_INNOS_S0_2_STAND, StartPage),
                    new AnimationPage("????", AnimVoices.T_INNOS_S0_2_S1, AnimVoices.T_INNOS_S1_2_S0, StartPage),
                    PraySpecial,
            };

            PraySpecial.AnimationPages = new List<AnimationPage>() { 
                new AnimationPage("Schaukeln", AnimVoices.T_PRAY_RANDOM, PrayPage),
            };
            #endregion

            #region GreetPList
            GreetPList = new List<AnimationPage>(){
                    new AnimationPage("Gruß mit linkem Arm",AnimVoices.T_GREETCOOL, StartPage),
                    new AnimationPage("Gruß mit rechtem Arm",AnimVoices.T_GREETLEFT, StartPage),
                    new AnimationPage("Herzlicher Gruß",AnimVoices.T_GREETGRD, StartPage),
                    new AnimationPage("Gruß mit Verbeugung",AnimVoices.T_GREETNOV, StartPage),
                    new AnimationPage("Winken mit rechtem Arm",AnimVoices.T_GREETRIGHT, StartPage),
            };
            #endregion

            #region GesturePList
            GesturePList = new List<AnimationPage>(){
                    new AnimationPage("Boden treten",AnimVoices.T_BORINGKICK, StartPage),
                    new AnimationPage("Umschauen",AnimVoices.T_SEARCH, StartPage),
                    new AnimationPage("Nicken",AnimVoices.T_YES, StartPage),
                    new AnimationPage("Schulterzucken",AnimVoices.T_DONTKNOW, StartPage),
                    new AnimationPage("Verneinen",AnimVoices.T_NO, StartPage),
            };
            #endregion

            #region ArmPList
            ArmPList = new List<AnimationPage>(){
                    new AnimationPage("Jubeln",AnimVoices.T_STAND_2_WATCHFIGHT, AnimVoices.T_WATCHFIGHT_2_STAND, StartPage),
                    new AnimationPage("Enttäuschtes Jubeln",AnimVoices.T_WATCHFIGHT_OHNO, AnimVoices.T_WATCHFIGHT_2_STAND, StartPage),
                    new AnimationPage("Anfeuern",AnimVoices.T_WATCHFIGHT_YEAH, AnimVoices.T_WATCHFIGHT_2_STAND, StartPage),
                    new AnimationPage("Abwinken",AnimVoices.T_FORGETIT, StartPage),
                    new AnimationPage("Rumfuchteln 1",AnimVoices.T_GETLOST, StartPage),
                    new AnimationPage("Rumfuchteln 2",AnimVoices.T_GETLOST2, StartPage),
                    new AnimationPage("Ich kriege dich noch!",AnimVoices.T_IGETYOU, StartPage),
            };
            #endregion

            #region DancePList
            DancePList = new List<AnimationPage>(){
                    new AnimationPage("Tanzen1",AnimVoices.T_DANCE_01, StartPage),
                    new AnimationPage("Tanzen2",AnimVoices.T_DANCE_02, StartPage),
                    new AnimationPage("Tanzen3",AnimVoices.T_DANCE_03, StartPage),
                    new AnimationPage("Tanzen4",AnimVoices.T_DANCE_04, StartPage),
                    new AnimationPage("Tanzen5",AnimVoices.T_DANCE_05, StartPage),
                    new AnimationPage("Tanzen6",AnimVoices.T_DANCE_06, StartPage),
                    new AnimationPage("Tanzen7",AnimVoices.T_DANCE_07, StartPage),
                    new AnimationPage("Tanzen8",AnimVoices.T_DANCE_08, StartPage),
            };
            #endregion

            #region MagicPList
            MagicPList = new List<AnimationPage>(){
                    new AnimationPage("Magie üben ohne Wirkung",AnimVoices.T_PRACTICEMAGIC5, StartPage),
                    new AnimationPage("Feuermagie üben",AnimVoices.T_PRACTICEMAGIC, StartPage),
                    new AnimationPage("Feuermagie üben 2",AnimVoices.T_PRACTICEMAGIC4, StartPage),
                    new AnimationPage("Beschwörungsmagie üben",AnimVoices.T_PRACTICEMAGIC2, StartPage),
                    new AnimationPage("Teleportmagie üben",AnimVoices.T_PRACTICEMAGIC3, StartPage),
            };
            #endregion

            #region EatPList
            EatPList = new List<AnimationPage>(){
                    new AnimationPage("mit beiden Händen essen",AnimVoices.T_FOODHUGE_RANDOM_1, StartPage),
                    new AnimationPage("mit einer Hand essen",AnimVoices.T_FOOD_RANDOM_1, StartPage),
                    new AnimationPage("Essen in den Mund werfen",AnimVoices.T_FOOD_RANDOM_2, StartPage),
                    new AnimationPage("Trinken",AnimVoices.T_POTION_RANDOM_1, StartPage),
                    new AnimationPage("Mund abwischen",AnimVoices.T_POTION_RANDOM_2, StartPage),
                    new AnimationPage("Trank hochhalten und beobachten",AnimVoices.T_POTION_RANDOM_3, StartPage),
                    new AnimationPage("Suppe essen",AnimVoices.T_RICE_RANDOM_1, StartPage),
                    new AnimationPage("Suppe trinken",AnimVoices.T_RICE_RANDOM_2, StartPage),
                    new AnimationPage("Charakter hebt eine Reisschüssel",AnimVoices.S_RICE_S0, AnimVoices.S_RICE_S0, StartPage),
            };
            #endregion

            #endregion

            ActionPage.AnimationPages = ActionPList;
            ArmPage.AnimationPages = ArmPList;
            PrayPage.AnimationPages = PrayPList;
            EatPage.AnimationPages = EatPList;
            GesturePage.AnimationPages = GesturePList;
            GreetPage.AnimationPages = GreetPList;
            ScratchPage.AnimationPages = ScratchPList;
            MagicPage.AnimationPages = MagicPList;
            DancePage.AnimationPages = DancePList;

            HGUARDPage.AnimationPages = HGUARDPList;
            LGUARDPage.AnimationPages = LGUARDPList;
            SitPage.AnimationPages = SitPList;
            WorkPage.AnimationPages = WorkPList;
            AmbientPage.AnimationPages = AmbientPList;

            StartPList.Add(ActionPage);
            StartPList.Add(ArmPage);
            StartPList.Add(PrayPage);
            StartPList.Add(EatPage);
            StartPList.Add(GesturePage);
            StartPList.Add(GreetPage);
            StartPList.Add(ScratchPage);
            StartPList.Add(EatPage);
            StartPList.Add(PrayPage);

            StartPage.AnimationPages = StartPList;

            currentShownAnimationPage = StartPage;

        }

    }

    public class AnimationPage
    {
        // Die Animation die beim Aufruf der Seite ausgeführt wird.
        public AnimVoices ani = AnimVoices.INVALID;
        // Wird eine stopAni angegeben, so wird davon ausgegangen das die aktuelle Animation dauerharft ist und durch eine andere beendet wird.
        // wird keine StopAni angeben so wird die Animation im Falle der 0 Taste immer gestoppt. Egal ob bereits durch oder nicht.
        public AnimVoices stopAni = AnimVoices.INVALID;
        // Die Seite die über der aktuellen Seite steht. Wenn diese == null dann ist es die oberste in der Hirachie.
        public AnimationPage prevAniPage;
        // Eine Liste aller vorhanden Pages, bzw. Animationen in der aktuellen Page. Ist dieses Attribut in der auszuführenden AnimPage == null,
        // so wird die aktuelle Page beibehalten.
        public List<AnimationPage> AnimationPages;
        // Der Text mit der die Seite/Animation angezeigt wird.
        public string text;


        public AnimationPage()
        {
            this.text = "";
        }

        public AnimationPage(string text, AnimationPage prevAni)
        {
            // Ein Untermenü bei dem keine Animation abgespielt wird.
            this.text = text;
            this.prevAniPage = prevAni;
        }

        public AnimationPage(string text, AnimVoices ani, AnimationPage prevAni)
        {
            // Eine einfache Animation die abgespielt wird und beim abspielen in die Liste der zu beenden Animationen eingetragen.
            this.text = text;
            this.ani = ani;
            this.prevAniPage = prevAni;
        }

        public AnimationPage(string text, AnimationPage prevAni, List<AnimationPage> AnimationPages)
        {
            // Ein Untermenü bei dem keine Animation abgespielt wird.
            this.text = text;
            this.prevAniPage = prevAni;
            this.AnimationPages = AnimationPages;
        }

        public AnimationPage(string text, AnimVoices ani, AnimVoices stopAni, AnimationPage prevAni)
        {
            // Eine Animation die über eine weitere Animation gestoppt werden muss.
            this.text = text;
            this.ani = ani;
            this.prevAniPage = prevAni;
            this.stopAni = stopAni;
        }

        public AnimationPage(string text, AnimVoices ani, AnimationPage prevAni, AnimVoices stopAni, List<AnimationPage> AnimationPages)
        {
            // Eine Anmation die über eine andere gestoppt werden muss und zusätzlich eine weitere Seite öffnet.
            this.text = text;
            this.ani = ani;
            this.prevAniPage = prevAni;
            this.stopAni = stopAni;
            this.AnimationPages = AnimationPages;
        }

        public string toString()
        {
            return " CurrentAnimationPage: TEXT(" + text + ") ANI(" + ani + ") STOPANI(" + stopAni + ") PREVAP(" + prevAniPage + ")"; 
        }

    }

    class AnimationMenuMessage : IMessage
    {
        public void Read(RakNet.BitStream stream, RakNet.Packet packet, Client client)
        {
        }

        public void StartAnimation(AnimVoices animation)
        {
            BitStream stream = Program.client.sentBitStream;
            stream.Reset();
            stream.Write((byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.AnimationMenuMessage);
            stream.Write(Player.Hero.ID);
            stream.Write((short)animation);
            stream.Write(1);
            Program.client.client.Send(stream, PacketPriority.IMMEDIATE_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }
        public void StopAnimation(AnimVoices animation)
        {
            BitStream stream = Program.client.sentBitStream;
            stream.Reset();
            stream.Write((byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.AnimationMenuMessage);
            stream.Write(Player.Hero.ID);
            stream.Write((short)animation);
            stream.Write(0);
            Program.client.client.Send(stream, PacketPriority.IMMEDIATE_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }
    }
}
