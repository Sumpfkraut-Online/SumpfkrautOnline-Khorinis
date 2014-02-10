using System;
using System.Collections.Generic;
using System.Text;
using WinApi;
using Gothic.zTypes;

namespace Gothic.zClasses
{
    public class zCWaypoint : zCObject 
    {
                #region OffsetLists
        public enum Offsets : uint
        {
            pos = 68,
            dir = 80,
            name = 92
        }

        public enum FuncOffsets : uint
        {
            
        }

        public enum HookSize : uint
        {
            
        }
        #endregion

        #region Standard
        public zCWaypoint(Process process, int address) : base (process, address)
        {
            
        }

        public zCWaypoint()
        {

        }

        public override uint ValueLength()
        {
            return 4;
        }
        #endregion

        #region statics
        

        #endregion

        #region Fields
        public zString Name
        {
            get { return new zString(Process, Address + (int)Offsets.name); }
        }

        public zVec3 Position
        {
            get { return new zVec3(Process, Address + (int)Offsets.pos); }
        }

        public zVec3 Direction
        {
            get { return new zVec3(Process, Address + (int)Offsets.dir); }
        }
        #endregion


        #region methods

        #endregion
    }
}
