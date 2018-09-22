using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace GUC.Hooks.VDFS
{
    // Achieved with the source code of the Gothic VDFS Tool and Systempack
    class VDFSArchive
    {
        public struct TimeStamp
        {
            public int Value;
            public int Second { get { return 2 * (Value & 0x1F); } } // 5 bits
            public int Minute { get { return (Value >> 5) & 0x3F; } } // 6 bits
            public int Hour { get { return (Value >> 11) & 0x1F; } } // 5 bits
            public int Day { get { return (Value >> 16) & 0x1F; } } // 5 bits
            public int Month { get { return (Value >> 21) & 0xF; } } // 4 bits
            public int Year { get { return 1980 + (Value >> 25) & 0x7F; } } // 7 bits

            public TimeStamp(int value)
            {
                this.Value = value;
            }
        }

        public struct Header
        {
            public const int CommentLength = 256;
            public const int SignatureLength = 16;

            public char[] Comment;
            public char[] Signature;
            public uint EntryCount;
            public uint FileCount;
            public TimeStamp TimeStamp;
            public uint DataSize;
            public uint TableOffset;
            public uint Version;

            public Header(BinaryReader br)
            {
                this.Comment = br.ReadChars(CommentLength);
                this.Signature = br.ReadChars(SignatureLength);
                this.EntryCount = br.ReadUInt32();
                this.FileCount = br.ReadUInt32();
                this.TimeStamp = new TimeStamp(br.ReadInt32());
                this.DataSize = br.ReadUInt32();
                this.TableOffset = br.ReadUInt32();
                this.Version = br.ReadUInt32();
            }
        }

        public struct Entry
        {
            public const int NameLength = 64;

            public string Name;
            public uint JumpTo; // Dirs = child entry's number, Files = data offset
            public uint Size;
            public uint Type;
            public uint Attribute;

            public const uint AttributeDirectory = 0x80000000;
            public const uint AttributeLastEntry = 0x40000000;

            public Entry(BinaryReader br)
            {
                this.Name = new string(br.ReadChars(NameLength)).TrimEnd(' ');
                this.JumpTo = br.ReadUInt32();
                this.Size = br.ReadUInt32();
                this.Type = br.ReadUInt32();
                this.Attribute = br.ReadUInt32();
            }

            public bool IsDirectory { get { return (this.Type & AttributeDirectory) == AttributeDirectory; } }
            public bool IsLastEntry { get { return (this.Type & AttributeLastEntry) == AttributeLastEntry; } }
        }

        public static Dictionary<string, VDFSDirectoryInfo> vDirs = new Dictionary<string, VDFSDirectoryInfo>(StringComparer.OrdinalIgnoreCase);
        public static Dictionary<string, VDFSFileInfo> vFiles = new Dictionary<string, VDFSFileInfo>(StringComparer.OrdinalIgnoreCase);

        TimeStamp timeStamp;
        FileInfo fileInfo;
        public FileInfo FileInfo { get { return this.fileInfo; } }
        bool projectVDFS;

        public VDFSArchive(FileInfo info, bool projectVDFS)
        {
            this.fileInfo = info;
            this.projectVDFS = projectVDFS;
            using (Stream stream = info.OpenRead())
            using (BinaryReader br = new BinaryReader(stream, Encoding.Default))
            {
                this.timeStamp = new Header(br).TimeStamp;
                ReadEntry(br, "", null);
            }
        }

        void ReadEntry(BinaryReader br, string name, VDFSDirectoryInfo parent)
        {
            string path = parent == null ? name : Path.Combine(parent.Path, name);

            if (!vDirs.TryGetValue(path, out VDFSDirectoryInfo dir))
            {
                dir = new VDFSDirectoryInfo(path);
                if (parent != null)
                    parent.SubDirectories.Add(dir);
                vDirs.Add(path, dir);
            }

            List<Entry> directories = new List<Entry>(1);
            Entry entry;
            do
            {
                entry = new Entry(br);
                if (entry.IsDirectory)
                {
                    directories.Add(entry);
                }
                else
                {
                    AddFile(dir, entry);
                }
            } while (!entry.IsLastEntry);

            for (int i = 0; i < directories.Count; i++)
            {
                ReadEntry(br, directories[i].Name, dir);
            }
        }

        void AddFile(VDFSDirectoryInfo dir, Entry entry)
        {
            string filePath = Path.Combine(dir.Path, entry.Name);

            if (vFiles.TryGetValue(filePath, out VDFSFileInfo other))
            {// there is already a file with that path
                if (this.projectVDFS) // this one is more important
                {
                    if (other.Archive.projectVDFS)
                    {
                        if (other.Archive.timeStamp.Value < this.timeStamp.Value)
                        { // this file is newer
                            other.SetSource(this, entry.JumpTo, entry.Size); // replace
                        }
                    }
                    else
                    {
                        other.SetSource(this, entry.JumpTo, entry.Size); // replace
                    }
                }
                else if (!other.Archive.projectVDFS && other.Archive.timeStamp.Value < this.timeStamp.Value)
                {// this file is newer
                    other.SetSource(this, entry.JumpTo, entry.Size); // replace 
                }

            }
            else
            {
                VDFSFileInfo file = new VDFSFileInfo(filePath, dir, this, entry.JumpTo, entry.Size);
                vFiles.Add(filePath, file);
                dir.FileNames.Add(entry.Name, file);
            }
        }
    }
}
