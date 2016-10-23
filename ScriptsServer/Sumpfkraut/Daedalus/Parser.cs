using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace GUC.Scripts.Sumpfkraut.Daedalus
{
    class Parser : IDisposable
    {
        StreamReader sr;
        StringBuilder sb;

        public Parser(Stream stream)
        {
            if (!stream.CanRead)
                throw new ArgumentException("Stream is not readable!");

            sr = new StreamReader(stream, Encoding.Default);
            sb = new StringBuilder();
        }

        public string ReadWord()
        {
            sb.Clear();

            int i;
            char c;
            do
            {
                i = ReadChar();
                if (i == -1)
                    return null;

                c = (char)i;
            } while (char.IsWhiteSpace(c));

            do
            {
                sb.Append(c);
                i = ReadChar();
                if (i == -1)
                    break;

                c = (char)i;
            } while (!char.IsWhiteSpace(c));

            return sb.Length > 0 ? sb.ToString() : null;
        }

        public bool ReadNextCommand(out Tuple<string, string> func, out Tuple<string, string, string> equation)
        {
            string command = ReadTill(";");
            if (!string.IsNullOrWhiteSpace(command))
            {
                int index = command.IndexOf('=');
                if (index != -1)
                {
                    string left = command.Remove(index);
                    string right = command.Substring(index + 1);
                    if (!string.IsNullOrWhiteSpace(left) && !string.IsNullOrWhiteSpace(right))
                    {
                        // check left for arrays
                        int startIndex = left.IndexOf('[') + 1;
                        if (startIndex != 0)
                        {
                            int endIndex = left.LastIndexOf(']', left.Length - 1, left.Length - startIndex);
                            if (endIndex != -1)
                            {
                                equation = new Tuple<string, string, string>(left.Remove(startIndex - 1).Trim(), right.Trim(), left.Substring(startIndex, endIndex - startIndex).Trim());
                                func = null;
                                return true;
                            }
                        }
                        equation = new Tuple<string, string, string>(left.Trim(), right.Trim(), null);
                        func = null;
                        return true;
                    }
                }
                else
                {
                    // func?
                    index = command.IndexOf('(') + 1;
                    if (index != 0)
                    {
                        string funcName = command.Remove(index - 1);
                        int endIndex = command.LastIndexOf(')', command.Length - 1, command.Length - index);
                        if (endIndex != -1)
                        {
                            func = new Tuple<string, string>(funcName.Trim(), command.Substring(index, endIndex - index).Trim());
                            equation = null;
                            return true;
                        }
                    }
                }
            }
            func = null;
            equation = null;
            return false;
        }

        public bool SkipTillLineStartsWith(string str)
        {
            if (string.IsNullOrEmpty(str))
                return true;

            while (true)
            {
                int index = 0;
                while (true)
                {
                    int i = ReadChar();
                    if (i == -1) return false;
                    if (i == '\r' && sr.Peek() == '\n')
                    {
                        sr.Read();
                        index = 0;
                        continue;
                    }

                    if (index > 0 || !char.IsWhiteSpace((char)i))
                        if (char.ToUpperInvariant((char)i) == char.ToUpperInvariant(str[index]))
                        {
                            index++;
                            if (index == str.Length)
                            {
                                return true;
                            }
                        }
                        else
                        {
                            break;
                        }
                }

                while (true)
                {
                    int i = ReadChar();
                    if (i == -1) return false;
                    if (i == '\r' && sr.Peek() == '\n')
                    {
                        sr.Read();
                        break;
                    }
                }
            }
        }


        public bool SkipTill(string str)
        {
            if (string.IsNullOrEmpty(str))
                return true;

            int index = 0;

            int i;
            while ((i = ReadChar()) != -1)
            {
                if (char.ToUpperInvariant((char)i) == char.ToUpperInvariant(str[index]))
                {
                    index++;
                    if (index == str.Length)
                    {
                        return true;
                    }
                }
                else
                {
                    index = 0;
                }
            }

            return false;
        }

        public string ReadTill(string str)
        {
            if (string.IsNullOrEmpty(str))
                return "";

            sb.Clear();

            int index = 0;

            int i;
            while ((i = ReadChar()) != -1)
            {
                sb.Append((char)i);
                if (char.ToUpperInvariant((char)i) == char.ToUpperInvariant(str[index]))
                {
                    index++;
                    if (index == str.Length)
                    {
                        sb.Length -= str.Length;
                        return sb.ToString();
                    }
                }
                else
                {
                    index = 0;
                }
            }

            return null;
        }
        
        int ReadChar()
        {
            int i = sr.Read();

            if (i == '/')
            {
                int peekChar = sr.Peek();
                if (peekChar == '/') // comment line, read till end
                {
                    sr.ReadLine();
                    return ReadChar();
                }
                else if (peekChar == '*') // comment block
                {
                    sr.Read();
                    while ((i = sr.Read()) != -1)
                    {
                        if (i == '*')
                        {
                            peekChar = sr.Peek();
                            if (peekChar == '/')
                            {
                                sr.Read();
                                return ReadChar();
                            }
                        }
                    }
                }
            }
            
            return i;
        }

        public void Dispose()
        {
            sr.Dispose();
        }
    }
}
