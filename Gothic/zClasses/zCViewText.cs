using System;
using System.Collections.Generic;
using System.Text;
using WinApi;
using Gothic.zTypes;

namespace Gothic.zClasses
{
    public class zCViewText : zClass, IDisposable
    {

        public enum Offsets
        {
            VTBL = 0,
            PosX = 4,
            PosY = 8,
            text = 12,
            font = 32,
            timer = 36,
            inPrintwin = 40,
            color = 44,
            timed = 48,
            colored = 52

        }
        public zCViewText()
        {

        }

        public zCViewText(Process process, int address)
            : base(process, address)
        {

        }

        public override uint ValueLength()
        {
            return 4;
        }

        private bool disposed = false;
        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                Process.THISCALL<NullReturnCall>((uint)Address, (uint)0x007AC700, new CallValue[] { });
                Process.Free(new IntPtr(Address), 0x38);
                disposed = true;
            }
        }

        public zCFont Font
        {
            get { return new zCFont(Process,Process.ReadInt(Address + (int)Offsets.font)); }
            set { Process.Write(value.Address, Address + (int)Offsets.font); }
        }

        public zColor Color
        {
            get { return new zColor(Process, Address + (int)Offsets.color); }
        }

        public int PosX
        {
            get { return Process.ReadInt(Address + (int)Offsets.PosX); }
            set { Process.Write(value, Address + (int)Offsets.PosX); }
        }

        public int PosY
        {
            get { return Process.ReadInt(Address + (int)Offsets.PosY); }
            set { Process.Write(value, Address + (int)Offsets.PosY); }
        }

        public float Timer
        {
            get { return Process.ReadFloat(Address + (int)Offsets.timer); }
            set { Process.Write(value, Address + (int)Offsets.timer); }
        }

        public int Timed
        {
            get { return Process.ReadInt(Address + (int)Offsets.timed); }
            set { Process.Write(value, Address + (int)Offsets.timed); }
        }

        public zString Text
        {
            get { return new zString(Process, Address + (int)Offsets.text); }
            set { Text.Set(value.Value); }
        }

    }
}
