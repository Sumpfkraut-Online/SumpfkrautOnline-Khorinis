using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;

namespace GUCLauncher
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
                if (string.Compare(dirs[i].Name, "user", true) == 0) // ignore "user" folders
                    continue;

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
        List<PackFile> checkList = new List<PackFile>();
        List<PackFile> neededList = new List<PackFile>();

        public int Read(PacketStream header)
        {
            this.fileSize = header.ReadInt();
            this.URL = header.ReadStringShort();

            list.Clear();
            checkList.Clear();

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
                    checkBytes += ((PackFile)p).PreCheck(checkList, neededList);
                }

                if (p.IsLast)
                    break;
            }
            return checkBytes;
        }

        public void EndCheck(Action<int> AddBytes)
        {
            for (int i = 0; i < checkList.Count; i++)
                if (checkList[i].Check(AddBytes))
                    neededList.Add(checkList[i]);

            checkList = null;
        }

        public bool NeedsUpdate()
        {
            return this.neededList.Count > 0;
        }

        List<DownloadJob> jobs = new List<DownloadJob>();
        public int PreUpdate()
        {
            List<PackObject> sortList = new List<PackObject>(list);
            sortList.Sort(SortOffsets);

            for (int i = 0; i < sortList.Count; i++)
            {
                if (!(sortList[i] is PackFile))
                    continue;

                int nextOffset = -1;
                for (int j = i + 1; j < sortList.Count; j++)
                    if (sortList[j] is PackFile)
                    {
                        nextOffset = ((PackFile)sortList[j]).offset;
                        break;
                    }

                if (nextOffset < 0)
                    nextOffset = fileSize;

                ((PackFile)sortList[i]).CompressedSize = nextOffset - ((PackFile)sortList[i]).offset;
            }

            neededList.Sort(SortOffsets);

            jobs.Clear();
            int bytesToLoad = 0;
            for (int i = 0; i < neededList.Count; i++)
            {
                DownloadJob job = new DownloadJob();
                job.Files.Add(neededList[i]);
                bytesToLoad += neededList[i].CompressedSize;

                for (int j = i + 1; j < neededList.Count; j++)
                {
                    if (neededList[j].offset - (neededList[i].offset + neededList[i].CompressedSize) < 500000)
                    {
                        job.Files.Add(neededList[j]);
                        bytesToLoad += neededList[j].CompressedSize;
                        i = j;
                    }
                    else
                    {
                        break;
                    }
                }
                jobs.Add(job);
            }

            return bytesToLoad;
        }

        static int SortOffsets(PackObject p1, PackObject p2)
        {
            if (p1 is PackFile)
            {
                if (p2 is PackDir)
                    return 0;

                return ((PackFile)p1).offset.CompareTo(((PackFile)p2).offset);
            }
            return 0;
        }

        public void DoUpdate(Action<int> AddBytes)
        {
            for (int i = 0; i < jobs.Count; i++)
                jobs[i].Load(this.URL, AddBytes);
        }
    }
}
