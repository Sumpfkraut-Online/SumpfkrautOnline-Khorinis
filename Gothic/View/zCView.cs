using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;
using Gothic.Types;

namespace Gothic.View
{
    public class zCView : zClass, IDisposable
    {
        public enum zTviewID
        {
            VIEW_SCREEN,
            VIEW_VIEWPORT,
            VIEW_ITEM
        }

        public abstract class VarOffsets
        {
            public const int m_bFillZ = 8,
            next = 12,
            text_lines = 132,
            psizex = 0x5C,
            psizey = 0x60,
            font = 0x64;
        }

        public abstract class FuncAddresses
        {
            public const int zView = 0x007A5700;
            public const int zView_noParam = 0x007A5640;
            public const int insertBack_str = 0x007A6130;
            public const int print = 0x007A9A40;
            public const int Open = 0x007A6C00;
            public const int Close = 0x007A6E30;
            public const int insertItem = 0x007ABAD0;
            public const int CreateText_3 = 0x007AA2B0;
            public const int PrintTimedCXY = 0x007A7FC0;
            public const int RemoveItem = 0x007ABD10;
            public const int SetColor = 0x007A6080;
            public const int SetFont_Str = 0x007A9920;
            public const int SetPos = 0x007A75B0;
            public const int Move = 0x007A76E0;
            public const int SetSize = 0x007A77A0;
            public const int Top = 0x007A6790;
            public const int FontSize = 0x007A9A10;
            public const int anx = 0x007A5E80;
            public const int any = 0x007A5EC0;
            public const int nax = 0x007A5E00;
            public const int nay = 0x007A5E40;
            public const int FontY = 0x007A99F0;
            public const int DrawItems = 0x007A6750;
            public const int Blit = 0x007A63C0;
        }

        /*public enum HookSizes
        {
            CreateText_3 = 7,
            PrintTimedCXY = 6,
            RemoveItem = 8,
            SetColor = 6,
            SetFont_Str = 9,
            SetPos = 5,
            Move = 5,
            DrawItems = 5
        }*/

        public const int ByteSize = 0x100;

        public zCView()
        {

        }

        public zCView(int address)
            : base(address)
        {

        }

        bool disposed = false;
        public void Dispose()
        {
            Dispose(true);
        }

        public const int screen = 0x00AB6468;

        public static zCView GetScreen()
        {
            return new zCView(Process.ReadInt(screen));
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                Process.THISCALL<NullReturnCall>(Address, 0x007A57C0);
                Process.Free(new IntPtr(Address), ByteSize); //Wird von destruktor aufgerufen!?
                disposed = true;
            }
        }

        public int pSizeX { get { return Process.ReadInt(Address + VarOffsets.psizex); } }
        public int pSizeY { get { return Process.ReadInt(Address + VarOffsets.psizey); } }

        public bool FillZ
        {
            get { return Process.ReadInt(Address + VarOffsets.m_bFillZ) >= 1; }
            set { Process.Write(Address + VarOffsets.m_bFillZ, value); }
        }

        public void Top()
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.Top);
        }

        public zCView Next
        {
            get { return new zCView(Process.ReadInt(Address + VarOffsets.next)); }
        }

        public zCFont Font
        {
            get { return new zCFont(Process.ReadInt(Address + VarOffsets.font)); }
            set { Process.Write(Address + VarOffsets.font, value.Address); }
        }

        public zCList<zCViewText> TextLines
        {
            get { return new zCList<zCViewText>(Address + VarOffsets.text_lines); }
        }

        public static zCView STDOutput()
        {
            return new zCView(Process.ReadInt(0x00AB646C));
        }

        public static int ReadDebug()
        {
            return Process.ReadInt(0xAB6474);
        }
        public static void WriteDebug(int debug)
        {
            Process.Write(0xAB6474, debug);
        }

        public static zCView Create(int x, int y, int width, int height, zTviewID type)
        {
            int ptr = Process.Alloc(ByteSize).ToInt32();

            Process.THISCALL<NullReturnCall>(ptr, FuncAddresses.zView,  new IntArg(x), new IntArg(y), new IntArg(width), new IntArg(height), new IntArg((int)type));

            return new zCView(ptr);
        }

        public static zCView Create()
        {
            int ptr = Process.Alloc(ByteSize).ToInt32();
            Process.THISCALL<NullReturnCall>(ptr, FuncAddresses.zView_noParam);
            return new zCView(ptr);
        }

        public static zCView Create(int x, int y, int width, int height)
        {
            return Create(x, y, width, height, zTviewID.VIEW_ITEM);
        }

        public void Blit()
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.Blit);
        }

        public void InsertBack(String texture)
        {
            using (zString z = zString.Create(texture))
                InsertBack(z);
        }

        public void InsertBack(zString texture)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.insertBack_str, texture);
        }

        public void SetFont(String font)
        {
            using (zString f = zString.Create(font))
                SetFont(f);
        }

        public int nax(int x)
        {
            return Process.THISCALL<IntArg>(Address, FuncAddresses.nax, new IntArg(x));
        }

        public int nay(int y)
        {
            return Process.THISCALL<IntArg>(Address, FuncAddresses.nay, new IntArg(y));
        }

        public int anx(int x)
        {
            return Process.THISCALL<IntArg>(Address, FuncAddresses.anx, new IntArg(x));
        }

        public int any(int y)
        {
            return Process.THISCALL<IntArg>(Address, FuncAddresses.any, new IntArg(y));
        }

        public int FontSize(String str)
        {
            int ret;
            using (zString z = zString.Create(str))
                ret = FontSize(z);

            return ret;
        }
        public int FontSize(zString str)
        {
            return Process.THISCALL<IntArg>(Address, FuncAddresses.FontSize, str);

        }
        public int FontY()
        {
            return Process.THISCALL<IntArg>(Address, FuncAddresses.FontY);
        }

        public void SetFont(zString font)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.SetFont_Str, font);
        }

        public void SetColor(zColor color)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.SetColor, color);
        }

        public void InsertItem(zCView view, int stayonTop)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.insertItem, view, new IntArg(stayonTop));
        }

        public void RemoveItem(zCView view)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.RemoveItem, view);
        }

        public void Print(int x, int y, zString str)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.print, new IntArg(x), new IntArg(y), str);
        }

        public void SetPos(int x, int y)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.SetPos, new IntArg(x), new IntArg(y));
        }

        public void SetSize(int x, int y)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.SetSize, new IntArg(x), new IntArg(y));
        }

        public void Move(int x, int y)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.Move, new IntArg(x), new IntArg(y));
        }

        public void PrintTimedCXY(String str, float time, byte r, byte g, byte b, byte a)
        {
            using (zColor color = zColor.Create(r, g, b, a))
            using (zString zstr = zString.Create(str))
                PrintTimedCXY(zstr, time, color);
        }

        public void PrintTimedCXY(zString str, float time, zColor color)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.PrintTimedCXY, str, new IntArg((int)time), color);
        }

        /// <summary>
        /// The PrintTimedCXY_TV-function does not clear eax so even if it does not officially return a zCViewText it should do it.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="time"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public zCViewText PrintTimedCXY_TV(zString str, float time, zColor color)
        {
            return Process.THISCALL<zCViewText>(Address, FuncAddresses.PrintTimedCXY, str, new FloatArg(time), color);
        }

        public void Open()
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.Open);
        }

        public void Close()
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.Close);
        }

        public void Render()
        {
            Process.THISCALL<NullReturnCall>(Address, 0x7AC210);
        }

        public void DrawItems()
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.DrawItems);
        }

        public zCViewText CreateText(int x, int y, zString text)
        {
            return Process.THISCALL<zCViewText>(Address, FuncAddresses.CreateText_3, new IntArg(x), new IntArg(y), text);
        }

        public static void Printwin(string text)
        {
            using (zString z = zString.Create(text))
            {
                Printwin(z);
            }
        }

        public static void Printwin(zString text)
        {
            int textViewAddr = Process.THISCALL<IntArg>(Process.ReadInt(oCGame.ogame), 0x6C2C70);
            Process.THISCALL<NullReturnCall>(textViewAddr, 0x7AA8D0, text);
        }
    }
}
