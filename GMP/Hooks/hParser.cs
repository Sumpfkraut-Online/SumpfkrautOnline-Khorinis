using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;
using Gothic.Types;
using GUC.Log;
using System.IO;
using System.Reflection;

namespace GUC.Client.Hooks
{
    static class hParser
    {
        static bool _loadDatBlocked = false;
        static HookInfos _infoLoadDat = null;

        static HookInfos _infoLoadParserFile = null;
        public static void AddHooks()
        {
            if (_infoLoadDat != null)
                return;

            _infoLoadDat = Process.Hook(Program.GUCDll, typeof(hParser).GetMethod("hook_LoadDat"), 0x0078E900, 7, 1);
            _infoLoadParserFile = Process.Hook(Program.GUCDll, typeof(hParser).GetMethod("hook_LoadParserFile"), 0x006C4BE0, 6, 1);

            Process.Write(new byte[] { 0x33, 0xC0, 0xC2, 0x04, 0x00 }, _infoLoadParserFile.oldFuncInNewFunc.ToInt32());

            Logger.Log("Added parser hooks.");
        }

        static void BlockLoadDat()
        {
            if (_loadDatBlocked)
                return;
            _loadDatBlocked = true;

            Process.Write(new byte[] { 0x33, 0xC0, 0xC2, 0x04, 0x00 }, _infoLoadDat.oldFuncInNewFunc.ToInt32());
        }

        static void UnblockLoadDat()
        {
            if (!_loadDatBlocked)
                return;
            _loadDatBlocked = false;

            Process.Write(new byte[] { 0x6A, 0xFF, 0x68, 0x10, 0xA4 }, _infoLoadDat.oldFuncInNewFunc.ToInt32());
        }

        public static Int32 hook_LoadDat(String message)
        {
            try
            {
                int address = Convert.ToInt32(message);
                zString str = new zString(Process.ReadInt(address + 4));
                string datName = str.ToString().Trim().ToUpper();
                if (datName == "GOTHIC.DAT" || datName == "FIGHT.DAT" || datName == "MENU.DAT")
                {
                    if (datName == "GOTHIC.DAT")
                        initDefaultScripts();

                    BlockLoadDat();
                }
                else
                {
                    UnblockLoadDat();
                }
                Logger.Log("LoadDat: '{0}', blocked: {1}", str, _loadDatBlocked);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
            return 0;
        }

        
        public static Int32 hook_LoadParserFile(String message)
        {
            try
            {
                int parameterAddress = Convert.ToInt32(message);
                zString str = new zString(Process.ReadInt(parameterAddress + 4));
                Logger.Log("LoadParserFile: " + str);

                Process.THISCALL<NullReturnCall>(0xAB40C0, 0x00793100); //parser.reset
                Process.THISCALL<NullReturnCall>(Process.ReadInt(Gothic.oCGame.ogame), 0x006D4780, new IntArg(0xAB40C0)); //Define_ulfi_externals
                Process.THISCALL<NullReturnCall>(0xAB40C0, 0x00793460, new IntArg(0)); //parser.enabletreeload(0)
                Process.THISCALL<NullReturnCall>(0xAB40C0, 0x00793440, new IntArg(0)); //parser.enabletreesave(0)

                initDefaultScripts();
                
                using (zString z = zString.Create("C_NPC"))
                    Process.THISCALL<NullReturnCall>(0xAB40C0, 0x00794730, z, new IntArg(0x120)); // parser.AddClassOffset

                using (zString z = zString.Create("C_ITEM"))
                    Process.THISCALL<NullReturnCall>(0xAB40C0, 0x00794730, z, new IntArg(0x120)); // parser.AddClassOffset
                
                zString mainfile = new zString(0xAB40C0 + 0x2074);
                    mainfile.Set(srcFile);

                Process.THISCALL<NullReturnCall>(0xAB40C0, 0x007900E0); //parser.createPCode
                Process.THISCALL<NullReturnCall>(0xAB40C0, 0x0078E730); //parser.error
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
            return 0;
        }

        static String srcFile = null;
        static void initDefaultScripts()
        {
            string dPath = Program.ProjectPath + "Daedalus";

            if (!Directory.Exists(dPath))
                Directory.CreateDirectory(dPath);

            String[] arr = new String[] { "GUC.Client.Resources.Constants.d", "GUC.Client.Resources.Classes.d", "GUC.Client.Resources.AI_Constants.d",
                "GUC.Client.Resources.BodyStates.d", "GUC.Client.Resources.Focus.d", "GUC.Client.Resources.Species.d", "GUC.Client.Resources.NPC_Default.d",
                "GUC.Client.Resources.PC_Hero.d" };

            zString str = null;
            String fileList = "";
            foreach (String internalFile in arr)
            {
                try
                {
                    using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream(internalFile), Encoding.Default))
                    {
                        String file = dPath + "\\" + internalFile.Substring("GUC.Client.Resources.".Length);
                        File.WriteAllText(file, sr.ReadToEnd(), Encoding.Default);
                        fileList += Path.GetFileName(file.ToUpper()) + "\r\n";

                        //using (str = zString.Create(file.ToUpper()))
                        //    Process.THISCALL<NullReturnCall>(0xAB40C0, 0x0078F660, str);
                    }
                }
                catch (Exception e)
                {
                    Logger.LogError(internalFile + ": " + e);
                }
            }


            String file_FileList = dPath + "\\GUC.src";
            srcFile = file_FileList;
            File.WriteAllText(file_FileList, fileList);

            Logger.Log("Parse " + file_FileList);
            using (str = zString.Create(file_FileList.ToUpper()))
                Process.THISCALL<NullReturnCall>(0xAB40C0, 0x0078EE20, str);
            
            using (zString z = zString.Create("C_NPC"))
            {
                int symbol = Process.THISCALL<IntArg>(0xAB40C0, 0x007938D0, z); // parser.GetSymbol
                Process.THISCALL<NullReturnCall>(symbol, 0x007A2F40, new IntArg(0x120)); //parsymbol.SetClassOffset
            }

            using (zString z = zString.Create("C_ITEM"))
            {
                int symbol = Process.THISCALL<IntArg>(0xAB40C0, 0x007938D0, z); // parser.GetSymbol
                Process.THISCALL<NullReturnCall>(symbol, 0x007A2F40, new IntArg(0x120)); //parsymbol.SetClassOffset
            }
            
            Logger.Log("Startup-Scripts-parsed!");
        }
    }
}
