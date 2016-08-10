using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FilePacker
{
    class DataPack
    {
        string name = string.Empty;
        public string Name { get { return this.name; } set { this.name = value; } }
        public string URL = string.Empty;
        public string Folder = string.Empty;
        public readonly List<PackObject> objectList = new List<PackObject>();

        public void LoadFolder()
        {
            objectList.Clear();
            Search(Folder);
        }

        bool Search(string path)
        {
            DirectoryInfo current = new DirectoryInfo(path);
            DirectoryInfo[] dirs = current.GetDirectories();
            FileInfo[] files = current.GetFiles();

            PackObject last = null;
            for (int i = 0; i < dirs.Length; i++)
            {
                var dir = new PackDir(dirs[i].Name);
                objectList.Add(dir);
                if (!Search(Path.Combine(path, dirs[i].Name)))
                {
                    dir.IsEmpty = true;
                }
                last = dir;
            }

            for (int i = 0; i < files.Length; i++)
            {
                var file = new PackFile(files[i].Name);
                objectList.Add(file);
                last = file;
            }

            if (last == null)
            {
                return false;
            }
            else
            {
                last.IsLast = true;
                return true;
            }
        }

        public void Write(BinaryWriter header, Action<int> SetPercent)
        {
            header.Write(name);
            header.Write(URL);
            header.Write(objectList.Count);

            using (FileStream fs = new FileStream(name + ".bin", FileMode.Create, FileAccess.Write))
            {
                WriteObjects(header, fs, 0, Folder, SetPercent);
            }
        }

        int WriteObjects(BinaryWriter header, Stream s, int index, string path, Action<int> SetPercent)
        {
            while (index < objectList.Count)
            {
                PackObject obj = objectList[index];

                obj.Write(header, s, path);
                SetPercent(100 * (index+1) / objectList.Count);

                if (obj is PackDir && !((PackDir)obj).IsEmpty)
                {
                    index = WriteObjects(header, s, index + 1, Path.Combine(path, obj.Name), SetPercent);
                }

                if (obj.IsLast)
                    break;

                index++;
            }
            return index;
        }
    }
}
