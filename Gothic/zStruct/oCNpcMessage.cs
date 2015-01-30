using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.zClasses;
using WinApi;
using Gothic.zTypes;

namespace Gothic.zStruct
{
    public class oCNpcMessage : zCEventMessage
    {
        public enum Offsets
        {
            targetVobName = 44,//zString
            Attributes = 64, // highpriority => 0, deleted = 2, inUse = 4
        }


        public oCNpcMessage()
            : base()
        {

        }

        public oCNpcMessage(Process process, int address)
            : base(process, address)
        {

        }



        #region statics

        
        #endregion

        #region Fields

        public zString TargetVobName
        {
            get
            {
                return new zString(Process, Address + (int)Offsets.targetVobName);
            }
        }

        public int Attributes
        {
            get
            {
                return Process.ReadInt(Address + (int)Offsets.Attributes);
            }
            set
            {
                Process.Write(value, Address + (int)Offsets.Attributes);
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
                Attributes = value ? Attributes | 1 :  Attributes & ~1 ;
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

        #endregion
    }
}
