using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;
using Gothic.zTypes;

namespace Gothic.zClasses
{
    public class zCSndSys_MSS: zClass
    {
        #region OffsetLists
        public enum Offsets
        {
        }
        public enum FuncOffsets : uint
        {
            PlaySound3D_Str = 0x004F1060,
            PlaySound3D = 0x004F10F0,
            LoadSoundFX = 0x004ED960,
            UpdateSound3D = 0x004F2410,
        }

        public enum HookSize : uint
        {

        }

        #endregion

        public zCSndSys_MSS()
        {

        }

        public zCSndSys_MSS(Process process, int address)
            : base(process, address)
        {
        }

        public int UpdateSound3D(ref int arg, zStruct.zTSound3DParams param)
        {
            IntPtr arg1 = Process.Alloc(4);
            Process.Write(arg, arg1.ToInt32());
            int rArg = Process.THISCALL<IntArg>((uint)Address, (int)FuncOffsets.UpdateSound3D, new CallValue[] { (IntArg)arg1.ToInt32(), param });

            
            Process.Free(arg1, 4);

            return rArg;
        }

        public int PlaySound3D(zString sound, zCVob vob, int arg, zStruct.zTSound3DParams param)
        {
            return Process.THISCALL<IntArg>((uint)Address, (int)FuncOffsets.PlaySound3D_Str, new CallValue[] { sound, vob, (IntArg)arg, param });
        }

        public int PlaySound3D(zCSoundFX sound, zCVob vob, int arg, zStruct.zTSound3DParams param)
        {
            return Process.THISCALL<IntArg>((uint)Address, (int)FuncOffsets.PlaySound3D, new CallValue[] { sound, vob, (IntArg)arg, param });
        }

        public zCSoundFX LoadSoundFX(zString text)
        {
            return Process.THISCALL<zCSoundFX>((uint)Address, (uint)FuncOffsets.LoadSoundFX, new CallValue[] { text });
        }

        public static zCSndSys_MSS SoundSystem(Process process)
        {
            return new zCSndSys_MSS(process, process.ReadInt(0x0099B03C));
        }
        
    }
}
