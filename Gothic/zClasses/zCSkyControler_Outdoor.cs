using System;
using System.Collections.Generic;
using System.Text;
using WinApi;
using Gothic.zTypes;
using Gothic.zStruct;

namespace Gothic.zClasses
{
    public class zCSkyControler_Outdoor : zCSkyControler
    {
        public zCSkyControler_Outdoor(Process process, int address)
            : base(process, address)
        { }

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
            CalcGlobalWind = 0x005EA210,
            GetBackgroundColor = 0x005E6710,
            GetGlobalWindVec = 0x005EA7B0,
            GetTime = 0x005E66F0,
            InitSkyPFX = 0x005E94E0,
            Interpolate = 0x005E8C20,
            ProcessRainFX = 0x005EAF30,
            RenderSky = 0x005EB3D0,
            RenderSkyPFX = 0x005E9ED0,
            RenderSkyPost = 0x005EB580,
            RenderSkyPre = 0x005EA850,
            ResetTime = 0x005E9380,
            SetBackgroundColor = 0x005E6700,
            SetOverrideColor = 0x005E6750,
            SetRainFXWeight = 0x005EB230,
            SetTime = 0x005E9350,
            SetWeatherType = 0x005EB830,
            UpdateWorldDependencies = 0x005E72C0,
        }

        #endregion


        public void CalcGlobalWind ()
        {
            Process.THISCALL<NullReturnCall>((uint) Address, (uint) FuncOffsets.CalcGlobalWind,
                new CallValue[] { });
        }

        public zColor GetBackgroundColor ()
        {
            return Process.THISCALL<zColor>((uint) Address, (uint) FuncOffsets.GetBackgroundColor,
                new CallValue[] { });
        }

        public zVec3 GetGlobalWindVec ()
        {
            return Process.THISCALL<zVec3>((uint) Address, (uint) FuncOffsets.GetGlobalWindVec,
                new CallValue[] { });
        }

        public FloatArg GetTime ()
        {
            return Process.THISCALL<FloatArg>((uint) Address, (uint) FuncOffsets.GetTime,
                new CallValue[] { });
        }

        public void InitSkyPFX ()
        {
            Process.THISCALL<NullReturnCall>((uint) Address, (uint) FuncOffsets.InitSkyPFX,
                new CallValue[] { });
        }

        public void Interpolate ()
        {
            Process.THISCALL<NullReturnCall>((uint) Address, (uint) FuncOffsets.Interpolate,
                new CallValue[] { });
        }

        public void ProcessRainFX ()
        {
            Process.THISCALL<NullReturnCall>((uint) Address, (uint) FuncOffsets.ProcessRainFX,
                new CallValue[] { });
        }

        public void RenderSky ()
        {
            Process.THISCALL<NullReturnCall>((uint) Address, (uint) FuncOffsets.RenderSky,
                new CallValue[] { });
        }

        public void RenderSkyPFX ()
        {
            Process.THISCALL<NullReturnCall>((uint) Address, (uint) FuncOffsets.RenderSkyPFX,
                new CallValue[] { });
        }

        public void RenderSkyPost ()
        {
            Process.THISCALL<NullReturnCall>((uint) Address, (uint) FuncOffsets.RenderSkyPost,
                new CallValue[] { });
        }

        public void RenderSkyPre ()
        {
            Process.THISCALL<NullReturnCall>((uint) Address, (uint) FuncOffsets.RenderSkyPre,
                new CallValue[] { });
        }

        public void ResetTime ()
        {
            Process.THISCALL<NullReturnCall>((uint) Address, (uint) FuncOffsets.ResetTime,
                new CallValue[] { });
        }

        public void SetBackgroundColor (zColor color)
        {
            Process.THISCALL<NullReturnCall>((uint) Address, (uint) FuncOffsets.SetBackgroundColor,
                new CallValue[] { color });
        }

        public void SetOverrideColor (zVec3 vec)
        {
            Process.THISCALL<NullReturnCall>((uint) Address, (uint) FuncOffsets.SetOverrideColor,
                new CallValue[] { vec });
        }

        public void SetRainFXWeight (float weight, float duration)
        {
            // weight: 0 - 1
            // duration: ?
            Process.THISCALL<NullReturnCall>((uint) Address, (uint) FuncOffsets.SetRainFXWeight,
                new CallValue[] { new FloatArg(weight), new FloatArg(duration) });
        }

        public void SetTime (float time)
        {
            Process.THISCALL<NullReturnCall>((uint) Address, (uint) FuncOffsets.SetTime,
                new CallValue[] { new FloatArg(time) });
        }

        public void SetWeatherType(int type)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.SetWeatherType, 
                new CallValue[] { new IntArg(type) });
        }

        public void UpdateWorldDependencies ()
        {
            Process.THISCALL<NullReturnCall>((uint) Address, (uint) FuncOffsets.UpdateWorldDependencies,
                new CallValue[] { });
        }

        


        #region statics
        public static zCSkyControler_Outdoor Create (Process process, int value)
        {
            IntPtr address = process.Alloc(0x6BC);//0x6c4

            zCClassDef.ObjectCreated(process, address.ToInt32(), 0x0099ACD8);
            process.THISCALL<NullReturnCall>((uint)address.ToInt32(), 0x005E6220, new CallValue[] { new IntArg(value) });
            return new oCSkyControler_Barrier(process, address.ToInt32());
        }

        public static zCSkyControler_Outdoor _CreateNewInstance (Process process)
        {
            return process.CDECLCALL<zCSkyControler_Outdoor>(0x005E0FB0, new CallValue[] { });
        }
        #endregion


        public void SetRainTime (byte sh, byte sm, byte eh, byte em)
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
