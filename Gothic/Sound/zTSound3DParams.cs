using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;

namespace Gothic.Sound
{
    public class zTSound3DParams : zClass
    {
        public const int ByteSize = 32;

        public zTSound3DParams()
        {
        }

        public zTSound3DParams(int address) : base(address)
        {
        }

        public static zTSound3DParams Create()
        {
            var ret = new zTSound3DParams(Process.Alloc(ByteSize).ToInt32());
            ret.SetDefaults();
            return ret;
        }

        bool freed = false;
        public void Free()
        {
            if (!freed)
            {
                Process.Free(new IntPtr(this.Address), ByteSize);
                freed = true;
            }
        }

        public void SetDefaults()
        {
            Process.THISCALL<NullReturnCall>(Address, 0x612F50);
        }

        /// <summary>
        /// Default = 0
        /// </summary>
        public int Obstruction
        {
            get { return Process.ReadInt(this.Address); }
            set { Process.Write(this.Address, value); }
        }

        /// <summary>
        /// Default = 1
        /// </summary>
        public float Volume
        {
            get { return Process.ReadFloat(this.Address+4); }
            set { Process.Write(this.Address + 4, value); }
        }

        /// <summary>
        /// Default = -1
        /// </summary>
        public float Radius
        {
            get { return Process.ReadFloat(this.Address + 8); }
            set { Process.Write(this.Address + 8, value); }
        }

        /// <summary>
        /// Default = 0
        /// </summary>
        public int LoopType
        {
            get { return Process.ReadInt(this.Address + 12); }
            set { Process.Write(this.Address + 12, value); }
        }

        /// <summary>
        /// Default = 0
        /// </summary>
        public float Angle
        {
            get { return Process.ReadFloat(this.Address + 16); }
            set { Process.Write(this.Address + 16, value); }
        }

        /// <summary>
        /// Default = 1
        /// </summary>
        public float Reverb
        {
            get { return Process.ReadFloat(this.Address + 20); }
            set { Process.Write(this.Address + 20, value); }
        }

        /// <summary>
        /// Default = False
        /// </summary>
        public bool IsAmbient
        {
            get { return Process.ReadBool(this.Address + 24); }
            set { Process.Write(this.Address + 24, value); }
        }

        /// <summary>
        /// Default = -999999
        /// </summary>
        public float Pitch
        {
            get { return Process.ReadFloat(this.Address + 28); }
            set { Process.Write(this.Address + 28, value); }
        }
    }
}
