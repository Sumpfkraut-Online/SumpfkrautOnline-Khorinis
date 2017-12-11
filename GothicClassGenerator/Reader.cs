using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace GothicClassGenerator
{
    public class Reader
    {
        const string lineBreak = "\r\n";

        int pos;
        string text;
        StringBuilder builder;

        public Reader(string filePath)
        {
            pos = 0;
            builder = new StringBuilder(128);
            using (StreamReader sr = new StreamReader(filePath))
                text = sr.ReadToEnd();
        }

        public string ReadLine(bool skipComments = true, bool skipEmpty = true)
        {
            while (pos < text.Length)
            {
                int endIndex = text.IndexOf(lineBreak, pos);
                string line;
                if (endIndex < 0)
                {
                    line = text.Substring(pos);
                    pos = text.Length;
                }
                else
                {
                    line = text.Substring(pos, endIndex - pos);
                    pos = endIndex + 2;
                }

                if (skipComments)
                {
                    int startIndex = 0;
                    while ((startIndex = line.IndexOf('/', startIndex)) >= 0)
                    {
                        int nextIndex = startIndex + 1;
                        if (nextIndex == line.Length)
                            break;

                        if (line[nextIndex] == '/')
                        {
                            line = line.Remove(startIndex);
                            break;
                        }
                        else if (line[nextIndex] == '*')
                        {
                            endIndex = line.IndexOf("*/", nextIndex + 1);
                            if (endIndex >= 0)
                            {
                                line = line.Remove(startIndex, endIndex + 2 - startIndex);
                                continue;
                            }
                            else // not in this line
                            {
                                line = line.Remove(startIndex);
                                endIndex = text.IndexOf("*/", pos);
                                pos = endIndex < 0 ? text.Length : (endIndex + 2);
                                break;
                            }
                        }

                        startIndex++;
                    }
                }

                line = line.Trim();
                if (skipEmpty && line.Length == 0)
                    continue;

                return line;
            }
            return null;
        }

        public string SkipUntil(string str)
        {
            string line;
            while ((line = ReadLine()) != null)
            {
                if (line.StartsWith(str))
                    return line.Substring(str.Length).TrimStart();
            }
            return null;
        }

        public string ReadRegion(string str)
        {
            if (SkipUntil("#region " + str) == null)
                return null;

            int start = pos;
            int regionCount = 0;
            
            while (true)
            {
                int end = pos;
                string line = ReadLine();
                if (line == null)
                    return null;

                if (line.StartsWith("#endregion"))
                {
                    if (regionCount == 0)
                    {
                        return text.Substring(start, end - start);
                    }
                    regionCount--;
                }
                else if (line.StartsWith("#region"))
                {
                    regionCount++;
                }
            }
        }
    }
}
