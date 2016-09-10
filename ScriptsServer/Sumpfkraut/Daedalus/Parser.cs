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

            sr = new StreamReader(stream);
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

        public Tuple<string, string> ReadEquation()
        {
            string left = ReadTill("=");
            if (left == null)
                return null;

            string right = ReadTill(";");
            if (right == null)
                return null;

            return new Tuple<string, string>(left, right);
        }

        public bool SkipTill(string str)
        {
            if (string.IsNullOrEmpty(str))
                return false;

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
                return null;

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
                        return sb.Length > 0 ? sb.ToString() : null;
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
