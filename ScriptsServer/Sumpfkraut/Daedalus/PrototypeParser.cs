using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using GUC.Log;

namespace GUC.Scripts.Sumpfkraut.Daedalus
{
    class PrototypeParser
    {
        public static void ParsePrototypes()
        {
            DirectoryInfo dir = new DirectoryInfo("daedalus");
            if (!dir.Exists)
            {
                Logger.Log("PrototypeParser: No daedalus folder found!");
                return;
            }

            FileInfo[] files = dir.GetFiles("*.d", SearchOption.AllDirectories);
            if (files.Length == 0)
            {
                Logger.Log("PrototypeParser: No files to parse found!");
                return;
            }

            //Logger.Log("PrototypeParser: Checking for prototypes...");
            ReadPrototypes(files);
            Logger.Log("PrototypeParser: {0} prototypes parsed.", prototypes.Count);


            //File.WriteAllText("prototypes.txt", "");
            //foreach (string key in prototypes.Keys)
            //    File.AppendAllText("prototypes.txt", "'" + key + "'\r\n");
        }

        public static void Free()
        {
            prototypes.Clear();
        }

        readonly static Dictionary<string, DDLInstance> prototypes = new Dictionary<string, DDLInstance>(StringComparer.OrdinalIgnoreCase);

        public static bool TryGetPrototype(string name, out DDLInstance prototype)
        {
            return prototypes.TryGetValue(name, out prototype);
        }

        static void ReadPrototypes(FileInfo[] files)
        {
            prototypes.Clear();
            
            for (int i = 0; i < files.Length; i++)
            {
                using (Parser parser = new Parser(files[i].OpenRead()))
                {
                    while (parser.SkipTillLineStartsWith("PROTOTYPE"))
                    {
                        string codeName = parser.ReadTill("(");
                        if (codeName == null) continue;

                        string type = parser.ReadTill(")");
                        if (type == null) continue;

                        string nextWord = parser.ReadWord();
                        if (nextWord != "{") continue;

                        type = type.Trim();

                        DDLInstance instance;
                        if (string.Compare(type, "C_NPC", true) == 0)
                        {
                            instance = new DDLNpc();
                        }
                        else if (string.Compare(type, "C_ITEM", true) == 0)
                        {
                            instance = new DDLItem();
                        }
                        else
                        {
                            parser.SkipTill("};");
                            continue;
                        }

                        instance.HandleTextBody(parser.ReadTill("};"));
                        prototypes.Add(codeName.Trim(), instance);
                    }
                }
            }
        }
    }
}
