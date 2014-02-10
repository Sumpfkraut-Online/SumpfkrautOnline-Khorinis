using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace WinApi.FileFormat
{
    public class IniFile
    {

        private Dictionary<String, Dictionary<String, String>> mEntrys = new Dictionary<string, Dictionary<string, string>>();
        private List<String> mFiles = new List<string>();

        private String CommentCharacters = "#;";


        private String regCommentStr = "";
        private Regex regEntry = null;
        private Regex regCaption = null;

        public IniFile()
        {
            regCommentStr = @"(\s*[" + CommentCharacters + "](?<comment>.*))?";
            regEntry = new Regex(@"^[ \t]*(?<entry>([^=])+)=(?<value>([^=" + CommentCharacters + "])+)" + regCommentStr + "$");
            regCaption = new Regex(@"^[ \t]*(\[(?<caption>([^\]])+)\]){1}" + regCommentStr + "$");
        }

        public IniFile(string dir)
            : this()
        {

            if (File.Exists(dir))
            {
                loadFile(dir, true);
            }
            else if (Directory.Exists(dir))
            {
                string[] files = Directory.GetFiles(dir);
                foreach (String file in files)
                {
                    if (file.Trim().ToLower().EndsWith(".ini"))
                    {
                        loadFile(file, false);
                    }
                }

            }
            else
            {
                throw new Exception("INI-File or Directory not found");
            }
        }


        public String getValue(String Caption, String Entryname)
        {
            if (!mEntrys.ContainsKey(Caption.ToLower()))
                return null;
            if (!mEntrys[Caption.ToLower()].ContainsKey(Entryname.ToLower()))
                return null;
            return mEntrys[Caption.ToLower()][Entryname.ToLower()];
        }

        
        public String getValue(String Entryname)
        {
            String value = null;

            foreach (KeyValuePair<String, Dictionary<String, String>> entry in mEntrys)
            {
                if (entry.Value.ContainsKey(Entryname.ToLower()))
                {
                    value = entry.Value[Entryname.ToLower()];
                    break;
                }
            }

            return value;
        }

        /// <summary>
        /// Durchsucht erst alle Captions aus der Liste, wenn dort nicht gefunden, werden alle durchsucht.
        /// </summary>
        /// <param name="Captions"></param>
        /// <param name="Entryname"></param>
        /// <returns></returns>
        public String getValue(List<String> Captions, String Entryname)
        {
            foreach (String caption in Captions)
            {
                String val = getValue(caption, Entryname);
                if (val != null)
                    return val;
            }

            String value = getValue(Entryname);
            if ((value != null))
                return value;

            return Entryname;
        }



        private void loadFile(String file, bool onlyThisFile)
        {
            mFiles.Add(file);

            String lastCaption = "";
            using (StreamReader sr = new StreamReader(file))
            {
                while (!sr.EndOfStream)
                {
                    String line = sr.ReadLine().TrimEnd();

                    if (String.IsNullOrEmpty(line.Trim()))
                        continue;

                    if (isCommentary(line))
                        continue;

                    if (isCaption(line))
                    {
                        Match mCaption = regCaption.Match(line);
                        if (mCaption.Success)
                            lastCaption = mCaption.Groups["caption"].Value.Trim().ToLower();

                        if (!mEntrys.ContainsKey(lastCaption))
                            mEntrys.Add(lastCaption, new Dictionary<string, string>());

                        continue;
                    }

                    if (!isEntry(line))
                        continue;
                    int pos = line.IndexOf("=");
                    String entryName = line.Substring(0, pos).Trim().ToLower();
                    String entryValue = line.Substring(pos + 1).Trim();


                    if(mEntrys[lastCaption].ContainsKey(entryName))
                        mEntrys[lastCaption][entryName]  = entryValue;
                    else
                        mEntrys[lastCaption].Add(entryName, entryValue);
                }
            }
        }

        private bool isCommentary(String line)
        {
            if (Regex.IsMatch(line, @"^[ \t]*[" + CommentCharacters + "]"))
                return true; // Kommentar
            return false;
        }

        private bool isCaption(String line)
        {
            Match mCaption = regCaption.Match(line);
            if (mCaption.Success)
                return true;
            return false;
        }

        private bool isEntry(String line)
        {
            int pos = line.IndexOf("=");
            if (pos == -1)
                return false;
            else
                return true;
        }
    }
}
