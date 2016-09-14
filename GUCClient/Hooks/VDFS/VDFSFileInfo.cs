using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Hooks.VDFS
{
    class VDFSFileInfo
    {
        string path;
        public string Path { get { return this.path; } }
        VDFSArchive archive;
        public VDFSArchive Archive { get { return this.archive; } }
        uint offset;
        public uint Offset { get { return this.offset; } }
        uint size;
        public uint Size { get { return this.size; } }

        public VDFSFileInfo(string path, VDFSArchive archive, uint offset, uint size)
        {
            this.path = path;
            this.archive = archive;
            this.offset = offset;
            this.size = size;
        }
    }
}
