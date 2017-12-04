using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace WinApi.Calls
{
    public class CdeclCall : AbstractCall
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void CallHandler(int self);

        CallHandler call;
        public CallHandler Call { get { return this.call; } }

        public CdeclCall(int address) : base(address)
        {
            this.call = GetDelegateFromFunction<CallHandler>(address);
        }
    }

    public class CdeclCall<T> : AbstractCall
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void CallHandler(T arg0);

        CallHandler call;
        public CallHandler Call { get { return this.call; } }

        public CdeclCall(int address) : base(address)
        {
            this.call = GetDelegateFromFunction<CallHandler>(address);
        }
    }

    public class CdeclCall<T0, T1> : AbstractCall
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void CallHandler(T0 arg0, T1 arg1);

        CallHandler call;
        public CallHandler Call { get { return this.call; } }

        public CdeclCall(int address) : base(address)
        {
            this.call = GetDelegateFromFunction<CallHandler>(address);
        }
    }

    public class CdeclCall<T0, T1, T2> : AbstractCall
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void CallHandler(T0 arg0, T1 arg1, T2 arg2);

        CallHandler call;
        public CallHandler Call { get { return this.call; } }

        public CdeclCall(int address) : base(address)
        {
            this.call = GetDelegateFromFunction<CallHandler>(address);
        }
    }

    public class CdeclCall<T0, T1, T2, T3> : AbstractCall
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void CallHandler(T0 arg0, T1 arg1, T2 arg2, T3 arg3);

        CallHandler call;
        public CallHandler Call { get { return this.call; } }

        public CdeclCall(int address) : base(address)
        {
            this.call = GetDelegateFromFunction<CallHandler>(address);
        }
    }

    public class CdeclCall<T0, T1, T2, T3, T4> : AbstractCall
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void CallHandler(T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4);

        CallHandler call;
        public CallHandler Call { get { return this.call; } }

        public CdeclCall(int address) : base(address)
        {
            this.call = GetDelegateFromFunction<CallHandler>(address);
        }
    }

    public class CdeclCall<T0, T1, T2, T3, T4, T5> : AbstractCall
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void CallHandler(T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);

        CallHandler call;
        public CallHandler Call { get { return this.call; } }

        public CdeclCall(int address) : base(address)
        {
            this.call = GetDelegateFromFunction<CallHandler>(address);
        }
    }

    public class CdeclCall<T0, T1, T2, T3, T4, T5, T6> : AbstractCall
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void CallHandler(T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6);

        CallHandler call;
        public CallHandler Call { get { return this.call; } }

        public CdeclCall(int address) : base(address)
        {
            this.call = GetDelegateFromFunction<CallHandler>(address);
        }
    }
}
