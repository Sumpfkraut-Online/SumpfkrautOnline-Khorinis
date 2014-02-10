using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using WinApi;
using System.Threading;
using Gothic.zClasses;
using Gothic.zTypes;
using WinApi.User;
using WinApi.User.Enumeration;
using RakNet;
using Network;
using GMP.Net;
using GMP.Net.Messages;
using GMP.Logger;
using GMP.Injection.Synch;
using GMP.Modules;
using Gothic.mClasses;
using GMP.Helper;
using GMP.Injection.Hooks;
using Gothic.mClasses.Elements;
using GMP.Network.Messages;
using Gothic.zStruct;
using GMP.RenderTasks;
using GMP.Injection;
using Network.Types;
using GMP.Network.Messages.update;

namespace Injection
{
    public class Program
    {
        public static Client client;
        /// <summary>
        /// Sortiert nach ID, beinhaltet alle Spieler und NPCs
        /// </summary>
        public static List<Player> playerList = new List<Player>();
        public static Player Player = null;
        public static PlayerKeyMessage playerKeyMessage;


        static bool spawned = false;
        public static bool FullLoaded = false;
        public static ClientOptions clientOptions;
        public static List<String> PrimLangList = null;

        public static bool Loaded = false;
        static int render = -2;

        public static Chatbox chatBox;





        private static long lastTimeSended = 0;
        private static long lastTimeStatusSended = 0;
        private static bool LoadedStatusSend = false;

        private static long lastFade = 0;
        static Stamina stamina;
        public static bool FirstTime = true;

        public static long lastAnimFrame = 0;
        public static Thread PlayerUpdateThread = null;

        public static HookInfos StartAnim = null;
        public static HookInfos ActivateRtnState = null;

        public static HookInfos oCSpawnManager_SpawnNPC = null;
        public static HookInfos oCSpawnManager_SummonNPC = null;

        public static HookInfos AssessTalk_S = null;

        
        static bool started = false;



        
        public static Int32 InjectedMain(String message)
        {
            try
            {
                SetUpConfig();

                SetUpStartMap();

                client = new Client();
                client.Startup();
                client.Connect(clientOptions.ip, clientOptions.port, clientOptions.password);

                AddHooks();
                DisableNPCAI();
                DisableFKeys();
                MarvinModeDisable();

                SetUpLanguages();
                

                while (true)
                {
                    Thread.Sleep(10000);
                }
            }
            catch (System.Exception ex)
            {
                zERROR.GetZErr(Process.ThisProcess()).Report(4, 'G', ex.ToString(), 0, "Program.cs", 0);
            }
            
                
            return 11;
        }

        public static void SetUpConfig()
        {
            String pfad = @".\System\gmp.xml";
            if (!File.Exists(pfad))
                pfad = @".\system\gmp.xml";
            if (!File.Exists(pfad))
                pfad = "gmp.xml";
            if (!File.Exists(pfad))
                System.Windows.Forms.MessageBox.Show(Environment.CurrentDirectory);
            clientOptions = ClientOptions.Load(Environment.CurrentDirectory + "\\" + pfad);
        }

        public static void SetUpStartMap()
        {
            Process Process = Process.ThisProcess();

            ASCIIEncoding enc = new ASCIIEncoding();
            Process.Write(enc.GetBytes(@"gmp-rp/STARTLOCATION.ZEN"), 0x008907B0);
            Process.Write(new byte[] { 0 }, 0x008907B0 + @"gmp-rp/STARTLOCATION.ZEN".Length);
        }

        public static void DisableNPCAI()
        {
            Process Process = Process.ThisProcess();
            Process.VirtualProtect(0x00745A20, 0x31A);
            byte[] arr = new byte[0x31A];
            for (int i = 0; i < arr.Length; i++)
                arr[i] = 0x90;
            Process.Write(arr, 0x00745A20);

            //Löschen von toten charakteren...
            Process.VirtualProtect(0x007792E0, 40);
            arr = new byte[] { 0x33, 0xC0, 0xC2, 0x04, 0x00 };
            Process.Write(arr, 0x007792E0);
        }

        public static void DisableFKeys()
        {
            Process Process = Process.ThisProcess();
            Process.Write(new byte[] { 0xE9, 0x77, 0x0D, 0x00, 0x00 }, 0x006FC669);
        }

        public static void SetUpLanguages()
        {
            PrimLangList = new List<String>() { Program.clientOptions.lang, "de", "en" };

            String path = "";
            if (Directory.Exists("./gothic_multiplayer/Lang"))
                path = "./gothic_multiplayer/Lang";
            else if (Directory.Exists("../gothic_multiplayer/Lang"))
                path = "../gothic_multiplayer/Lang";

            if (path != "")
            {
                StaticVars.Languages = new WinApi.FileFormat.IniFile(path);
            }
        }
        
        public static void AddHooks()
        {
            Process Process = Process.ThisProcess();
            Gothic.mClasses.Cursor.Init(Process);

            ActivateRtnState = Process.Hook("GUC.dll", typeof(AI).GetMethod("ActivateRtnState"), (int)0x0076C330, (int)6, 1);
            Process.Hook("GUC.dll", typeof(GMP.Injection.Synch.MobSynch).GetMethod("oCMobInterByAI"), (int)oCMobInter.FuncOffsets.AI_UseMobToState, (int)oCMobInter.HookSize.AI_UseMobToState, 2);
            
            

            Process.Hook("GUC.dll", typeof(AI).GetMethod("Output"), (int)oCNpc.FuncOffsets.EV_OutputSVM_Overlay, (int)oCNpc.HookSize.EV_OutputSVM_Overlay, 1);
            Process.Hook("GUC.dll", typeof(AI).GetMethod("Output"), (int)oCNpc.FuncOffsets.EV_Output, (int)oCNpc.HookSize.EV_Output, 1);
            Process.Hook("GUC.dll", typeof(AI).GetMethod("Output"), (int)oCNpc.FuncOffsets.EV_OutputSVM, (int)oCNpc.HookSize.EV_OutputSVM, 1);

            Process.Hook("GUC.dll", typeof(Externals).GetMethod("AddExternals"), (int)0x006D4780, (int)7, 1);

            oCSpawnManager_SpawnNPC = Process.Hook("GUC.dll", typeof(SpawnManager).GetMethod("SpawnNPC"), (int)oCSpawnManager.FuncOffsets.SpawnNPCInt, (int)oCSpawnManager.HookSize.SpawnNPCInt, 3);
            oCSpawnManager_SummonNPC = Process.Hook("GUC.dll", typeof(SpawnManager).GetMethod("SummonNPC"), (int)oCSpawnManager.FuncOffsets.SummonNPC, (int)oCSpawnManager.HookSize.SummonNPC, 3);
            
            //oCSpawnManager_SpawnNPC = Process.Hook("GUC.dll", typeof(SpawnManager).GetMethod("SpawnNPC"), (int)oCSpawnManager.FuncOffsets.SpawnNPC_Str, (int)oCSpawnManager.HookSize.SpawnNPC_Str, 3);
            //oCSpawnManager_SummonSpawnNPC = Process.Hook("GUC.dll", typeof(SpawnManager).GetMethod("SummonSpawnNPC"), (int)oCSpawnManager.FuncOffsets.SpawnNPC, (int)oCSpawnManager.HookSize.SpawnNPC, 3);



            Process.Hook("GUC.dll", typeof(AI).GetMethod("OnDamage_DD"), (int)oCNpc.FuncOffsets.OnDamage_DD, (int)oCNpc.HookSize.OnDamage_DD, 1);
            Process.Hook("GUC.dll", typeof(Game).GetMethod("ExitGame"), (int)CGameManager.FuncOffsets.Done, (int)CGameManager.HookSize.Done, 0);

            Process.Hook("GUC.dll", typeof(AI).GetMethod("CreatePassivePerception"), (int)oCNpc.FuncOffsets.CreatePassivePerception, (int)oCNpc.HookSize.CreatePassivePerception, 3);


            Process.Hook("GUC.dll", typeof(MagSynch).GetMethod("MagBookOpen"), (int)oCMag_Book.FuncOffsets.Open, (int)oCMag_Book.HookSize.Open, 1);
            Process.Hook("GUC.dll", typeof(MagSynch).GetMethod("MagBookClose"), (int)oCMag_Book.FuncOffsets.Close, (int)oCMag_Book.HookSize.Close, 1);
            Process.Hook("GUC.dll", typeof(MagSynch).GetMethod("SpellCast"), (int)oCMag_Book.FuncOffsets.SpellCast, (int)oCMag_Book.HookSize.SpellCast, 0);
            Process.Hook("GUC.dll", typeof(MagSynch).GetMethod("Spell_Invest"), (int)oCMag_Book.FuncOffsets.Spell_Invest, (int)oCMag_Book.HookSize.Spell_Invest, 0);
            Process.Hook("GUC.dll", typeof(MagSynch).GetMethod("Spell_Setup"), (int)oCMag_Book.FuncOffsets.Spell_Setup, (int)oCMag_Book.HookSize.Spell_Setup, 3);
            Process.Hook("GUC.dll", typeof(MagSynch).GetMethod("oCSpellCast"), (int)oCSpell.FuncOffsets.Cast, (int)oCSpell.HookSize.Cast, 0);

            Process.Hook("GUC.dll", typeof(MagSynch).GetMethod("MagBookStartCastEffect"), (int)oCMag_Book.FuncOffsets.StartCastEffect, (int)oCMag_Book.HookSize.StartCastEffect, 2);
            Process.Hook("GUC.dll", typeof(MagSynch).GetMethod("MagBookStartInvestEffect"), (int)oCMag_Book.FuncOffsets.StartInvestEffect, (int)oCMag_Book.HookSize.StartInvestEffect, 4);

            Process.Hook("GUC.dll", typeof(GMP.Injection.Synch.MobSynch).GetMethod("oCItemContainer_Remove_2"), (int)oCItemContainer.FuncOffsets.Remove_2, (int)oCItemContainer.HookSizes.Remove_2, 2);
            Process.Hook("GUC.dll", typeof(GMP.Injection.Synch.MobSynch).GetMethod("oCItemContainer_Insert"), (int)oCItemContainer.FuncOffsets.Insert, (int)oCItemContainer.HookSizes.Insert, 1);

            Process.Hook("GUC.dll", typeof(GMP.Injection.Synch.MobSynch).GetMethod("OnUnTrigger"), (int)oCMobInter.FuncOffsets.OnUnTrigger, (int)oCMobInter.HookSize.OnUnTrigger, 2);
            Process.Hook("GUC.dll", typeof(GMP.Injection.Synch.MobSynch).GetMethod("OnTrigger"), (int)oCMobInter.FuncOffsets.OnTrigger, (int)oCMobInter.HookSize.OnTrigger, 2);

            Process.Hook("GUC.dll", typeof(Program).GetMethod("ocGameUpdate"), 0x006C8AB2, 5, 0);
            Process.Hook("GUC.dll", typeof(World).GetMethod("hChangeLevelStart"), (int)oCGame.FuncOffsets.ChangeLevel, (int)oCGame.HookSize.ChangeLevel, 2);
            Process.Hook("GUC.dll", typeof(World).GetMethod("hChangeLevelEnd"), 0x006C7AD5, 7, 0);

            Process.Hook("GUC.dll", typeof(GMP.Injection.Synch.MobSynch).GetMethod("StartInteraction"), (int)oCMobInter.FuncOffsets.StartInteraction, (int)oCMobInter.HookSize.StartInteraction, 1);
            Process.Hook("GUC.dll", typeof(GMP.Injection.Synch.MobSynch).GetMethod("StopInteraction"), (int)oCMobInter.FuncOffsets.StopInteraction, (int)oCMobInter.HookSize.StopInteraction, 1);
            Process.Hook("GUC.dll", typeof(GMP.Injection.Synch.MobSynch).GetMethod("StartStateChange"), (int)oCMobInter.FuncOffsets.StartStateChange, (int)oCMobInter.HookSize.StartStateChange, 3);
            Process.Hook("GUC.dll", typeof(GMP.Injection.Synch.MobSynch).GetMethod("SetMobBodyState"), (int)oCMobInter.FuncOffsets.SetMobBodyState, (int)oCMobInter.HookSize.SetMobBodyState, 1);
            Process.Hook("GUC.dll", typeof(GMP.Injection.Synch.MobSynch).GetMethod("CallOnStateFunc"), (int)oCMobInter.FuncOffsets.CallOnStateFunc, (int)oCMobInter.HookSize.CallOnStateFunc, 2);

            Process.Hook("GUC.dll", typeof(GMP.Injection.Synch.MobSynch).GetMethod("Open"), (int)oCMobContainer.FuncOffsets.Open, (int)oCMobContainer.HookSize.Open, 1);
            Process.Hook("GUC.dll", typeof(GMP.Injection.Synch.MobSynch).GetMethod("Close"), (int)oCMobContainer.FuncOffsets.Close, (int)oCMobContainer.HookSize.Close, 1);

            Process.Hook("GUC.dll", typeof(GMP.Injection.Synch.MobSynch).GetMethod("PickLock"), (int)oCMobLockable.FuncOffsets.PickLock, (int)oCMobLockable.HookSize.PickLock, 2);

            


            //Animationen!
            Process.Hook("GUC.dll", typeof(Animation).GetMethod("SetActFrame"), (int)zCModelAniActive.FuncOffsets.SetActFrame, (int)zCModelAniActive.HookSize.SetActFrame, 1);
            StartAnim = Process.Hook("GUC.dll", typeof(Animation).GetMethod("oCStartAnim_ModelAnim"), (int)zCModel.FuncOffsets.StartAni_ModelInt, (int)zCModel.FuncSize.StartAni_ModelInt, 2);



            Process.Hook("GUC.dll", typeof(Animation).GetMethod("oCApplyOverlay"), (int)oCNpc.FuncOffsets.ApplyOverlay, (int)oCNpc.HookSize.ApplyOverlay, 1);
            Process.Hook("GUC.dll", typeof(Animation).GetMethod("oCApplyTimedOverlayMDS"), (int)oCNpc.FuncOffsets.ApplyTimedOverlayMds, (int)oCNpc.HookSize.ApplyTimedOverlayMds, 2);
            Process.Hook("GUC.dll", typeof(Animation).GetMethod("oCRemoveOverlay"), (int)oCNpc.FuncOffsets.RemoveOverlay, (int)oCNpc.HookSize.RemoveOverlay, 1);

            Process.Hook("GUC.dll", typeof(VisualSynchro).GetMethod("ocSetAsPlayer"), (int)oCNpc.FuncOffsets.SetAsPlayer, (int)oCNpc.HookSize.SetAsPlayer, 0);

            Process.Hook("GUC.dll", typeof(WeaponMode).GetMethod("SetWeaponMode2_Str"), (int)oCNpc.FuncOffsets.SetWeaponMode2_Str, (int)oCNpc.HookSize.SetWeaponMode2_Str, 1);
            Process.Hook("GUC.dll", typeof(WeaponMode).GetMethod("SetWeaponMode2_Int"), (int)oCNpc.FuncOffsets.SetWeaponMode2_Int, (int)oCNpc.HookSize.SetWeaponMode2_Int, 1);
            Process.Hook("GUC.dll", typeof(WeaponMode).GetMethod("SetWeaponMode"), (int)oCNpc.FuncOffsets.SetWeaponMode, (int)oCNpc.HookSize.SetWeaponMode, 1);

            Process.Hook("GUC.dll", typeof(ItemSynchro).GetMethod("oCNpc_SetInteractItem"), (int)oCNpc.FuncOffsets.SetInteractItem, (int)oCNpc.HookSize.SetInteractItem, 1);
            //Process.Hook("GUC.dll", typeof(ItemSynchro).GetMethod("oCNpc_EV_UseItem"), (int)oCNpc.FuncOffsets.EV_UseItemToState, (int)oCNpc.HookSize.EV_UseItemToState, 1);
            Process.Hook("GUC.dll", typeof(ItemSynchro).GetMethod("DoDropVob"), (int)oCNpc.FuncOffsets.DoDropVob, (int)oCNpc.HookSize.DoDropVob, 1);
            Process.Hook("GUC.dll", typeof(ItemSynchro).GetMethod("DoTakeVob"), (int)oCNpc.FuncOffsets.DoTakeVob, (int)oCNpc.HookSize.DoTakeVob, 1);

        }

        static byte[] mmenable;
        public static void MarvinModeEnable()
        {
            Process Process = Process.ThisProcess();
            Process.Write(mmenable, 0x006CBF60);
        }

        public static void MarvinModeDisable()
        {
            Process Process = Process.ThisProcess();
            Process.VirtualProtect(0x006CBF60, 25);
            mmenable = Process.ReadBytes(0x006CBF60, 25);
            byte[] arr = new byte[25];
            for (int i = 0; i < arr.Length; i++)
                arr[i] = 0x90;
            Process.Write(arr, 0x006CBF60);


            arr = new byte[] { 0xC3 };
            Process.Write(arr, 0x00432EC0);//Eventhandle marvin mode
        }


        public static void UpdateInput()
        {
            Process Process = Process.ThisProcess();

            //Beim Pressen von K alle Beschworenen Monster töten!
            if (StaticVars.Ingame && InputHooked.IsPressed((int)clientOptions.killSummoned))
            {
                foreach (NPC npc in StaticVars.npcControlList)
                {
                    if (npc.npcPlayer.instance.Trim().ToUpper().StartsWith("Summoned_".ToUpper()))
                    {
                        oCNpc summoned = new oCNpc(Process, npc.npcPlayer.NPCAddress);
                        summoned.HP = 0;
                    }
                }
            }


            if (StaticVars.Ingame && InputHooked.IsPressed((int)VirtualKeys.F6))
            {
                oCNpc.Player(Process).Inventory.Draw();
            }



            if (InputHooked.IsPressed((int)VirtualKeys.F7))
            {
                Process.Write(new byte[] { 0xE9 }, 0x0070AAB4);
                Process.Write(0x11C, 0x0070AAB5);
                oCItemContainer oIC = oCItemContainer.Create(Process);
                zString str = zString.Create(Process, "BEDHIGH_NW_EDEL_01.ASC");

                oCItem itm = oCItem.Create(Process);
                itm.SetVisual(str);
                oIC.Insert(itm);

                itm = oCItem.Create(Process);
                itm.SetVisual(str);
                oIC.Insert(itm);

                itm = oCItem.Create(Process);
                itm.SetVisual(str);
                oIC.Insert(itm);

                //2097152
                oIC.Open(0, 0, 0);
            }

            if (InputHooked.IsPressed((int)VirtualKeys.F8))
            {
                Process process = Process.ThisProcess();
                IntPtr ptr = process.Alloc(512);
                zCClassDef.ObjectCreated(process, ptr.ToInt32(), 0x00AB1518);
                process.THISCALL<NullReturnCall>((uint)ptr.ToInt32(), (uint)0x0071D010, new CallValue[] { });

                oCMobDoor mob = new oCMobDoor(process, ptr.ToInt32());
                zString str = zString.Create(process, "BEDHIGH_NW_EDEL_01.ASC");
                mob.SetVisual(str);
                mob.ObjectName.Set("MEINERSTESBETT");
                str.Dispose();
                mob.OnStateFunc.Set("SLEEPABIT");
                mob.VobType = zCVob.VobTypes.MobDoor;

                oCGame.Game(process).World.AddVob(mob);
                
                //oCNpc.Player(process).SetPosition(mob.TrafoObjToWorld.get(3), mob.TrafoObjToWorld.get(7), mob.TrafoObjToWorld.get(11));

                zVec3 dir = zVec3.Create(process);
                dir.Y = -100000;
                dir.X = 0;
                dir.Z = 0;

                zVec3 pos = oCNpc.Player(process).GetPosition();
                pos.Y += 1000;
                oCGame.Game(process).World.TraceRayNearestHit(oCNpc.Player(process).GetPosition(), dir, zCWorld.zTraceRay.Ignore_Vob_All & zCWorld.zTraceRay.Test_Water & zCWorld.zTraceRay.Return_Normal & zCWorld.zTraceRay.Return_POLY);
                pos.Dispose();
                dir.Dispose();
                if (oCGame.Game(process).World.Raytrace_FoundHit != 0)
                {
                    mob.SetPositionWorld(oCGame.Game(process).World.Raytrace_FoundIntersection);
                }
                else
                {
                    zERROR.GetZErr(Process).Report(3, 'G', "No Intersaction!", 0, "Program.cs", 0);
                }


                started = true;
                zERROR.GetZErr(Process).Report(3, 'G', "Mob spawned", 0, "Program.cs", 0);
     

            }


            if (InputHooked.IsPressed((int)VirtualKeys.F12))
            {
                int count = 0;
                zCArray<zCVob> acVobList = oCGame.Game(Process).World.ActiveVobList;
                for (int i = 0; i < acVobList.Size; i++)
                {
                    if (acVobList.get(i).VobType == zCVob.VobTypes.Npc)
                        count += 1;
                }
                zERROR.GetZErr(Process).Report(3, 'G', Program.playerList.Count + " | " + StaticVars.npcList.Count + " | " + count, 0, "Program.cs", 0);
            }

            if (lastConnectTime + 10000*500 < DateTime.Now.Ticks &&InputHooked.IsPressed((int)VirtualKeys.F10))
            {
                oCNpc.Player(Process).SetPosition(Program.playerList[lastNPC].pos[0], Program.playerList[lastNPC].pos[1], Program.playerList[lastNPC].pos[2]);
                lastNPC++;
                lastConnectTime = DateTime.Now.Ticks;
            }

            if (InputHooked.IsPressed((int)VirtualKeys.F9))
            {
                foreach(Player p in Program.playerList)
                {
                    zVec3 pos = new oCNpc(Process, p.NPCAddress).GetPosition();
                    zERROR.GetZErr(Process).Report(3, 'G', "ID: " + p.id + " Instance:" + p.instance + " pos:" + p.pos[0] + "|" + p.pos[1] + "|" + p.pos[2] + " pos:" + pos.X + "|" + pos.Y + "|" + pos.Z, 0, "Program.cs", 0);
                    pos.Dispose();
                }
            }
        }
        static int lastNPC = 0;
        static long lastConnectTime = 0;

        static Button connect;
        static Button exit;
        static Button exitTop;
        public static void StartupModule()
        {
            //Exit Button hinzufügen
            Process Process = Process.ThisProcess();
            exitTop = new Button(Process, "E");
            exitTop.setSize(400, 470);
            exitTop.setPos(7600, 300);
            exitTop.setTexture("button_exit.tga");
            exitTop.ButtonPressed += new EventHandler<EventArgs>(ExitGame);
            exitTop.Show();

            zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', "Module laden", 0, "Program.cs", 0);
            LoadingScreen.Hide(Process);
            
            

            StaticVars.ModuleLoad = true;
            ModuleLoader.startFirstStartState();
            
        }

        
        public static void StartGame(object obj, EventArgs args)
        {
            Process Process = Process.ThisProcess();
            connect.Hide();
            exit.Hide();
            exitTop.Hide();


            Cursor.Hide(Process);
            InputHooked.activateFullControl(Process);


            chatBox = new Chatbox(Process);
            client.messageListener.Add((byte)NetWorkIDS.ChatMessage, new ChatMessage(chatBox));
            new ConnectionRequest().Write(client.sentBitStream, client);
            //StaticVars.Ingame = true;

            zERROR.GetZErr(Process).Report(2, 'G', "Start Game...", 0, "Program.cs", 0);

            zString str = zString.Create(Process, StaticVars.StartWorld);
            zString str2 = zString.Create(Process, StaticVars.StartWP);
            oCGame.Game(Process).ChangeLevel(str, str2);
            str.Dispose();
            str2.Dispose();

            zERROR.GetZErr(Process).Report(2, 'G', "Start Game: Changed Level...", 0, "Program.cs", 0);
        }

        public static void StopModule()
        {
            Process Process = Process.ThisProcess();

            connect = new Button(Process, StaticVars.Languages.getValue(Program.PrimLangList, "Connect"));
            connect.Show();
            connect.setPos(1433, 6432);
            connect.ButtonPressed += new EventHandler<EventArgs>(StartGame);

            exit = new Button(Process, StaticVars.Languages.getValue(Program.PrimLangList, "Exit"));
            exit.Show();
            exit.setPos(4998, 6432);
            exit.ButtonPressed += new EventHandler<EventArgs>(ExitGame);
            Cursor.Show(Process);
            Cursor.ToTop(Process);

            StaticVars.ModuleLoad = false;

            InputHooked.deaktivateFullControl(Process);
            //StaticVars.Ingame = true;
        }

        public static void ExitGame(object obj, EventArgs args)
        {
            Process Process = Process.ThisProcess();

            zCOption.GetOption(Process).getSection("INTERNAL").getEntry("gameAbnormalExit").VarValue.Set("0");
            zCOption.GetOption(Process).Save("Gothic.INI");
            CGameManager.GameManager(Process).ExitGameVar = 1;
            //CGameManager.ExitGameFunc(Process);
        }

        public static void Load()
        {
            Process Process = Process.ThisProcess();


            zERROR zerr = zERROR.GetZErr(Process);
            EnableMenu();
            Cursor.Show(Process);
            StealContainer sc = new StealContainer(Process);
            sc.Enable();

            StaticVars.printView = zCView.Create(Process, 0, 0, 0x2000, 0x1000);
            zCView.GetStartscreen(Process).InsertItem(StaticVars.printView, 0);
            LoadingScreen.Show(Process);
            LoadedStatusSend = true;
            Loaded = true;

            zString str = zString.Create(Process, "PC_HERO");
            StaticVars.STRHELPER = oCObjectFactory.GetFactory(Process).CreateNPC(zCParser.getParser(Process).GetIndex(str));
            str.Dispose();
        }

       
        public static Int32 ocGameUpdate(String message)
        {
            Process Process = Process.ThisProcess();
            try
            {
                
                if (!Loaded )
                {
                    Load();
                }

                if (LoadedStatusSend && client.isConnected)
                {
                    LoadedStatusSend = false;
                    new StatusMessage().Write(client.sentBitStream, client);
                }

                

                //Gothic.mClasses.Cursor.Update(Process);
                InputHooked.Update();
                
                UpdateInput();

                if (client != null)
                {
                    if (!StaticVars.Ingame)
                        client.Connect();
                    client.Update();

                    if (StaticVars.ModuleLoad)
                    {
                        ModuleLoader.updateStartState();
                    }


                    if (StaticVars.Ingame)
                    {
                        ModuleLoader.updateAllModules();

                        if (StaticVars.serverConfig.enableStamina)
                        {
                            if (stamina == null)
                            {
                                stamina = new Stamina();
                            }
                            stamina.update();
                            stamina.updateInput();
                        }


                        if (render == -2)
                        {
                            zString str = zString.Create(Process, "render_loop");
                            render = zCParser.getParser(Process).GetIndex(str);
                            str.Dispose();
                        }

                        if (render > 1)
                        {
                            zCParser.CallFunc(Process, new CallValue[] {
                                new IntArg(zCParser.getParser(Process).Address),
                                new IntArg(render)
                            });
                        }


                        if (FullLoaded)
                        {
                            
                        }

                        if (FirstTime)
                        {
                            Random rand = new Random();
                            if (StaticVars.serverConfig.Spawn == null || StaticVars.serverConfig.Spawn.Count == 0)
                            {
                                NPCHelper.SetRespawnPoint("START");
                            }
                            else
                            {
                                int lang = rand.Next(0, StaticVars.serverConfig.Spawn.Count);
                                NPCHelper.SetRespawnPoint(StaticVars.serverConfig.Spawn[lang]);
                            }
                            FirstTime = false;
                        }


                        //if (Player != null && lastTimeSended + 10000 *30 < DateTime.Now.Ticks)
                        if (Player != null)//Zeit wird über die Klassen gesetzt...
                        {
                            new PlayerStatusMessage().Write(client.sentBitStream, client);
                            new AllPlayerSynchMessage().Write(client.sentBitStream, client, true);//TODO: dex str hitchances unnötig.... Über synchro effekte wie trigger und spellcast regeln...

                            
                            lastTimeSended = DateTime.Now.Ticks;
                        }
                        if (Player != null && FullLoaded && lastTimeStatusSended + 10000*1000 < DateTime.Now.Ticks)
                        {
                            new MobInteractDiffMessage().Write(Program.Player);

                            new PlayerStatusMessage2().Write(client.sentBitStream, client);
                            new AllPlayerSynchMessage2().Write(client.sentBitStream, client, true);
                            lastTimeStatusSended = DateTime.Now.Ticks;
                        }



                        if (StaticVars.Ingame && Player != null && FullLoaded)
                        {
                            PlayerStatusMessage.update();
                        }

                        if (StaticVars.Ingame && Program.playerKeyMessage != null)
                        {
                            Program.playerKeyMessage.update();
                        }


                        //
                        //SetActFrame senden! Aus dem Dictionary heraus!
                        //
                        if (Player != null && FullLoaded && lastAnimFrame + 10000 * 100 < DateTime.Now.Ticks)
                        {
                            float[] heropos = new float[] { oCNpc.Player(Process).TrafoObjToWorld.get(3), oCNpc.Player(Process).TrafoObjToWorld.get(7), oCNpc.Player(Process).TrafoObjToWorld.get(11) };
                            foreach (Player spawningPlayer in Program.playerList)
                            {
                                if (!spawningPlayer.isSpawned)
                                {
                                    if (Player.isSameMap(spawningPlayer.actualMap, Program.Player.actualMap) && new Vec3f(spawningPlayer.pos).getDistance((Vec3f)heropos) < 10000)
                                    {
                                        //Spawn!
                                        if (!spawningPlayer.isNPC)
                                        {
                                            NPCHelper.SpawnPlayer(spawningPlayer, true);
                                            NPCHelper.SetStandards(spawningPlayer);
                                        }
                                        else
                                        {
                                            NPCHelper.SpawnPlayer(spawningPlayer, false);
                                        }
                                        //NPCHelper.SpawnPlayer(spawningPlayer, false);
                                    }
                                }
                                else
                                {
                                    if (!Player.isSameMap(spawningPlayer.actualMap, Program.Player.actualMap) || new Vec3f(spawningPlayer.pos).getDistance((Vec3f)heropos) > 11000)
                                    {
                                        //Spawn!
                                        NPCHelper.RemovePlayer(spawningPlayer, false);
                                    }
                                }


                            }

                            foreach (KeyValuePair<Player, Dictionary<int, float>> plPair in Animation.actFrameList)
                            {
                                foreach (KeyValuePair<int, float> animPair in plPair.Value)
                                {
                                    new AnimationMessage().Write(Program.client.sentBitStream, Program.client, 6, plPair.Key, animPair.Key, 0, animPair.Value);
                                }
                            }
                            Animation.actFrameList.Clear();
                            lastAnimFrame = DateTime.Now.Ticks;
                        }
                        RenderTask.run();

                        


                    }

                }

                if (lastFade + 10000 * 360 <= DateTime.Now.Ticks)
                {
                    foreach (Player pl in StaticVars.playerlist)
                    {
                        if (!pl.isSpawned)
                            continue;
                        oCNpc npc = new oCNpc(Process, pl.NPCAddress);
                        npc.GetModel().FadeOutAnisLayerRange(20, 20);
                    }
                    lastFade = DateTime.Now.Ticks;
                }

                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            catch (System.Exception ex)
            {
                zERROR.GetZErr(Process).Report(2, 'G', ex.ToString(), 0, "Program.cs", 0);
            }
            
            return 0;
        }

        

        public static void EnableMenu()
        {
            Process Process = Process.ThisProcess();
            ASCIIEncoding enc = new ASCIIEncoding();
            Process.Write(enc.GetBytes("AAAAAA"), 0x890898); //nomenu überschreiben
        }
    }
}
