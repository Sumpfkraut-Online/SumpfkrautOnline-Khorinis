using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Launcher2
{
    class DataPack
    {
        private string name;
        private string URL;
        private uint size;
        private ushort fileNum;
        private ushort dirNum;

        private List<PackDir> dirList;
        private List<PackFile> fileList;
        private JobList jobList;

        public bool needsUpdate;

        private VDFS oldVDFS;
        private VDFS newVDFS;
        private bool VDFSneedsUpdate;
                
        public DataPack(string inputName)
        {
            name = inputName;
            URL = "";
            fileNum = 0;
            dirNum = 0;

            try
            {
                oldVDFS = new VDFS(name);
            }
            catch
            {
                if (oldVDFS != null)
                {
                    if (oldVDFS.stream != null)
                    {
                        oldVDFS.stream.Close();
                    }
                    oldVDFS = null;
                }
            }
        }

        public async Task ReadInfoFile(BinaryReader br)
        {
            VDFSneedsUpdate = false;
            needsUpdate = false;

            URL = br.ReadString();

            size = br.ReadUInt32();

            dirNum = br.ReadUInt16();
            dirList = new List<PackDir>();
            PackDir dir;
            for (int i = 0; i < dirNum; i++)
            {
                dir = new PackDir(br);
                dirList.Add(dir);
            }

            fileNum = br.ReadUInt16();
            fileList = new List<PackFile>();
            PackFile file;

            Global.Progress.SetMaximum(fileNum);
            for (int i = 0; i < fileNum; i++)
            {
                file = new PackFile(br);
                bool fileUpdate = await file.CheckForUpdate(dirList, oldVDFS);
                fileList.Add(file);

                if (fileUpdate)
                {
                    needsUpdate = true;
                    if (file.IsVDFS())
                    {
                        VDFSneedsUpdate = true;
                    }
                }

                if (i > 0)
                {
                    fileList[i - 1].compressedSize = file.offset - fileList[i - 1].offset;
                }

                Global.Progress.Add(1);
            }
            fileList[fileNum - 1].compressedSize = size - fileList[fileNum - 1].offset;
        }

        public void Update()
        {
            jobList = new JobList();
            jobList.CreateList(fileList);
            if (VDFSneedsUpdate)
            {
                newVDFS = new VDFS(name, dirList, fileList);
                if (oldVDFS != null)
                {
                    if (oldVDFS.stream != null)
                    {
                        oldVDFS.stream.Close();
                    }
                    (new FileInfo(Global.Paths.Data + "\\" + name + ".tmp")).Delete();
                    oldVDFS.fileInfo.MoveTo(Global.Paths.Data + "\\" + name + ".tmp");
                }
                newVDFS.CreateEmptyFile();
            }  
            jobList.Do(URL, dirList, fileList, oldVDFS, newVDFS);

        }

        public void FinishUpdate()
        {
            if (VDFSneedsUpdate && oldVDFS != null && oldVDFS.fileInfo.Exists)
            {
                oldVDFS.fileInfo.Delete();
            }
        }
    }
}
