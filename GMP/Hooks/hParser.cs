using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;
using Gothic;
using GUC.Log;

namespace GUC.Client.Hooks
{
    class hParser
    {
        static bool _loadDatBlocked = false;
        static HookInfos _infoLoadDat = null;

        static HookInfos _infoLoadParserFile = null;
        public static void Init()
        {
            _infoLoadDat = Process.Hook(Constants.GUCDll, typeof(hParser).GetMethod("hook_LoadDat"), 0x0078E900, 7, 1);
            //_infoLoadParserFile = Process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(hParser).GetMethod("hook_LoadParserFile"), 0x006C4BE0, 6, 1);

            //Process.Write(new byte[] { 0x33, 0xC0, 0xC2, 0x04, 0x00 }, _infoLoadParserFile.oldFuncInNewFunc.ToInt32());
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
                    //States.StartupState.initDefaultScripts();
                    BlockLoadDat();
                }
                else
                {
                    UnblockLoadDat();
                }
                Logger.LogWarning("LoadDat: '{0}', blocked: {1}", str, _loadDatBlocked);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
            return 0;
        }




     /*   public static Int32 hook_LoadParserFile(String message)
        {
            try
            {
                int parameterAddress = Convert.ToInt32(message);
                zString str = new zString(Program.Process, Program.Process.ReadInt(parameterAddress + 4));
                zERROR.GetZErr(Program.Process).Report(2, 'G', "LoadParserFile: " + str.Value, 0, "Program.cs", 0);

                zCParser parser = zCParser.getParser(Program.Process);
                parser.Reset();
                oCGame.Game(Program.Process).DefineExternals_Ulfi(parser);
                parser.EnableTreeLoad(0);
                parser.EnableTreeSave(0);

                States.StartupState.initDefaultScripts();

                zString str2 = zString.Create(Program.Process, "C_NPC");
                parser.AddClassOffset(str2, 0x120);
                str2.Dispose();

                str2 = zString.Create(Program.Process, "C_ITEM");
                parser.AddClassOffset(str2, 0x120);
                str2.Dispose();

                parser.MainFileName.Set(States.StartupState.srcFile);

                parser.CreatePCode();
                parser.Error();
            }
            catch (Exception ex)
            {
                zERROR.GetZErr(Program.Process).Report(2, 'G', ex.ToString(), 0, "Program.cs", 0);
            }
            return 0;
        }*/



        
    }
}
