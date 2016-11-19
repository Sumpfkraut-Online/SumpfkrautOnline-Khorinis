using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;

namespace Gothic.Objects.EventManager
{
    public class zCEventMessage : zCObject
    {
        new public abstract class VarOffsets : zCObject.VarOffsets
        {
            public const int SubType = 36, //18=Overlay 14=keinOverlay 19=keinOverlay????
            InCutscene = 40;
        }

        public zCEventMessage()
        {
        }

        public zCEventMessage(int address)
            : base(address)
        {
        }


        public ushort SubType
        {
            get { return Process.ReadUShort(Address + VarOffsets.SubType); }
            set { Process.Write(Address + VarOffsets.SubType, value); }
        }

        public bool InCutscene
        {
            get
            {
                return Process.ReadBool(Address + VarOffsets.InCutscene);
            }
        }
    }
}
