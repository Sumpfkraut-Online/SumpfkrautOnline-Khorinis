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

            public char[] Name;
            public uint JumpTo; // Dirs = child entry's number, Files = data offset
            public uint Size;
            public uint Type;
            public uint Attribute;

            public const uint AttributeDirectory = 0x80000000;
            public const uint AttributeLastEntry = 0x40000000;

            public Entry(BinaryReader br)
            {
                this.Name = br.ReadChars(NameLength);
                this.JumpTo = br.ReadUInt32();
                this.Size = br.ReadUInt32();
                this.Type = br.ReadUInt32();
                this.Attribute = br.ReadUInt32();
            }

            public bool IsDirectory { get { return (this.Type & AttributeDirectory) == AttributeDirectory; } }
            public bool IsLastEntry { get { return (this.Type & AttributeLastEntry) == AttributeLastEntry; } }
        }

        Header header;
        FileInfo fileInfo;
        public FileInfo FileInfo { get { return this.fileInfo; } }
        bool projectVDFS;
        int fileCount;
        public int FileCount { get { return this.fileCount; } }

        public VDFSArchive(FileInfo info, Dictionary<string, VDFSFileInfo> fileDict, bool projectVDFS)
        {
            this.fileInfo = info;
            this.projectVDFS = projectVDFS;
            using (Stream stream = info.OpenRead())
            using (BinaryReader br = new BinaryReader(stream, Encoding.UTF7))
            {
                this.header = new Header(br);
                ReadEntry(br, "", fileDict);
            }
        }

        void ReadEntry(BinaryReader br, string path, Dictionary<string, VDFSFileInfo> fileDict)
        {
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
                    string filePath = CombinePaths(path, entry.Name);
                    if (filePath != null)
                    {
                        VDFSFileInfo other;
                        if (fileDict.TryGetValue(filePath, out other))
                        {
                            // there is already a file with that path
                            if (this.projectVDFS) // this one is more important
                            {
                                if (other.Archive.projectVDFS)
                                {
                                    if (other.Archive.header.TimeStamp.Value < this.header.TimeStamp.Value)
                                    { // this file is newer
                                        fileDict[filePath] = new VDFSFileInfo(filePath, this, entry.JumpTo, entry.Size); // replace
                                        other.Archive.fileCount--;
                                        this.fileCount++;
                                    }
                                    else
                                    { // this file is older
                                        continue;
                                    }
                                }
                                else
                                {
                                    fileDict[filePath] = new VDFSFileInfo(filePath, this, entry.JumpTo, entry.Size); // replace
                                    other.Archive.fileCount--;
                                    this.fileCount++;
                                }
                            }
                            else
                            {
                                if (other.Archive.projectVDFS)
                                {
                                    continue;
                                }
                                else
                                {
                                    if (other.Archive.header.TimeStamp.Value < this.header.TimeStamp.Value)
                                    { // this file is newer
                                        fileDict[filePath] = new VDFSFileInfo(filePath, this, entry.JumpTo, entry.Size); // replace 
                                        other.Archive.fileCount--;
                                        this.fileCount++;
                                    }
                                    else
                                    { // this file is older
                                        continue;
                                    }
                                }
                            }
                        }
                        else
                        {
                            fileDict.Add(filePath, new VDFSFileInfo(filePath, this, entry.JumpTo, entry.Size));
                            this.fileCount++;
                        }
                    }
                }
            } while (!entry.IsLastEntry);

            for (int i = 0; i < directories.Count; i++)
            {
                string dirPath = CombinePaths(path, entry.Name);
                if (dirPath != null)
                {
                    ReadEntry(br, dirPath, fileDict);
                }
            }
        }

        readonly static StringBuilder pathBuilder = new StringBuilder(256);
        string CombinePaths(string path, char[] name)
        {
            int length;
            for (length = Entry.NameLength - 1; length > 0; length--)
            {
                if (!char.IsWhiteSpace(name[length]))
                {
                    break;
                }
                else if (length == 0)
                {
                    return null; // empty name 
                }
            }

            pathBuilder.Clear();
            pathBuilder.Append(path);
            pathBuilder.Append('\\');
            pathBuilder.Append(name, 0, length + 1);
            return pathBuilder.ToString();
        }
    }
}
