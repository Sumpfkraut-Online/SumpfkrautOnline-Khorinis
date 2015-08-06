using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Enumeration
{
    /** 
    * Types of network messages (data) which can be exchanged between clients and server.
    */
    public enum NetworkID : byte
    {
        //login stuff
        ConnectionMessage,

        AccountCreationMessage,
        AccountLoginMessage,
        AccountCharCreationMessage,
        AccountCharLoginMessage,
        AccountErrorMessage,

        //player stuff
        PlayerControlMessage,
        PlayerPickUpItemMessage,

        //world
        WorldVobSpawnMessage,
        WorldVobDeleteMessage,
        WorldNPCSpawnMessage,
        WorldItemSpawnMessage,

        //vobs
        VobPosDirMessage,

        //npcs
        NPCAniStartMessage,
        NPCAniStopMessage,
        NPCEquipMessage,
        NPCFoodMessage,

        NPCStateMessage,
        NPCAttackMessage,
        NPCHitMessage,

        NPCWeaponStateMessage,
        NPCTalentMessage,

        //item instances
        ItemInstanceMessage,

        //inventory
        InventoryAddMessage, //add iteminstance to player inventory
        InventoryRemoveMessage, //remove
        InventoryDropItemMessage,
        InventoryUseItemMessage,

        //controller stuff
        ControlAddVobMessage,
        ControlRemoveVobMessage
    }

    public enum NPCState : byte
    {
        Stand,

        MoveForward,
        MoveBackward,
        MoveLeft,
        MoveRight,

        Jump,

        AttackForward,
        AttackLeft,
        AttackRight,
        AttackRun,
        Parry,
        DodgeBack,
    }

    public enum TradeStatus : byte
    {
        Request,
        Accept,
        Break,
        SelfOfferItem,
        SelfRemoveItem,
        OtherOfferItem,
        OtherRemoveItem
    }

    public enum ChatTextType : byte
    {
        //RP
        Say,
        Shout,
        Whisper,
        Ambient,
        RPGlobal,
        RPEvent,
        MAX_RP,

        //OOC
        OOC,
        OOCGlobal,
        PM,
        OOCEvent,
        MAX_OOC,

        //Add to both
        _Error,
        _Hint

    }

    public enum Animations : ushort // Animation and Voices
    {
        INVALID,

        // Animation's
        T_STAND_2_HGUARD,
        T_HGUARD_2_STAND,
        T_STAND_2_LGUARD,
        T_LGUARD_2_STAND,
        R_LEGSHAKE,
        S_SIT,
        S_SLEEPGROUND,
        T_PEE,
        T_STAND_2_PEE,
        T_PEE_2_STAND,
        T_PLUNDER,
        T_TRADEITEM, // Geben


        T_STAND_2_SLEEPGROUND,
        T_SLEEPGROUND_2_STAND,
        T_RAKE_STAND_2_S0,
        T_RAKE_S0_2_STAND,
        T_REPAIR_STAND_2_S0,
        T_REPAIR_S0_2_STAND,
        T_BROOM_STAND_2_S0,
        T_BROOM_S0_2_STAND,
        T_BRUSH_STAND_2_S0,
        T_BRUSH_S0_2_STAND,
        T_HORN_STAND_2_S0,
        T_HORN_S0_2_STAND,
        T_LUTE_STAND_2_S0,
        T_LUTE_S0_2_STAND,
        T_STAND_2_WASH,
        T_WASH_2_STAND,
        T_STAND_2_PRAY,
        T_PRAY_2_STAND,
        T_IDOL_STAND_2_S0,
        T_IDOL_S0_2_STAND,
        T_IDOL_S0_2_S1,
        T_IDOL_S1_2_S0,
        T_INNOS_STAND_2_S0,
        T_INNOS_S0_2_STAND,
        T_INNOS_S0_2_S1,
        T_INNOS_S1_2_S0,
        T_STAND_2_WATCHFIGHT,
        T_WATCHFIGHT_2_STAND,
        T_STAND_2_SIT,
        T_SIT_2_STAND,

        T_RAKE_S0_2_S1,
        T_RAKE_S1_2_STAND,
        T_REPAIR_S0_2_S1,
        T_REPAIR_S1_2_STAND,
        T_BROOM_S0_2_S1,
        T_BROOM_S1_2_STAND,
        T_BRUSH_S0_2_S1,
        T_BRUSH_S1_2_STAND,
        T_HORN_S0_2_S1,
        T_HORN_S1_2_STAND,
        T_LUTE_S0_2_S1,
        T_LUTE_S1_2_STAND,
        // "Kratzen"
        R_SCRATCHEGG,
        R_SCRATCHHEAD,
        R_SCRATCHLSHOULDER,
        R_SCRATCHRSHOULDER,

        // "Beten"
        S_IDOL_S1,
        S_INNOS_S1,
        S_PRAY,
        T_PRAY_RANDOM,

        // "Grüßen"
        T_GREETCOOL,
        T_GREETLEFT,
        T_GREETGRD,
        T_GREETNOV,
        T_GREETRIGHT,

        // "Gesten"
        T_BORINGKICK,
        T_SEARCH,
        T_YES,
        T_DONTKNOW,
        T_NO,

        // "Arme"
        S_WATCHFIGHT,
        T_WATCHFIGHT_OHNO,
        T_WATCHFIGHT_YEAH,
        T_FORGETIT,
        T_GETLOST,
        T_GETLOST2,
        T_IGETYOU,

        // "Tanzen"
        T_DANCE_01,
        T_DANCE_02,
        T_DANCE_03,
        T_DANCE_04,
        T_DANCE_05,
        T_DANCE_06,
        T_DANCE_07,
        T_DANCE_08,
        T_DANCE_09,

        // "Magie wirken"
        T_PRACTICEMAGIC,
        T_PRACTICEMAGIC2,
        T_PRACTICEMAGIC3,
        T_PRACTICEMAGIC4,
        T_PRACTICEMAGIC5,

        // "Essen und Trinken"
        T_FOODHUGE_RANDOM_1,
        T_FOOD_RANDOM_1,
        T_FOOD_RANDOM_2,
        T_POTION_RANDOM_1,
        T_POTION_RANDOM_2,
        T_POTION_RANDOM_3,
        T_RICE_RANDOM_1,
        T_RICE_RANDOM_2,
        S_RICE_S0,

        // "Ambiente"
        T_1HSFREE, // Waffentraining
        T_1HSINSPECT, // Waffe inspizieren
        S_HORN_S1, // Horn blasen 
        S_LUTE_S1, // Gitarre spielen
        S_WASH, // sich Waschen?

        // Arbeiten
        S_RAKE_S1, // Harken
        S_REPAIR_S1, // Hämmern
        S_BROOM_S1, // Fegen
        S_BRUSH_S1, // wischen

        // beim Sitzen
        R_CHAIR_RANDOM_1,
        R_CHAIR_RANDOM_2,
        R_CHAIR_RANDOM_3,
        R_CHAIR_RANDOM_4,

        // beim Stehen
        // -> Hände in der Hüfte
        T_HGUARD_LOOKAROUND,
        T_HGUARD_NOENTRY,
        T_HGUARD_GREET,
        T_HGUARD_COMEIN,
        // -> Arme verscränkt
        T_LGUARD_NOENTRY,
        T_LGUARD_GREET,
        T_LGUARD_COMEIN,
        T_LGUARD_ALLRIGHT,
        T_LGUARD_CHANGELEG,
        T_LGUARD_SCRATCH,
        T_LGUARD_STRETCH,
    }

    public enum Voices : ushort // Animation and Voices
    {
        // Gothic 2 Voices
        ABS_GOOD, //Das ist gut.
        ADDON_ADDON_NOARMOR_BDT, //Hast ja nichtmal ne Rüstung. Verschwinde!
        ADDON_DIEBANDIT, //Stirb Bandit!
        ADDON_DIRTYPIRATE, //Dreckiger Pirat!
        ADDON_WRONGARMOR, //Das ist nicht deine Kleidung. Ich rede nicht mit dir.
        ADDON_WRONGARMOR_KDF,
        ADDON_WRONGARMOR_MIL,
        ADDON_WRONGARMOR_SLD,
        ALARM,
        ATTACK_CRIME, //Mit miesen Schlägern rede ich nicht
        AWAKE, //Gähnen
        CHEERFRIEND01, //Kampfgejubel
        CHEERFRIEND02,
        CHEERFRIEND03,
        DIEENEMY, //Ich mach dich fertig!
        DIEMONSTER, //Da ist wieder eins von diesen Drecksviechern.
        DIESTUPIDBEAST, //Hier kommen keine Viecher rein.
        DIRTYTHIEF, //Na warte du dreckiger Dieb!
        ENEMYKILLED, //Das hast du verdient, Mistkerl!
        GETOUTOFHERE, //Raus hier!
        GETOUTOFMYBED, //Raus aus meinem Bett!
        GETUPANDBEGONE, //Und jetzt sieh zu dass du hier verschwindest!
        GOODKILL, //Ja! Mach das Schwein fertig!
        GOODMONSTERKILL, //Gut gemacht! Ein Drecksvieh weniger.
        GOODVICTORY, //Dem hast dus gezeigt!
        GUARDS, //Wache!
        HANDSOFF, //Finger weg da!
        HELP, //Hilfe!
        IGETYOUSTILL, //Krieg ich dich doch noch!
        ISAIDSTOPMAGIC, //Weg mit der Magie, hörst du schlecht?
        ISAIDWEAPONDOWN, //Steck endlich die scheiß Waffe weg!
        ITAKEYOURWEAPON, //Die Waffe nehm ich mal besser an mich.
        ITOOKYOURGOLD, //Danke für das Gold, du Held!
        KILLENEMY, //Stirb, Mistkerl!
        KILLMURDERER, //Stirb, Mörder!
        LOOKINGFORTROUBLEAGAIN, //Hast du immer noch nicht genug?
        MILGREETINGS, //Für den König!
        MONSTERKILLED, //Ein Mistvieh weniger.
        NEVERENTERROOMAGAIN, //Und lass dich ja nie wieder da drin erwischen!
        NEVERHITMEAGAIN, //Leg dich nie wieder mit mir an!
        NEXTTIMEYOUREINFORIT, //Das nächste mal werden wir ja sehen...
        NOLEARNPOINTS, //Komm wieder wenn du mehr Erfahrung hast.
        NOLEARNOVERPERSONALMAX, //Du verlangst mehr von mir als ich dir beibringen kann.
        NOLEARNYOUREBETTER, //Ich kann dir nichts mehr beibringen, du bist schon zu gut.
        NOTBAD, //Nicht schlecht!
        NOTNOW, //Lass mich in Ruhe.
        OHMYGODHESDOWN, //Mein Gott, wie brutal!
        OHMYGODITSAFIGHT, //Mein Gott, ein Kampf!
        OHMYHEAD, //Oaa mein Schädel!
        OOH01, //Kampfgejubel
        OOH02,
        OOH03,
        PALGREETINGS, //Für Innos!
        RUMFUMMLERDOWN, //Lass in Zukunft die Finger von Sachen an denen du nichts zu suchen hast!
        RUNAWAY, //Scheiße, nichts wie weg!
        RUNCOWARD, //Ja, renn so schnell wie du kannst!
        SHEEPATTACKERDOWN, //Tu das nie wieder, das sind unsere Schafe!
        SHEEPKILLER, //Der Mistkerl schlachtet unsere Schafe!
        SHEEPKILLER_CRIME, //Einfach unsere Schafe zu schlachten, mach dass du hier wegkommst!
        SHEEPKILLERMONSTER, //Das Mistvieh frisst unsere Schafe!
        SHITNOGOLD, //Du arme Wurst hast ja nichtmal Gold dabei.
        SMALLTALK01, //Gerede
        SMALLTALK28, //Klostergerede
        SMALLTALK30,
        SPAREME, //Tu mir nichts!
        STOPMAGIC, //Hör auf mit dieser Magiescheiße!
        STUPIDBEASTKILLED, //So ein saublödes Vieh!
        THEFT_CRIME, //Lass mich in Ruhe du mieser kleiner Dieb!
        THENIBEATYOUOUTOFHERE, //Dann muss ich dich eben rausprügeln!
        THEREISNOFIGHTINGHERE, //Hier wird nicht gekämpft, lass dir das eine Lehre sein!
        THEREISAFIGHT, //Aah! Ein Kampf!
        THIEFDOWN, //Versuch nie wieder mich zu bestehlen!
        TOUGHGUY_ATTACKLOST, //Okay okay, du bist der bessere von uns beiden. Was willst du?
        TOUGHGUY_ATTACKWON, //Ich nehme an du hast mittlerweile begriffen wer von uns der stärkere ist. Was willst du?
        TOUGHGUY_PLAYERATTACK, //Ich dachte du willst dich mit mir anlegen. Willst doch lieber reden?
        WEAPONDOWN, //Steck die Waffe weg!
        WEATHER, //So ein Mistwetter!
        WHATAREYOUDOING, //Pass auf, nochmal und ich verpass dir eine!
        WHATDIDYOUDOINTHERE, //Was hattest du da drin zu suchen?
        WHATSTHISSUPPOSEDTOBE, //Was schleichst du hier rum?
        WHATWASTHAT, //Was war das?
        WHERETO, //Wo willst du hin?
        WHYAREYOUINHERE, //Was suchst du hier? Geh!
        WILLYOUSTOPFIGHTING, //Wollt ihr wohl damit aufhören!
        WISEMOVE, //Kluges Kerlchen!
        YESGOOUTOFHERE, //Ja, mach dass du wegkommst!
        YOUASKEDFORIT, //Du hast es so gewollt!
        YOUBETTERSHOULDHAVELISTENED, //Du hättest auf mich hören sollen!
        YOUDAREHITME, //Na warte, du Mistkerl!
        YOUDISTURBEDMYSLUMBER, //Verdammt, was ist los? (geweckt)
        YOULEARNEDSOMETHING, //Siehst du, du bist schon besser geworden.
        YOUMURDERER, //Mörder!
        MAX_G2_VOICES,

        // Gothic 1 Voices
        // ALARM, //Alarm!
        //AWAKE, //Gähnen
        BEHINDYOU, //Hinter dir!
        CHEERFIGHT, //Kampfgejubel
        CHEERFIREND,
        //DIEMONSTER, //Stirb, Monster!
        DIEMORTALENEMY, //Jetzt musst du dran glauben!
        //DIRTYTHIEF, //Ich mach dich fertig, du Dieb!
        FRIENDLYGREETINGS, //Hallo
        //GETOUTOFHERE, //Verschwinde hier!
        GETTHINGSRIGHT, //DAS wieder hinzubiegen wird nicht einfach.
        GIVEITTOME, //Her mit dem Ding!
        //HANDSOFF, //Flossen weg!
        HEDEFEATEDHIM, //Der hat genug!
        HEDESERVEDIT, //Geschieht ihm recht!
        HEKILLEDHIM, //Einfach einen kaltmachen, du hast jetzt echt ein Problem!
        //HELP, //Hilfe
        HEYHEYHEY, //Kampfgejubel
        HEYYOU, //Hey du!
        INTRUDERALERT, //Alarm! Eindringling!
        //ISAIDSTOPMAGIC, //Willst du Schläge? Hör sofort damit auf!
        //ISAIDWEAPONDOWN, //Steck endlich die scheiß Waffe weg!
        ISAIDWHATDOYOUWANT, //Was willst du?
        ITAKEYOUWEAPON, //Nette Waffe. Her damit!
        ITWASAGOODFIGHT, //Schöner Kampf!
        IWILLTEACHYOURESPECTFORFOREIGNPROPERTY, //Ich hab dich gewarnt die Drecksfinger von meinen Sachen zu lassen!
        LETSFORGETOURLITTLEFIGHT, //Lass uns den kleinen Streit vergessen, ok?
        LOOKAWAY, //Ich hab nichts gesehen!
        //LOOKINGFORTROUBLEAGAIN, //Suchst du wieder Streit?
        MAGEGREETINGS, //Magie zu Ehren!
        MAKEWAY, //Lass mich mal vorbei!
        NEVERTRYTHATAGAIN, //Noch einmal und du kannst was erleben!
        //NOLEARNPOINTS, //Komm wieder wenn du mehr Erfahrung hast.
        NOLEARNOVERMAX, //Du bist am Ende deiner Möglichkeiten, du solltest etwas anderes lernen.
        NOLEARNYOUALREADYKNOW, //Du musst erst fortgeschritten sein, bevor du Meister werden kannst.
        //NOLEARNYOUREBETTER, //Du bist jetzt schon besser.
        //NOTNOW, //Nicht jetzt!
        NOWWAIT, //Jetzt gibts was aufs Maul!
        NOWWAITINTRUDER, //Hier werden sie dich raustragen müssen!
        OKAYKEEPIT, //Okay okay, behalt es einfach.
        OM, //Ommm.
        OOH, //Kampfgejubel
        OUTOFMYWAY, //Mach endlich Platz man!
        //RUNCOWARD, //Komm zurück du Feigling!
        SHITWHATAMONSTER, //Scheiße, nichts wie weg!
        //SMALLTALK01, //Gerede
        SMALLTALK24,
        STOLEFROMMAGE, //Du hast die Magier beklaut, eine echt dämliche Idee!
        //STOPMAGIC, //Hör auf mit dieser Magiescheiße!
        STANGE, //Komm raus du Dreckskerl!
        SUCKERDEFEATEDMAGE, //Einen Magier zu besiegen ist ne wirklich beschissene Idee!
        SUCKERDEFEATEDNOV_GUARD, //Du kannst hier nicht rumrennen und die Novizen verprügeln!
        SUCKERDEFEATEDVLK_GUARD, //Lass die Finger von meinen Jungs!
        SUCKERGOTSOME, //Haste was aufs Maul gekriegt? Geschieht dir recht!
        THATISMYWEAPON, //Du rennst mit meiner Waffe rum!
        THEREHEIS, //Da drüben ist er.
        //THEREISAFIGHT, //Aah! Ein Kampf!
        THEYKILLEDMYFRIEND, //Sie haben einen unserer Jungs erwischt! Wenn ich das Schwein kriege...
        WATCHYOURAIM, //Nimm das Ding runter!
        WATCHYOURAIMANGRY, //Wenn du eine aufs Maul willst, ziel ruhig weiter auf mich!
        //WEAPONDOWN, //Steck die Waffe weg!
        WEWILLMEETAGAIN, //Beim nächsten mal sieht die Sache anders aus!
        //WHATAREYOUDOING, //He, bist du blind oder was?
        WHATDIDYOUINTHERE, //Du hast da drinnen nichts verloren, verschwinde!
        WHATDOYOUWANT, //Was willst du?
        WHATSTHAT, //Was war das denn?
        //WHATSTHISSUPPOSEDTOBE, //Was schleichst du da rum?
        //WHYAREYOUINHERE, //Raus aus meiner Hütte oder ich ruf die Wachen!
        //WISEMOVE, //Kluges Kerlchen
        YEAHWELLDONE, //Gibs ihm!
        YESYES, //Keine Panik, hast gewonnen!
        //YOUASKEDFORIT, //Du wolltest es so haben!
        YOUATTACKEDMYCHARGE, //Niemand vergreift sich ungestraft an meinen Jungs!
        YOUCANKEEPTHECRAP, //Dann behalt es eben, ich brauch es sowieso nicht.
        YOUDEAFORWHAT, //Bist du taub? Weg da!
        YOUDEFEATEDMEWELL, //War ein guter Kampf! Du hast mir ganz schön aufs Maul gehauen.
        YOUDEFEATEDMYCOMRADE, //Du hast einen Kumpel von mir umgehauen!
        YOUDEFEATEDNOV_GUARD,
        YOUDEFEATEDVLK_GUARD,
        //YOUDISTURBEDMYSLUMBER, //Warum weckst du mich?
        YOUKILLEDMAGE, //Du hast einen Magier umgebracht!
        YOUKILLEDMYFRIEND, //Du hast einen von unseren Jungs auf dem Gewissen!
        YOULLBESORRYFORTHIS, //Das wird dir noch Leid tun!
        YOUSTILLNOTHAVEENOUGH, //Hast du immer noch nicht genug?
        YOUVIOLATEDFORBIDDENTERRITORY, //Hee, wie kommst du denn hier rein?
        YOUWANNAFOOLME, //Hälst du mich für so dämlich?
        MAX_G1_VOICES,

        MAX_VOICES
    }
}
