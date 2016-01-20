using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using GUC.Log;

namespace GUC.Utilities
{
    public class XmlObj
    {
        public void Save(string path)
        {
            try
            {
                XmlSerializer ser = new XmlSerializer(this.GetType());

                using (FileStream fs = new FileStream(path, FileMode.Create))
                    ser.Serialize(fs, this);
            }
            catch (Exception e)
            {
                Logger.LogWarning("Failed to save {0}!<br>{1}", path, e);
            }
        }

        public static T Load<T>(string path) where T : XmlObj, new()
        {
            try
            {
                T ret;
                XmlSerializer ser = new XmlSerializer(typeof(T));

                using (FileStream fs = new FileStream(path, FileMode.Open))
                    ret = (T)ser.Deserialize(fs);

                return ret;
            }
            catch (Exception e)
            {
                Logger.LogWarning("Could not load {0}, returning empty object.<br>{1}", path, e);
                return new T();
            }
        }
    }
}
