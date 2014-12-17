using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.zClasses;
using WinApi;
using Gothic.zTypes;
using GUC.WorldObjects;

namespace GUC.Hooks
{
    class hParser
    {
        static bool _loadDatBlocked = false;
        static HookInfos _infoLoadDat = null;

        static HookInfos _infoLoadParserFile = null;
        public static void HookLoadDat()
        {
            _infoLoadDat = Process.ThisProcess().Hook("UntoldChapter\\DLL\\GUC.dll", typeof(hParser).GetMethod("hook_LoadDat"), 0x0078E900, 7, 1);

            _infoLoadParserFile = Process.ThisProcess().Hook("UntoldChapter\\DLL\\GUC.dll", typeof(hParser).GetMethod("hook_LoadParserFile"), 0x006C4BE0, 6, 1);

            Process.ThisProcess().Write(new byte[] { 0x33, 0xC0, 0xC2, 0x04, 0x00 }, _infoLoadParserFile.oldFuncInNewFunc.ToInt32());
        }

        public static void BlockLoadDat()
        {
            if (_infoLoadDat == null)
                throw new Exception("Call HookLoadDat first!");
            if (_loadDatBlocked)
                return;

            Process.ThisProcess().Write(new byte[] { 0x33, 0xC0, 0xC2, 0x04, 0x00 }, _infoLoadDat.oldFuncInNewFunc.ToInt32());

            _loadDatBlocked = true;
        }

        public static void UnblockLoadDat()
        {
            if (_infoLoadDat == null)
                throw new Exception("Call HookLoadDat first!");
            if (!_loadDatBlocked)
                return;
            Process.ThisProcess().Write(new byte[] { 0x6A, 0xFF, 0x68, 0x10, 0xA4 }, _infoLoadDat.oldFuncInNewFunc.ToInt32());
            _loadDatBlocked = false;
        }

        public static Int32 hook_LoadDat(String message)
        {
            try
            {
                int parameterAddress = Convert.ToInt32(message);
                zString str = new zString(Process.ThisProcess(), Process.ThisProcess().ReadInt(parameterAddress + 4));
                if (str.Value.ToUpper().Trim() == "GOTHIC.DAT")
                {
                    
                    GUC.States.StartupState.initDefaultScripts();
                    BlockLoadDat();
                }
                else
                {
                    UnblockLoadDat();
                }



                
                
                zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', "Load-Dat: "+str.Value, 0, "Program.cs", 0);
            }
            catch (Exception ex) {
                zERROR.GetZErr(Process.ThisProcess()).Report(4, 'G', ex.ToString(), 0, "Program.cs", 0);
            }
            return 0;
        }




        public static Int32 hook_LoadParserFile(String message)
        {
            try
            {
                int parameterAddress = Convert.ToInt32(message);
                zString str = new zString(Process.ThisProcess(), Process.ThisProcess().ReadInt(parameterAddress + 4));
                zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', "LoadParserFile: " + str.Value, 0, "Program.cs", 0);

                zCParser parser = zCParser.getParser(Process.ThisProcess());
                parser.Reset();
                oCGame.Game(Process.ThisProcess()).DefineExternals_Ulfi(parser);
                parser.EnableTreeLoad(0);
                parser.EnableTreeSave(0);

                GUC.States.StartupState.initDefaultScripts();

                zString str2 = zString.Create(Process.ThisProcess(), "C_NPC");
                parser.AddClassOffset(str2, 0x120);
                str2.Dispose();

                str2 = zString.Create(Process.ThisProcess(), "C_ITEM");
                parser.AddClassOffset(str2, 0x120);
                str2.Dispose();

                parser.MainFileName.Set(GUC.States.StartupState.srcFile);

                parser.CreatePCode();
                parser.Error();
            }
            catch (Exception ex)
            {
                zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', ex.ToString(), 0, "Program.cs", 0);
            }
            return 0;
        }




        static int symbol_GetValue_P_Type = 0;
        public static Int32 hook_Symbol_GetValue(String message)
        {
            try
            {
                Process process = Process.ThisProcess();
                int address = Convert.ToInt32(message);

                zCPar_Symbol symbol = new zCPar_Symbol(process, process.ReadInt(address));

                String name = symbol.Name.Value.Trim().ToLower();
                if (name.Equals("spellfxaniletters"))
                {
                    zString str = new zString(process, process.ReadInt(address + 4));
                    int id = process.ReadInt(address + 8);

                    String value = "FBT";
                    Spell spell = null;
                    Spell.SpellDict.TryGetValue(id, out spell);
                    if (spell == null)
                        spell = new Spell();
                    value = spell.AniName;

                    //Generating Buffer with String:
                    
                    System.Text.Encoding enc = System.Text.Encoding.Default;
                    byte[] arr = enc.GetBytes(value);

                    //Creating Pointer to char*
                    IntPtr charArr = process.Alloc((uint)arr.Length + 1);
                    if (arr.Length > 0)
                        process.Write(arr, charArr.ToInt32());

                    //Calling constructor and free char*
                    process.THISCALL<NullReturnCall>((uint)str.Address, (uint)0x004010C0, new CallValue[] { new IntArg(charArr.ToInt32()) });
                    process.Free(charArr, (uint)arr.Length + 1);


                    if (symbol_GetValue_P_Type != 1)
                    {
                        process.Write(new byte[]{0xC2, 0x08, 0x00}, Program.ParSymbol_GetValueHook.oldFuncInNewFunc.ToInt32());
                    
                        symbol_GetValue_P_Type = 1;
                    }

                }
                else
                {
                    if (symbol_GetValue_P_Type == 0)
                        return 0;
                    process.Write(Program.ParSymbol_GetValueHook.oldFunc, Program.ParSymbol_GetValueHook.oldFuncInNewFunc.ToInt32());
                    symbol_GetValue_P_Type = 0;
                }

            }
            catch (Exception ex)
            {
                zERROR.GetZErr(Process.ThisProcess()).Report(4, 'G', ex.ToString(), 0, "Program.cs", 0);
            }
            return 0;
        }
    }
}
