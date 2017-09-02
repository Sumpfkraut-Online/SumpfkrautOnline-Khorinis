using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC
{
    public enum NetPriority
    {
        Immediate,
        High,
        Medium,
        Low
    }

    public enum NetReliability
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
