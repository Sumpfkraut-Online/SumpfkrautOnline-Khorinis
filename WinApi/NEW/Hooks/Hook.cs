using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApiNew.Utilities;

namespace WinApiNew.Hooks
{
    public abstract class Hook : IDisposable
    {
        // keep references to hooks here to prevent the GC from killing the delegates
        static List<Hook> hookList = new List<Hook>();

        int address;
        /// <summary> Address of the start of the hook in the process. </summary>
        public int Address { get { return this.address; } }

        int length;
        /// <summary> Length of the hook in the process </summary>
        public int Length { get { return this.length; } }

        Delegate method;
        public Delegate Method { get { return this.method; } }

        protected byte[] oriCode;

        bool injected;
        public bool Injected { get { return this.injected; } }

        byte[] hookCode;
        protected Hook(Delegate method, int address, int length)
        {
            ExceptionHelper.ArgumentNull(method, "method");

            ExceptionHelper.AddressZero(address);
            this.address = address;

            ExceptionHelper.SEQZero(length, "length");
            this.length = length;
            
            // Read original code in the process
            this.oriCode = Process.ReadBytes(address, length);

            this.hookCode = CreateCode(method);
            ExceptionHelper.EmptyArray(hookCode);

            this.method = method;
        }

        protected abstract byte[] CreateCode(Delegate method);

        public void Inject()
        {
            if (injected)
                return;

            Process.WriteBytes(address, hookCode);
            hookList.Add(this);

            injected = true;
        }

        public void Remove()
        {
            if (!injected)
                return;

            Process.WriteBytes(address, oriCode);
            hookList.Remove(this);

            injected = false;
        }

        public virtual void Dispose()
        {
            Remove();
        }
    }
}
