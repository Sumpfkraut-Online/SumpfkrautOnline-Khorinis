using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace GothicClassGenerator
{
    public class Writer : IDisposable
    {
        StreamWriter sw;

        public Writer(string fileName)
        {
            string dir = Path.GetDirectoryName(fileName);
            if (!string.IsNullOrWhiteSpace(dir))
                Directory.CreateDirectory(dir);
            sw = new StreamWriter(fileName);
        }

        public void Dispose()
        {
            sw.Dispose();
        }

        public void LineBreak()
        {
            Write(null);
        }

        public void Write(string line, params string[] args)
        {
            string text = new string(' ', 4 * openNum) + line;
            sw.WriteLine(args.Length == 0 ? text : string.Format(text, args));
        }

        int openNum = 0;
        public void Open()
        {
            Write("{");
            openNum++;
        }

        public void Close()
        {
            openNum--;
            if (openNum < 0)
                throw new IndexOutOfRangeException("openNum");

            Write("}");
        }
    }
}
