using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace WinApi
{
    public delegate void HookCallback(Hook hook, RegisterMemory ptr);

    public class Hook
    {
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
        
        internal Hook(int hookAddr, HookCallback callback, int newCodeAddr, uint newCodeLen, int oldInNewAddr, byte[] oldCode)
        {
            this.hookAddr = hookAddr;
            this.callback = callback;
            this.newCodeAddr = newCodeAddr;
            this.newCodeLen = newCodeLen;
            this.oldInNewAddr = oldInNewAddr;
            this.oldCode = oldCode;
        }
        
        bool skipOldCode = false;
        public void SetSkipOldCode(bool skip)
        {
            if (skip)
            {
                if (!skipOldCode)
                {
                    skipOldCode = true;

                    using (MemoryStream ms = new MemoryStream(5))
                    using (BinaryWriter bw = new BinaryWriter(ms))
                    {
                        bw.Write((byte)0xE9);
                        bw.Write(HookAddress + HookLength - OldInNewAddress - 5);

                        Process.Write(OldInNewAddress, ms.ToArray());
                    }
                }
            }
            else
            {
                if (skipOldCode)
                {
                    RestoreOldInNewCode();
                }
            }
        }

        public void RestoreOldInNewCode()
        {
            skipOldCode = false;
            Process.Write(OldInNewAddress, oldCode);
        }
    }
}
