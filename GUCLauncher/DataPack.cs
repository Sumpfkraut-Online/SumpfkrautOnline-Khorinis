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

        readonly List<PackObject> contents = new List<PackObject>();

        int fileSize;
        List<PackFile> checkList = new List<PackFile>();
        List<PackFile> neededList = new List<PackFile>();

        public int Read(PacketStream header, string path)
        {
            this.fileSize = header.ReadInt();
            this.URL = header.ReadStringShort();

            contents.Clear();
            checkList.Clear();

            int count = header.ReadUShort();
            return ReadObjects(header, path, count, 0);
        }

        int ReadObjects(PacketStream header, string path, int count, int checkBytes)
        {
            while (contents.Count < count)
            {
                PackObject p = PackObject.ReadNew(header, path);
                contents.Add(p);

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
            List<PackFile> fileList = new List<PackFile>(contents.Count);

            // create directories and sort them out
            for (int i = 0; i < contents.Count; i++)
            {
                if (contents[i] is PackFile)
                    fileList.Add((PackFile)contents[i]);
                else
                {
                    DirectoryInfo info = ((PackDir)contents[i]).Info;
                    if (!info.Exists)
                        info.Create();
                }
            }

            // sort file list by offsets
            fileList.Sort(SortOffsets);

            // calculate the sizes of all files
            for (int i = 0; i < fileList.Count; i++)
            {
                int nextOffset;
                if (i + 1 < fileList.Count)
                {
                    nextOffset = fileList[i + 1].offset;
                }
                else
                {
                    nextOffset = fileSize;
                }

                fileList[i].CompressedSize = nextOffset - fileList[i].offset;
            }

            // sort needed files by offsets
            neededList.Sort(SortOffsets);

            jobs.Clear();
            int bytesToLoad = 0;
            // create job list
            for (int i = 0; i < neededList.Count; i++)
            {
                DownloadJob job = new DownloadJob();
                job.Files.Add(neededList[i]);
                bytesToLoad += neededList[i].CompressedSize;

                for (int j = i + 1; j < neededList.Count; j++)
                {
                    if (neededList[j].offset - (neededList[i].offset + neededList[i].CompressedSize) < 500000) // if the next file is within 500kb, don't stop the connection
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

        static int SortOffsets(PackFile p1, PackFile p2)
        {
            return p1.offset.CompareTo(p2.offset);
        }

        public void DoUpdate(Action<int> AddBytes)
        {
            for (int i = 0; i < jobs.Count; i++)
                jobs[i].Load(this.URL, AddBytes);
        }
    }
}
