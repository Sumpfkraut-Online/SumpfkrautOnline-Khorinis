using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;
using Gothic.Types;

namespace Gothic.Objects
{
    public class zCAICamera : zCAIBase
    {
        public const int current = 0x8CEAB8;
        public static int CurrentAddress { get { return Process.ReadInt(current); } }
        public static zCAICamera CurrentCam { get { return new zCAICamera(CurrentAddress); } }

        public zCAICamera()
        {
        }

        public zCAICamera(int address) : base(address)
        {
        }

        public void CreateInstance(string instance)
        {
            using (var z = zString.Create(instance))
                CreateInstance(z);
        }

        public void CreateInstance(zString instance)
        {
            Process.THISCALL<NullReturnCall>(Address, 0x4A3670, instance);
        }

        public zString CurrentMode
        {
            get { return new zString(Address + 0x26C); }
        }

        public float BestElevation
        {
            get { return Process.ReadFloat(Address + 0x38); }
            set { Process.Write(Address + 0x38, value); }
        }

        public float BestAzimuth
        {
            get { return Process.ReadFloat(Address + 0x44); }
            set { Process.Write(Address + 0x44, value); }
        }

        public float BestRotZ
        {
            get { return Process.ReadFloat(Address + 0x50); }
            set { Process.Write(Address + 0x50, value); }
        }

        public float TargetOffsetX
        {
            get { return Process.ReadFloat(Address + 0x5C); }
            set { Process.Write(Address + 0x5C, value); }
        }

        public float TargetOffsetY
        {
            get { return Process.ReadFloat(Address + 0x60); }
            set { Process.Write(Address + 0x60, value); }
        }

        public float TargetOffsetZ
        {
            get { return Process.ReadFloat(Address + 0x64); }
            set { Process.Write(Address + 0x64, value); }
        }

        public void SetByScript(string instance)
        {
            using (var z = zString.Create(instance))
                SetByScript(z);
        }

        public void SetByScript(zString instance)
        {
            Process.THISCALL<NullReturnCall>(Address, 0x4A26E0, instance);
        }
        
        public static readonly zString CamModRanged = new zString(0x8CE910);
        public static readonly zString CamModDeath = new zString(0x8CE8AC);
        public static readonly zString CamModNormal = new zString(0x8CE9F0);

        public void SetMode(zString mode)
        {
            Process.THISCALL<NullReturnCall>(Address, 0x04A09C0, mode, (IntArg)0, (IntArg)0, (IntArg)0);  
        }
    }
}
