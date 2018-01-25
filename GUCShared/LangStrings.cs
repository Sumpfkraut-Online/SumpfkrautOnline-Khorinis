using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace GUC
{
    public static class LangStrings
    {
        class LanguageInfo
        {
            public int Index;
            public string Name;
            public Dictionary<string, string> Dict;
        }

        static List<LanguageInfo> texts = new List<LanguageInfo>();
        public static int LanguageCount { get { return texts.Count; } }
        public static IEnumerable<string> GetNames() { return texts.Select(t => t.Name); }

        public static void LoadFile(string filePath)
        {
            if (!File.Exists(filePath))
                return;

            using (StreamReader sr = new StreamReader(filePath))
            {
                Dictionary<string, string> current = null;

                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    line = line.Trim();
                    if (line.Length == 0)
                        continue;

                    if (line[0] == '{')
                    {
                        if (line[line.Length - 1] != '}')
                            continue;

                        string name = line.Substring(1, line.Length - 2).Trim();
                        if (name.Length == 0)
                            continue;

                        LanguageInfo other = texts.Find(l => string.Equals(name, l.Name, StringComparison.OrdinalIgnoreCase));
                        if (other != null)
                        {
                            current = other.Dict;
                        }
                        else
                        {
                            current = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                            texts.Add(new LanguageInfo()
                            {
                                Index = texts.Count,
                                Name = name,
                                Dict = current,
                            });
                        }
                    }

                    if (current == null)
                        continue;

                    int index = line.IndexOf(':');
                    if (index >= 0)
                    {
                        string ident = line.Remove(index).TrimEnd();
                        string text = line.Substring(index + 1).TrimStart();

                        if (current.ContainsKey(ident))
                        {
                            current[ident] = text;
                        }
                        else
                        {
                            current.Add(ident, text);
                        }
                    }
                }
            }

            if (LanguageIndex < 0)
                LanguageIndex = 0;
        }

        static LanguageInfo currentLanguage;
        public static int LanguageIndex
        {
            get { return currentLanguage != null ? currentLanguage.Index : -1; }
            set
            {
                if (value >= 0 && value < texts.Count)
                {
                    currentLanguage = texts[value];
                }
                else if (texts.Count > 0)
                {
                    currentLanguage = texts[0];
                }
                else
                {
                    currentLanguage = null;
                }
            }
        }

        public static string LanguageName
        {
            get
            {
                return currentLanguage != null ? currentLanguage.Name : "[Unknown Language]";
            }
        }

        public static string Get(string identifier)
        {
            if (currentLanguage != null && currentLanguage.Dict.TryGetValue(identifier, out string text))
            {
                return text;
            }
            else if (texts.Count >= 2 && (currentLanguage == null || currentLanguage.Index != 0))
            {
                if (texts[0].Dict.TryGetValue(identifier, out text))
                    return text;
            }
            return string.Format("[Unknown: {0}]", identifier);
        }

        public static void Reset()
        {
            texts.Clear();
            currentLanguage = null;
        }
    }
}
