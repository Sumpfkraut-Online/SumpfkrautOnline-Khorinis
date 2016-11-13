using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;
using Gothic.Types;
using GUC.Log;
using System.IO;
using System.Reflection;

namespace GUC.Hooks
{
    static class hParser
    {
        static bool inited = false;
        public static void AddHooks()
        {
            if (inited)
                return;
            inited = true;

            Process.AddHook(hook_LoadDat, 0x0078E900, 7, 1);
            var h = Process.AddHook(hook_LoadParserFile, 0x006C4BE0, 6, 1);
            Process.Write(new byte[6] { 0x33, 0xC0, 0xC2, 0x04, 0x00, 0x00 }, h.OldInNewAddress); // block it

            Logger.Log("Added parser hooks.");
        }

        static void hook_LoadDat(Hook hook)
        {
            try
            {
                zString str = new zString(hook.GetArgument(0));
                string datName = str.ToString().Trim().ToUpper();
                if (datName == "GOTHIC.DAT" || datName == "FIGHT.DAT" || datName == "MENU.DAT")
                {
                    if (datName == "GOTHIC.DAT")
                        initDefaultScripts();

                    Process.Write(new byte[7] { 0x33, 0xC0, 0xC2, 0x04, 0x00, 0x00, 0x00 }, hook.OldInNewAddress); // block it
                    Logger.Log("LoadDat: '{0}', blocked!", str);
                }
                else
                {
                    Process.Write(hook.GetOldCode(), hook.OldInNewAddress); // unblock it
                    Logger.Log("LoadDat: '{0}'", str);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
        }

        static void hook_LoadParserFile(Hook hook)
        {
            try
            {
                zString str = new zString(hook.GetArgument(0));
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
                mainfile.Set("GUC.src");

                Process.THISCALL<NullReturnCall>(0xAB40C0, 0x007900E0); //parser.createPCode
                Process.THISCALL<NullReturnCall>(0xAB40C0, 0x0078E730); //parser.error
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
        }

        static void initDefaultScripts()
        {
            string dPath = Program.GetProjectPath(@"_work\data\scripts");

            if (!Directory.Exists(dPath))
                Directory.CreateDirectory(dPath);

            String[] arr = new String[] { "GUC.Resources.Constants.d", "GUC.Resources.Classes.d", "GUC.Resources.AI_Constants.d",
                "GUC.Resources.BodyStates.d", "GUC.Resources.Focus.d", "GUC.Resources.Species.d", "GUC.Resources.NPC_Default.d" };

            StringBuilder fileList = new StringBuilder(100);
            foreach (String internalFile in arr)
            {
                try
                {
                    using (var rs = Assembly.GetExecutingAssembly().GetManifestResourceStream(internalFile))
                    {
                        string file = Path.Combine(dPath, internalFile.Substring(14)); //("GUC.Resources.".Length));
                        using (var fs = new FileStream(file, FileMode.Create, FileAccess.Write))
                            rs.CopyTo(fs);

                        fileList.AppendLine(Path.GetFileName(file));

                        //using (zString str = zString.Create(file.ToUpper()))
                        //    Process.THISCALL<NullReturnCall>(0xAB40C0, 0x0078F660, str);
                    }
                }
                catch (Exception e)
                {
                    Logger.LogError(internalFile + ": " + e);
                }
            }


            string file_FileList = Path.Combine(dPath, "GUC.src");
            File.WriteAllText(file_FileList, fileList.ToString());

            Logger.Log("Parse " + file_FileList);
            using (zString str = zString.Create("GUC.src"))
                Process.THISCALL<NullReturnCall>(0xAB40C0, 0x0078EE20, str); // zCParse::ParseSource

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

            Logger.Log("Daedalus scripts parsed!");
        }
    }
}
