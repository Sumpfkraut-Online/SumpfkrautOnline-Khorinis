using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace GUC.Utilities
{
    public static class XMLHelper
    {
        public static void SaveObject<T>(T obj, string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException("Path is null or white space!");

            string dir = Path.GetDirectoryName(path);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            try
            {
                XmlSerializer xml = new XmlSerializer(typeof(T));
                using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write))
                {
                    xml.Serialize(fs, obj);
                }
            }
            catch (Exception e)
            {
                Log.Logger.Log("XML Serialization of '{0}' of type '{1}' failed.", path, typeof(T));
                throw e;
            }
        }

        public static T LoadObject<T>(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException("Path is null or white space!");
            
            if (!Directory.Exists(Path.GetDirectoryName(path)))
            {
                Log.Logger.Log("XMLHelper: Path '{0}' for type '{1}' is not existing.", path, typeof(T));
                return default(T);
            }

            try
            {
                object result;
                XmlSerializer xml = new XmlSerializer(typeof(T));
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    result = xml.Deserialize(fs);
                }
                return (T)result;
            }
            catch (Exception e)
            {
                Log.Logger.Log("XML Serialization of '{0}' of type '{1}' failed.", path, typeof(T));
                throw e;
            }
        }
    }
}
