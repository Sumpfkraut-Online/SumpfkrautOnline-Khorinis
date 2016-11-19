using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.Types;
using WinApi;

namespace Gothic.Objects.EventManager
{
    public class oCNpcMessage : zCEventMessage
    {
        new public abstract class VarOffsets : zCEventMessage.VarOffsets
        {
            public const int targetVobName = 44,//zString
            Attributes = 64; // highpriority => 0, deleted = 2, inUse = 4
        }

        public oCNpcMessage()  : base()
        {

        }

        public oCNpcMessage(int address)  : base(address)
        {

        }
        

        public zString TargetVobName
        {
            get
            {
                return new zString(Address + VarOffsets.targetVobName);
            }
        }

        public int Attributes
        {
            get
            {
                return Process.ReadInt(Address + VarOffsets.Attributes);
            }
            set
            {
                Process.Write(Address + VarOffsets.Attributes, value);
            }
        }
        public bool HighPriority
        {
            get
            {
                return (Attributes & 1) == 1;
            }
            set
            {
                Attributes = value ? Attributes | 1 : Attributes & ~1;
            }
        }

        public bool Deleted
        {
            get
            {
                return (Attributes & 2) == 2;
            }
            set
            {
                Attributes = value ? Attributes | 2 : Attributes & ~2;
            }
        }

        public bool InUse
        {
            get
            {
                return (Attributes & 4) == 4;
            }
            set
            {
                Attributes = value ? Attributes | 4 : Attributes & ~4;
            }
        }
    }
}
