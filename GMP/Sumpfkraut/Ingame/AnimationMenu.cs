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
        AnimationPage currentShownAnimationPage, StartPage, FirstPage, SecondPage, Test;
        List<AnimationPage> MainAnimationList, Ambiente, Working, Aho;


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

                        // neue Animation page setzen
                        currentShownAnimationPage = currentShownAnimationPage.AnimationPages[position];

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
            FirstPage = new AnimationPage("Erste Aktion", StartPage);
            SecondPage = new AnimationPage("Zweite Aktion", StartPage);
            Test = new AnimationPage("Rang 2", SecondPage);

            MainAnimationList = new List<AnimationPage>();

            Ambiente = new List<AnimationPage>()
            {
                new AnimationPage("Pinkeln", AnimVoices.T_STAND_2_PEE, AnimVoices.T_PEE_2_STAND, FirstPage),
            };

            Working = new List<AnimationPage>()
            {

            };

            Aho = new List<AnimationPage>()
            {
                new AnimationPage("Pinkeln", AnimVoices.T_STAND_2_PEE, AnimVoices.T_PEE_2_STAND, Test),
            };

            Test.AnimationPages = Aho;

            Working.Add(Test);
            MainAnimationList.Add(FirstPage);
            MainAnimationList.Add(SecondPage);

            
            FirstPage.AnimationPages = Ambiente;
            SecondPage.AnimationPages = Working;
            StartPage.AnimationPages = MainAnimationList;

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
