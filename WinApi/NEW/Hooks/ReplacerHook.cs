using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using GUC.Injection.Utilities;

namespace GUC.Injection
{
    /// <summary>
    /// Address should start at the 0xE8 (...) of a call!!!
    /// </summary>
    public class ReplacerHook : Hook
    {
        #region CDECL
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void CDECL0();
        public static ReplacerHook CDECL(CDECL0 method, int address)
        {
            return new ReplacerHook(method, address);
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void CDECL1(int arg0);
        public static ReplacerHook CDECL(CDECL1 method, int address)
        {
            return new ReplacerHook(method, address);
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void CDECL2(int arg0, int arg1);
        public static ReplacerHook CDECL(CDECL2 method, int address)
        {
            return new ReplacerHook(method, address);
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void CDECL3(int arg0, int arg1, int arg2);
        public static ReplacerHook CDECL(CDECL3 method, int address)
        {
            return new ReplacerHook(method, address);
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void CDECL4(int arg0, int arg1, int arg2, int arg3);
        public static ReplacerHook CDECL(CDECL4 method, int address)
        {
            return new ReplacerHook(method, address);
        }

        #endregion

        #region CDECL RETURN

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int CDECLR0();
        public static ReplacerHook CDECL(CDECLR0 method, int address)
        {
            return new ReplacerHook(method, address);
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int CDECLR1(int arg0);
        public static ReplacerHook CDECL(CDECLR1 method, int address)
        {
            return new ReplacerHook(method, address);
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int CDECLR2(int arg0, int arg1);
        public static ReplacerHook CDECL(CDECLR2 method, int address)
        {
            return new ReplacerHook(method, address);
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int CDECLR3(int arg0, int arg1, int arg2);
        public static ReplacerHook CDECL(CDECLR3 method, int address)
        {
            return new ReplacerHook(method, address);
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int CDECLR4(int arg0, int arg1, int arg2, int arg3);
        public static ReplacerHook CDECL(CDECLR4 method, int address)
        {
            return new ReplacerHook(method, address);
        }

        #endregion

        protected ReplacerHook(Delegate method, int address) : base(method, address + 1, 4)
        {
        }

        protected override byte[] CreateCode(Delegate method)
        {
            int methodPtr = Marshal.GetFunctionPointerForDelegate(method).ToInt32();
            return BitConverter.GetBytes(methodPtr - (this.Address + 4));
        }
    }
}
