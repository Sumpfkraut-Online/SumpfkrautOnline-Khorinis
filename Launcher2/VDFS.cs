using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace Launcher2
{


    class VDFS
    {
        private string name;
        public FileInfo fileInfo;

        public VDFSHeader header;
        public List<VDFSEntry> table;

        public FileStream stream;

        //For reading an old VDFS
        public VDFS(string inputName)
        {
            name = inputName;

            fileInfo = new FileInfo(Global.Paths.Data + "\\" + name + ".vdf");
            stream = fileInfo.OpenRead();

            using (BinaryReader br = new BinaryReader(stream, Encoding.UTF8, true))
            {
                header = new VDFSHeader(br);
                table = new List<VDFSEntry>();
                /*for (int i = 0; i < header.entryCount; i++)
                {
                    table.Add(new VDFSEntry(br));
                }*/
                VDFSEntry _work = new VDFSEntry(br, "");
                table.Add(_work);
                ReadEntries(br, _work, "");
            }
        }

        private void ReadEntries(BinaryReader br, VDFSEntry curEntry, string path)
        {
            string curPath = string.Concat(path, curEntry.name.TrimEnd(' '), "\\");

            List<VDFSEntry> eList = new List<VDFSEntry>();
            VDFSEntry entry;
            do
            {
                entry = new VDFSEntry(br, curPath);
                eList.Add(entry);
            } while (!entry.IsLastEntry());
            
            table.AddRange(eList);
            foreach (VDFSEntry item in eList)
            {
                if (item.IsDirectory())
                {
                    ReadEntries(br, item, curPath);
                }
            }
        }

        //For creating a new VDFS
        public VDFS(string inputName, List<PackDir> packDirList, List<PackFile> packFileList)
        {
            name = inputName;
            header = new VDFSHeader(name);

            table = new List<VDFSEntry>();
            foreach (PackDir dir in packDirList)
            {
                if (dir.IsVDFS())
                {
                    table.Add(new VDFSEntry(dir));
                }
            }
            foreach (PackFile file in packFileList)
            {
                if (file.IsVDFS())
                {
                    table.Add(new VDFSEntry(file));
                    header.fileCount++;
                }
            }
            table = table.OrderBy(o => o.VDFSnum).ToList(); //sort

            header.entryCount = (uint)table.Count;

            uint offset = header.tableOffset + header.entryCount * header.entrySize;
            foreach (VDFSEntry entry in table)
            {
                if (!entry.IsDirectory())
                {
                    entry.offset = offset;
                    offset += entry.size;
                    header.dataSize += entry.size;
                }
            }
        }

        public void CreateEmptyFile()
        {
            using (FileStream newStrm = new FileStream(Global.Paths.Data + "\\" + name + ".vdf", FileMode.Create, FileAccess.Write, FileShare.Read))
            using (BinaryWriter bw = new BinaryWriter(newStrm))
            {
                header.Write(bw);
                foreach (VDFSEntry entry in table)
                {
                    entry.Write(bw);
                }

                //fill data with 00
                byte[] buffer = Enumerable.Repeat((byte)0, 524288).ToArray(); //512kB buffer
                uint n = 0;
                while (n + buffer.Length < header.dataSize)
                {
                    newStrm.Write(buffer, 0, buffer.Length);
                    n += (uint)buffer.Length;
                }
                uint rest = header.dataSize - n;
                if (rest > 0)
                {
                    newStrm.Write(buffer, 0, (int)rest);
                }
            }

            fileInfo = new FileInfo(Global.Paths.Data + "\\" + name + ".vdf");
        }
    }
}
