using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Sumpfkraut.Visuals
{
    public enum HumBodyMeshs : byte
    {
        HUM_BODY_NAKED0,
        HUM_BODY_BABE0
    }

    public enum HumBodyTexs : byte
    {
        M_Pale = 0,
        M_Normal = 1,
        M_Latino = 2,
        M_Black = 3,
        F_Pale = 4,
        F_Normal = 5,
        F_Latino = 6,
        F_Black = 7,
        G1Hero = 8,
        G2Hero = 9,

        M_Tattooed = 10,
        F_Babe1 = 11,
        F_Babe2 = 12
    }

    public enum HumHeadMeshs : byte
    {
        HUM_HEAD_BALD,
        HUM_HEAD_FATBALD,
        HUM_HEAD_FIGHTER,
        HUM_HEAD_PONY,
        HUM_HEAD_PSIONIC,
        HUM_HEAD_THIEF,
        MAX_MALE,

        HUM_HEAD_BABE,
        HUM_HEAD_BABE1,
        HUM_HEAD_BABE2,
        HUM_HEAD_BABE3,
        HUM_HEAD_BABE4,
        HUM_HEAD_BABE5,
        HUM_HEAD_BABE6,
        HUM_HEAD_BABE7,
        HUM_HEAD_BABE8,
        HUM_HEAD_BABEHAIR,
        MAX_FEMALE
    }

    public enum HumHeadTexs : byte
    {
        Face_N_Gomez = 0,
        Face_N_Scar = 1,
        Face_N_Raven = 2,
        Face_N_Bullit = 3,	//zu lieb!
        Face_B_Thorus = 4,
        Face_N_Corristo = 5,
        Face_N_Milten = 6,
        Face_N_Bloodwyn = 7,	//zu lieb!
        Face_L_Scatty = 8,
        Face_N_YBerion = 9,
        Face_N_CoolPock = 10,
        Face_B_CorAngar = 11,
        Face_B_Saturas = 12,
        Face_N_Xardas = 13,
        Face_N_Lares = 14,
        Face_L_Ratford = 15,
        Face_N_Drax = 16,	//Buster
        Face_B_Gorn = 17,
        Face_N_Player = 18,
        Face_P_Lester = 19,
        Face_N_Lee = 20,
        Face_N_Torlof = 21,
        Face_N_Mud = 22,
        Face_N_Ricelord = 23,
        Face_N_Horatio = 24,
        Face_N_Richter = 25,
        Face_N_Cipher_neu = 26,
        Face_N_Homer = 27,	//Headmesh thief
        Face_B_Cavalorn = 28,
        Face_L_Ian = 29,
        Face_L_Diego = 30,
        Face_N_MadPsi = 31,
        Face_N_Bartholo = 32,
        Face_N_Snaf = 33,
        Face_N_Mordrag = 34,
        Face_N_Lefty = 35,
        Face_N_Wolf = 36,
        Face_N_Fingers = 37,
        Face_N_Whistler = 38,
        Face_P_Gilbert = 39,
        Face_L_Jackal = 40,

        //Pale
        Face_P_ToughBald = 41,
        Face_P_Tough_Drago = 42,
        Face_P_Tough_Torrez = 43,
        Face_P_Tough_Rodriguez = 44,
        Face_P_ToughBald_Nek = 45,
        Face_P_NormalBald = 46,
        Face_P_Normal01 = 47,
        Face_P_Normal02 = 48,
        Face_P_Normal_Fletcher = 49,
        Face_P_Normal03 = 50,
        Face_P_NormalBart01 = 51,
        Face_P_NormalBart_Cronos = 52,
        Face_P_NormalBart_Nefarius = 53,
        Face_P_NormalBart_Riordian = 54,
        Face_P_OldMan_Gravo = 55,
        Face_P_Weak_Cutter = 56,
        Face_P_Weak_Ulf_Wohlers = 57,

        //Normal
        Face_N_Important_Arto = 58,
        Face_N_ImportantGrey = 59,
        Face_N_ImportantOld = 60,
        Face_N_Tough_Lee_ähnlich = 61,
        Face_N_Tough_Skip = 62,
        Face_N_ToughBart01 = 63,
        Face_N_Tough_Okyl = 64,
        Face_N_Normal01 = 65,
        Face_N_Normal_Cord = 66,
        Face_N_Normal_Olli_Kahn = 67,
        Face_N_Normal02 = 68,
        Face_N_Normal_Spassvogel = 69,
        Face_N_Normal03 = 70,
        Face_N_Normal04 = 71,
        Face_N_Normal05 = 72,
        Face_N_Normal_Stone = 73,
        Face_N_Normal06 = 74,
        Face_N_Normal_Erpresser = 75,
        Face_N_Normal07 = 76,
        Face_N_Normal_Blade = 77,
        Face_N_Normal08 = 78,
        Face_N_Normal14 = 79,
        Face_N_Normal_Sly = 80,
        Face_N_Normal16 = 81,
        Face_N_Normal17 = 82,
        Face_N_Normal18 = 83,
        Face_N_Normal19 = 84,
        Face_N_Normal20 = 85,
        Face_N_NormalBart01 = 86,
        Face_N_NormalBart02 = 87,
        Face_N_NormalBart03 = 88,
        Face_N_NormalBart04 = 89,
        Face_N_NormalBart05 = 90,
        Face_N_NormalBart06 = 91,
        Face_N_NormalBart_Senyan = 92,
        Face_N_NormalBart08 = 93,
        Face_N_NormalBart09 = 94,
        Face_N_NormalBart10 = 95,
        Face_N_NormalBart11 = 96,
        Face_N_NormalBart12 = 97,
        Face_N_NormalBart_Dexter = 98,
        Face_N_NormalBart_Graham = 99,
        Face_N_NormalBart_Dusty = 100,
        Face_N_NormalBart16 = 101,
        Face_N_NormalBart17 = 102,
        Face_N_NormalBart_Huno = 103,
        Face_N_NormalBart_Grim = 104,
        Face_N_NormalBart20 = 105,
        Face_N_NormalBart21 = 106,
        Face_N_NormalBart22 = 107,
        Face_N_OldBald_Jeremiah = 108,
        Face_N_Weak_Ulbert = 109,
        Face_N_Weak_BaalNetbek = 110,
        Face_N_Weak_Herek = 111,
        Face_N_Weak04 = 112,
        Face_N_Weak05 = 113,
        Face_N_Weak_Orry = 114,
        Face_N_Weak_Asghan = 115,
        Face_N_Weak_Markus_Kark = 116,
        Face_N_Weak_Cipher_alt = 117,
        Face_N_NormalBart_Swiney = 118,
        Face_N_Weak12 = 119,

        //Latinos
        Face_L_ToughBald01 = 120,
        Face_L_Tough01 = 121,
        Face_L_Tough02 = 122,
        Face_L_Tough_Santino = 123,
        Face_L_ToughBart_Quentin = 124,
        Face_L_Normal_GorNaBar = 125,
        Face_L_NormalBart01 = 126,
        Face_L_NormalBart02 = 127,
        Face_L_NormalBart_Rufus = 128,

        //Black
        Face_B_ToughBald = 129,
        Face_B_Tough_Pacho = 130,
        Face_B_Tough_Silas = 131,
        Face_B_Normal01 = 132,
        Face_B_Normal_Kirgo = 133,
        Face_B_Normal_Sharky = 134,
        Face_B_Normal_Orik = 135,
        Face_B_Normal_Kharim = 136,

        // ------ Gesichter für Frauen ------

        FaceBabe_N_BlackHair = 137,
        FaceBabe_N_Blondie = 138,
        FaceBabe_N_BlondTattoo = 139,
        FaceBabe_N_PinkHair = 140,
        FaceBabe_L_Charlotte = 141,
        FaceBabe_B_RedLocks = 142,
        FaceBabe_N_HairAndCloth = 143,
        //
        FaceBabe_N_WhiteCloth = 144,
        FaceBabe_N_GreyCloth = 145,
        FaceBabe_N_Brown = 146,
        FaceBabe_N_VlkBlonde = 147,
        FaceBabe_N_BauBlonde = 148,
        FaceBabe_N_YoungBlonde = 149,
        FaceBabe_N_OldBlonde = 150,
        FaceBabe_P_MidBlonde = 151,
        FaceBabe_N_MidBauBlonde = 152,
        FaceBabe_N_OldBrown = 153,
        FaceBabe_N_Lilo = 154,
        FaceBabe_N_Hure = 155,
        FaceBabe_N_Anne = 156,
        FaceBabe_B_RedLocks2 = 157,
        FaceBabe_L_Charlotte2 = 158,


        //-----------------ADD ON---------------------------------
        Face_N_Fortuno = 159,

        //Piraten
        Face_P_Greg = 160,
        Face_N_Pirat01 = 161,
        Face_N_ZombieMud = 162
    }

    public enum HumVoices : byte
    {
        None = 0,
        Moe = 1,
        Milten = 3,
        Hagen = 4,
        Vatras = 5,
        Bennet = 6,
        Biff = 7,
        Andre = 8,
        Matteo = 9,
        Garond = 10,
        Diego = 11,
        Gorn = 12,
        Lester = 13,
        Xardas = 14,

        Hero = 15,

        Nadja = 16,
        Thekla = 17
    }
}
