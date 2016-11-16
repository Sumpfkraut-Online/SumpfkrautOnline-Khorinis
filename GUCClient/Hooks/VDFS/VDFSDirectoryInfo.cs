using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace GUC.Hooks.VDFS
{
    class VDFSDirectoryInfo
    {
        List<VDFSDirectoryInfo> dirs = new List<VDFSDirectoryInfo>();
        public List<VDFSDirectoryInfo> SubDirectories { get { return this.dirs; } }

        Dictionary<string, VDFSFileInfo> fileNames = new Dictionary<string, VDFSFileInfo>(StringComparer.OrdinalIgnoreCase);
        public Dictionary<string, VDFSFileInfo> FileNames { get { return this.fileNames; } }

        string path;
        public string Path { get { return this.path; } }

        public VDFSDirectoryInfo(string path)
        {
            this.path = path;
        }

        public VDFSFileInfo SearchFile(string fileName)
        {
            VDFSFileInfo fi;
            if (fileNames.TryGetValue(fileName, out fi))
                return fi;

            for (int i = 0; i < dirs.Count; i++)
            {
                fi = dirs[i].SearchFile(fileName);
                if (fi != null)
                    return fi;
            }

            return null;
        }
    }
}
