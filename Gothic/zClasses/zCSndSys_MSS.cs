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

        public int PlaySound(zCSoundFX sound, int arg)
        {
            return Process.THISCALL<IntArg>((uint)Address, 0x4EF7B0, new CallValue[] { sound, (IntArg)arg });
        }

        public int PlaySound(zCSoundFX sound, int arg1, int arg2, float vol)
        {
            return Process.THISCALL<IntArg>((uint)Address, 0x4F0B70, new CallValue[] { sound, (IntArg)arg1, (IntArg)arg2, (FloatArg)vol });
        }

        public int PlaySound3D(zString sound, zCVob vob, int arg, zStruct.zTSound3DParams param)
        {
            return Process.THISCALL<IntArg>((uint)Address, (int)FuncOffsets.PlaySound3D_Str, new CallValue[] { sound, vob, (IntArg)arg, param });
        }

        public int PlaySound3D(zCSoundFX sound, zCVob vob, int arg, zStruct.zTSound3DParams param)
        {
            return Process.THISCALL<IntArg>((uint)Address, (int)FuncOffsets.PlaySound3D, new CallValue[] { sound, vob, (IntArg)arg, param });
        }

        public zCSoundFX LoadSingle(zString text)
        {
            return Process.THISCALL<zCSoundFX>((uint)Address, (uint)0x4EF0E0, new CallValue[] { text });
        }

        public zCSoundFX LoadSoundFX(zString text)
        {
            return Process.THISCALL<zCSoundFX>((uint)Address, (uint)FuncOffsets.LoadSoundFX, new CallValue[] { text });
        }

        public zCSoundFX LoadSoundFXScript(zString text)
        {
            return Process.THISCALL<zCSoundFX>((uint)Address, (uint)0x4EE120, new CallValue[] { text });
        }

        public static zCSndSys_MSS SoundSystem(Process process)
        {
            return new zCSndSys_MSS(process, process.ReadInt(0x0099B03C));
        }

        public void StopAllSounds()
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)0x4F23C0, null );
        }

        public void StopSound(int num)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)0x4F2300, new CallValue[] { (IntArg)num });
        }
    }
}
