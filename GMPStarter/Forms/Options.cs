using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace Gothic_Untold_Chapter
{
    public class updateFile
    {
        public String filename;
        public String[] filePath;
        public String md5;
    }
    public class Options
    {
        public updateFile[] Files;

        public static Options Load(String filename)
        {
            XmlSerializer ser = new XmlSerializer(typeof(Options));
            StreamReader sr = new StreamReader(filename);
            Options op = (Options)ser.Deserialize(sr);
            sr.Close();
            return op;
        }

        public void Save(String strf)
        {
            XmlSerializer ser = new XmlSerializer(typeof(Options));

            FileStream str = new FileStream(strf, FileMode.Create);
            ser.Serialize(str, this);
            str.Close();
        }
    }
}
