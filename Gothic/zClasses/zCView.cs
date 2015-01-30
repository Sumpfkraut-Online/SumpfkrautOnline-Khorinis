using System;
using System.Collections.Generic;
using System.Text;
using WinApi;
using Gothic.zTypes;

namespace Gothic.zClasses
{
    public class zCView :zClass, IDisposable
    {
        public enum zTviewID
        {
            VIEW_SCREEN,
            VIEW_VIEWPORT,
            VIEW_ITEM
        }

        public enum Offsets
        {
            next = 12,
            text_lines = 132,
            font = 0x64
        }

        public enum FuncOffsets
        {
            zView = 0x007A5700,
            zView_noParam = 0x007A5640,
            insertBack_str = 0x007A6130,
            print = 0x007A9A40,
            Open = 0x007A6C00,
            Close = 0x007A6E30,
            insertItem = 0x007ABAD0,
            CreateText_3 = 0x007AA2B0,
            PrintTimedCXY = 0x007A7FC0,
            RemoveItem = 0x007ABD10,
            SetColor = 0x007A6080,
            SetFont_Str = 0x007A9920,
            SetPos = 0x007A75B0,
            Move = 0x007A76E0,
            SetSize = 0x007A77A0,
            Top = 0x007A6790,
            FontSize = 0x007A9A10,
            anx = 0x007A5E80,
            any = 0x007A5EC0,
            nax = 0x007A5E00,
            nay = 0x007A5E40,
            FontY = 0x007A99F0
        }

        public enum HookSizes
        {
            CreateText_3 = 7,
            PrintTimedCXY = 6,
            RemoveItem = 8,
            SetColor = 6,
            SetFont_Str = 9,
            SetPos = 5,
            Move = 5
        }

        public zCView()
        {
            
        }

        public zCView(Process process, int address)
            : base(process, address)
        {

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
                Process.THISCALL<NullReturnCall>((uint)Address, (uint)0x007A57C0, new CallValue[] { });
                Process.Free(new IntPtr(Address), 0x100);//Wird von destruktor aufgerufen!?
                
                disposed = true;
            }
        }



        public void Top()
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.Top, new CallValue[] { });
        }

        public zCView Next
        {
            get { return new zCView(Process, Process.ReadInt(Address + (int)Offsets.next)); }
        }

        public zCFont Font
        {
            get { return new zCFont(Process, Process.ReadInt(Address + (int)Offsets.font)); }
            set { Process.Write(value.Address, Address + (int)Offsets.font); }
        }

        public zCList<zCViewText> TextLines
        {
            get { return new zCList<zCViewText>(Process, Address + (int)Offsets.text_lines); }
        }

        public static zCView GetStartscreen(Process process)
        {
            return new zCView(process, process.ReadInt(0x00AB6468));
        }

        public static zCView STDOutput(Process process)
        {
            return new zCView(process, process.ReadInt(0x00AB646C));
        }

        public static int ShowDebug(Process process)
        {
            return process.ReadInt(0xAB6474);
        }
        public static void ShowDebug(Process process,int debug)
        {
            process.Write(debug, 0xAB6474);
        }
        

        public static zCView Create(Process process, int x, int y, int width, int height, zTviewID type)
        {
            int ptr = process.Alloc(0x100).ToInt32();

            process.THISCALL<NullReturnCall>((uint)ptr, (uint)FuncOffsets.zView, new CallValue[]{ 
                new IntArg(x), 
                new IntArg(y), 
                new IntArg(width), 
                new IntArg(height), 
                new IntArg((int)type)});


            return new zCView(process, ptr);
        }

        public static zCView Create(Process process)
        {
            //0x100?
            int ptr = process.Alloc(0x100).ToInt32();
            process.THISCALL<NullReturnCall>((uint)ptr, (uint)FuncOffsets.zView_noParam, new CallValue[] { });
            return new zCView(process, ptr);
        }

        public static zCView Create(Process process, int x, int y, int width, int height)
        {
            return Create(process, x, y, width, height, zTviewID.VIEW_ITEM);
        }


        public void InsertBack(zString texture)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.insertBack_str, new CallValue[] { texture });
        }

        public void SetFont(String font)
        {
            zString f = zString.Create(Process, font);
            SetFont(f);
            f.Dispose();
        }

        public int nax(int x)
        {
            return Process.THISCALL<IntArg>((uint)Address, (uint)FuncOffsets.nax, new CallValue[] { new IntArg(x) });
        }

        public int nay(int y)
        {
            return Process.THISCALL<IntArg>((uint)Address, (uint)FuncOffsets.nay, new CallValue[] { new IntArg(y) });
        }

        public int anx(int x)
        {
            return Process.THISCALL<IntArg>((uint)Address, (uint)FuncOffsets.anx, new CallValue[] { new IntArg(x) });
        }

        public int any(int y)
        {
            return Process.THISCALL<IntArg>((uint)Address, (uint)FuncOffsets.any, new CallValue[] { new IntArg(y) });
        }
        public int FontSize(String str)
        {
            using (zString str2 = zString.Create(Process, str))
            {
                return FontSize(str2);
            }
        }
        public int FontSize(zString str)
        {
            return Process.THISCALL<IntArg>((uint)Address, (uint)FuncOffsets.FontSize, new CallValue[] { str });

        }
        public int FontY()
        {
            return Process.THISCALL<IntArg>((uint)Address, (uint)FuncOffsets.FontY, new CallValue[] { });
        }

        public void SetFont(zString font)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.SetFont_Str, new CallValue[] { font });
        }

        public void SetColor(zColor color)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.SetColor, new CallValue[] { color });
        }

        public void InsertItem(zCView view, int stayonTop)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.insertItem, new CallValue[] { view, new IntArg(stayonTop) });
        }

        public void RemoveItem(zCView view)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.RemoveItem, new CallValue[] { view });
        }

        public void Print(int x, int y, zString str)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.print, new CallValue[] { new IntArg(x), new IntArg(y), str });
        }

        public void SetPos(int x, int y)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.SetPos, new CallValue[] { new IntArg(x), new IntArg(y) });
        }

        public void SetSize(int x, int y)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.SetSize, new CallValue[] { new IntArg(x), new IntArg(y) });
        }

        public void Move(int x, int y)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.Move, new CallValue[] { new IntArg(x), new IntArg(y) });
        }

        public void PrintTimedCXY(String str, float time, byte r, byte g, byte b, byte a)
        {
            zColor colr = zColor.Create(Process, r, g, b, a);
            zString zstr = zString.Create(Process, str);
            PrintTimedCXY(zstr, time, colr);
            colr.Dispose();
            zstr.Dispose();
        }

        public void PrintTimedCXY(zString str, float time, zColor color)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.PrintTimedCXY, new CallValue[] { str, new IntArg((int)time), color });
        }

        /// <summary>
        /// The PrintTimedCXY_TV-function does not clear eax so even if it does not official returns a zCViewText it should do it.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="time"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public zCViewText PrintTimedCXY_TV(zString str, float time, zColor color)
        {
            return Process.THISCALL<zCViewText>((uint)Address, (uint)FuncOffsets.PrintTimedCXY, new CallValue[] { str, new IntArg((int)time), color });
        }

        public void Open()
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.Open, new CallValue[] { });
        }

        public void Close()
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.Close, new CallValue[] { });
        }

        public zCViewText CreateText(int x, int y, zString text)
        {
            return Process.THISCALL<zCViewText>((uint)Address, (uint)FuncOffsets.CreateText_3, new CallValue[] { new IntArg(x), new IntArg(y), text });
        }

        public static zCView GetScreen(Process process)
        {
            return new zCView(process, process.ReadInt(0x00AB6468));
        }
    }
}
