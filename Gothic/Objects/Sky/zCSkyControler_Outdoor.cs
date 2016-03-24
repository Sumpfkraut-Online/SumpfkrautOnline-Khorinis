﻿using System;
using System.Collections.Generic;
using System.Text;
using WinApi;

namespace Gothic.Objects.Sky
{
    public class zCSkyControler_Outdoor : zCSkyControler
    {
        public zCSkyControler_Outdoor(int address)
            : base(address)
        {
        }

        public zCSkyControler_Outdoor()
        {
        }
        
        public abstract class VarOffsets
        {
            public const int Layer0 = 1448,
            Layer1 = 1472,
            LayerRainClouds = 1496,
            MasterState = 136,
            StartRainTime = 1704,
            EndRainTime = 1708,
            outdoorRainFX = 0x694;
        }

        public abstract class FuncAddresses
        {
            public const int SetWeatherType = 0x005EB830;
        }

        public void SetWeatherType(int type)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.SetWeatherType, new IntArg(type));
        }

        public static zCSkyControler_Outdoor Create(int value)
        {
            int address = Process.CDECLCALL<IntArg>(0x5E0FB0); //_CreateInstance()
            Process.THISCALL<NullReturnCall>(address, 0x005E6220, new IntArg(value)); //Konstruktor...
            return new zCSkyControler_Outdoor(address);
        }

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

        public float StartRainTime
        {
            get { return Process.ReadFloat( Address + VarOffsets.StartRainTime); }
            set { Process.Write(value, Address + VarOffsets.StartRainTime); }
        }

        public float EndRainTime
        {
            get { return Process.ReadFloat(Address + VarOffsets.EndRainTime); }
            set { Process.Write(value, Address + VarOffsets.EndRainTime); }
        }

        public zCSkyState MasterState
        {
            get { return new zCSkyState(Address + VarOffsets.MasterState); }
        }

        public zCSkyLayer Layer0
        {
            get { return new zCSkyLayer(Address+VarOffsets.Layer0); }
        }
        public zCSkyLayer Layer1
        {
            get { return new zCSkyLayer(Address + VarOffsets.Layer1); }
        }
        public zCSkyLayer LayerRainClouds
        {
            get { return new zCSkyLayer(Address + VarOffsets.LayerRainClouds); }
        }

        public zCOutdoorRainFX OutdoorRainFX
        {
            get { return new zCOutdoorRainFX(Process.ReadInt(Address + VarOffsets.outdoorRainFX)); }
        }
    }
}