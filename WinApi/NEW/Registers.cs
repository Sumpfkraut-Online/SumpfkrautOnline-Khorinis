using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinApi.NEW
{
    [Flags]
    public enum Registers // PUSHAD ORDER !!!
    {
        EDI = 1 << 0,
        ESI = 1 << 1,
        EBP = 1 << 2,
        ESP = 1 << 3,
        EBX = 1 << 4,
        EDX = 1 << 5,
        ECX = 1 << 6,
        EAX = 1 << 7,
        
        ABC = EAX | EBX | ECX,
        ABCD = EAX | EBX | ECX | EDX,
        ALL = EDI | ESI | EBP | ESP | EBX | EDX | ECX | EAX,
    }
}
