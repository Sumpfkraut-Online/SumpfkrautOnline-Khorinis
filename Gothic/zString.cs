using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;
using WinApi.Calls;

namespace Gothic
{
    public class zString
    {
        public const int ByteSize = 20;

        public class VarOffsets
        {
            public const int VTBL = 0;
            public const int Allocater = 4;
            public const int Ptr = 8;
            public const int Length = 12;
            public const int Res = 16;
        }

        public class FuncAddresses
        {
            public const int Insert = 0x0046B400; /// <summary> zString::Insert(uint, zSTRING const &) </summary>
            public const int Clear = 0x0059D010; /// <summary> zSTRING::Clear(void) </summary>

            public const int OperatorAssignZString = 0x0059CEB0; /// <summary> zSTRING::operator=(zSTRING const &) </summary>
            public const int OperatorAssignConstChar = 0x004CFAF0; /// <summary> zSTRING::operator=(char const &) </summary>

            public const int ConstructorConstChar = 0x004010C0; /// <summary> zSTRING::zSTRING(char const *) </summary>
            public const int Destructor = 0x00401160; /// <summary> zSTRING::~zSTRING(void) </summary>
        }

        static ThisCall<int> constructorConstChar = new ThisCall<int>(FuncAddresses.ConstructorConstChar);
        public static zPointer Create(string value)
        {
            zPointer zStr = Process.Alloc(ByteSize).ToInt32();

            byte[] chars = value == null ? new byte[0] : Encoding.Default.GetBytes(value); // improve me
            int charsPtr = Process.Alloc((uint)chars.Length + 1).ToInt32();

            if (chars.Length > 0)
                Process.Write(charsPtr, chars);

            Process.Write(charsPtr + chars.Length, (byte)0);

            constructorConstChar.Call(zStr, charsPtr);
            Process.Free(charsPtr, (uint)chars.Length + 1);

            return zStr;
        }

        static ThisCall<int> operatorAssignConstChar = new ThisCall<int>(FuncAddresses.OperatorAssignConstChar);
        public static void Set(zPointer zStr, string str)
        {
            byte[] arr = str == null ? new byte[0] : Encoding.Default.GetBytes(str);
            int charPtr = Process.Alloc((uint)arr.Length + 1).ToInt32();

            if (arr.Length > 0)
                Process.Write(charPtr, arr);

            Process.Write(charPtr + arr.Length, (byte)0);
            operatorAssignConstChar.Call(zStr, charPtr);
            Process.Free(charPtr, (uint)str.Length + 1);
        }

        static ThisCall clear = new ThisCall(FuncAddresses.Clear);
        public static void Clear(zPointer self)
        {
            clear.Call(self);
        }

        static ThisCall<int, zPointer> insert = new ThisCall<int, zPointer>(FuncAddresses.Insert);
        public static void Insert(zPointer self, int pos, zPointer zstring)
        {
            insert.Call(self, pos, zstring);
        }

        static ThisCall<zPointer> set = new ThisCall<zPointer>(FuncAddresses.OperatorAssignZString);
        public static void Set(zPointer self, zPointer zstring)
        {
            set.Call(self, zstring);
        }

        static ThisCall destructor = new ThisCall(FuncAddresses.Destructor);
        public static void Dispose(zPointer self)
        {
            destructor.Call(self);
            Process.Free(self, ByteSize);
        }
        
        public static int GetCharsPtr(zPointer self)
        {
            return Process.ReadInt(self + VarOffsets.Ptr);
        }
        
        public static int GetLength(zPointer self)
        {
            return Process.ReadInt(self + VarOffsets.Length); 
        }
    }
}
