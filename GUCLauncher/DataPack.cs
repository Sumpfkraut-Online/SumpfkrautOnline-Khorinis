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

        readonly List<PackObject> list = new List<PackObject>();

        int fileSize;
        List<PackFile> checkList = new List<PackFile>();
        List<PackFile> neededList = new List<PackFile>();

        public int Read(PacketStream header, string path)
        {
            this.fileSize = header.ReadInt();
            this.URL = header.ReadStringShort();

            list.Clear();
            checkList.Clear();

            int count = header.ReadUShort();
            return ReadObjects(header, path, count, 0);
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
                if ((sortList[i] is PackFile))
                {
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
                else
                {
                    DirectoryInfo info = ((PackDir)sortList[i]).Info;
                    if (!info.Exists)
                        info.Create();
                }
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
