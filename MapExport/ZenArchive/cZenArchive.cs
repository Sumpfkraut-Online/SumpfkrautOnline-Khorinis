using System;
using System.Collections.Generic;
using System.Text;

namespace MapExport.ZenArchive
{
    public class cZenArchive
    {
        public cFile File { get; private set; }

        public cHeader Header { get; private set; }
        public cMesh Mesh { get; private set; }
        public cVobTree VobTree { get; private set; }
        public List<cVob> AllVobs { get; private set; }

        public MapExport ImpExp { get; set; }
        public String Filename { get; set; }

        public cZenArchive()
        {

        }

        public void load() { load(Filename); }
        public void load(String filename)
        {
            try { vobTypes.load(); }
            catch (Exception ex) { }

            AllVobs = new List<cVob>();

            ImpExp.Percent = 0;

            File = new cFile();
            File.open(filename, System.IO.FileMode.Open);

            //Lesevorgang beginnt
            Header = new cHeader(this);
            Header.readHeader();
            ImpExp.Percent = Convert.ToUInt16(100.0f / (float)File.FileS.Length * (float)File.FileS.Position);

            //Mesh lesen
            Mesh = new cMesh(this);
            Mesh.read();
            ImpExp.Percent = Convert.ToUInt16(100.0f / (float)File.FileS.Length * (float)File.FileS.Position);


            VobTree = new cVobTree(this);
            VobTree.read();


            try { vobTypes.save(); }
            catch (Exception ex) { }

            File.close();
        }

        public void save() { save(Filename); }

        public void save(String filename)
        {
            File = new cFile();
            File.open(filename, System.IO.FileMode.Create);

            //Schreibvorgang beginnt
            Header = new cHeader(this);
            Header.writeHeader();

        }



        public void destory()
        {
            //Datei handle schließen und Datei löschen, sollte er diese gerade abspeichern.
            File.FileS.Close();
            if (File.FileS.CanWrite)
                System.IO.File.Delete(File.FileS.Name);
        }
    }
}
