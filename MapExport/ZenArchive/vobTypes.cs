using System;
using System.Collections.Generic;
using System.Text;

namespace MapExport.ZenArchive
{
    public class vobTypes
    {
        public static Dictionary<string, Dictionary<string, string>> vobTypeLists = new Dictionary<string, Dictionary<string, string>>();
        public static bool mustSave;
        public static void insert(String name, String propname, string type)
        {
            if (!vobTypeLists.ContainsKey(name))
                vobTypeLists.Add(name, new Dictionary<string, string>());
            if (!vobTypeLists[name].ContainsKey(propname))
            {
                vobTypeLists[name].Add(propname, type);
                mustSave = true;
            }
        }
        public static void load()
        {
            try
            {
                cFile file = new cFile();
                file.open("classes.opt", System.IO.FileMode.Open);
                string actualKey = "";
                while (!file.EOF())
                {
                    String line = file.readLineB();
                    if (line.StartsWith("09"))
                    {
                        vobTypeLists.Add(file.Encoding.GetString(ByteHelper.GetStringToBytes(line.Substring("09".Length))), new Dictionary<string, string>());
                        actualKey = file.Encoding.GetString(ByteHelper.GetStringToBytes(line.Substring("09".Length)));
                    }
                    else
                        vobTypeLists[actualKey].Add(file.Encoding.GetString(ByteHelper.GetStringToBytes(line.Substring(0, line.IndexOf("09")))),
                            file.Encoding.GetString(ByteHelper.GetStringToBytes(line.Substring(line.IndexOf("09") + "09".Length))));
                }
            }
            catch (Exception ex)
            {

            }

        }

        public static void save()
        {
            if (!mustSave)
                return;
            cFile file = new cFile();
            file.open("classes.opt", System.IO.FileMode.Create);

            foreach (KeyValuePair<string, Dictionary<string, string>> kvp in vobTypeLists)
            {
                file.Write("09" + ByteHelper.RealStringtoByteString(kvp.Key) + "0A");
                foreach (KeyValuePair<string, string> kvp2 in kvp.Value)
                {
                    file.Write(ByteHelper.RealStringtoByteString(kvp2.Key) + "09" + ByteHelper.RealStringtoByteString(kvp2.Value) + "0A");
                }
            }
        }
    }
}
