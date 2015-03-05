using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;
using GUC.Options;
using System.IO;
using GUC.Network;
using GUC.WorldObjects.Character;
using Gothic.zClasses;
using GUC.Network.Messages.Connection;
using Gothic.zTypes;
using System.Reflection;
using GUC.Network.Messages.PlayerCommands;
using GUC.Types;

namespace GUC.States
{
    public class StartupState : AbstractState
    {
        public static ClientOptions clientOptions = null;

        public static void SetUpStartMap()
        {
            Process Process = Process.ThisProcess();

            ASCIIEncoding enc = new ASCIIEncoding();
            Process.Write(enc.GetBytes(@"gmp-rp/STARTLOCATION.ZEN"), 0x008907B0);
            Process.Write(new byte[] { 0 }, 0x008907B0 + @"gmp-rp/STARTLOCATION.ZEN".Length);
        }

        public static void SetupFuncBlocking()
        {
            Process Process = Process.ThisProcess();

            //First disable all:
            Gothic.mClasses.InputHooked.deactivateStatusScreen(Process, false);
            Gothic.mClasses.InputHooked.deactivateLogScreen(Process, false);
            //Gothic.mClasses.InputHooked.deactivateInventory(Process, false);



            //Block gothic.dat loading:
            //Process.Write(new byte[] { 0x33, 0xC0, 0xC2, 0x04, 0x00 }, 0x0078E900);
            Process.Write(new byte[] { 0xC3 }, 0x006C1C70);//Blocking Call Startup Scripts!
            Process.Write(new byte[] { 0xC3 }, 0x006C1F60);//Blocking Call Init Scripts!

            Process.Write(new byte[] { 0xC3 }, 0x00780D80);//Blocking time!
            Process.Write(new byte[] { 0x90, 0x90 }, 0x0073E480 + 0x189);
            Process.Write(new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90 }, 0x0073E480 + 0x193);//Blocking Dive Damage!
            Process.Write(new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90 }, 0x0066CAC9);//Block Damage!
            Process.Write(new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC2, 0x04, 0x00 }, 0x007546F0);//Block removing, when using mobs


            Process.VirtualProtect(0x007792E0, 40);
            Process.Write(new byte[] { 0x33, 0xC0, 0xC2, 0x04, 0x00 }, 0x007792E0);//Block deleting of dead characters!
            Process.Write(new byte[] { 0xE9, 0x77, 0x0D, 0x00, 0x00 }, 0x006FC669);//Blocking F-Keys


            #region Waffen nicht stapelbar
            //Process.Write(new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90 }, 0x00745FA6);//Blocking EquipBestWeapon when enabling a npc

            //Process.Write(new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0xEB, 0x0C }, 0x007125DA);
            //Process.Write(new byte[] { 0x68, 0x00, 0x00, 0x10, 0x00 }, 0x0073272D);
            //Process.Write(new byte[] { 0x68, 0x00, 0x00, 0x10, 0x00 }, 0x00732745);
            //Process.Write(new byte[] { 0x68, 0x00, 0x00, 0x10, 0x00 }, 0x00732759);
            //Process.Write(new byte[] { 0x68, 0x00, 0x00, 0x10, 0x00 }, 0x0073276D);
            //Process.Write(new byte[] { 0x68, 0x00, 0x00, 0x10, 0x00 }, 0x00732781);
            //Process.Write(new byte[] { 0x68, 0x00, 0x00, 0x10, 0x00 }, 0x00732791);
            //Process.Write(new byte[] { 0x68, 0x00, 0x00, 0x10, 0x00 }, 0x007327AB);

            Process.Write(new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90 }, 0x00712618);
            #endregion

            //Disable First-Person:
            //Process.Write(new byte[] { 0x83, 0xC4, 0x6C, 0xC3 }, 0x004A40E0);

            //Disable NPC-AI:
            //Process.VirtualProtect(0x00745A20, 0x31A);
            //byte[] arr = new byte[0x31A];
            //for (int i = 0; i < arr.Length; i++)
            //    arr[i] = 0x90;
            //Process.Write(arr, 0x00745A20);



            //Disable Marvin-Mode:
            Process.VirtualProtect(0x006CBF60, 25);
            byte[] arr = new byte[25];
            for (int i = 0; i < arr.Length; i++)
                arr[i] = 0x90;
            Process.Write(arr, 0x006CBF60);


            arr = new byte[] { 0xC3 };
            Process.Write(arr, 0x00432EC0);


        }

        public static String srcFile = null;
        public static void initDefaultScripts()
        {
            String[] arr = new String[] { "GUC.Resources.Constants.d", "GUC.Resources.Classes.d", "GUC.Resources.AI_Constants.d", "GUC.Resources.Text.d", 
                "GUC.Resources.BodyStates.d", "GUC.Resources.Focus.d", "GUC.Resources.Species.d",
                "GUC.Resources.NPC_Default.d", "GUC.Resources.PC_Hero.d"};

            zString str = null;
            String fileList = "";
            foreach (String internalFile in arr)
            {
                zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', "Parse: "+internalFile, 0, "Program.cs", 0);
                using (StreamReader sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream(internalFile)))
                {

                    String file = getRandomScriptString(".d");
                    File.WriteAllText(file, sr.ReadToEnd());
                    fileList += Path.GetFileName(file.ToUpper()) + "\r\n";
                    
                    //str = zString.Create(Process.ThisProcess(), file.ToUpper());
                    //zCParser.getParser(Process.ThisProcess()).ParseFile(str);
                    //str.Dispose();
                }
            }


            String file_FileList = getRandomScriptString(".src");
            srcFile = file_FileList;
            File.WriteAllText(file_FileList, fileList);

            str = zString.Create(Process.ThisProcess(), file_FileList.ToUpper());
            zCParser.getParser(Process.ThisProcess()).ParseSource(str);
            str.Dispose();

            str = zString.Create(Process.ThisProcess(), "C_NPC");
            zCPar_Symbol sym = zCParser.getParser(Process.ThisProcess()).GetSymbol(str);
            str.Dispose();
            sym.SetClassOffset(0x120);

            str = zString.Create(Process.ThisProcess(), "C_ITEM");
            sym = zCParser.getParser(Process.ThisProcess()).GetSymbol(str);
            str.Dispose();
            sym.SetClassOffset(0x120);

            zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', "Startup-Scripts-parsed!", 0, "Program.cs", 0);
        }

        static Random rand = new Random();
        public static String getRandomScriptString(String ending)
        {
            while (true)
            {
                String file = getDaedalusPath() + rand.Next(0, 1000000) + ending;

                if (!File.Exists(file))
                    return file;
            }            
        }

        public static void SetUpConfig()
        {
            String pfad = getConfigPath() + @"\gmp.xml";
            //zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', "Path: "+pfad, 0, "Program.cs", 0);
            if (!File.Exists(pfad))
                zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', "File does not exists: "+pfad, 0, "Program.cs", 0);
            clientOptions = ClientOptions.Load(pfad);
        }

        public static String getConfigPath()
        {
            return getSystemPath() + "\\UntoldChapter\\conf\\";
        }

        public static String getDaedalusPath()
        {
            String path = getSystemPath() + "\\UntoldChapter\\tempScripts\\";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            return path;
        }

        public static String getSystemPath()
        {
            if (Path.GetFileName(Environment.CurrentDirectory).ToUpper().Trim() == "SYSTEM" && File.Exists("Gothic2.exe"))
                return Path.GetFullPath(Environment.CurrentDirectory);
            else if (Path.GetFileName(Environment.CurrentDirectory).ToUpper().Trim() == "UntoldChapter" && 
                Path.GetFileName(Path.GetDirectoryName(Environment.CurrentDirectory)).ToUpper().Trim() == "SYSTEM" &&
                File.Exists(Path.GetFullPath(Path.GetDirectoryName(Environment.CurrentDirectory))+"\\Gothic2.exe"))
                return Path.GetFullPath(Path.GetDirectoryName(Environment.CurrentDirectory));
            else if(Directory.Exists(".\\System") && File.Exists(".\\System\\Gothic2.exe"))
                return Path.GetFullPath(".\\System");
            else if (Directory.Exists(".\\system") && File.Exists(".\\system\\Gothic2.exe"))
                return Path.GetFullPath(".\\system");

            return Path.GetFullPath(".\\");
        }


        public static void Start()
        {
            
            Program.client = new Client();
            Program.client.Startup();
            Program.client.Connect(clientOptions.ip, clientOptions.port, clientOptions.password);
        }



        public override void Init()
        {
            if (_init)
                return;

            //Enable Menu:
            Process Process = Process.ThisProcess();
            ASCIIEncoding enc = new ASCIIEncoding();
            Process.Write(enc.GetBytes("AAAAAA"), 0x890898);



            setupPlayer();

            ConnectionMessage.Write();

            PlayerKeyMessage.getPlayerKeyMessage().Init();

            _init = true;
        }

        protected void setupPlayer()
        {
            Player player = new Player(true, StartupState.clientOptions.name);
            player.Address = oCNpc.Player(Process.ThisProcess()).Address;
            player.IsSpawned = true;
            player.Position = (Vec3f)oCNpc.Player(Process.ThisProcess()).GetPositionArray();

            Player.Hero = player;
        }


        public override void update()
        {
            Player.Hero.setPosition(Player.Hero.Position);
            PlayerKeyMessage.getPlayerKeyMessage().update();
            Program.client.Update();
        }



    }
}
