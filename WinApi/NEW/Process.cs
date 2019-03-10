using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using WinApiNew.Utilities;
using WinApiNew.Hooks;

namespace WinApiNew
{
    public unsafe static class Process
    {
        static int Handle;
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
                Handle = PInvoke.OpenProcess(PInvoke.ProcessAccess.PROCESS_ALL_ACCESS, false, ProcessID);
                if (Handle == 0) Error.GetLast();

                const int ExecuteCodeStart = 0x401000;
                const int ExecuteCodeEnd = 0xB0BFFF;
                PInvoke.VirtualProtect(ExecuteCodeStart, ExecuteCodeEnd - ExecuteCodeStart, PInvoke.MemoryProtection.ExecuteReadWrite, out PInvoke.MemoryProtection old);
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
        
        public static bool ReadBool(int address)
        {
            ExceptionHelper.AddressZero(address);
            return *(bool*)address;
        }

        public static float ReadFloat(int address)
        {
            ExceptionHelper.AddressZero(address);
            return *(float*)address;
        }

        public static void ReadBytes(int address, byte[] buffer, int count)
        {
            ExceptionHelper.AddressZero(address);
            ExceptionHelper.ArgumentNull(buffer, "buffer");
            ExceptionHelper.ArrayCount(buffer, count);

            if (count <= 0)
                return;

            fixed (byte* buf = buffer)
            {
                int index = 0;
                int nextIndex;
                while ((nextIndex = index + 8) <= count)
                {
                    *(long*)(buf + index) = *(long*)(address + index);
                    index = nextIndex;
                }

                while ((nextIndex = index + 1) <= count)
                {
                    *(buf + index) = *(byte*)(address + index);
                    index = nextIndex;
                }
            }
        }

        /// <summary> Creates a new byte array and reads. </summary>
        public static byte[] ReadBytes(int address, int count)
        {
            ExceptionHelper.AddressZero(address);
            byte[] result = new byte[count];
            ReadBytes(address, result, count);
            return result;
        }

        #endregion

        #region Write

        /// <summary> Sets the specified bit from the left of the 4 bytes at address. </summary>
        public static void WriteBit(int address, int bitNum, bool value)
        {
            ExceptionHelper.AddressZero(address);

            int bitValue = 1 << bitNum;
            int read = ReadInt(address);
            if (((read & bitValue) != 0) != value)
            {
                *(int*)address = read ^ bitValue;
            }
        }

        /// <summary> Writes bool as 4 bytes. </summary>
        public static void WriteBool(int address, bool value)
        {
            ExceptionHelper.AddressZero(address);
            *(int*)address = value ? 1 : 0;
        }

        public static void WriteByte(int address, byte value)
        {
            ExceptionHelper.AddressZero(address);
            *(byte*)address = value;
        }

        public static void WriteSByte(int address, sbyte value)
        {
            ExceptionHelper.AddressZero(address);
            *(sbyte*)address = value;
        }

        public static void WriteUShort(int address, ushort value)
        {
            ExceptionHelper.AddressZero(address);
            *(ushort*)address = value;
        }

        public static void WriteShort(int address, short value)
        {
            ExceptionHelper.AddressZero(address);
            *(short*)address = value;
        }

        public static void WriteUInt(int address, uint value)
        {
            ExceptionHelper.AddressZero(address);
            *(uint*)address = value;
        }

        public static void WriteInt(int address, int value)
        {
            ExceptionHelper.AddressZero(address);
            *(int*)address = value;
        }

        public static void WriteBytes(int address, byte[] buffer, int count)
        {
            ExceptionHelper.AddressZero(address);
            ExceptionHelper.ArgumentNull(buffer, "buffer");
            ExceptionHelper.ArrayCount(buffer, count);

            if (count <= 0)
                return;

            fixed(byte* ptr = buffer)
            {
                int index = 0;
                int nextIndex;
                while ((nextIndex = index + 8) <= count)
                {
                    *(long*)(address + index) = *(long*)(ptr + index);
                    index = nextIndex;
                }

                while ((nextIndex = index + 1) <= count)
                {
                    *(byte*)(address + index) = *(ptr + index);
                    index = nextIndex;
                }
            }
        }

        public static void WriteBytes(int address, params byte[] buffer)
        {
            WriteBytes(address, buffer, buffer.Length);
        }

        public static void Nop(int address, int count)
        {
            ExceptionHelper.AddressZero(address);
            if (count <= 0) return;
            for (int i = 0; i < count; i++)
                *(byte*)(address + i) = 0x90;
        }

        #endregion

        /// <summary> "Transmits via CLR / .NET, slow but safe, allocates space for call code and jumps there." </summary>
        public static object AddSafeHook(Delegate method, int address, int length)
        {
            throw new NotImplementedException();
        }

        /// <summary> "Directly calls the C# method, breaks (at least) FileStreams, allocates space for call code and jumps there." </summary>
        public static FastHook AddFastHook(FastHook.MethodScheme method, int address, int length)
        {
            FastHook hook = new FastHook(method, address, length);
            hook.Inject();
            return hook;
        }

        #region Alloc & Free

        public static int Alloc(int size)
        {
            ExceptionHelper.SEQZero(size, "size");

            int ptr = PInvoke.VirtualAllocEx(Handle, 0, (uint)size, PInvoke.AllocationType.Reserve | PInvoke.AllocationType.Commit, PInvoke.MemoryProtection.ExecuteReadWrite);
            if (ptr == 0)
                Error.GetLast();

            return ptr;
        }

        public static void Free(int address, int count)
        {
            ExceptionHelper.AddressZero(address);
            ExceptionHelper.SEQZero(count, "count");

            if (!PInvoke.VirtualFreeEx(Handle, address, (uint)count, PInvoke.AllocationType.Decommit))
                Error.GetLast();
            if (!PInvoke.VirtualFreeEx(Handle, address, 0, PInvoke.AllocationType.Release))
                Error.GetLast();
        }

        #endregion

        public static void MessageBox(string text)
        {
            PInvoke.MessageBox(0, text, "WinApi", 0);
        }
    }
}
