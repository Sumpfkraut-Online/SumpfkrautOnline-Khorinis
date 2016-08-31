using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUC.Scripts.Sumpfkraut.Networking
{
    public enum NetMsgID
    {
        MaxMessages
    }

    public enum NetWorldMsgID // byte
    {
        // Npc commands
        DropItem,
        DropItemAmount,

        HitMessage,
        ParryMessage,
        MaxWorldMessages
    }
}
