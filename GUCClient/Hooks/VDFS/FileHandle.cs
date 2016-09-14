using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using WinApi;

namespace GUC.Hooks.VDFS
{
    interface IFileHandle
    {
        void Open();
        void Close();
        void Seek(long position);
        long GetSize();
        int Read(int ptr, int count);
    }

    class FileHandle : IFileHandle
    {
        FileInfo info;
        Stream stream;

        public FileHandle(FileInfo info)
        {
            this.info = info;
        }

        public void Open()
        {
            this.stream = info.OpenRead();
        }

        public void Close()
        {
            this.stream.Dispose();
            this.stream = null;
        }

        public void Seek(long position)
        {
            stream.Seek(position, SeekOrigin.Begin);
        }

        public long GetSize()
        {
            return info.Length;
        }

        public int Read(int ptr, int count)
        {
            byte[] buf = new byte[count];
            int readBytes = stream.Read(buf, 0, count);
            Process.Write(buf, ptr);
            return readBytes;
        }
    }

    class VDFSFileHandle : IFileHandle
    {
        VDFSFileInfo info;
        Stream stream;

        public VDFSFileHandle(VDFSFileInfo info)
        {
            this.info = info;
        }

        public void Open()
        {
            this.stream = info.Archive.FileInfo.OpenRead();
            this.Seek(0);
        }

        public void Close()
        {
            this.stream.Dispose();
            this.stream = null;
        }

        public void Seek(long position)
        {
            stream.Seek(info.Offset + position, SeekOrigin.Begin);
        }

        public long GetSize()
        {
            return info.Size;
        }

        public int Read(int ptr, int count)
        {
            byte[] buf = new byte[count];
            int readBytes = stream.Read(buf, 0, count);
            Process.Write(buf, ptr);
            return readBytes;
        }
    }
}
