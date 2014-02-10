using System;
using System.Collections.Generic;
using System.Text;

namespace MapExport.ZenArchive
{
    public class cTreeObject
    {
        public List<cTreeObject> Children { get; set; }
        public cTreeObject Parent { get; set; }
        public Dictionary<String, Object> Eigenschaften { get; set; }

        public int ID { get; set; }


        public cTreeObject getNextParent()
        {

            if (!Eigenschaften.ContainsKey("childs") || (Eigenschaften.ContainsKey("childs") && (int)Eigenschaften["childs"] > Children.Count))
            {
                return this;
            }
            else if (Parent != null)
                return Parent.getNextParent();
            else
                return this;
        }

        public cTreeObject getParentWithoutCount()
        {
            if (Eigenschaften.ContainsKey("childs") && Parent != null)
            {
                return getParentWithoutCount();
            }
            if (!Eigenschaften.ContainsKey("childs"))
                return this;
            return null;
        }

        public List<cTreeObject> getAllChildren(List<cTreeObject> list)
        {
            list.Add(this);
            foreach (cTreeObject obj in Children)
            {
                list = obj.getAllChildren(list);
            }
            return list;
        }

        public List<cTreeObject> getVobsWithType(String type)
        {
            List<cTreeObject> allList = getAllChildren(new List<cTreeObject>());
            List<cTreeObject> typeList = new List<cTreeObject>();
            foreach (cTreeObject obj in allList)
            {
                if (obj.Eigenschaften["object_type"].Equals(type))
                    typeList.Add(obj);
            }
            return typeList;
        }
    }
}
