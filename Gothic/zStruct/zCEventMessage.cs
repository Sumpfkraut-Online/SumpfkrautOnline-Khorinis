using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.zClasses;
using WinApi;
using Gothic.zTypes;

namespace Gothic.zStruct
{
    public class zCEventMessage : zCObject
    {
        public enum Offsets
        {
            subType = 36, //18=Overlay 14=keinOverlay 19=keinOverlay????
            inCutscene = 40,

        }


        public zCEventMessage()
            : base()
        {

        }

        public zCEventMessage(Process process, int address)
            : base(process, address)
        {

        }



        #region statics

        
        #endregion

        #region Fields

        /// <summary>
        /// Wird durch den Konstruktor angegeben.
        /// <return>Subtype als ushort, entspricht der Enumeration TConversationSubType</return>
        /// </summary>
        public ushort subType
        {
            get
            {
                return Process.ReadUShort(Address + (int)Offsets.subType);
            }
        }

        public bool inCutscene
        {
            get
            {
                return Process.ReadInt(Address + (int)Offsets.inCutscene) != 0;
            }
        }



        #endregion
    }
}
