using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;

namespace Launcher2
{
    class PackFile
    {
        public bool lastEntry;
        public int parentNum;
        public int VDFSNum; //so we don't have to sort them again
        public uint oldVDFSOffset;
        public string name;
        public uint size; //uncompressed size
        public uint offset;
        public string hash;

        public uint compressedSize;
        public bool needsUpdate;

        public PackFile(BinaryReader br)
        {
            parentNum = br.ReadUInt16();
            lastEntry = (parentNum & 0x8000) == 0x8000;
            parentNum = (parentNum & ~0x8000) - 1; //attention: no parent directory == -1

            VDFSNum = br.ReadUInt16();

            name = br.ReadString();
            size = br.ReadUInt32();
            offset = br.ReadUInt32();

            char[] hashArray = new char[32];
            br.Read(hashArray, 0, hashArray.Length);
            hash = new string(hashArray);
        }

        public bool IsVDFS()
        {
            return VDFSNum < (ushort)0xFFFF;
        }

        public async Task<bool> CheckForUpdate(List<PackDir> dirList, VDFS oldVDFS)
        {
            needsUpdate = true;
            
            try
            {
                if (IsVDFS())
                {
                    if (oldVDFS != null)
                    {
                        string vdfsName = name.PadRight(64, ' ');
                        VDFSEntry entry = oldVDFS.table[oldVDFS.table.FindIndex(item => String.Compare(item.name, vdfsName, 0) == 0)];
                        if (entry.offset > 0 && entry.size == size) //Größe stimmt
                        {
                            oldVDFS.stream.Seek(entry.offset, SeekOrigin.Begin);
                            if (await SameHash(oldVDFS.stream, size)) //Hash stimmt
                            {
                                oldVDFSOffset = entry.offset;
                                needsUpdate = false;
                                if (entry.oldPath != GetFullPath(dirList)) //Pfade stimmen nicht
                                {
                                    return true; //update
                                }
                            }
                        }
                    }
                }
                else
                {
                    FileInfo fi;
                    if (name == "Launcher2.exe")
                    {
                        fi = new FileInfo(System.AppDomain.CurrentDomain.FriendlyName);
                    }
                    else
                    {
                        fi = new FileInfo(Global.Paths.Main + "\\" + GetFullName(dirList));
                    }
                    
                    if (fi.Exists && fi.Length == size)
                    {
                        using (FileStream fs = fi.OpenRead())
                        {
                            if (await SameHash(fs, size))
                            {
                                needsUpdate = false;
                            }
                        }
                    }
                }
            }
            catch {}

            return needsUpdate;
        }

        private string GetFullName(List<PackDir> dirList)
        {
            string path = name; PackDir curDir;
            int parent = parentNum;

            while (parent > -1)
            {
                curDir = dirList[parent];
                parent = curDir.parentNum;
                path = string.Concat(curDir.name, "\\", path);
            }

            return path;
        }

        public string GetFullPath(List<PackDir> dirList)
        {
            string path = ""; PackDir curDir;
            int parent = parentNum;
            while (parent > -1)
            {
                curDir = dirList[parent];
                parent = curDir.parentNum;
                path = string.Concat(curDir.name, "\\", path);
            }

            return path;
        }

        private async Task<bool> SameHash(Stream fs, long length)
        {
            try
            {
                string oldHash = "";

                using (MD5 md5 = new MD5CryptoServiceProvider())
                {
                    byte[] buffer = new byte[262144]; //256kB

                    

                    int n = 0;
                    while (n + buffer.Length < length)
                    {
                        n += await fs.ReadAsync(buffer, 0, buffer.Length);
                        md5.TransformBlock(buffer, 0, buffer.Length, null, 0);
                    }
                    int rest = (int)(length - n);
                    if (rest > 0)
                    {
                        await fs.ReadAsync(buffer, 0, rest);
                        md5.TransformFinalBlock(buffer, 0, rest);
                    }

                    oldHash = BitConverter.ToString(md5.Hash).Replace("-", "").ToLower(); //in string wandeln  
                }

                if (oldHash == hash)
                {
                    return true;
                }
            }
            catch {}

            return false; 
        }
    }
}
