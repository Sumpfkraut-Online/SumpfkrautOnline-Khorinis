using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using GUC.Log;

namespace GUC.Options
{
    public partial class BaseOptions
    {
        public static BaseOptions Current { get; private set; }

        public void Save(string path)
        {
            try
            {
                XmlSerializer ser = new XmlSerializer(typeof(BaseOptions));

                using (FileStream fs = new FileStream(path, FileMode.Create))
                    ser.Serialize(fs, this);
            }
            catch (Exception e)
            {
                Logger.LogError("Failed to save BaseOptions! " + e.Message);
            }
        }

        internal static void Load(string path)
        {
            try
            {
                XmlSerializer ser = new XmlSerializer(typeof(BaseOptions));

                using (FileStream fs = new FileStream(path, FileMode.Open))
                    Current = (BaseOptions)ser.Deserialize(fs);
            }
            catch (Exception e)
            {
                Logger.LogError("Failed to load BaseOptions, creating new file! " + e.Message);
                Current = new BaseOptions();
                Current.Save(path);
            }
        }
    }
}
