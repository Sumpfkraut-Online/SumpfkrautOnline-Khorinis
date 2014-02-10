using System;
using System.Collections.Generic;
using System.Text;

namespace GMP_Server.Scripting.Listener
{
    public interface IChatListener
    {
        bool OnChatMessageReceived(Player pl, ref String name, ref String content, ref byte type);
    }
}
