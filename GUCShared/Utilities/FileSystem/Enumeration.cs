using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Utilities.FileSystem.Enumeration
{

    public enum FileSystemManipulation
    {
        //Undefined, 

        Create, 
        Delete, 
        Lock, 
        Move, 
        Unlock, 

        CreateFile, 
        DeleteFile, 
        LockFile, 
        MoveFile, 
        UnlockFile, 

        CreateFolder, 
        DeleteFolder, 
        LockFolder, 
        MoveFolder, 
        UnlockFolder, 
    }

    public enum ProtocolStatus
    {
        Undefined, 
        Fail, 
        FinalFail, 
        FinalSuccess, 
    }
    
}
