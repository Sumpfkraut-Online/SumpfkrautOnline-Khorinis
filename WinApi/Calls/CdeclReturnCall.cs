using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace WinApi.Calls
{
    public class CdeclReturnCall<TReturn> : AbstractCall
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate TReturn CallHandler();

        CallHandler call;
        public CallHandler Call { get { return this.call; } }

        public CdeclReturnCall(int address) : base(address)
        {
            this.call = GetDelegateFromFunction<CallHandler>(address);
        }
    }

    public class CdeclReturnCall<TReturn, T> : AbstractCall
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate TReturn CallHandler(T arg);

        CallHandler call;
        public CallHandler Call { get { return this.call; } }

        public CdeclReturnCall(int address) : base(address)
        {
            this.call = GetDelegateFromFunction<CallHandler>(address);
        }
    }

    public class CdeclReturnCall<TReturn, T0, T1> : AbstractCall
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate TReturn CallHandler(T0 arg0, T1 arg1);

        CallHandler call;
        public CallHandler Call { get { return this.call; } }

        public CdeclReturnCall(int address) : base(address)
        {
            this.call = GetDelegateFromFunction<CallHandler>(address);
        }
    }

    public class CdeclReturnCall<TReturn, T0, T1, T2> : AbstractCall
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate TReturn CallHandler(T0 arg0, T1 arg1, T2 arg2);

        CallHandler call;
        public CallHandler Call { get { return this.call; } }

        public CdeclReturnCall(int address) : base(address)
        {
            this.call = GetDelegateFromFunction<CallHandler>(address);
        }
    }

    public class CdeclReturnCall<TReturn, T0, T1, T2, T3> : AbstractCall
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate TReturn CallHandler(T0 arg0, T1 arg1, T2 arg2, T3 arg3);

        CallHandler call;
        public CallHandler Call { get { return this.call; } }

        public CdeclReturnCall(int address) : base(address)
        {
            this.call = GetDelegateFromFunction<CallHandler>(address);
        }
    }

    public class CdeclReturnCall<TReturn, T0, T1, T2, T3, T4> : AbstractCall
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate TReturn CallHandler(T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4);

        CallHandler call;
        public CallHandler Call { get { return this.call; } }

        public CdeclReturnCall(int address) : base(address)
        {
            this.call = GetDelegateFromFunction<CallHandler>(address);
        }
    }

    public class CdeclReturnCall<TReturn, T0, T1, T2, T3, T4, T5> : AbstractCall
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate TReturn CallHandler(T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);

        CallHandler call;
        public CallHandler Call { get { return this.call; } }

        public CdeclReturnCall(int address) : base(address)
        {
            this.call = GetDelegateFromFunction<CallHandler>(address);
        }
    }

    public class CdeclReturnCall<TReturn, T0, T1, T2, T3, T4, T5, T6> : AbstractCall
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate TReturn CallHandler(T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6);

        CallHandler call;
        public CallHandler Call { get { return this.call; } }

        public CdeclReturnCall(int address) : base(address)
        {
            this.call = GetDelegateFromFunction<CallHandler>(address);
        }
    }
}
