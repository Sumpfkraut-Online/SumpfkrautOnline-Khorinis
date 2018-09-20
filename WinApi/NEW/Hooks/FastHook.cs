using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using GUC.Injection.Utilities;

namespace GUC.Injection
{
    public class FastHook : Hook
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void MethodScheme(RegisterMemory registers);

        public FastHook(MethodScheme method, int address, int length) : base(method, address, length)
        {
            ExceptionHelper.Length(length, 5);
        }

        int methodAddress;
        int methodCodeLen { get { return Length + 14; } }
        int oriCodeAddress { get { return methodAddress + 9; } }

        IntPtr ptr;

        protected override byte[] CreateCode(Delegate method)
        {
            #region Method Code
            methodAddress = Process.Alloc(methodCodeLen);

            ByteWriter bw = new ByteWriter(methodCodeLen);
            bw.WriteByte(0x9C); // pushfd
            bw.WriteByte(0x60); // pushad

            // call method (cdecl call)
            this.ptr = Marshal.GetFunctionPointerForDelegate(method);
            int methodPtr = ptr.ToInt32();
            bw.WriteByte(0xE8);
            bw.WriteInt(methodPtr - (methodAddress + 7));

            bw.WriteByte(0x61); // popad
            bw.WriteByte(0x9D); // popfd
            
            bw.WriteBytes(oriCode);

            // jump back
            bw.WriteByte(0xE9);
            bw.WriteInt(Address + Length - (methodAddress + methodCodeLen));

            Process.WriteBytes(methodAddress, bw.GetBuffer(), bw.Length);
            #endregion

            #region Hook Code
            // hook jmp code
            bw.Reset();
            bw.WriteByte(0xE9);
            bw.WriteInt(methodAddress - (Address + 5));
            for (int i = 5; i < Length; i++) // fill rest with nops
                bw.WriteByte(0x90);
            return bw.CopyData();
            #endregion
        }

        public override void Dispose()
        {
            // free method code
            base.Dispose();
            Process.Free(methodAddress, methodCodeLen);
        }

        enum OriCodeSetting
        {
            Enabled,
            Skip,
            Return,
        }

        OriCodeSetting oriCodeSetting = OriCodeSetting.Enabled;

        /// <summary> Nops out the original code inside the method code. </summary>
        public void SetOriCodeSkip()
        {
            if (oriCodeSetting != OriCodeSetting.Skip)
            {
                Process.Nop(oriCodeAddress, Length);
                oriCodeSetting = OriCodeSetting.Skip;
            }
        }

        byte returnParams;
        /// <summary> Replaces the original code with a return instruction inside the method code </summary>
        /// <param name="stackCleanupParams"> Number of parameters which should be considered in the return instruction. </param>
        public void SetOriCodeReturn(byte stackCleanupParams = 0)
        {
            if (oriCodeSetting != OriCodeSetting.Return || this.returnParams != stackCleanupParams)
            {
                if (stackCleanupParams == 0)
                {
                    Process.WriteByte(oriCodeAddress, 0xC3);
                    Process.Nop(oriCodeAddress + 1, Length - 1);
                }
                else
                {
                    Process.WriteByte(oriCodeAddress, 0xC2);
                    Process.WriteUShort(oriCodeAddress + 1, (ushort)(4 * stackCleanupParams));
                    Process.Nop(oriCodeAddress + 3, Length - 3);
                }

                oriCodeSetting = OriCodeSetting.Return;
                this.returnParams = stackCleanupParams;
            }
        }

        /// <summary> Restores the original code inside the method code. (Does not remove the hook, call Remove() for that!) </summary>
        public void SetOriCodeRestore()
        {
            if (oriCodeSetting != OriCodeSetting.Enabled)
            {
                Process.WriteBytes(oriCodeAddress, oriCode, Length);
                oriCodeSetting = OriCodeSetting.Enabled;
            }
        }
    }
}
