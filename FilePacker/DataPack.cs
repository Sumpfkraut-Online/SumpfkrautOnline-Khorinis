using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FilePacker
{
    /// <summary>
    /// A Data Pack. Contains compressed files.
    /// </summary>
    class DataPack
    {
        string name;
        public string Name { get { return this.name; } set { this.name = value; } }
        public string URL;
        public string Folder;

        readonly List<PackObject> list = new List<PackObject>();

        public void Write(BinaryWriter header)
        {
            list.Clear();
            Search(list, new DirectoryInfo(Folder));

            using (FileStream fs = new FileStream(name + ".bin", FileMode.Create, FileAccess.Write))
            {
                foreach (PackFile file in list.Where(fi => fi is PackFile).OrderByDescending(fi => fi.Info.LastWriteTimeUtc).Cast<PackFile>())
                {
                    file.Write(fs);
                }
            }

            //header.Write(name);
            header.Write(URL);
            header.Write(list.Count);
            for (int i = 0; i < list.Count; i++)
            {
                list[i].WriteHeader(header);
            }
        }

        bool Search(List<PackObject> list, DirectoryInfo current)
        {
            PackObject last = null;

            DirectoryInfo[] dirs = current.GetDirectories();
            for (int i = 0; i < dirs.Length; i++)
            {
                // search all directories
                var dir = new PackDir(dirs[i]);
                list.Add(dir);

                if (!Search(list, dirs[i])) 
                    list.RemoveAt(list.Count - 1); // don't add empty folders

                last = dir;
            }

            FileInfo[] files = current.GetFiles();
            for (int i = 0; i < files.Length; i++)
            {
                // add files
                var file = new PackFile(files[i]);
                list.Add(file);
                last = file;
            }

            if (last == null)
            {
                return false;
            }
            else
            {
                last.IsLast = true; // last item in this folder is marked
                return true;
            }
        }

        public void Read(BinaryReader br)
        {
            //this.name = br.ReadString();
            this.URL = br.ReadString();

            list.Clear();
            int count = br.ReadInt32();
            ReadObjects(br, "", count);
        }

        void ReadObjects(BinaryReader br, string path, int count)
        {
            while (list.Count < count)
            {
                PackObject p = PackObject.ReadNew(br, path);
                list.Add(p);
                
                if (p is PackDir)
                {
                    ReadObjects(br, p.Info.FullName, count);
                }

                if (p.IsLast)
                    return;
            }
        }
    }
}
