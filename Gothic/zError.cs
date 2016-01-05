using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;

namespace Gothic
{
    public static class zError
    {
        /// <summary>
        /// Reports the message to zSpy.
        /// </summary>
        /// <param name="levelType">1=Info, 2=Warning, 3=Fault(Message Box!), 4=Critical (MessageBox + Crash!)</param>
        /// <param name="user"></param>
        /// <param name="message"></param>
        /// <param name="errorlevel">0-9?</param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static int Report(int levelType, char user, String message, int errorlevel, String filename, int line)
        {
            //zERROR::Report(zERROR_TYPE,int,zSTRING const &,signed char,uint,int,char *,char *)
            //zERROR::Report(zERROR_Level,int,zSTRING const &,signed char,uint,int,char *,char *)

            message = user + ":" + message;

            zString messageStr = zString.Create(Process, message);
            IntPtr filenamePtr = Process.Alloc((uint)(filename.Length + 1));
            System.Text.Encoding enc = System.Text.Encoding.Default;
            Process.Write(enc.GetBytes(filename), filenamePtr.ToInt32());
            Process.Write(0, filenamePtr.ToInt32() + filename.Length);


            int x = Process.THISCALL<IntArg>((uint)Address, (uint)FuncOffsets.Report_Level, new CallValue[] { (IntArg)levelType, (IntArg)0, messageStr, (IntArg)errorlevel, (IntArg)0, (IntArg)line, (IntArg)filenamePtr.ToInt32(), (IntArg)0 });
            messageStr.Dispose();
            Process.Free(filenamePtr, (uint)(filename.Length + 1));
            return x;
        }
    }
}
