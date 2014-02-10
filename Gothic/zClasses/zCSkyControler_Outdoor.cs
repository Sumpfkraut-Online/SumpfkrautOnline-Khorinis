using System;
using System.Collections.Generic;
using System.Text;
using WinApi;

namespace Gothic.zClasses
{
    public class zCSkyControler_Outdoor : zCSkyControler
    {
        public zCSkyControler_Outdoor(Process process, int address)
            : base(process, address)
        {
        }

        public zCSkyControler_Outdoor() { }


        #region Offsets
        public enum Offsets
        {
            Layer0 = 1448,
            Layer1 = 1472,
            LayerRainClouds = 1496,
            MasterState = 136
        }
        #endregion


        #region statics
        public static zCSkyControler_Outdoor Create(Process process, int value)
        {
            IntPtr address = process.Alloc(0x6BC);//0x6c4

            zCClassDef.ObjectCreated(process, address.ToInt32(), 0x0099ACD8);
            process.THISCALL<NullReturnCall>((uint)address.ToInt32(), 0x005E6220, new CallValue[] { new IntArg(value) });
            return new oCSkyControler_Barrier(process, address.ToInt32());
        }

        public static zCSkyControler_Outdoor _CreateNewInstance(Process process)
        {
            return process.CDECLCALL<zCSkyControler_Outdoor>(0x005E0FB0, new CallValue[] { });
        }
        #endregion

        #region Fields
        public zCSkyState MasterState
        {
            get { return new zCSkyState(Process, Address + (int)Offsets.MasterState); }
        }
        public zCSkyLayer Layer0
        {
            get { return new zCSkyLayer(Process, Address+(int)Offsets.Layer0); }
        }
        public zCSkyLayer Layer1
        {
            get { return new zCSkyLayer(Process, Address + (int)Offsets.Layer1); }
        }
        public zCSkyLayer LayerRainClouds
        {
            get { return new zCSkyLayer(Process, Address + (int)Offsets.LayerRainClouds); }
        }
        #endregion
    }
}
