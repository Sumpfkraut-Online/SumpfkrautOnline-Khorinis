using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;
using Gothic.Types;

namespace Gothic.System
{
    public class zError
    {
        public abstract class StaticVarAddresses
        {
            public const int zerr = 0x008CDCD0; /// <summary> zERROR </summary>
        }

        public abstract class FuncAddresses
        {
            public const int ReportByLevel = 0x0044C8C0; /// <summary> int zERROR::Report(zERROR_Level,int,zSTRING const &,signed char,uint,int,char *,char *) </summary>
        }

        /// <summary>
        /// Reports a message to zSpy.
        /// </summary>
        /// <param name="type">1=Info, 2=Warning, 3=Fault(Message Box!), 4=Critical (MessageBox + Crash!)</param>
        /// <param name="message"></param>
        /// <param name="level"> zSpy level 0-9 </param>
        /// <param name="filename"></param>
        public static int Report(int type, string message, int level, int line, string filename)
        {
            zString messageStr = zString.Create(message);

            IntPtr filenamePtr = Process.Alloc((uint)(filename.Length + 1));
            byte[] arr = Encoding.UTF8.GetBytes(filename);
            Process.Write(arr, filenamePtr.ToInt32());
            Process.Write(0, filenamePtr.ToInt32() + arr.Length);

            int x = Process.THISCALL<IntArg>(StaticVarAddresses.zerr, FuncAddresses.ReportByLevel, (IntArg)type, (IntArg)0, messageStr, (IntArg)level, (IntArg)0, (IntArg)line, (IntArg)filenamePtr.ToInt32(), (IntArg)0);

            Process.Free(filenamePtr, (uint)(arr.Length + 1));
            messageStr.Dispose();

            return x;
        }
    }
}
