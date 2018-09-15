using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace WinApi.NEW
{
    public class FastHook : IDisposable
    {
        public delegate void HookHandler(FastHook hook, Registers registers);

        int address;
        public int Address { get { return this.address; } }

        uint length;
        public uint Length { get { return this.length; } }

        Registers registers;
        public Registers Registers { get { return this.registers; } }

        HookHandler method;
        public HookHandler Method { get { return this.method; } }

        byte[] oriCode;
        ReadOnlyCollection<byte> oriCodeAccessor;
        public ReadOnlyCollection<byte> OriginalCode { get { return oriCodeAccessor; } }

        public FastHook(HookHandler method, int address, uint length, Registers registers = Registers.ALL)
        {
            if (method == null)
                throw new Exception("Method is null!");
            if (address <= 0)
                throw new Exception("Address cannot be <= 0!");
            if (this.length < 5)
                throw new Exception("Length must be at least 5!");

            this.address = address;
            this.length = length;
            this.registers = registers;
            this.method = method;
            this.oriCode = new byte[length];
            this.oriCodeAccessor = new ReadOnlyCollection<byte>(oriCode);
        }

        bool injected;
        public bool Injected { get { return this.injected; } }

        public void Inject()
        {
            if (injected)
                return;

            //Process.Read(address, oriCode, length);

            uint neededSize = 100;
            int hookAddress = Process.Alloc(neededSize);
            byte[] buf = new byte[neededSize];

            // jmp to hook
            buf[0] = 0xE9;
            int jmpAddress = hookAddress - (address + 5);
            buf[1] = (byte)jmpAddress;
            buf[2] = (byte)jmpAddress;

            injected = true;
        }

        public void Remove()
        {
            if (!injected)
                return;

            injected = false;
        }

        public void Dispose()
        {
            Remove();
        }
    }
}
