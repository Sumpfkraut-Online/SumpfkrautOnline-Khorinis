using System;
using System.Collections.Generic;
using System.Text;

namespace MapExport.ZenArchive
{
    public class cHeader
    {
        public cZenArchive ZenArchive { get; set; }
        public Format Format { get; private set; }
        public String Bearbeiter { get; set; }
        public int ObjectCount { get; set; }
        public DateTime ModifyDate { get; set; }

        public cHeader(cZenArchive za)
        {
            ZenArchive = za;
        }

        public void readHeader()
        {
            String line = "";

            //Name auslesen:
            ZenArchive.File.readLine();

            //Version auslesen: ("ver: 1")
            ZenArchive.File.readLine();

            //Format auslesen:
            ZenArchive.File.readLine();
            line = ZenArchive.File.readLine().ToUpper();
            if (line == "ASCII")
                Format = Format.ASCII;
            else if (line == "BIN_SAFE")
                Format = Format.BIN_SAFE;

            //Anderes:
            line = ZenArchive.File.readLine();
            line = ZenArchive.File.readLine().Trim();//Datum
            ModifyDate = ByteHelper.getStringToDateTime(line.Substring(line.LastIndexOf(" ") + 1));
            line = ZenArchive.File.readLine().Trim();//User
            Bearbeiter = line.Substring(line.LastIndexOf(" ") + 1);
            line = ZenArchive.File.readLine();
            line = ZenArchive.File.readLine().Trim();//Objekte
            ObjectCount = Convert.ToInt32(line.Substring(line.LastIndexOf(" ") + 1));
        }

        public void writeHeader()
        {

        }
    }
}
