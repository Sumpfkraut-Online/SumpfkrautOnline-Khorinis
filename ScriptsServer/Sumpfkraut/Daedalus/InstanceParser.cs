using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using GUC.Log;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;
using GUC.Scripts.Sumpfkraut.Visuals;

namespace GUC.Scripts.Sumpfkraut.Daedalus
{
    class InstanceParser
    {
        public static void AddInstances()
        {
            foreach (KeyValuePair<string, DDLInstance> pair in instances)
            {
                if (pair.Value is DDLNpc)
                    AddNPC(pair.Key, (DDLNpc)pair.Value);
                else if (pair.Value is DDLItem)
                    AddItem(pair.Key, (DDLItem)pair.Value);
            }
        }

        static void AddItem(string codeName, DDLItem instance)
        {
            DDLString str;
            if (!instance.TryGetValue("visual", out str))
                return;

            string modelName = Path.GetFileNameWithoutExtension(str.Value);

            ModelDef model;
            if (!ModelDef.TryGetModel(modelName, out model))
            {
                model = new ModelDef(modelName, str.Value);
                model.Create();
            }

            ItemDef item = ItemDef.Get(codeName);
            if (item == null)
            {
                item = new ItemDef(codeName);
            }
            else
            {
                item.Delete();
            }

            item.Model = model;

            if (instance.TryGetValue("name", out str))
                item.Name = str.Value;
            if (string.IsNullOrWhiteSpace(item.Name))
                item.Name = "no name (" + codeName + ")";

            if (instance.TryGetValue("effect", out str))
                item.Effect = str.Value;

            item.Create();
        }

        static void AddNPC(string codeName, DDLNpc instance)
        {
            DDLString str;
            if (!instance.TryGetValue("visual", out str))
                return;

            string modelName = Path.GetFileNameWithoutExtension(str.Value);

            ModelDef model;
            if (!ModelDef.TryGetModel(modelName, out model))
            {
                model = new ModelDef(modelName, str.Value);
                model.Create();
            }

            NPCDef npc = NPCDef.Get(codeName);
            if (npc == null)
            {
                npc = new NPCDef(codeName);
            }
            else
            {
                npc.Delete();
            }

            npc.Model = model;

            if (instance.TryGetValue("name", out str))
                npc.Name = str.Value;
            if (string.IsNullOrWhiteSpace(npc.Name))
                npc.Name = "no name (" + codeName + ")";

            if (instance.TryGetValue("bodymesh", out str))
                npc.BodyMesh = str.Value;
            if (instance.TryGetValue("headmesh", out str))
                npc.HeadMesh = str.Value;

            DDLInt dint;
            if (instance.TryGetValue("bodytex", out dint))
                npc.BodyTex = dint.Value;
            if (instance.TryGetValue("headtex", out dint))
                npc.HeadTex = dint.Value;
            
            npc.Create();
        }

        public static void ParseInstances()
        {
            DirectoryInfo dir = new DirectoryInfo("daedalus");
            if (!dir.Exists)
            {
                Logger.Log("InstanceParser: No daedalus folder found!");
                return;
            }

            FileInfo[] files = dir.GetFiles("*.d", SearchOption.AllDirectories);
            if (files.Length == 0)
            {
                Logger.Log("InstanceParser: No files to parse found!");
                return;
            }

            //Logger.Log("InstanceParser: Checking for instances...");
            ReadInstances(files);
            Logger.Log("InstanceParser: {0} instances parsed.", instances.Count);

            /*File.WriteAllText("instances.txt", "");
            foreach (KeyValuePair<string, DDLInstance> pair in instances)
            {
                if (pair.Value is DDLNpc)
                {
                    string name = "", armor = "";
                    DDLValueType v;
                    if (pair.Value.TryGetValue("name", out v) && v is DDLString)
                        name = ((DDLString)v).Value;
                    if (pair.Value.TryGetValue("armor", out v) && v is DDLString)
                        armor = ((DDLString)v).Value;

                    File.AppendAllText("instances.txt", "NPC: '" + pair.Key + "' Name=" + name + " Armor=" + armor + " Eq: ");
                    foreach(string item in ((DDLNpc)pair.Value).Equipment)
                        File.AppendAllText("instances.txt", " " + item);
                    File.AppendAllText("instances.txt", "\r\n");
                }
                else if (pair.Value is DDLItem)
                {
                    string name = "";
                    int dmg = 0, protection = 0;
                    DDLValueType v;
                    if (pair.Value.TryGetValue("name", out v) && v is DDLString)
                        name = ((DDLString)v).Value;
                    if (pair.Value.TryGetValue("visual", out v) && v is DDLArray<DDLInt>)
                        dmg = ((DDLArray<DDLInt>)v).GetValue(2).Value;
                    if (pair.Value.TryGetValue("bodymesh", out v) && v is DDLString)
                        protection = ((DDLArray<DDLInt>)v).GetValue(2).Value;

                    File.AppendAllText("instances.txt", "ITEM: '" + pair.Key + "' Name=" + name + "\r\n");
                }
            }*/
        }

        public static void Free()
        {
            instances.Clear();
        }

        readonly static Dictionary<string, DDLInstance> instances = new Dictionary<string, DDLInstance>(StringComparer.OrdinalIgnoreCase);

        static void ReadInstances(FileInfo[] files)
        {
            instances.Clear();

            for (int i = 0; i < files.Length; i++)
            {
                using (Parser parser = new Parser(files[i].OpenRead()))
                {
                    while (parser.SkipTillLineStartsWith("INSTANCE"))
                    {
                        string codeName = parser.ReadTill("(");
                        if (string.IsNullOrWhiteSpace(codeName)) continue;

                        

                        string type = parser.ReadTill(")");
                        if (string.IsNullOrWhiteSpace(type)) continue;

                        string nextWord = parser.ReadWord();
                        if (nextWord != "{") continue;

                        type = type.Trim();
                        codeName = codeName.Trim();

                        DDLInstance instance;
                        if (string.Compare(type, "C_NPC", true) == 0)
                        {
                            instance = new DDLNpc();
                        }
                        else if (string.Compare(type, "C_ITEM", true) == 0)
                        {
                            instance = new DDLItem();
                        }
                        else if (PrototypeParser.TryGetPrototype(type, out instance))
                        {
                            if (instance is DDLNpc)
                            {
                                instance = new DDLNpc((DDLNpc)instance);
                            }
                            else if (instance is DDLItem)
                            {
                                instance = new DDLItem((DDLItem)instance);
                            }
                            else
                            {
                                parser.SkipTill("};");
                                continue;
                            }
                        }
                        else
                        {
                            parser.SkipTill("};");
                            continue;
                        }

                        instance.HandleTextBody(parser.ReadTill("};"));
                        instances.Add(codeName, instance);
                    }
                }
            }
        }
    }
}
