using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.Network;
using WinApi.User.Enumeration;

namespace GUC.Scripts.Sumpfkraut.Menus.MainMenus
{
    /*class CharCreationMenu : GUCMainMenu
    {
        public override void Open()
        {
            throw new NotSupportedException("The character creation menu needs a slot number.");
        }

        public void Open(int slotNum)
        {
            if (slotNum >= 0 && slotNum < GUC.Network.AccCharInfo.Max_Slots)
            {
                base.Open();
                Character.Info.SlotNum = slotNum;
            }
        }

        #region Character <-> Choices switching on Tab
        MainMenuItem lastChoice;
        MainMenuItem lastOther;
        public override void KeyPressed(VirtualKeys key)
        {
            if (key == VirtualKeys.Tab)
            {
                if (items[cursor] is MainMenuChoice)
                {
                    lastChoice = items[cursor];
                    SetCursor(lastOther == null ? Character : lastOther);
                }
                else
                {
                    lastOther = items[cursor];
                    SetCursor(lastChoice == null ? bodymesh : lastChoice);
                }
            }
            else
            {
                base.KeyPressed(key);
            }
        }
        #endregion

        MainMenuTextBox Name;
        MainMenuCharacter Character;
        MainMenuChoice bodymesh;
        MainMenuChoice bodytex;
        MainMenuChoice headmesh;
        MainMenuChoice headtex;
        MainMenuChoice fatness;
        MainMenuChoice bodyheight;
        MainMenuChoice bodywidth;
        MainMenuChoice voice;
        MainMenuChoice formerclass;

        protected override void OnCreate()
        {
            Back.SetBackTexture("Menu_CharacterCreation.tga");

            Name = AddTextBox("Name:", "Wähle den Namen deines Charakters.", 150, 15, 200, 45, MoveCursor);
            Name.AllowSymbols = false;

            Character = AddCharacter("", -60, 50, 533, 400);
            Character.Info.BodyMesh = (int)HumBodyMeshs.HUM_BODY_BABE0;
            AddButton("Start", "Mit dem Charakter in die Welt starten.", 160, 425, CreateNewCharacter);

            const int offsetX = 415;
            const int offsetY = 27;
            const int dist = 50;

            bodymesh = AddChoice("Geschlecht", "Wähle das Geschlecht deines Charakters.", offsetX, offsetY + dist * 0, c_BodyMesh, false, MoveCursor, ChangedVisual);
            bodytex = AddChoice("Haut", "Wähle die Haut deines Charakters.", offsetX, offsetY + dist * 1, c_BodyTex_M, false, MoveCursor, ChangedVisual);
            headmesh = AddChoice("Kopf", "Wähle die Kopfform deines Charakters.", offsetX, offsetY + dist * 2, c_HeadMeshes_M, true, MoveCursor, ChangedVisual);
            headtex = AddChoice("Gesicht", "Wähle das Gesicht deines Charakters.", offsetX, offsetY + dist * 3, c_Faces_M_N, true, MoveCursor, ChangedVisual);
            fatness = AddChoice("Statur", "Wähle die Statur deines Charakters.", offsetX, offsetY + dist * 4, c_Fatness, false, MoveCursor, ChangedVisual);
            bodyheight = AddChoice("Größe", "Wähle die Körpergröße deines Charakters.", offsetX, offsetY + dist * 5, c_BodyHeight, false, MoveCursor, ChangedVisual);
            bodywidth = AddChoice("Breite", "Wähle die Körperbreite deines Charakters.", offsetX, offsetY + dist * 6, c_BodyWidth, false, MoveCursor, ChangedVisual);
            voice = AddChoice("Stimme", "Wähle die Stimme deines Charakters.", offsetX, offsetY + dist * 7, c_Voices_M, true, PlayVoice, PlayVoice);
            formerclass = AddChoice("ehem. Beruf", "Wähle den Beruf den dein Charakter früher tätigte.", offsetX, offsetY + dist * 8, c_FormerClass, false, MoveCursor, null);

            OnEscape = GUCMenus.CharSelection.Open;

            ChangedVisual();
        }

        void CreateNewCharacter()
        {
            AccCharInfo info = Character.Info;
            info.Name = Name.Input;
            if (info.Name.Length < 2)
            {
                SetCursor(Name);
                SetHelpText("Dein Charaktername ist zu kurz.");
                return;
            }

            info.Voice = voice.Choice;
            info.FormerClass = formerclass.Choice;
            
            Network.Messages.AccountMessage.CreateNewCharacter(info);
        }

        Random random = new Random();
        void PlayVoice()
        {
            zCSndSys_MSS ss = zCSndSys_MSS.SoundSystem(Program.Process);
            Voices sfx = (Voices)random.Next(0,(int)Voices.MAX_G2_VOICES);
            using (zString z = zString.Create(Program.Process, String.Format("SVM_{0}_{1}.WAV",voice.Choice, sfx)))
            {
                ss.PlaySound(ss.LoadSoundFX(z), 0, 0, 0.65f * Program.GetSoundVol());
            }
        }

        void ChangedVisual() //FIXME: Fleischzoepfe verhindern!
        {
            AccCharInfo info = Character.Info;
            if (bodymesh.Choice == (int)HumBodyMeshs.HUM_BODY_NAKED0)
            {
                bodytex.Choices = c_BodyTex_M;
                headmesh.Choices = c_HeadMeshes_M;
                voice.Choices = c_Voices_M;
                if (bodytex.Choice == 0)
                {
                    headtex.Choices = c_Faces_M_P;
                }
                else if (bodytex.Choice == 1 || bodytex.Choice == 10)
                {
                    headtex.Choices = c_Faces_M_N;
                }
                else if (bodytex.Choice == 2)
                {
                    headtex.Choices = c_Faces_M_L;
                }
                else if (bodytex.Choice == 3)
                {
                    headtex.Choices = c_Faces_M_B;
                }
            }
            else
            {
                bodytex.Choices = c_BodyTex_F;
                headmesh.Choices = c_HeadMeshes_F;
                voice.Choices = c_Voices_F;
                if (bodytex.Choice == 4)
                {
                    headtex.Choices = c_Faces_F_P;
                }
                else if (bodytex.Choice == 5)
                {
                    headtex.Choices = c_Faces_F_N;
                }
                else if (bodytex.Choice == 6)
                {
                    headtex.Choices = c_Faces_F_L;
                }
                else if (bodytex.Choice == 7)
                {
                    headtex.Choices = c_Faces_F_B;
                }
            }

            info.BodyMesh = bodymesh.Choice;
            info.BodyTex = bodytex.Choice;
            info.HeadMesh = headmesh.Choice;
            info.HeadTex = headtex.Choice;
            info.Fatness = fatness.Choice/100.0f;
            info.BodyHeight = bodyheight.Choice / 100.0f;
            info.BodyWidth = bodywidth.Choice / 100.0f;

            Character.Info = info; //update visual
        }

        #region Character Templates
        static Dictionary<int, string> c_BodyMesh = new Dictionary<int, string>() 
        { 
            { (int)HumBodyMeshs.HUM_BODY_NAKED0, "männlich" }, 
            { (int)HumBodyMeshs.HUM_BODY_BABE0, "weiblich" } 
        };

        static Dictionary<int, string> c_BodyTex_M = new Dictionary<int, string>() 
        { 
            { 1, "gebräunt" }, 
            { 0, "blass" }, 
            { 2, "latino" }, 
            { 3, "dunkelhäutig" }, 
            /*{ 8, "Kettenhemd" }, 
              { 9, "zerfetzte Kleidung" },*//* 
            { 10, "tätowiert" } 
        };

        static Dictionary<int, string> c_BodyTex_F = new Dictionary<int, string>() 
        { 
            { 5, "gebräunt" }, 
            { 4, "blass" }, 
            { 6, "latino" }, 
            { 7, "dunkelhäutig" } 
            /*{ 11, "Fellkragen" }, 
             { 12, "schwarzklein" }*//* 
        };

        static Dictionary<int, string> c_HeadMeshes_M = new Dictionary<int, string>() 
        { 
            {(int)HumHeadMeshs.HUM_HEAD_BALD, "Ulu" },
            {(int)HumHeadMeshs.HUM_HEAD_FATBALD, "Mulu" },
            {(int)HumHeadMeshs.HUM_HEAD_FIGHTER, "hoch" },
            {(int)HumHeadMeshs.HUM_HEAD_PONY, "mit Zopf" },
            {(int)HumHeadMeshs.HUM_HEAD_PSIONIC, "spitz" }, 
            {(int)HumHeadMeshs.HUM_HEAD_THIEF, "mit Zopf2" }
        };

        static Dictionary<int, string> c_HeadMeshes_F = new Dictionary<int, string>() 
        { 
            {(int)HumHeadMeshs.HUM_HEAD_BABE, "Fremder" },
            {(int)HumHeadMeshs.HUM_HEAD_BABE1, "tragen" },
            {(int)HumHeadMeshs.HUM_HEAD_BABE2, "Ulu" },
            {(int)HumHeadMeshs.HUM_HEAD_BABE3, "Mulu" },
            {(int)HumHeadMeshs.HUM_HEAD_BABE4, "," },
            {(int)HumHeadMeshs.HUM_HEAD_BABE5, "dann" },
            {(int)HumHeadMeshs.HUM_HEAD_BABE6, "Fremder" },
            {(int)HumHeadMeshs.HUM_HEAD_BABE7, "nicht" },
            {(int)HumHeadMeshs.HUM_HEAD_BABE8, "sterben" },
            {(int)HumHeadMeshs.HUM_HEAD_BABEHAIR, "!" } 
        };

        static Dictionary<int, string> c_Fatness = new Dictionary<int, string>() 
        { 
            { 0, "durchschnittlich" }, 
            { 25, "gedrungen" }, 
            { 50, "stämmig" }, 
            { 75, "stattlich" }, 
            { 100, "massig" }, 
            { -100, "mager" }, 
            { -75, "schmächtig" }, 
            { -50, "dünn" }, 
            { -25, "schlank" } 
        };

        static Dictionary<int, string> c_BodyHeight = new Dictionary<int, string>() 
        { 
            { 100, "durchschnittlich" }, 
            { 105, "groß" }, 
            { 110, "riesig" }, 
            { 90, "klein" }, 
            { 95, "kurz" } 
        };

        static Dictionary<int, string> c_BodyWidth = new Dictionary<int, string>() 
        { 
            { 100, "100%" }, 
            { 105, "105%" }, 
            { 110, "110%" }, 
            { 90, "90%" }, 
            { 95, "95%" } 
        };

        static Dictionary<int, string> c_FormerClass = new Dictionary<int, string>() 
        { 
            { (int)FormerClass.Farmer, "Bauer" }, 
            { (int)FormerClass.Hunter, "Jäger" }, 
            { (int)FormerClass.Soldier, "Soldat" }, 
            { (int)FormerClass.Mercenary, "Söldner" },
            { (int)FormerClass.Novice, "Novize" }, 
            { (int)FormerClass.HerbGatherer, "Kräutersammler" } 
        };

        static Dictionary<int, string> c_Faces_M_P = new Dictionary<int, string>() 
        { 
            { 19, "Lester" }, { 39, "Gilbert" }, { 41, "Gesicht 1" }, { 42, "Drago" }, { 43, "Torrez" }, 
            { 44, "Rodriguez" }, { 45, "Nek" }, { 46, "Gesicht 2" }, { 47, "Gesicht 3" }, { 48, "Gesicht 4" }, 
            { 49, "Fletcher" }, { 50, "Gesicht 5" }, { 51, "Gesicht 6" }, { 52, "Cronos" }, { 53, "Nefarius" }, 
            { 54, "Riordian" }, { 55, "Gravo" }, { 56, "Cutter" }, { 57, "Ulf" } 
        };

        static Dictionary<int, string> c_Faces_M_N = new Dictionary<int, string>() 
        { 
            { 0, "Gomez" }, { 1, "Scar" }, { 2, "Raven" }, { 3, "Bullit" }, { 5, "Corristo" }, 
            { 6, "Milten" }, { 7, "Bloodwyn" }, { 9, "Y'Berion" }, { 10, "Pock" }, { 13, "Xardas" }, 
            { 14, "Lares" }, { 16, "Drax" }, { 20, "Lee" }, { 21, "Torlof" }, { 22, "Mud" }, 
            { 23, "Reislord" }, { 24, "Horatio" }, { 25, "Richter" }, { 26, "Cipher" }, { 27, "Homer" }, 
            { 32, "Bartholo" }, { 33, "Snaf" }, { 34, "Mordrag" }, { 35, "Lefty" }, { 36, "Wolf" }, 
            { 37, "Fingers" }, { 38, "Whistler" }, {159, "Fortuno"}, {160, "Greg"}, {161, "Pirat" },
            { 58, "Arto" }, { 59, "Grey" }, { 60, "Old" }, { 61, "Lee ähnlich" }, { 62, "Skip" }, 
            { 63, "Bart01" }, { 64, "Okyl" }, { 65, "Normal01" }, { 66, "Cord" }, { 67, "Olli" }, 
            { 68, "Normal02" }, { 69, "Spassvogel" }, { 70, "Normal03" }, { 71, "Normal04" }, { 72, "Normal05" }, 
            { 73, "Stone" }, { 74, "Normal06" }, { 75, "Erpresser" }, { 76, "Normal07" }, { 77, "Blade" }, 
            { 78, "Normal08" }, { 79, "Normal14" }, { 80, "Sly" }, { 81, "Normal16" }, { 82, "Normal17" }, 
            { 83, "Normal18" }, { 84, "Normal19" }, { 85, "Normal20" }, { 86, "NormalBart01" }, { 87, "NormalBart02" }, 
            { 88, "NormalBart03" }, { 89, "NormalBart04" }, { 90, "NormalBart05" }, { 91, "NormalBart06" }, { 92, "Senyan" }, 
            { 93, "NormalBart08" }, { 94, "NormalBart09" }, { 95, "NormalBart10" }, { 96, "NormalBart11" }, { 97, "NormalBart12" },
            { 98, "Dexter" }, { 99, "Graham" }, { 100, "Dusty" }, { 101, "NormalBart16" }, { 102, "NormalBart17" }, 
            { 103, "Huno" }, { 104, "Grim" }, { 105, "NormalBart20" }, { 106, "NormalBart21" }, { 107, "NormalBart22" }, 
            { 108, "Jeremiah" }, { 109, "Ulbert" }, { 110, "Baal Netbek" }, { 111, "Herek" }, { 112, "Weak04" }, 
            { 113, "Weak05" }, { 114, "Orry" }, { 115, "Asghan" }, { 116, "Markus" }, { 117, "Cipher alt" }, 
            { 118, "Swiney" }, { 119, "Weak12" } //sind noch nicht alle
        };

        static Dictionary<int, string> c_Faces_M_L = new Dictionary<int, string>() 
        { 
            { 8, "Scatty" }, { 15, "Ratford" }, { 29, "Ian" }, { 30, "Diego" }, { 40, "Jackal" }, 
            { 120, "Gesicht 7" }, { 121, "Gesicht 8" }, { 122, "Gesicht 9" }, { 123, "Santino" }, { 124, "Quentin" }, 
            { 125, "Gor Na Bar" }, { 126, "Gesicht 10" }, { 127, "Gesicht 11" }, { 128, "Rufus" } 
        };

        static Dictionary<int, string> c_Faces_M_B = new Dictionary<int, string>() 
        { 
            { 4, "Thorus" }, { 11, "Cor Angar" }, { 12, "Saturas" }, { 17, "Gorn" }, { 28, "Cavalorn" }, 
            { 129, "Gesicht 12" }, { 130, "Pacho" }, { 131, "Silas" }, { 132, "Gesicht 13" }, { 133, "Kirgo" }, 
            { 134, "Sharky" }, { 135, "Orik" }, { 136, "Kharim" } 
        };

        static Dictionary<int, string> c_Faces_F_P = new Dictionary<int, string>() 
        {
            { 151, "MidBlonde" } 
        };

        static Dictionary<int, string> c_Faces_F_N = new Dictionary<int, string>() 
        {
            { 137, "BlackHair" }, { 138, "Blondie" }, { 139, "BlondTattoo" }, { 140, "PinkHair" }, { 143, "HairAndCloth" }, 
            { 144, "WhiteCloth" }, { 145, "GreyCloth" }, { 146, "Brown" }, { 147, "VlkBlonde" }, { 148, "BauBlonde" }, 
            { 149, "YoungBlonde" }, { 150, "OldBlonde" }, { 152, "MidBauBlonde" }, { 153, "OldBrown" }, { 154, "Lilo" }, 
            { 155, "Hure" }, { 156, "Anne" } 
        };

        static Dictionary<int, string> c_Faces_F_L = new Dictionary<int, string>() 
        { 
            { 141, "Charlotte" }, { 158, "Charlotte 2" } 
        };

        static Dictionary<int, string> c_Faces_F_B = new Dictionary<int, string>() 
        { 
            { 142, "RedLocks" }, { 157, "RedLocks 2" } 
        };

        static Dictionary<int, string> c_Voices_M = new Dictionary<int, string>() 
        { 
            { 1, "Moe" }, { 3, "Milten" }, { 4, "Lord Hagen" }, { 5, "Vatras" }, { 6, "Bennet" }, 
            { 7, "Biff" }, { 8, "Lord Andre" }, { 9, "Matteo" }, { 10, "Garond" }, { 11, "Diego" }, 
            { 12, "Gorn" }, { 13, "Lester" }, { 14, "Xardas" } 
        };

        static Dictionary<int, string> c_Voices_F = new Dictionary<int, string>() 
        { 
            { 16, "Nadja" }, { 17, "Thekla" } 
        };
        #endregion
    }*/
}
