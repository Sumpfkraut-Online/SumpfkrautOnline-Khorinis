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
            MasterState = 136,
            StartRainTime = 1704,
            EndRainTime = 1708
        }

        public enum FuncOffsets
        {
            SetWeatherType = 0x005EB830
        }

        #endregion


        public void SetWeatherType(int type)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.SetWeatherType, new CallValue[] { new IntArg(type) });
        }

        


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


        public void setRainTime(byte sh, byte sm, byte eh, byte em)
        {
            if (sh == 0 && sm == 0 && eh == 24 && em == 0)
            {
                StartRainTime = 0;
                EndRainTime = 1;

                return;
            }
            //Schritt 1: 12 uhr ist 0, 24 Uhr ist 1
            int start_hour = (sh + 12) % 24;
            int end_hour = (eh + 12) % 24;

            int startminutes = start_hour * 60 + sm;
            int endMinutes = end_hour * 60 + em;

            float s = startminutes / (24f*60f);
            float e = endMinutes / (24f*60f);

            StartRainTime = s;
            EndRainTime = e;
    
        }

        #region Fields

        public float StartRainTime
        {
            get { return Process.ReadFloat( Address + (int)Offsets.StartRainTime); }
            set { Process.Write(value, Address + (int)Offsets.StartRainTime); }
        }

        public float EndRainTime
        {
            get { return Process.ReadFloat(Address + (int)Offsets.EndRainTime); }
            set { Process.Write(value, Address + (int)Offsets.EndRainTime); }
        }

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
