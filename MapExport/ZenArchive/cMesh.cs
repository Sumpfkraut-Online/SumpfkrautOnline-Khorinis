using System;
using System.Collections.Generic;
using System.Text;

namespace MapExport.ZenArchive
{
    public class cMesh
    {
        public cZenArchive ZenArchive { get; set; }

        public cMesh(cZenArchive za)
        {
            ZenArchive = za;
        }

        public void read()
        {
            while (!ZenArchive.File.EOF())
            {
                String line = ZenArchive.File.readLine();
                if (line == "\t[VobTree % 0 0]")
                {
                    ZenArchive.File.FileS.Position -= (line.Length + 1);
                    break;
                }
            }
        }
    }
}
