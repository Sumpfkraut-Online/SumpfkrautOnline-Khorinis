using System;
using System.Collections.Generic;
using System.Text;
using WinApi;

namespace Gothic.Types
{
    public class zCTree<T> : zClass where T : zClass, new()
    {
        public zCTree(int address)
            : base(address)
        {

        }

        public zCTree()
        {

        }

        public T Data
        {
            get
            {
                T val = new T();
                val.Initialize(Address + 16);
                return val;
            }
        }

        public zCTree<T> Next
        {
            get
            {
                return new zCTree<T>(Process.ReadInt(Address + 8));
            }
        }

        public zCTree<T> Prev
        {
            get
            {
                return new zCTree<T>(Process.ReadInt(Address + 12));
            }
        }

        public zCTree<T> FirstChild
        {
            get
            {
                return new zCTree<T>(Process.ReadInt(Address + 4));
            }
        }

        public zCTree<T> Parent
        {
            get
            {
                return new zCTree<T>(Process.ReadInt(Address));
            }
        }


        public List<T> getAllObjects()
        {
            List<T> rList = new List<T>();

            getAllObjects(ref rList, this);

            return rList;
        }

        private void getAllObjects(ref List<T> list, zCTree<T> tree)
        {
            do
            {
                if (tree.Data != null && tree.Data.Address != 0)
                {
                    list.Add(tree.Data);
                }

                if (tree.FirstChild != null)
                {
                    getAllObjects(ref list, tree.FirstChild);
                }

            } while ((tree = tree.Next).Address != 0);
        }
    }
}
