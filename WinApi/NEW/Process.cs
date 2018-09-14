using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace WinApi.NEW
{
    public unsafe static class Process
    {
        static IntPtr Handle;
        static uint ProcessID;

        public static void Init()
        {
        }

        static Process()
        {
            try
            {
                ProcessID = (uint)System.Diagnostics.Process.GetCurrentProcess().Id;
                System.Diagnostics.Process.EnterDebugMode();
                Handle = PInvoke.OpenProcess((uint)PInvoke.ProcessAccess.PROCESS_ALL_ACCESS, false, ProcessID);
                if (Handle == IntPtr.Zero)
                    Error.GetLast();

                const int ExecuteCodeStart = 0x401000;
                const int ExecuteCodeEnd = 0xB0BFFF;
                PInvoke.VirtualProtect(new IntPtr(ExecuteCodeStart), ExecuteCodeEnd - ExecuteCodeStart, PInvoke.MemoryProtection.ExecuteReadWrite, out PInvoke.MemoryProtection old);
            }
            catch (Exception e)
            {
                MessageBox(e.ToString());
            }
        }

        #region Read

        /// <summary> Reads 4 bytes and checks whether the bit at bitNum from the right is set. </summary>
        public static bool ReadBit(int address, int bitNum)
        {
            return (ReadInt(address) & (1 << bitNum)) != 0;
        }

        public static byte ReadByte(int address)
        {
            ExceptionHelper.AddressZero(address);
            return *(byte*)address;
        }

        public static sbyte ReadSByte(int address)
        {
            ExceptionHelper.AddressZero(address);
            return *(sbyte*)address;
        }

        public static ushort ReadUShort(int address)
        {
            ExceptionHelper.AddressZero(address);
            return *(ushort*)address;
        }

        public static short ReadShort(int address)
        {
            ExceptionHelper.AddressZero(address);
            return *(short*)address;
        }

        public static uint ReadUInt(int address)
        {
            ExceptionHelper.AddressZero(address);
            return *(uint*)address;
        }

        public static int ReadInt(int address)
        {
            ExceptionHelper.AddressZero(address);
            return *(int*)address;
        }

        public static void ReadBytes(int address, byte[] buffer, uint count)
        {
            ExceptionHelper.AddressZero(address);
            ExceptionHelper.ArgumentNull(buffer, "buffer");
            ExceptionHelper.ArrayCount(buffer, count);

            fixed (byte* buf = buffer)
            {
                int index = 0;
                int nextIndex;
                while ((nextIndex = index + 4) < count)
                {
                    *(int*)(buf + index) = *(int*)(address + index);
                    index += 4;
                }

                while (index < count)
                {
                    *(buf + index) = *(byte*)(address + index);
                    index++;
                }
            }
        }

        /// <summary> Creates a new byte array and reads. </summary>
        public static byte[] ReadBytes(int address, uint count)
        {
            ExceptionHelper.AddressZero(address);
            byte[] result = new byte[count];
            ReadBytes(address, result, count);
            return result;
        }

        #endregion

        #region Write

        public static void WriteBit(int address, int bitNum, bool value)
        {
            /*ExceptionHelper.AddressZero(address);
            *(byte*)address = value;

            int bitValue = 1 << bitNum;
            int read = ReadInt(address);
            if (((read & bitValue) != 0) != value)
            {
                Write(address, read ^ bitValue);
            }´*/
        }

        /// <summary> Writes bool as 4 bytes. </summary>
        public static void Write(int address, bool value)
        {
            ExceptionHelper.AddressZero(address);
            *(int*)address = value ? 1 : 0;
        }

        public static void Write(int address, byte value)
        {
            ExceptionHelper.AddressZero(address);
            *(byte*)address = value;
        }

        public static void Write(int address, sbyte value)
        {
            ExceptionHelper.AddressZero(address);
            *(sbyte*)address = value;
        }

        public static void Write(int address, ushort value)
        {
            ExceptionHelper.AddressZero(address);
            *(ushort*)address = value;
        }

        public static void Write(int address, short value)
        {
            ExceptionHelper.AddressZero(address);
            *(short*)address = value;
        }

        public static void Write(int address, uint value)
        {
            ExceptionHelper.AddressZero(address);
            *(uint*)address = value;
        }

        public static void Write(int address, int value)
        {
            ExceptionHelper.AddressZero(address);
            *(int*)address = value;
        }

        #endregion

        public static FastHook AddFastHook(FastHook.HookHandler method, int address, uint length, Registers registers = Registers.ALL)
        {
            FastHook hook = new FastHook(method, address, length, registers);
            hook.Inject();
            return hook;
        }

        public static int Alloc(uint size)
        {
            return 0;
            if (size == 0)
                throw new Exception("Size is zero!");

            IntPtr ptr = PInvoke.VirtualAllocEx(Handle, IntPtr.Zero, size, PInvoke.AllocationType.Reserve | PInvoke.AllocationType.Commit, PInvoke.MemoryProtection.ReadWrite);
            if (ptr == IntPtr.Zero)
                Error.GetLast();

            return ptr.ToInt32();
        }

        public static uint Write(int address, byte[] arr, int count)
        {
            if (address <= 0)
                throw new ArgumentException("Address is <= 0!");

            if (!PInvoke.WriteProcessMemory(Handle, new IntPtr(address), arr, (uint)count, out uint byteWritten))
                Error.GetLast();

            return byteWritten;
        }

        static void MessageBox(string text)
        {
            PInvoke.MessageBox(IntPtr.Zero, text, "WinApi", 0);
        }
    }
}
