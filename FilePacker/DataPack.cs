using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

        public void Write(PacketStream header)
        {
            list.Clear();
            Search(list, new DirectoryInfo(Folder));
            if (list.Count > ushort.MaxValue)
                throw new Exception("Pack has more than 65535 files/directories!");

            using (FileStream fs = new FileStream(name + ".bin", FileMode.Create, FileAccess.Write))
            {
                foreach (PackFile file in list.Where(fi => fi is PackFile).OrderByDescending(fi => fi.Info.LastWriteTimeUtc).Cast<PackFile>())
                {
                    file.Write(fs);
                }
                header.Write((int)fs.Length);
            }
            
            header.WriteStringShort(URL);
            header.WriteUShort(list.Count);
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

        int fileSize;
        public List<PackFile> CheckList = new List<PackFile>();
        public List<PackFile> NeededList = new List<PackFile>();

        public int Read(PacketStream header)
        {
            this.fileSize = header.ReadInt();
            this.URL = header.ReadStringShort();

            list.Clear();
            CheckList.Clear();

            int count = header.ReadUShort();
            return ReadObjects(header, "", count, 0);
        }

        int ReadObjects(PacketStream header, string path, int count, int checkBytes)
        {
            while (list.Count < count)
            {
                PackObject p = PackObject.ReadNew(header, path);
                list.Add(p);

                if (p is PackDir)
                {
                    checkBytes = ReadObjects(header, Path.Combine(path, p.Name), count, checkBytes);
                }
                else
                {
                    checkBytes += ((PackFile)p).PreCheck(CheckList, NeededList);
                }

                if (p.IsLast)
                    break;
            }
            return checkBytes;
        }

        public void EndCheck(Action<int> AddBytes)
        {
            for (int i = 0; i < CheckList.Count; i++)
                if (CheckList[i].Check(AddBytes))
                    NeededList.Add(CheckList[i]);

            CheckList = null;
        }
    }
}
