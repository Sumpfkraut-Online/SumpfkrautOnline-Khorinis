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
        string name = string.Empty;
        public string Name { get { return this.name; } set { this.name = value; } }
        public string URL = string.Empty;
        public string Folder = string.Empty;
        public readonly List<PackObject> objectList = new List<PackObject>();

        public void LoadFolder()
        {
            // Load all files from a folder and its sub-folders.

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
                // search all directories
                var dir = new PackDir();
                dir.Name = dirs[i].Name;
                objectList.Add(dir);
                if (!Search(Path.Combine(path, dirs[i].Name)))
                {
                    dir.IsEmpty = true;
                }
                last = dir;
            }

            for (int i = 0; i < files.Length; i++)
            {
                // add files
                var file = new PackFile();
                file.Name = files[i].Name;
                objectList.Add(file);
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

        public void Write(BinaryWriter header, Action<double> SetPercent)
        {
            header.Write(name);
            header.Write(URL);
            header.Write(objectList.Count);

            using (FileStream fs = new FileStream(name + ".bin", FileMode.Create, FileAccess.Write))
            {
                WriteObjects(header, fs, 0, Folder, SetPercent);
            }
        }

        int WriteObjects(BinaryWriter header, Stream s, int index, string path, Action<double> SetPercent)
        {
            while (index < objectList.Count)
            {
                PackObject obj = objectList[index];

                obj.Write(header, s, path);
                SetPercent(100d * (index+1) / objectList.Count);

                if (obj is PackDir && !((PackDir)obj).IsEmpty)
                { // object is a non-empty folder, go deeper
                    index = WriteObjects(header, s, index + 1, Path.Combine(path, obj.Name), SetPercent);
                }

                if (obj.IsLast) // last object in this folder, back out!
                    break;

                index++;
            }
            return index;
        }

        public void Read(BinaryReader br, Action<double> SetPercent)
        {
            this.name = br.ReadString();
            this.URL = br.ReadString();

            int count = br.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                objectList.Add(PackObject.ReadNew(br));
            }
        }
    }
}
