using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Network
{
    public enum PktPriority
    {
        Immediate,
        High,
        Medium,
        Low
    }

    public enum PktReliability
    {
        Unreliable,
        UnreliableSequenced,
        Reliable,
        ReliableOrdered,
        ReliableSequenced,
        UnreliableWithAckReceipt,
        ReliableWithAckReceipt,
        ReliableOrderedWithAckReceipt
    }
}
