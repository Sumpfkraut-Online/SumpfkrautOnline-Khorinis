using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Log;
using System.IO;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;
using GUC.Scripts.Sumpfkraut.Visuals;

namespace GUC.Scripts.Sumpfkraut.Daedalus
{
    static class ItemParser
    {
        const string DaedalusItemsFolder = "items";

        public static void ParseItems()
        {
            DirectoryInfo dir = new DirectoryInfo(DaedalusItemsFolder);
            if (!dir.Exists)
            {
                Logger.Log("ItemParser: No {0} folder found!", DaedalusItemsFolder);
                return;
            }

            FileInfo[] files = dir.GetFiles("*.d", SearchOption.AllDirectories);
            if (files.Length == 0)
            {
                Logger.Log("ItemParser: No files to parse found!");
                return;
            }

            Logger.Log("ItemParser: Checking for const values...");
            SearchConstValues(files);
            Logger.Log("ItemParser: {0} const values parsed. Reading items...", ConstantValues.Count);
            int items = ReadItems(files);
            Logger.Log("ItemParser: {0} items parsed.", items);
        }

        static Dictionary<string, object> ConstantValues = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        static void SearchConstValues(FileInfo[] files)
        {
            ConstantValues.Clear();

            for (int i = 0; i < files.Length; i++)
            {
                using (Parser parser = new Parser(files[i].OpenRead()))
                {
                    while (parser.SkipTill("const"))
                    {
                        var tuple = parser.ReadEquation();
                        if (tuple == null || string.IsNullOrWhiteSpace(tuple.Item1) || string.IsNullOrWhiteSpace(tuple.Item2))
                            continue;

                        string[] strs = tuple.Item1.Split(default(char[]), StringSplitOptions.RemoveEmptyEntries);
                        if (strs.Length != 2) continue;

                        string type = strs[0];
                        string name = strs[1];

                        if (string.Compare(type, "int", true) == 0)
                        {
                            int value;
                            if (TryParse(tuple.Item2, out value))
                            {
                                ConstantValues.Add(name, value);
                            }
                        }
                        else if (string.Compare(type, "string", true) == 0)
                        {
                            string value;
                            if (TryParse(tuple.Item2, out value))
                            {
                                ConstantValues.Add(name, value);
                            }
                        }
                        else if (string.Compare(type, "float", true) == 0)
                        {
                            float value;
                            if (float.TryParse(tuple.Item2, out value))
                            {
                                ConstantValues.Add(name, value);
                            }
                        }
                    }
                }
            }
        }

        static int ReadItems(FileInfo[] files)
        {
            int newItems = 0;
            for (int i = 0; i < files.Length; i++)
            {
                using (Parser parser = new Parser(files[i].OpenRead()))
                {
                    while (parser.SkipTill("INSTANCE"))
                    {
                        string codeName = parser.ReadTill("(");
                        if (codeName == null) continue;

                        string type = parser.ReadTill(")");
                        if (type == null) continue;

                        string nextWord = parser.ReadWord();
                        if (nextWord != "{") continue;

                        if (string.Compare(type.Trim(), "C_ITEM", true) == 0)
                        {
                            if (ParseItem(codeName.Trim(), parser.ReadTill("};")))
                                newItems++;
                        }
                        else
                        {
                            if (!parser.SkipTill("};"))
                                break;
                        }
                    }
                }
            }
            return newItems;
        }

        static readonly Dictionary<string, object> PropertyDict = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
        {
            { "visual", "" },
            { "name", "" },
            { "damageTotal", 0 },
            { "range", 0 },
            { "material", 0 },
            { "mainFlags", 0 },
            { "flags", 0 },
            { "visualChange", "" },
            { "protection [PROT_EDGE]", 0 },
            { "effect", "" },
        };

        static bool ParseItem(string codeName, string text)
        {
            if (string.IsNullOrWhiteSpace(codeName) || string.IsNullOrWhiteSpace(text))
                return false;

            // reset the dictionary
            foreach (string key in PropertyDict.Keys.ToList())
            {
                object obj = PropertyDict[key];
                if (obj is int) PropertyDict[key] = 0;
                else if (obj is string) PropertyDict[key] = "";
                else if (obj is float) PropertyDict[key] = 0f;
            }

            using (MemoryStream ms = new MemoryStream())
            using (StreamWriter sw = new StreamWriter(ms))
            {
                sw.Write(text);
                sw.Flush();
                ms.Position = 0;

                using (Parser parser = new Parser(ms))
                {
                    Tuple<string, string> tuple;
                    while ((tuple = parser.ReadEquation()) != null)
                    {
                        string valueName = tuple.Item1.Trim();
                        if (string.IsNullOrWhiteSpace(valueName))
                            continue;

                        if (PropertyDict.ContainsKey(valueName))
                        {
                            object obj = PropertyDict[valueName];
                            if (obj is int)
                            {
                                int result;
                                if (TryParse(tuple.Item2, out result))
                                {
                                    PropertyDict[valueName] = result;
                                }
                            }
                            else if (obj is string)
                            {
                                string result;
                                if (TryParse(tuple.Item2, out result))
                                {
                                    PropertyDict[valueName] = result;
                                }
                            }
                            if (obj is float)
                            {
                                float result;
                                if (TryParse(tuple.Item2, out result))
                                {
                                    PropertyDict[valueName] = result;
                                }
                            }
                        }
                    }
                }
            }

            string modelStr = (string)PropertyDict["visual"];
            if (string.IsNullOrWhiteSpace(modelStr))
            {
                Logger.Log("ItemParser: Could not find Visual for '{0}'.", codeName);
                return false;
            }

            string nameStr = (string)PropertyDict["name"];
            if (string.IsNullOrWhiteSpace(nameStr))
            {
                Logger.Log("ItemParser: Could not find Name for '{0}'.", codeName);
                return false;
            }

            ItemDef def = new ItemDef(codeName);

            // set model
            string modelCodeName = Path.GetFileNameWithoutExtension(modelStr);
            ModelDef model;
            if (!ModelDef.TryGetModel(modelCodeName, out model))
            {
                model = new ModelDef(modelCodeName, modelStr);
                model.Create();
            }
            def.Model = model;

            // set name
            def.Name = nameStr;

            // set damage
            def.Damage = (int)PropertyDict["damageTotal"];

            // set range
            def.Range = (int)PropertyDict["range"];

            // set material
            int material = (int)PropertyDict["material"];
            if (!Enum.IsDefined(typeof(ItemMaterials), (byte)material))
            {
                Logger.Log("ItemParser: Unknown Material '{0}' for '{1}'.", material, codeName);
            }
            def.Material = (ItemMaterials)material;

            // set item type
            def.ItemType = GetItemTypeFromFlags((int)PropertyDict["mainflags"], (int)PropertyDict["flags"]);

            // set visual change
            def.VisualChange = (string)PropertyDict["visualchange"];

            // set protection
            def.Protection = (int)PropertyDict["protection [PROT_EDGE]"];

            // set effect
            def.Effect = (string)PropertyDict["effect"];

            def.Create();
            return true;
        }

        static bool TryParse(string s, out string value)
        {
            if (!string.IsNullOrWhiteSpace(s))
            {
                string valueStr = s.Trim();

                if (valueStr.Length >= 2 && valueStr[0] == '"' && valueStr[valueStr.Length - 1] == '"')
                {
                    value =  valueStr.Substring(1, valueStr.Length - 2);
                    return true;
                }
                else
                {
                    object obj;
                    if (ConstantValues.TryGetValue(valueStr, out obj) && obj is string)
                    {
                        value = (string)obj;
                        return true;
                    }
                }
            }

            value = null;
            return false;
        }
        
        static bool TryParse(string s, out int value)
        {
            if (!string.IsNullOrWhiteSpace(s))
            {
                string valueStr = s.Trim();

                // try a normal parse
                if (int.TryParse(valueStr, out value))
                    return true;

                // maybe it's bit shifting
                int index = valueStr.IndexOf("<<");
                if (index != -1)
                {
                    int left, right;
                    if (int.TryParse(valueStr.Remove(index).Trim(), out left)
                        && int.TryParse(valueStr.Substring(index + 2).Trim(), out right))
                    {
                        value = left << right;
                        return true;
                    }
                }

                // let's check the dictionary
                object obj;
                if (ConstantValues.TryGetValue(valueStr, out obj) && obj is int)
                {
                    value = (int)obj;
                    return true;
                }
            }
            value = 0;
            return false;
        }

        static bool TryParse(string s, out float value)
        {
            if (!string.IsNullOrWhiteSpace(s))
            {
                string valueStr = s.Trim();

                // try a normal parse
                if (float.TryParse(valueStr, out value))
                    return true;

                // let's check the dictionary
                object obj;
                if (ConstantValues.TryGetValue(valueStr, out obj))
                {
                    if (obj is float)
                    {
                        value = (float)obj;
                        return true;
                    }
                    else if (obj is int)
                    {
                        value = (float)obj;
                        return true;
                    }
                }
            }
            value = 0;
            return false;
        }

        #region ItemTypes

        static ItemTypes GetItemTypeFromFlags(int mainFlags, int flags)
        {
            if ((flags & ItemFlags.ITEM_SWD) != 0 || (flags & ItemFlags.ITEM_AXE) != 0)
            {
                return ItemTypes.Wep1H;
            }
            else if ((flags & ItemFlags.ITEM_2HD_SWD) != 0 || (flags & ItemFlags.ITEM_2HD_AXE) != 0)
            {
                return ItemTypes.Wep2H;
            }
            else if ((mainFlags & MainFlags.ITEM_KAT_ARMOR) != 0)
            {
                return ItemTypes.Armor;
            }
            else if ((flags & ItemFlags.ITEM_BOW) != 0)
            {
                return ItemTypes.WepBow;
            }
            else if ((flags & ItemFlags.ITEM_CROSSBOW) != 0)
            {
                return ItemTypes.WepXBow;
            }
            else if ((mainFlags & MainFlags.ITEM_KAT_MUN) != 0)
            {
                if ((flags & ItemFlags.ITEM_BOW) != 0)
                {
                    return ItemTypes.AmmoBow;
                }
                else if ((flags & ItemFlags.ITEM_CROSSBOW) != 0)
                {
                    return ItemTypes.AmmoXBow;
                }
            }

            return ItemTypes.Misc;
        }

        public abstract class MainFlags
        {
            // categories (mainflag)
            public const int ITEM_KAT_NONE = 1 << 0;  // Sonstiges
            public const int ITEM_KAT_NF = 1 << 1;  // Nahkampfwaffen
            public const int ITEM_KAT_FF = 1 << 2;  // Fernkampfwaffen
            public const int ITEM_KAT_MUN = 1 << 3; // Munition (MULTI)
            public const int ITEM_KAT_ARMOR = 1 << 4;  // Ruestungen
            public const int ITEM_KAT_FOOD = 1 << 5;  // Nahrungsmittel (MULTI)
            public const int ITEM_KAT_DOCS = 1 << 6;  // Dokumente
            public const int ITEM_KAT_POTIONS = 1 << 7;  // Traenke
            public const int ITEM_KAT_LIGHT = 1 << 8;  // Lichtquellen
            public const int ITEM_KAT_RUNE = 1 << 9;  // Runen/Scrolls
            public const int ITEM_KAT_MAGIC = 1 << 31;  // Ringe/Amulette/Guertel
            public const int ITEM_KAT_KEYS = ITEM_KAT_NONE;
        }

        public abstract class ItemFlags
        {
            // weapon type (flags)
            public const int ITEM_DAG = 1 << 13;  // (OBSOLETE!)
            public const int ITEM_SWD = 1 << 14;  // Schwert
            public const int ITEM_AXE = 1 << 15;  // Axt
            public const int ITEM_2HD_SWD = 1 << 16;  // Zweihaender
            public const int ITEM_2HD_AXE = 1 << 17;  // Streitaxt
            public const int ITEM_SHIELD = 1 << 18;  // (OBSOLETE!)
            public const int ITEM_BOW = 1 << 19;  // Bogen
            public const int ITEM_CROSSBOW = 1 << 20;  // Armbrust
            // magic type (flags)
            public const int ITEM_RING = 1 << 11;  // Ring
            public const int ITEM_AMULET = 1 << 22;  // Amulett
            public const int ITEM_BELT = 1 << 24;  // Guertel
            // attributes (flags)
            public const int ITEM_DROPPED = 1 << 10;  // (INTERNAL!)
            public const int ITEM_MISSION = 1 << 12;  // Missionsgegenstand
            public const int ITEM_MULTI = 1 << 21;  // Stapelbar
            public const int ITEM_NFOCUS = 1 << 23;  // (INTERNAL!)
            public const int ITEM_CREATEAMMO = 1 << 25;  // Erzeugt Munition selbst (magisch)
            public const int ITEM_NSPLIT = 1 << 26;  // Kein Split-Item (Waffe als Interact-Item!)
            public const int ITEM_DRINK = 1 << 27;  // (OBSOLETE!)
            public const int ITEM_TORCH = 1 << 28;  // Fackel
            public const int ITEM_THROW = 1 << 29;  // (OBSOLETE!)
            public const int ITEM_ACTIVE = 1 << 30;  // (INTERNAL!)
        }

        #endregion
    }
}
