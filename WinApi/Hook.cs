using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinApi
{
    public delegate void HookCallback(Hook hook);

    public class Hook
    {
        uint id;
        internal uint ID { get { return this.id; } }
        int idAddr;
        internal int IDAddress { get { return this.idAddr; } }

        int hookAddr;
        /// <summary>
        /// Gets the address of the hook.
        /// </summary>
        public int HookAddress { get { return this.hookAddr; } }

        HookCallback callback;
        /// <summary>
        /// Gets the method which is called by the hook.
        /// </summary>
        public HookCallback Callback { get { return this.callback; } }

        int infoAddr;
        internal int InfoAddress { get { return this.infoAddr; } }

        int newCodeAddr;
        /// <summary>
        /// Gets the address of the hook function.
        /// </summary>
        public int NewCodeAddress { get { return this.newCodeAddr; } }

        uint newCodeLen;
        /// <summary>
        /// Gets the length of the hook function.
        /// </summary>
        public uint NewCodeLength { get { return this.newCodeLen; } }
        
        int oldInNewAddr;
        /// <summary>
        /// Gets the address of the old code in the hook function.
        /// </summary>
        public int OldInNewAddress { get { return this.oldInNewAddr; } }

        byte[] oldCode;
        /// <summary>
        /// Gets a byte array of the old code.
        /// </summary>
        public byte[] GetOldCode()
        {
            byte[] ret = new byte[oldCode.Length];
            Array.Copy(this.oldCode, ret, ret.Length);
            return ret;
        }
        /// <summary>
        /// Gets the length of the hook.
        /// </summary>
        public uint HookLength { get { return (uint)this.oldCode.Length; } }

        uint argCount;

        /// <summary>
        /// The number of arguments this hook has been assigned.
        /// </summary>
        public uint ArgumentCount { get { return this.argCount; } }

        internal Hook(uint id, int idAddress, int hookAddr, HookCallback callback, int infoAddr, int newCodeAddr, uint newCodeLen, int oldInNewAddr, byte[] oldCode, uint argCount)
        {
            this.id = id;
            this.idAddr = idAddress;
            this.hookAddr = hookAddr;
            this.callback = callback;
            this.infoAddr = infoAddr;
            this.newCodeAddr = newCodeAddr;
            this.newCodeLen = newCodeLen;
            this.oldInNewAddr = oldInNewAddr;
            this.oldCode = oldCode;
            this.argCount = argCount;
        }

        public int GetEAX() { return Process.ReadInt(infoAddr); }
        public void SetEAX(int value) { Process.Write(value, infoAddr); }

        public int GetEBX() { return Process.ReadInt(infoAddr+4); }
        public void SetEBX(int value) { Process.Write(value, infoAddr+4); }

        public int GetECX() { return Process.ReadInt(infoAddr + 8); }
        public void SetECX(int value) { Process.Write(value, infoAddr + 8); }

        public int GetEDX() { return Process.ReadInt(infoAddr + 12); }
        public void SetEDX(int value) { Process.Write(value, infoAddr + 12); }

        public int GetEDI() { return Process.ReadInt(infoAddr + 16); }
        public void SetEDI(int value) { Process.Write(value, infoAddr + 16); }

        public int GetEBP() { return Process.ReadInt(infoAddr + 20); }
        public void SetEBP(int value) { Process.Write(value, infoAddr + 20); }

        public int GetESI() { return Process.ReadInt(infoAddr + 24); }
        public void SetESI(int value) { Process.Write(value, infoAddr + 24); }

        public int GetArgument(int index)
        {
            if (index < 0 || index >= argCount)
                throw new IndexOutOfRangeException(string.Format("Argument index of '{0}' hook must be 0 <= index < {1}! (Is {2})", this.callback.Method.Name, this.argCount, index));

            return Process.ReadInt(infoAddr + 28 + index * 4);
        }

        public void SetArgument(int index, int value)
        {
            if (index < 0 || index >= argCount)
                throw new IndexOutOfRangeException(string.Format("Argument index of '{0}' hook must be 0 <= index < {1}! (Is {2})", this.callback.Method.Name, this.argCount, index));

            Process.Write(value, infoAddr + 28 + index * 4);
        }
    }
}
