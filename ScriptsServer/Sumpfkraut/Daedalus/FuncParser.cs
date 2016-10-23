using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using GUC.Log;

namespace GUC.Scripts.Sumpfkraut.Daedalus
{
    class FuncParser
    {
        public static void ParseConstValues()
        {
            DirectoryInfo dir = new DirectoryInfo("Daedalus");
            if (!dir.Exists)
            {
                Logger.Log("FuncParser: No Daedalus folder found.");
                return;
            }

            //Logger.Log("FuncParser: Checking for funcs...");
            SearchFuncs(dir.GetFiles("*.d", SearchOption.AllDirectories));
            Logger.Log("FuncParser: {0} funcs parsed.", Funcs.Count);

            //File.WriteAllText("funcs.txt", "");
            //foreach (KeyValuePair<string, string> pair in Funcs)
            //    File.AppendAllText("funcs.txt", "'" + pair.Key + "'\r\n");
        }

        public static void Free()
        {
            Funcs.Clear();
        }

        readonly static Dictionary<string, string> Funcs = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        public static bool TryGetFunc(string funcName, out string funcBody)
        {
            return Funcs.TryGetValue(funcName, out funcBody);
        }

        static void SearchFuncs(FileInfo[] files)
        {
            Funcs.Clear();

            for (int i = 0; i < files.Length; i++)
            {
                using (Parser parser = new Parser(files[i].OpenRead()))
                {
                    while (parser.SkipTillLineStartsWith("func"))
                    {
                        string type = parser.ReadWord();
                        if (string.Compare(type, "void", true) != 0)
                            continue;

                        string name = parser.ReadTill("(");
                        if (string.IsNullOrWhiteSpace(name))
                            continue;
                        
                        string args = parser.ReadTill(")");
                        if (!string.IsNullOrWhiteSpace(args))
                            continue; // we just want void()s
                        
                        string nextWord = parser.ReadWord();
                        if (nextWord != "{")
                            continue;

                        string funcBody = parser.ReadTill("};");
                        if (string.IsNullOrWhiteSpace(funcBody))
                            continue;

                        Funcs.Add(name.Trim(), funcBody.Trim());
                    }
                }
            }
        }
    }
}
