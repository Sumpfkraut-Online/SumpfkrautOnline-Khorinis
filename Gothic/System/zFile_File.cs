using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;
using Gothic.Types;

namespace Gothic.System
{
    public class zFile_File : zFile
    {
        public zFile_File(int address) : base(address)
        {
        }

        public static bool InitFileSystem()
        {
            return Process.CDECLCALL<BoolArg>(0x4485E0);
        }

        public static bool DeinitFileSystem()
        {
            return Process.CDECLCALL<BoolArg>(0x448650);
        }

        public zString Directory { get { return new zString(Address + 0x10); } }
        public zString Drive { get { return new zString(Address + 0x24); } }
        public zString FileName { get { return new zString(Address + 0x38); } }
        public zString Extension { get { return new zString(Address + 0x4C); } }
        public zString FullPath { get { return new zString(Address + 0x60); } }
        public zString FullDir { get { return new zString(Address + 0x74); } }


        public void SetCompletePath()
        {
            Process.THISCALL<NullReturnCall>(this.Address, 0x445360);
        }

        public void SetPath(string path)
        {
            // using (zString z = zString.Create(path)) this causes crashes because the file keeps the zstring
            zString z = zString.Create(path);
            Process.THISCALL<NullReturnCall>(this.Address, 0x4455D0, (IntArg)z.VTBL, (IntArg)z.ALLOCATER, (IntArg)z.PTR, (IntArg)z.Length, (IntArg)z.Res);
        }
    }
}
