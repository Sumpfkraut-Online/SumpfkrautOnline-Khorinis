using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;
using GUC.Options;
using System.IO;
using GUC.Client.Network;
using GUC.Client.Network.Messages;
using Gothic.zClasses;
using Gothic.zTypes;
using System.Reflection;
using GUC.Client.Menus;
using WinApi.User.Enumeration;

namespace GUC.Client.States
{
    public class StartupState : AbstractState
    {
        public static ClientOptions clientOptions = null;

        public static void SetUpStartMap()
        {
            ASCIIEncoding enc = new ASCIIEncoding();
            Program.Process.Write(enc.GetBytes(@"gmp-rp/STARTLOCATION.ZEN"), 0x008907B0);
            Program.Process.Write(new byte[] { 0 }, 0x008907B0 + @"gmp-rp/STARTLOCATION.ZEN".Length);
            //Program.Process.Write(enc.GetBytes(@"OLDWORLD/OLDWORLD.ZEN"), 0x008907B0);
            //Program.Process.Write(new byte[] { 0 }, 0x008907B0 + @"OLDWORLD/OLDWORLD.ZEN".Length);
        }

        public static void SetupFuncBlocking()
        {
            //First disable all:
            Gothic.mClasses.InputHooked.deactivateStatusScreen(Program.Process, false);
            Gothic.mClasses.InputHooked.deactivateLogScreen(Program.Process, false);
            Gothic.mClasses.InputHooked.deactivateInventory(Program.Process, false);

            Program.Process.Write(new byte[] { 233, 229, 2, 0, 0, 0 }, 0x42AE7E); //disable ingame ESC menu

            //Block gothic.dat loading:
            //Process.Write(new byte[] { 0x33, 0xC0, 0xC2, 0x04, 0x00 }, 0x0078E900);
            Program.Process.Write(new byte[] { 0xC3 }, 0x006C1C70);//Blocking Call Startup Scripts!
            Program.Process.Write(new byte[] { 0xC3 }, 0x006C1F60);//Blocking Call Init Scripts!

            Program.Process.Write(new byte[] { 0xC3 }, 0x00780D80);//Blocking time!
            Program.Process.Write(new byte[] { 0x90, 0x90 }, 0x0073E480 + 0x189);
            Program.Process.Write(new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90 }, 0x0073E480 + 0x193);//Blocking Dive Damage!
            Program.Process.Write(new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90 }, 0x0066CAC9);//Block Damage!
            Program.Process.Write(new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC2, 0x04, 0x00 }, 0x007546F0);//Block removing, when using mobs


            Program.Process.VirtualProtect(0x007792E0, 40);
            Program.Process.Write(new byte[] { 0x33, 0xC0, 0xC2, 0x04, 0x00 }, 0x007792E0);//Block deleting of dead characters!
            Program.Process.Write(new byte[] { 0xE9, 0x77, 0x0D, 0x00, 0x00 }, 0x006FC669);//Blocking F-Keys


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

            Program.Process.Write(new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90 }, 0x00712618);
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
            Program.Process.VirtualProtect(0x006CBF60, 25);
            byte[] arr = new byte[25];
            for (int i = 0; i < arr.Length; i++)
                arr[i] = 0x90;
            Program.Process.Write(arr, 0x006CBF60);


            arr = new byte[] { 0xC3 };
            Program.Process.Write(arr, 0x00432EC0);


        }

        public static String srcFile = null;
        public static void initDefaultScripts()
        {
            String[] arr = new String[] { "GUC.Client.Resources.Constants.d", "GUC.Client.Resources.Classes.d", "GUC.Client.Resources.AI_Constants.d", "GUC.Client.Resources.Text.d", 
                "GUC.Client.Resources.BodyStates.d", "GUC.Client.Resources.Focus.d", "GUC.Client.Resources.Species.d",
                "GUC.Client.Resources.NPC_Default.d", "GUC.Client.Resources.PC_Hero.d" };

            zString str = null;
            String fileList = "";
            foreach (String internalFile in arr)
            {
                zERROR.GetZErr(Program.Process).Report(2, 'G', "Parse: "+internalFile, 0, "Program.cs", 0);
                using (StreamReader sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream(internalFile), System.Text.Encoding.Default))
                {

                    String file = getDaedalusPath() + internalFile.Substring("GUC.Client.Resources.".Length);
                    File.WriteAllText(file, sr.ReadToEnd(), System.Text.Encoding.Default);
                    fileList += Path.GetFileName(file.ToUpper()) + "\r\n";
                    
                    //str = zString.Create(Program.Process, file.ToUpper());
                    //zCParser.getParser(Program.Process).ParseFile(str);
                    //str.Dispose();
                }
            }


            String file_FileList = getDaedalusPath() + "GUC.src";
            srcFile = file_FileList;
            File.WriteAllText(file_FileList, fileList);

            str = zString.Create(Program.Process, file_FileList.ToUpper());
            zCParser.getParser(Program.Process).ParseSource(str);
            str.Dispose();

            str = zString.Create(Program.Process, "C_NPC");
            zCPar_Symbol sym = zCParser.getParser(Program.Process).GetSymbol(str);
            str.Dispose();
            sym.SetClassOffset(0x120);

            str = zString.Create(Program.Process, "C_ITEM");
            sym = zCParser.getParser(Program.Process).GetSymbol(str);
            str.Dispose();
            sym.SetClassOffset(0x120);

            zERROR.GetZErr(Program.Process).Report(2, 'G', "Startup-Scripts-parsed!", 0, "Program.cs", 0);
        }

        public static void SetUpConfig()
        {
            String pfad = getConfigPath() + @"\gmp.xml";
            if (!File.Exists(pfad))
                zERROR.GetZErr(Program.Process).Report(3, 'G', "File does not exists: "+pfad, 0, "Program.cs", 0);
            clientOptions = ClientOptions.Load(pfad);
        }

        public static String getConfigPath()
        {
            return getSystemPath() + "\\UntoldChapter\\conf\\";
        }

        public static String getDownloadPath()
        {
            return getSystemPath() + "\\UntoldChapter\\Downloads\\"+clientOptions.ip+"_"+clientOptions.port+"\\";
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
            Program.client = new Network.Client();
            Program.client.Startup();
            Program.client.Connect(clientOptions.ip, clientOptions.port, "");
        }

        static Dictionary<VirtualKeys, Action> shortcuts = new Dictionary<VirtualKeys, Action>();
        public override Dictionary<VirtualKeys, Action> Shortcuts { get { return shortcuts; } }

        public StartupState()
        {
            //close gothic main menu
            //zCMenu.GetMenuByName(Program.Process, zCMenu.MainMenu).ScreenDone();

            //Enable Menu:
            ASCIIEncoding enc = new ASCIIEncoding();
            Program.Process.Write(enc.GetBytes("AAAAAA"), 0x890898);

            Version v = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            zERROR.GetZErr(Program.Process).Report(2, 'G', "GUC-Version: " + GUC.Options.Constants.VERSION +" - Build:" + v.ToString(), 0, "Program.cs", 0);

            ConnectionMessage.Write();

            GUCMenus._Background.Show();
            GUCMenus.Main.Open();
        }

        public override void Update()
        {
            InputHandler.Update();
            Program.client.Update();
        }
    }
}
