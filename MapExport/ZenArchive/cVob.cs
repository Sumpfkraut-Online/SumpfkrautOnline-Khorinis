using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace MapExport.ZenArchive
{
    public class cVob : cTreeObject
    {
        public cZenArchive ZenArchive { get; set; }



        public cVob(cZenArchive zA, cTreeObject parent)
        {
            ZenArchive = zA;
            Parent = parent;
            Parent.Children.Add(this);
            Eigenschaften = new Dictionary<string, object>();
            Children = new List<cTreeObject>();

            zA.AllVobs.Add(this);
        }

        public cVob read()
        {
            Regex vobtree = new Regex("095B566F62547265652025203020305D", RegexOptions.IgnoreCase);
            Regex vobtype = new Regex("5B2520.*?20.*?20.*?5D", RegexOptions.IgnoreCase);
            Regex vobProp = new Regex("0909.*?3D.*?3A.*?", RegexOptions.IgnoreCase);
            while (!ZenArchive.File.EOF())
            {
                String line = "";
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

                    name = name.Replace("\t", "");
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
                    cTreeObject vb = getParentWithoutCount();
                    if (name == "childs" && vb != null)
                        vb.Eigenschaften.Add(name, typeEntry);
                    else
                        Eigenschaften.Add(name, typeEntry);

                    vobTypes.insert((String)Eigenschaften["object_type"], name, type);
                }
                else
                {
                    match = vobtype.Match(line);
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
                            vob = new cVob(ZenArchive, this);
                        }
                        else
                        {
                            vob = new cVob(ZenArchive, Parent.getNextParent());
                        }

                        vob.Eigenschaften.Add("object_type", type);
                        vob.Eigenschaften.Add("big_id", Convert.ToInt32(bid));
                        vob.Eigenschaften.Add("small_id", Convert.ToInt32(lid));
                        return vob;
                    }
                }
            }

            return null;
        }
    }
}
