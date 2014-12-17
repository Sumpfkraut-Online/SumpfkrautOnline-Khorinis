using System;
using System.Collections.Generic;
using System.Text;
using WinApi;
using Gothic.zTypes;

namespace Gothic.zClasses
{
    public class zCSoundSystem : zClass
    {
        #region OffsetLists
        public enum Offsets
        {
        }
        public enum FuncOffsets : uint
        {
            
        }

        public enum HookSize : uint
        {

        }

        #endregion

        public zCSoundSystem()
        {

        }

        public zCSoundSystem(Process process, int address)
            : base(process, address)
        {
        }

        public static zCSoundSystem SoundSystem(Process process)
        {
            return new zCSoundSystem(process, process.ReadInt(0x0099B03C));
        }
        
    }
}
