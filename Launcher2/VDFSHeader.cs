using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Launcher2
{
    class VDFSHeader
    {
        const int COMMENT_LENGTH = 256;
        const int VERSION_LENGTH = 16;

        public string comment;
        public string version;

        public uint entryCount;
        public uint fileCount;
        public uint timeStamp;
        public uint dataSize;
        public uint tableOffset;
        public uint entrySize;

        //For reading an old VDFSHeader
        public VDFSHeader(BinaryReader br)
        {
            comment = new string(br.ReadChars(COMMENT_LENGTH));
            version = new string(br.ReadChars(VERSION_LENGTH));
            entryCount = br.ReadUInt32();
            fileCount = br.ReadUInt32();
            timeStamp = br.ReadUInt32();
            dataSize = br.ReadUInt32();
            tableOffset = br.ReadUInt32();
            entrySize = br.ReadUInt32();
        }

        //For creating a new VDFSHeader
        public VDFSHeader(string name)
        {
            comment = string.Copy((name + " Moddatei").PadRight(COMMENT_LENGTH, (char)0x1A));
            version = string.Copy("PSVDSC_V2.00\n\r\n\r");
            timeStamp = 0x462B9604;
            tableOffset = 296;//sizeof(VDFSHeader);
            entrySize = 80; //sizeof(VDFSEntry);

            dataSize = 0; //will be set by Create Table
            entryCount = 0; //will be set by Create Table
            fileCount = 0; //will be set by Create Table
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write(comment.ToCharArray());
            bw.Write(version.ToCharArray());
            bw.Write(entryCount);
            bw.Write(fileCount);
            bw.Write(timeStamp);
            bw.Write(dataSize);
            bw.Write(tableOffset);
            bw.Write(entrySize);
        }
    }
}
