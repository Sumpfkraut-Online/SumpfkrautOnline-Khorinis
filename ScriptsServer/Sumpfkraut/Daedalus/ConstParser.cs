using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using GUC.Log;

namespace GUC.Scripts.Sumpfkraut.Daedalus
{
    static class ConstParser
    {
        public static void ParseConstValues()
        {
            DirectoryInfo dir = new DirectoryInfo("Daedalus");
            if (!dir.Exists)
            {
                Logger.Log("ConstParser: No Daedalus folder found.");
                return;
            }

            //Logger.Log("ConstParser: Checking for const values...");
            SearchConstValues(dir.GetFiles("*.d", SearchOption.AllDirectories));
            Logger.Log("ConstParser: {0} const values parsed.", ConstantValues.Count);

            //File.WriteAllText("consts.txt", "");
            //foreach (KeyValuePair<string, object> pair in ConstantValues)
            //    File.AppendAllText("consts.txt", "'" + pair.Key + "' = '" + pair.Value + "'\r\n");
        }

        public static void Free()
        {
            ConstantValues.Clear();
        }

        readonly static Dictionary<string, object> ConstantValues = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        static void SearchConstValues(FileInfo[] files)
        {
            ConstantValues.Clear();

            for (int i = 0; i < files.Length; i++)
            {
                using (Parser parser = new Parser(files[i].OpenRead()))
                {
                    while (parser.SkipTillLineStartsWith("const"))
                    {
                        Tuple<string, string> func;
                        Tuple<string, string, string> equation;
                        if (!parser.ReadNextCommand(out func, out equation) || equation == null)
                            continue;
                        
                        if (equation == null || string.IsNullOrWhiteSpace(equation.Item1) || string.IsNullOrWhiteSpace(equation.Item2))
                            continue;

                        string[] strs = equation.Item1.Split(default(char[]), StringSplitOptions.RemoveEmptyEntries);
                        if (strs.Length != 2) continue;

                        string type = strs[0];
                        string name = strs[1];

                        if (string.Compare(type, "int", true) == 0)
                        {
                            int value;
                            if (TryParse(equation.Item2, out value))
                            {
                                ConstantValues.Add(name, value);
                            }
                        }
                        else if (string.Compare(type, "string", true) == 0)
                        {
                            string value;
                            if (TryParse(equation.Item2, out value))
                            {
                                ConstantValues.Add(name, value);
                            }
                        }
                        else if (string.Compare(type, "float", true) == 0)
                        {
                            float value;
                            if (float.TryParse(equation.Item2, out value))
                            {
                                ConstantValues.Add(name, value);
                            }
                        }
                    }
                }
            }
        }


        public static bool TryParse(string s, out string value)
        {
            if (!string.IsNullOrWhiteSpace(s))
            {
                string valueStr = s.Trim();

                if (valueStr.Length >= 2 && valueStr[0] == '"' && valueStr[valueStr.Length - 1] == '"')
                {
                    value = valueStr.Substring(1, valueStr.Length - 2);
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

        public static bool TryParse(string s, out int value)
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
                    if (TryParse(valueStr.Remove(index).Trim(), out left)
                        && TryParse(valueStr.Substring(index + 2).Trim(), out right))
                    {
                        value = left << right;
                        return true;
                    }
                }

                // maybe it's addition
                index = valueStr.IndexOf("+");
                if (index != -1)
                {
                    int left, right;
                    if (TryParse(valueStr.Remove(index).Trim(), out left)
                        && TryParse(valueStr.Substring(index + 1).Trim(), out right))
                    {
                        value = left + right;
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

        public static bool TryParse(string s, out float value)
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
    }
}
