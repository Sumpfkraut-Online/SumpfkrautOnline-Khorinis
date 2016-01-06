using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Launcher2
{
    class VDFSEntry
    {
        const int MAX_NAME_LENGTH = 64;
        const uint VDFS_FILE_DIRECTORY = 0x80000000;
        const uint VDFS_FILE_LASTENTRY = 0x40000000;

        public string name;
        public uint offset;
        public uint size;
        public uint flags;
        public uint attributes;

        public int VDFSnum;

        public string oldPath; //just for the update

        public VDFSEntry(BinaryReader br, string path)
        {
            name = new string(br.ReadChars(MAX_NAME_LENGTH));
            offset = br.ReadUInt32();
            size = br.ReadUInt32();
            flags = br.ReadUInt32();
            attributes = br.ReadUInt32();

            oldPath = path;
        }

        public bool IsDirectory()
        {
            return (flags & VDFS_FILE_DIRECTORY) == VDFS_FILE_DIRECTORY;
        }

        public bool IsLastEntry()
        {
            return (flags & VDFS_FILE_LASTENTRY) == VDFS_FILE_LASTENTRY;
        }

        public VDFSEntry(PackFile file)
        {
            name = string.Copy(file.name);
            flags = 0;
            size = file.size;
            attributes = 0;
            offset = 0;
            if (file.lastEntry)
            {
                flags = VDFS_FILE_LASTENTRY;
            }
            VDFSnum = file.VDFSNum;
        }

        public VDFSEntry(PackDir dir)
        {
            name = string.Copy(dir.name);
            flags = VDFS_FILE_DIRECTORY;
            size = 0;
            attributes = 0;
            offset = dir.vdfsOffset;
            if (dir.lastEntry)
            {
                flags |= VDFS_FILE_LASTENTRY;
            }
            VDFSnum = dir.VDFSNum;
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write(name.PadRight(MAX_NAME_LENGTH, ' ').ToCharArray());
            bw.Write(offset);
            bw.Write(size);
            bw.Write(flags);
            bw.Write(attributes);
        }
    }

}
