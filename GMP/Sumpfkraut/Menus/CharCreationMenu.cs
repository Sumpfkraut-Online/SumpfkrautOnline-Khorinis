using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Sumpfkraut.GUI;
using GUC.Enumeration;
using GUC.Sumpfkraut.Login;

namespace GUC.Sumpfkraut.Menus
{
    partial class MainMenus
    {
        class CharCreationMenu : AbstractMenu
        {
            GUCMainMenuTextBox name;
            GUCMainMenuChoice cBodyMesh;
            GUCMainMenuChoice cBodyTex;
            GUCMainMenuChoice cHeadMesh;
            GUCMainMenuChoice cHeadTex;
            GUCMainMenuChoice cFatness;
            GUCMainMenuChoice cBodyHeight;
            GUCMainMenuChoice cBodyWidth;
            GUCMainMenuChoice cVoice;
            GUCMainMenuChoice cFormerClass;

            bool male = true;
            int slotNum;

            protected override void CreateMenu()
            {
                const int offset = 27;
                const int dist = 50;

                thisMenu = new GUCMainMenu(1);
                name = thisMenu.AddMenuTextBox("Name:", "Wähle den Namen deines Charakters.", null, 45, 15, 200);
                thisMenu.AddCharacter(-60, 50, 533, 400, null, true);
                thisMenu.AddMenuButton("Start", "Mit dem Charakter in die Welt starten.", CreateNewCharacter, 160, 425);
                cBodyMesh = thisMenu.AddMenuChoice("Geschlecht", "Wähle das Geschlecht deines Charakters.", null, 415, offset + dist * 0, genders, ChangedGender);
                cBodyTex = thisMenu.AddMenuChoice("Haut", "Wähle die Haut deines Charakters.", null, 415, offset + dist * 1, bodyTex_M, ChangedBodyTex);
                cHeadMesh = thisMenu.AddMenuChoice("Kopf", "Wähle die Kopfform deines Charakters.", null, 415, offset + dist * 2, headMeshes_M, ChangedHeadMesh);
                cHeadTex = thisMenu.AddMenuChoice("Gesicht", "Wähle das Gesicht deines Charakters.", null, 415, offset + dist * 3, faces_M_N, ChangedHeadTex);
                cFatness = thisMenu.AddMenuChoice("Statur", "Wähle die Statur deines Charakters.", null, 415, offset + dist * 4, fatness, ChangedFatness);
                cBodyHeight = thisMenu.AddMenuChoice("Größe", "Wähle die Körpergröße deines Charakters.", null, 415, offset + dist * 5, bodyHeight, ChangedHeight);
                cBodyWidth = thisMenu.AddMenuChoice("Breite", "Wähle die Körperbreite deines Charakters.", null, 415, offset + dist * 6, bodyWidth, ChangedWidth);
                cVoice = thisMenu.AddMenuChoice("Stimme", "Wähle die Stimme deines Charakters.", PlayVoice, 415, offset + dist * 7, voices_M, null);
                cFormerClass = thisMenu.AddMenuChoice("ehem. Beruf", "Wähle den Beruf den dein Charakter früher tätigte.", null, 415, offset + dist * 8, formerClass, null);
                thisMenu.OnEscape = CharList.Open;

                cBodyTex.Sorted = true;
                //cHeadMesh.Sorted = true;
                cHeadTex.Sorted = true;
                cVoice.Sorted = true;
                cVoice.OnChange += PlayVoice; //so it doesn't play a sound on opening the menu
            }

            public void Open(int num)
            {
                slotNum = num; Open();
            }

            void CreateNewCharacter(object sender, EventArgs e)
            {
                string charName = name.TextBox.input;
                if (charName.Length < 2)
                {
                    thisMenu.SetCursor(0);
                    thisMenu.SetHelpText("Dein Charaktername ist zu kurz.");
                    return;
                }

                LoginMessage.CharInfo ci = new LoginMessage.CharInfo();
                ci.SlotNum = slotNum;
                ci.Name = charName;
                ci.BodyMesh = cBodyMesh.Choice;
                ci.BodyTex = cBodyTex.Choice;
                ci.HeadMesh = cHeadMesh.Choice;
                ci.HeadTex = cHeadTex.Choice;
                ci.Fatness = cFatness.Choice / 100.0f;
                ci.BodyHeight = cBodyHeight.Choice / 100.0f;
                ci.BodyWidth = cBodyWidth.Choice / 100.0f;
                ci.Voice = cVoice.Choice;
                ci.FormerClass = cFormerClass.Choice;

                LoginMessage.GetMsg().CreateNewCharacter(ci);
            }

            void ChangedGender(object sender, EventArgs e)
            {
                if (cBodyMesh.Choice == 0) //male
                {
                    male = true;
                    cHeadMesh.SetChoices(headMeshes_M);
                    cBodyTex.SetChoices(bodyTex_M);
                    cVoice.SetChoices(voices_M);
                }
                else if (cBodyMesh.Choice == 1) //female
                {
                    male = false;
                    cHeadMesh.SetChoices(headMeshes_F);
                    cBodyTex.SetChoices(bodyTex_F);
                    cVoice.SetChoices(voices_F);
                }
                thisMenu.Character.BodyMesh = cBodyMesh.Choice;
            }

            void ChangedBodyTex(object sender, EventArgs e)
            {
                if (cBodyTex.Choice == 0 || cBodyTex.Choice == 4) //pale
                {
                    cHeadTex.SetChoices(male ? faces_M_P : faces_F_P);
                }
                else if (cBodyTex.Choice == 2 || cBodyTex.Choice == 6) //Latino
                {
                    cHeadTex.SetChoices(male ? faces_M_L : faces_F_L);
                }
                else if (cBodyTex.Choice == 3 || cBodyTex.Choice == 7) //Black
                {
                    cHeadTex.SetChoices(male ? faces_M_B : faces_F_B);
                }
                else
                {
                    cHeadTex.SetChoices(male ? faces_M_N : faces_F_N);
                }
                thisMenu.Character.BodyTex = cBodyTex.Choice;
            }

            void ChangedHeadMesh(object sender, EventArgs e)
            {
                thisMenu.Character.HeadMesh = cHeadMesh.Choice;
            }

            void ChangedHeadTex(object sender, EventArgs e)
            {
                thisMenu.Character.HeadTex = cHeadTex.Choice;
            }

            void ChangedFatness(object sender, EventArgs e)
            {
                thisMenu.Character.Fatness = cFatness.Choice / 100.0f;
            }

            void ChangedHeight(object sender, EventArgs e)
            {
                thisMenu.Character.BodyHeight = cBodyHeight.Choice / 100.0f;
            }

            void ChangedWidth(object sender, EventArgs e)
            {
                thisMenu.Character.BodyWidth = cBodyWidth.Choice / 100.0f;
            }

            Random rand = null;
            void PlayVoice(object sender, EventArgs e)
            {
                if (rand == null)
                {
                    rand = new Random();
                }

                string v = string.Format("SVM_{0}_{1}.WAV", cVoice.Choice, ((AnimVoices)rand.Next((int)AnimVoices.MAX_ANIMATIONS+1, (int)AnimVoices.MAX_G2_VOICES)).ToString());
                InputHandler.PlaySound(v);
            }

            private Dictionary<int, string> genders = new Dictionary<int, string>() { { 0, "männlich" }, { 1, "weiblich" } };

            private Dictionary<int, string> bodyTex_M = new Dictionary<int, string>() { { 1, "gebräunt" }, { 0, "blass" }, { 2, "latino" }, { 3, "dunkelhäutig" }, /*{ 8, "Kettenhemd" }, { 9, "zerfetzte Kleidung" },*/ { 10, "tätowiert" } };
            private Dictionary<int, string> bodyTex_F = new Dictionary<int, string>() { { 5, "gebräunt" }, { 4, "blass" }, { 6, "latino" }, { 7, "dunkelhäutig" }, /*{ 11, "Fellkragen" }, { 12, "schwarzklein" }*/ };

            private Dictionary<int, string> headMeshes_M = new Dictionary<int, string>() { {0, "Ulu" }, //HUM_HEAD_BALD
                                                                                           {1, "Mulu" }, //HUM_HEAD_FATBALD
                                                                                           {2, "hoch" }, //HUM_HEAD_FIGHTER
                                                                                           {3, "mit Zopf" },    //HUM_HEAD_PONY
                                                                                           {4, "spitz" },       //HUM_HEAD_PSIONIC
                                                                                           {5, "mit Zopf" } };  //HUM_HEAD_THIEF

            private Dictionary<int, string> headMeshes_F = new Dictionary<int, string>() { {7, "Fremder" },
                                                                                {8, "tragen" },
                                                                                {9, "Ulu" },
                                                                                {10, "Mulu" },
                                                                                {11, "," },
                                                                                {12, "dann" },
                                                                                {13, "Fremder" },
                                                                                {14, "nicht" },
                                                                                {15, "sterben" },
                                                                                {16, "!" } };

            private Dictionary<int, string> fatness = new Dictionary<int, string>() { { 0, "durchschnittlich" }, { 25, "gedrungen" }, { 50, "stämmig" }, { 75, "stattlich" }, { 100, "massig" }, { -100, "mager" }, { -75, "schmächtig" }, { -50, "dünn" }, { -25, "schlank" } };

            private Dictionary<int, string> bodyHeight = new Dictionary<int, string>() { { 100, "durchschnittlich" }, { 105, "groß" }, { 110, "riesig" }, { 90, "klein" }, { 95, "kurz" } };
            private Dictionary<int, string> bodyWidth = new Dictionary<int, string>() { { 100, "100%" }, { 105, "105%" }, { 110, "110%" }, { 90, "90%" }, { 95, "95%" } };

            private Dictionary<int, string> formerClass = new Dictionary<int, string>() { { 0, "Bauer" }, { 1, "Jäger" }, { 2, "Soldat" }, { 3, "Söldner" }, { 4, "Novize" }, { 5, "Kräutersammler" } };

            private Dictionary<int, string> faces_M_P = new Dictionary<int, string>() { { 19, "Lester" }, { 39, "Gilbert" }, { 41, "Gesicht 1" }, { 42, "Drago" }, { 43, "Torrez" }, { 44, "Rodriguez" }, { 45, "Nek" }, { 46, "Gesicht 2" }, { 47, "Gesicht 3" }, { 48, "Gesicht 4" }, { 49, "Fletcher" }, { 50, "Gesicht 5" }, { 51, "Gesicht 6" }, { 52, "Cronos" }, { 53, "Nefarius" }, { 54, "Riordian" }, { 55, "Gravo" }, { 56, "Cutter" }, { 57, "Ulf" } };
            private Dictionary<int, string> faces_M_N = new Dictionary<int, string>() { { 0, "Gomez" }, { 1, "Scar" }, { 2, "Raven" }, { 3, "Bullit" }, { 5, "Corristo" }, { 6, "Milten" }, { 7, "Bloodwyn" }, { 9, "Y'Berion" }, { 10, "Pock" }, { 13, "Xardas" }, { 14, "Lares" }, { 16, "Drax" }, { 20, "Lee" }, { 21, "Torlof" }, { 22, "Mud" }, { 23, "Reislord" }, { 24, "Horatio" }, { 25, "Richter" }, { 26, "Cipher" }, { 27, "Homer" }, { 32, "Bartholo" }, { 33, "Snaf" }, { 34, "Mordrag" }, { 35, "Lefty" }, { 36, "Wolf" }, { 37, "Fingers" }, { 38, "Whistler" }, {159, "Fortuno"}, {160, "Greg"}, {161, "Pirat" },
                                                                                    { 58, "Arto" }, { 59, "Grey" }, { 60, "Old" }, { 61, "Lee ähnlich" }, { 62, "Skip" }, { 63, "Bart01" }, { 64, "Okyl" }, { 65, "Normal01" }, { 66, "Cord" }, { 67, "Olli" }, { 68, "Normal02" }, { 69, "Spassvogel" }, { 70, "Normal03" }, { 71, "Normal04" }, { 72, "Normal05" }, { 73, "Stone" }, { 74, "Normal06" }, { 75, "Erpresser" }, { 76, "Normal07" }, { 77, "Blade" }, { 78, "Normal08" }, { 79, "Normal14" }, { 80, "Sly" }, { 81, "Normal16" }, { 82, "Normal17" }, { 83, "Normal18" }, { 84, "Normal19" }, { 85, "Normal20" }, 
                                                                                    { 86, "NormalBart01" }, { 87, "NormalBart02" }, { 88, "NormalBart03" }, { 89, "NormalBart04" }, { 90, "NormalBart05" }, { 91, "NormalBart06" }, { 92, "Senyan" }, { 93, "NormalBart08" }, { 94, "NormalBart09" }, { 95, "NormalBart10" }, { 96, "NormalBart11" }, { 97, "NormalBart12" }, { 98, "Dexter" }, { 99, "Graham" }, { 100, "Dusty" }, { 101, "NormalBart16" }, { 102, "NormalBart17" }, { 103, "Huno" }, { 104, "Grim" }, { 105, "NormalBart20" }, { 106, "NormalBart21" }, { 107, "NormalBart22" }, { 108, "Jeremiah" },
                                                                                    { 109, "Ulbert" }, { 110, "Baal Netbek" }, { 111, "Herek" }, { 112, "Weak04" }, { 113, "Weak05" }, { 114, "Orry" }, { 115, "Asghan" }, { 116, "Markus" }, { 117, "Cipher alt" }, { 118, "Swiney" }, { 119, "Weak12" } };

            private Dictionary<int, string> faces_M_L = new Dictionary<int, string>() { { 8, "Scatty" }, { 15, "Ratford" }, { 29, "Ian" }, { 30, "Diego" }, { 40, "Jackal" }, { 120, "Gesicht 7" }, { 121, "Gesicht 8" }, { 122, "Gesicht 9" }, { 123, "Santino" }, { 124, "Quentin" }, { 125, "Gor Na Bar" }, { 126, "Gesicht 10" }, { 127, "Gesicht 11" }, { 128, "Rufus" } };
            private Dictionary<int, string> faces_M_B = new Dictionary<int, string>() { { 4, "Thorus" }, { 11, "Cor Angar" }, { 12, "Saturas" }, { 17, "Gorn" }, { 28, "Cavalorn" }, { 129, "Gesicht 12" }, { 130, "Pacho" }, { 131, "Silas" }, { 132, "Gesicht 13" }, { 133, "Kirgo" }, { 134, "Sharky" }, { 135, "Orik" }, { 136, "Kharim" } };

            private Dictionary<int, string> faces_F_P = new Dictionary<int, string>() { { 151, "MidBlonde" } };
            private Dictionary<int, string> faces_F_N = new Dictionary<int, string>() { { 137, "BlackHair" }, { 138, "Blondie" }, { 139, "BlondTattoo" }, { 140, "PinkHair" }, { 143, "HairAndCloth" }, { 144, "WhiteCloth" }, { 145, "GreyCloth" }, { 146, "Brown" }, { 147, "VlkBlonde" }, { 148, "BauBlonde" }, { 149, "YoungBlonde" }, { 150, "OldBlonde" }, { 152, "MidBauBlonde" }, { 153, "OldBrown" }, { 154, "Lilo" }, { 155, "Hure" }, { 156, "Anne" } };
            private Dictionary<int, string> faces_F_L = new Dictionary<int, string>() { { 141, "Charlotte" }, { 158, "Charlotte 2" } };
            private Dictionary<int, string> faces_F_B = new Dictionary<int, string>() { { 142, "RedLocks" }, { 157, "RedLocks 2" } };

            private Dictionary<int, string> voices_M = new Dictionary<int, string>() { { 1, "Moe" }, { 3, "Milten" }, { 4, "Lord Hagen" }, { 5, "Vatras" }, { 6, "Bennet" }, { 7, "Biff" }, { 8, "Lord Andre" }, { 9, "Matteo" }, { 10, "Garond" }, { 11, "Diego" }, { 12, "Gorn" }, { 13, "Lester" }, { 14, "Xardas" } };
            private Dictionary<int, string> voices_F = new Dictionary<int, string>() { { 16, "Nadja" }, { 17, "Thekla" } };
        }
    }
}
