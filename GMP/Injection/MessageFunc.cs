using System;
using System.Collections.Generic;
using System.Text;
using Gothic.zClasses;
using WinApi;

namespace GMP.Injection
{
    public class MessageFunc
    {
        public static UInt32 ShowAlreadyBanned()
        {
            zERROR.GetZErr(Process.ThisProcess()).Report(4, 'G', "Du bist von diesem Server gebannt", 0, "MessageFunc.cs", 0);
            return 0;
        }

        public static UInt32 ShowInvalidPassword()
        {
            zERROR.GetZErr(Process.ThisProcess()).Report(4, 'G', "Falsches Passwort", 0, "MessageFunc.cs", 0);
            return 0;
        }

        public static UInt32 ShowServerFull()
        {
            zERROR.GetZErr(Process.ThisProcess()).Report(4, 'G', "Der Server ist voll", 0, "MessageFunc.cs", 0);
            return 0;
        }
    }
}
