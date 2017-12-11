using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace GothicClassGenerator
{
    static class UIStuff
    {
        public static zType[] systemTypes = new zType[] { zVoid.Void, zInt.Int, zBool.Bool, zFloat.Float };

        static ObservableCollection<zClass> classes = new ObservableCollection<zClass>();
        public static ObservableCollection<zClass> Classes { get { return classes; } }

        static ObservableCollection<zType> allTypes = new ObservableCollection<zType>(systemTypes);
        public static ObservableCollection<zType> AllTypes { get { return allTypes; } }

        static ObservableCollection<zType> typesNoVoid = new ObservableCollection<zType>(systemTypes.Skip(1));
        public static ObservableCollection<zType> TypesNoVoid { get { return typesNoVoid; } }

        static ObservableCollection<object> treeViewItems = new ObservableCollection<object>();
        public static ObservableCollection<object> TreeViewItems { get { return treeViewItems; } }

        public static void AddClass(zClass newClass)
        {
            classes.Add(newClass);
            allTypes.Add(newClass);
            typesNoVoid.Add(newClass);

            string[] strs = newClass.NameSpace.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            IList items = treeViewItems;
            foreach (string space in strs)
            {
                TreeViewItem section = items.OfType<TreeViewItem>().FirstOrDefault(i => Equals(i.Header, space));
                if (section == null)
                {
                    section = new TreeViewItem();
                    section.Header = space;
                    section.DisplayMemberPath = "Name";
                    items.Add(section);
                }

                items = section.Items;
            }
            items.Add(newClass);
        }

        public static void RemoveClass(zClass remClass)
        {

        }
    }
}
