using System;
using System.Collections.Generic;
using System.Text;
using WinApi;
using Gothic.zTypes;

namespace Gothic.zClasses
{
    public class zCParser : zClass
    {
        public enum Offsets
        {
            table = 0x0018,
            array = 0x003C,
            Stack = 0x0048,
            DataStack = 0x0058,

            fileName = 8288,
            MainFileName = 8308

        }

        public enum FuncOffsets
        {
            SetInstance = 0x00794870,
            SetReturn_Int = 0x007A0960,
            GetInstance = 0x007A08F0,
            GetParameter_Int = 0x007A0760,
            GetParameter_String = 0x007A07B0,
            ParseFile = 0x0078F660,
            ParseSource = 0x0078EE20,
            Parse = 0x0078EBA0,
            EnableTreeLoad = 0x00793460,
            EnableTreeSave = 0x00793440,
            Reset = 0x00793100,
            AddClassOffset = 0x00794730,
            CreatePCode = 0x007900E0,
            Error = 0x0078E730
        }

        public enum HookSize
        {
            Parse = 7
        }

        public zCParser(Process process, int address) : base(process, address)
        {

        }

        public zCParser()
        {

        }


        #region Fields

        public zCPar_SymbolTable SymbolTable
        {
            get { return new zCPar_SymbolTable(Process, Address + 16); }
        }

        public zString MainFileName
        {
            get { return new zString(Process, Address + (int)Offsets.MainFileName); }
        }

        public zString FileName
        {
            get { return new zString(Process, Address + (int)Offsets.fileName); }
        }

        public zCArray<zCPar_Symbol> Table
        {
            get { return new zCArray<zCPar_Symbol>(Process, Address + (int)Offsets.table); }
        }

        public zCArray<zString> Array
        {
            get { return new zCArray<zString>(Process, Address + (int)Offsets.array); }
        }

        public zCPar_Stack Stack
        {
            get { return new zCPar_Stack(Process, Address + (int)Offsets.Stack); }
        }

        public zCPar_DataStack DataStack
        {
            get { return new zCPar_DataStack(Process, Address + (int)Offsets.DataStack); }
        }
        #endregion


        public void AddClassOffset(zString ptr, int offset)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.AddClassOffset, new CallValue[] { ptr, new IntArg(offset) });
        }

        public void CreatePCode()
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.CreatePCode, new CallValue[] { });
        }

        public int Error()
        {
            return Process.THISCALL<IntArg>((uint)Address, (uint)FuncOffsets.Error, new CallValue[] { });
        }

        public void Reset()
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.Reset, new CallValue[] {  });
        }

        public void EnableTreeLoad(int ptr)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.EnableTreeLoad, new CallValue[] { new IntArg(ptr) });
        }

        public void EnableTreeSave(int ptr)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.EnableTreeSave, new CallValue[] { new IntArg(ptr) });
        }

        public void ParseSource(zString ptr)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.ParseSource, new CallValue[] { ptr });
        }

        public void ParseFile(zString ptr)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.ParseFile, new CallValue[] { ptr });
        }

        public void SetReturn(int ptr)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.SetReturn_Int, new CallValue[] { new IntArg(ptr) });
        }

        public int CreateInstance(zString str, int ptr)
        {
            return Process.THISCALL<IntArg>((uint)Address, 0x00792F20, new CallValue[] { str, new IntArg(ptr) }).Address;
        }

        public int SetInstance(zString str, int ptr)
        {
            return Process.THISCALL<IntArg>((uint)Address, (int)FuncOffsets.SetInstance, new CallValue[] { str, new IntArg(ptr) }).Address;
        }

        public int CreateInstance(int id, int ptr)
        {
            return Process.THISCALL<IntArg>((uint)Address, 0x00792FA0, new CallValue[] { new IntArg(id), new IntArg(ptr) }).Address;
        }

        public T CreateInstance<T>(zString str) where T : zClass, new()
        {
            T rValue = new T();

            int alloc = GetSymbol(str).Offset;

            if (alloc == 0)
                return null;

            int ptr = Process.Alloc((uint)alloc).ToInt32();
            CreateInstance(str, ptr);
            rValue.Initialize(Process, ptr);
            
            return rValue;
        }

        public T CreateInstance<T>(int id) where T : zClass, new()
        {
            T rValue = new T();

            int alloc = GetSymbol(id).Offset;

            if (alloc == 0)
                return null;

            int ptr = Process.Alloc((uint)alloc).ToInt32();
            CreateInstance(id, ptr);
            rValue.Initialize(Process, ptr);

            return rValue;
        }

        public void CallFunc(zString str)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, 0x007929D0, new CallValue[] { str });

        }

        public int GetIndex(String str)
        {
            zString zS = zString.Create(Process, str);
            int i = GetIndex(zS);
            zS.Dispose();

            return i;
        }

        public int GetIndex(zString str)
        {
            return Process.THISCALL<IntArg>((uint)Address, 0x00793470, new CallValue[] { str }).Address;
        }

        public int GetInstance()
        {
            return Process.THISCALL<IntArg>((uint)Address, (uint)FuncOffsets.GetInstance, new CallValue[] { }).Address;
        }

        public int GetInstanceAndIndex(ref int x)
        {
            int ptr = Process.Alloc(4).ToInt32();
            int rVal = Process.THISCALL<IntArg>((uint)Address, (uint)FuncOffsets.GetParameter_Int, new CallValue[] { (IntArg)ptr });
            x = Process.ReadInt(ptr);
            Process.Free(new IntPtr(ptr), 4);
            return rVal;
        }

        public int getIntParameter()
        {
            int ptr = Process.Alloc(4).ToInt32();
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.GetParameter_Int, new CallValue[] { (IntArg)ptr });
            int x = Process.ReadInt(ptr);
            Process.Free(new IntPtr(ptr), 4);

            return x;
        }

        public String getStringParameter()
        {

            zString str = zString.Create(Process, "");
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.GetParameter_String, new CallValue[] { (IntArg)str.Address });
            String rStr = str.Value;
            str.Dispose();
            return rStr;
        }

        public zCPar_Symbol GetSymbol(zString str)
        {
            return Process.THISCALL<zCPar_Symbol>((uint)Address, 0x007938D0, new CallValue[] { str });
        }

        public zCPar_Symbol GetSymbol(int id)
        {
            return Process.THISCALL<zCPar_Symbol>((uint)Address, 0x007938C0, new CallValue[] { new IntArg(id) });
        }

        public static void setEnableParsing(Process process, bool b)
        {
            if(b)
                process.Write(1, 0x00AB6284);
            else
                process.Write(0, 0x00AB6284);
        }

        public static zCParser getParser(Process process)
        {
            return new zCParser(process, 0xAB40C0);
        }

        
        public static zCParser getMenuParser(Process process)
        {
            return new zCParser(process, 0x008D1E68);
        }

        public static zCParser getFXParser(Process process)
        {
            return new zCParser(process, 0x008CE6EC);
        }
        



        public static void CallFunc(Process process, CallValue[] cv )
        {
            process.CDECLCALL<NullReturnCall>((uint)0x007929F0, cv);
        }

        public static void DefineExternal(Process process, CallValue[] cv)
        {
            process.CDECLCALL<NullReturnCall>((uint)0x007A0190, cv);
        }

        public override uint ValueLength()
        {
            return 4;
        }
    }
}
