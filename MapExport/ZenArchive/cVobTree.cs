using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace MapExport.ZenArchive
{
    public class cVobTree : cTreeObject
    {
        public cZenArchive ZenArchive { get; set; }

        public cVobTree(cZenArchive zA)
        {
            ZenArchive = zA;
            Eigenschaften = new Dictionary<string, object>();
            Eigenschaften.Add("object_type", "VobTree");
            Children = new List<cTreeObject>();
        }

        public void read()
        {
            Regex vobtree = new Regex("095B566F62547265652025203020305D", RegexOptions.IgnoreCase);
            Regex vobtype = new Regex("5B2520.*?20.*?20.*?5D", RegexOptions.IgnoreCase);
            Regex vobProp = new Regex("0909.*?3D.*?3A.*?", RegexOptions.IgnoreCase);
            while (!ZenArchive.File.EOF())
            {
                String line = ZenArchive.File.readLineB();
                if (vobtree.Match(line).Success) //Vobtree?
                {


                    line = ZenArchive.File.readLineB();
                    Match match = vobProp.Match(line);
                    if (match.Success)//Eigenschaft?
                    {
                        int beginName = match.Index + "0909".Length;
                        int endName = line.IndexOf("3D", beginName);
                        int beginType = endName + "3D".Length;
                        int endType = line.IndexOf("3A", beginType);
                        int beginValue = endType + "3A".Length;

                        String name = ZenArchive.File.Encoding.GetString(ByteHelper.GetStringToBytes(line.Substring(beginName, endName - beginName)));
                        String type = ZenArchive.File.Encoding.GetString(ByteHelper.GetStringToBytes(line.Substring(beginType, endType - beginType)));
                        String value = ZenArchive.File.Encoding.GetString(ByteHelper.GetStringToBytes(line.Substring(beginValue)));

                        //Sonderstellung: Child
                        if (name.StartsWith("childs"))
                        {
                            ID = Convert.ToInt32(name.Substring("childs".Length));
                            name = "childs";
                        }

                        Object typeEntry = null;
                        switch (type)
                        {
                            case "int":
                                typeEntry = Convert.ToInt32(value);
                                break;
                            default:
                                typeEntry = value;
                                break;
                        }

                        Eigenschaften.Add(name, typeEntry);
                        continue;
                    }
                }

                cTreeObject latestVob = this;
                while (!ZenArchive.File.EOF())
                {
                    line = ZenArchive.File.readLineB();
                    Match match = vobtype.Match(line);
                    if (match.Success)
                    {

                        //5B2520.*?20.*?20.*?5D
                        int beginType = line.IndexOf("5B2520") + "5B2520".Length;
                        int endType = line.IndexOf("20", beginType);
                        int beginBigID = endType + "20".Length;
                        int endBigID = line.IndexOf("20", beginBigID);
                        int beginLitID = endBigID + "20".Length;
                        int endLitID = line.IndexOf("5D", beginLitID);

                        String type = ZenArchive.File.Encoding.GetString(ByteHelper.GetStringToBytes(line.Substring(beginType, endType - beginType)));
                        String bid = ZenArchive.File.Encoding.GetString(ByteHelper.GetStringToBytes(line.Substring(beginBigID, endBigID - beginBigID)));
                        String lid = ZenArchive.File.Encoding.GetString(ByteHelper.GetStringToBytes(line.Substring(beginLitID, endLitID - beginLitID)));

                        cVob vob = null;
                        if (!Eigenschaften.ContainsKey("childs") || Children.Count < (int)Eigenschaften["childs"])
                        {
                            vob = new cVob(ZenArchive, latestVob.getNextParent());
                        }
                        else
                        {
                            vob = new cVob(ZenArchive, latestVob.getNextParent());
                        }

                        vob.Eigenschaften.Add("object_type", type);
                        vob.Eigenschaften.Add("big_id", Convert.ToInt32(bid));
                        vob.Eigenschaften.Add("small_id", Convert.ToInt32(lid));
                        latestVob = vob.read();
                        while (latestVob != null && !((String)latestVob.Eigenschaften["object_type"]).Equals("zCWayNet"))
                            latestVob = ((cVob)latestVob).read();
                    }
                }



            }
        }
    }
}
