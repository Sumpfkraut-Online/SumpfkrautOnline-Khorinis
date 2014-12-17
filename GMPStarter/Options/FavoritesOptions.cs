using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace GMPStarter.Options
{
    public class FavoritesOptions
    {
        public class cFavorite
        {
            public String ip;
            public int port;
        
        }

        public List<cFavorite> Favorites = new List<cFavorite>();


        public void Save(String strf)
        {
            XmlSerializer ser = new XmlSerializer(typeof(FavoritesOptions), new Type[] { typeof(cFavorite) });

            FileStream str = new FileStream(strf, FileMode.Create);
            ser.Serialize(str, this);
            str.Close();
        }

        public static FavoritesOptions Load(String name)
        {
            FavoritesOptions co = null;
            using (Stream fileStream = new FileStream(name, FileMode.Open))
            {
                XmlSerializer ser = new XmlSerializer(typeof(FavoritesOptions), new Type[] { typeof(cFavorite) });
                co = (FavoritesOptions)ser.Deserialize(fileStream);
            }

            return co;
        }
    }
}
