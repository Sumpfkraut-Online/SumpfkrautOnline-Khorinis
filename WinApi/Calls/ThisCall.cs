﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace WinApi.Calls
{
    public class ThisCall : AbstractCall
    {
        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        public delegate void CallHandler(int self);

        CallHandler call;
        public CallHandler Call { get { return this.call; } }

        public ThisCall(int address) : base(address)
        {
            this.call = GetDelegateFromFunction<CallHandler>(address);
        }
    }

    public class ThisCall<T> : AbstractCall
    {
        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        public delegate void CallHandler(int self, T arg0);

        CallHandler call;
        public CallHandler Call { get { return this.call; } }

        public ThisCall(int address) : base(address)
        {
            this.call = GetDelegateFromFunction<CallHandler>(address);
        }
    }

    public class ThisCall<T0, T1> : AbstractCall
    {
        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        public delegate void CallHandler(int self, T0 arg0, T1 arg1);

        CallHandler call;
        public CallHandler Call { get { return this.call; } }

        public ThisCall(int address) : base(address)
        {
            this.call = GetDelegateFromFunction<CallHandler>(address);
        }
    }

    public class ThisCall<T0, T1, T2> : AbstractCall
    {
        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        public delegate void CallHandler(int self, T0 arg0, T1 arg1, T2 arg2);

        CallHandler call;
        public CallHandler Call { get { return this.call; } }

        public ThisCall(int address) : base(address)
        {
            this.call = GetDelegateFromFunction<CallHandler>(address);
        }
    }

    public class ThisCall<T0, T1, T2, T3> : AbstractCall
    {
        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        public delegate void CallHandler(int self, T0 arg0, T1 arg1, T2 arg2, T3 arg3);

        CallHandler call;
        public CallHandler Call { get { return this.call; } }

        public ThisCall(int address) : base(address)
        {
            this.call = GetDelegateFromFunction<CallHandler>(address);
        }
    }

    public class ThisCall<T0, T1, T2, T3, T4> : AbstractCall
    {
        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        public delegate void CallHandler(int self, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4);

        CallHandler call;
        public CallHandler Call { get { return this.call; } }

        public ThisCall(int address) : base(address)
        {
            this.call = GetDelegateFromFunction<CallHandler>(address);
        }
    }

    public class ThisCall<T0, T1, T2, T3, T4, T5> : AbstractCall
    {
        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        public delegate void CallHandler(int self, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);

        CallHandler call;
        public CallHandler Call { get { return this.call; } }

        public ThisCall(int address) : base(address)
        {
            this.call = GetDelegateFromFunction<CallHandler>(address);
        }
    }

    public class ThisCall<T0, T1, T2, T3, T4, T5, T6> : AbstractCall
    {
        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        public delegate void CallHandler(int self, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6);

        CallHandler call;
        public CallHandler Call { get { return this.call; } }

        public ThisCall(int address) : base(address)
        {
            this.call = GetDelegateFromFunction<CallHandler>(address);
        }
    }
}