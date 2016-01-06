using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;

namespace Launcher2
{
    class DownloadJob
    {
        public int startNum;
        public int endNum;

        //Local
        public DownloadJob(int start, int end)
        {
            startNum = start;
            endNum = end;
        }

        public async Task Download(string URL, List<PackDir> dirList, List<PackFile> fileList, VDFS newVDFS)
        {
            Download dl = new Download(URL);

            using (Stream res = await dl.StartStream(fileList[startNum].offset, fileList[endNum].offset + fileList[endNum].compressedSize))
            {
                PackFile file;
                for (int i = startNum; i <= endNum; i++)
                {
                    try
                    {
                        file = fileList[i];
                        FileStream fs;

                        if (file.IsVDFS())
                        {
                            fs = newVDFS.fileInfo.Open(FileMode.Open, FileAccess.Write, FileShare.Write);
                            fs.Seek(newVDFS.table[file.VDFSNum].offset, SeekOrigin.Begin);
                        }
                        else
                        {
                            string path = file.GetFullPath(dirList);
                            if (path != "" && !Directory.Exists(path))
                            {
                                Directory.CreateDirectory(Global.Paths.Main + "\\" + path);
                            }
                            if (file.name == "Launcher2.exe")
                            {
                                fs = new FileStream(System.AppDomain.CurrentDomain.FriendlyName + ".tmp", FileMode.Create, FileAccess.Write, FileShare.None);
                                Global.Progress.NewLauncher();
                            }
                            else
                            {
                                fs = new FileStream(Global.Paths.Main + "\\" + path + file.name, FileMode.Create, FileAccess.Write, FileShare.None);
                            }
                        }

                        using (DownloadStream dlStream = new DownloadStream(res, (int)file.compressedSize))
                        using (GZipStream zip = new GZipStream(dlStream, CompressionMode.Decompress, true))
                        {
                            await zip.CopyToAsync(fs);
                        }

                        fs.Close();
                    }
                    catch { }
                }
            }
        }
    }

    class CopyJob
    {
        public int num;

        //Local
        public CopyJob(int inputNum)
        {
            num = inputNum;
        }

        public async Task Copy(List<PackFile> fileList, VDFS oldVDFS, VDFS newVDFS)
        {
            oldVDFS.stream.Seek(fileList[num].oldVDFSOffset, SeekOrigin.Begin);
            newVDFS.stream.Seek(newVDFS.table[fileList[num].VDFSNum].offset, SeekOrigin.Begin);

            byte[] buffer = new byte[524288]; uint n = 0;
            while (n + buffer.Length < fileList[num].size)
            {
                await oldVDFS.stream.ReadAsync(buffer, 0, buffer.Length);
                await newVDFS.stream.WriteAsync(buffer, 0, buffer.Length);
                n += (uint)buffer.Length;
            }
            uint rest = fileList[num].size - n;
            if (rest > 0)
            {
                await oldVDFS.stream.ReadAsync(buffer, 0, (int)rest);
                await newVDFS.stream.WriteAsync(buffer, 0, (int)rest);
            }
        }
    }

    class JobList
    {
        private List<DownloadJob> downloadJobs;
        private List<CopyJob> copyJobs;
        private uint neededBytes;

        public void CreateList(List<PackFile> fileList)
        {
            downloadJobs = new List<DownloadJob>();
            copyJobs = new List<CopyJob>();
            neededBytes = 0;

            bool chaining = false;
            PackFile file;

            for (int i = 0; i < fileList.Count; i++)
            {
                file = fileList[i];

                if (file.needsUpdate)
                {
                    if (chaining == false)
                    {
                        neededBytes += file.compressedSize;
                        downloadJobs.Add(new DownloadJob(i, i));
                        chaining = true;
                    }
                    else
                    {
                        neededBytes += file.compressedSize;
                        downloadJobs[downloadJobs.Count - 1].endNum = i;
                    }
                }
                else
                {
                    if (chaining == false)
                    {
                        if (fileList[i].IsVDFS())
                        {
                            copyJobs.Add(new CopyJob(i));
                        }
                    }
                    else
                    {
                        //skip at least 512kB
                        uint skippedBytes = 0; int num = i; bool continueChaining = false;
                        while (skippedBytes + file.compressedSize < 524288)
                        {
                            skippedBytes += file.compressedSize;
                            num++; if (num == fileList.Count) break;
                            file = fileList[num];
                            if (file.needsUpdate == true)
                            {
                                continueChaining = true;
                                i = num - 1;
                                break;
                            }
                        }

                        if (!continueChaining)
                        {
                            if (fileList[i].IsVDFS())
                            {
                                copyJobs.Add(new CopyJob(i));
                            }
                            chaining = false; //stop
                        }
                        else
                        {
                            neededBytes += skippedBytes;
                        }
                    }
                }
            }
        }


        private bool copyJobsFinished;
        private bool downloadJobsFinished;
        public void Do(string URL, List<PackDir> dirList, List<PackFile> fileList, VDFS oldVDFS, VDFS newVDFS)
        {
            copyJobsFinished = false;
            downloadJobsFinished = false;

            DoCopyJobs(fileList, oldVDFS, newVDFS);
            DoDownloadJobs(URL, dirList, fileList, newVDFS);
        }

        private async void DoCopyJobs(List<PackFile> fileList, VDFS oldVDFS, VDFS newVDFS)
        {
            if (oldVDFS != null && newVDFS != null)
            {
                using (oldVDFS.stream = oldVDFS.fileInfo.OpenRead())
                using (newVDFS.stream = newVDFS.fileInfo.Open(FileMode.Open, FileAccess.Write, FileShare.Write))
                    foreach (CopyJob job in copyJobs)
                    {
                        await job.Copy(fileList, oldVDFS, newVDFS);
                    }
            }
            copyJobsFinished = true;
            Finish();
        }

        private async void DoDownloadJobs(string URL, List<PackDir> dirList, List<PackFile> fileList, VDFS newVDFS)
        {
            Global.Progress.SetMaximum(neededBytes);
            foreach (DownloadJob job in downloadJobs)
            {
                await job.Download(URL, dirList, fileList, newVDFS);
            }

            downloadJobsFinished = true;
            Finish();

        }

        private void Finish()
        {
            if (downloadJobsFinished && copyJobsFinished)
            {
                Global.Progress.DownloadsFinished();
            }
        }
    }
}
