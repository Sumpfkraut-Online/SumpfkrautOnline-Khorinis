using System;
using System.Collections.Generic;
using System.Text;
using WinApi;
using Gothic.Types;

namespace Gothic.View
{
    public class zCViewText : zClass//, IDisposable
    {
        public abstract class VarOffsets
        {
            public const int VTBL = 0,
            PosX = 4,
            PosY = 8,
            text = 12,
            font = 32,
            timer = 36,
            inPrintwin = 40,
            color = 44,
            timed = 48,
            colored = 52;

        }

        public zCViewText()
        {
        }

        public zCViewText(int address)
            : base(address)
        {
        }

        /*private bool disposed = false;
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
        }*/

        public zCFont Font
        {
            get { return new zCFont(Process.ReadInt(Address + VarOffsets.font)); }
            set { Process.Write(Address + VarOffsets.font, value.Address); }
        }

        public void SetFont(string fontName)
        {
            var fontMan = zCFontManager.Manager;
            this.Font = fontMan.GetFont(fontMan.Load(fontName));
        }

        public zColor Color
        {
            get { return new zColor(Address + VarOffsets.color); }
        }

        public int PosX
        {
            get { return Process.ReadInt(Address + VarOffsets.PosX); }
            set { Process.Write(Address + VarOffsets.PosX, value); }
        }

        public int PosY
        {
            get { return Process.ReadInt(Address + VarOffsets.PosY); }
            set { Process.Write(Address + VarOffsets.PosY, value); }
        }

        public float Timer
        {
            get { return Process.ReadFloat(Address + VarOffsets.timer); }
            set { Process.Write(Address + VarOffsets.timer, value); }
        }

        public int Timed
        {
            get { return Process.ReadInt(Address + VarOffsets.timed); }
            set { Process.Write(Address + VarOffsets.timed, value); }
        }

        public zString Text
        {
            get { return new zString(Address + VarOffsets.text); }
            set { Text.Set(value); }
        }

    }
}
