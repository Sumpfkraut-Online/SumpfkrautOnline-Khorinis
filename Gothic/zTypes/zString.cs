using System;
using System.Collections.Generic;
using System.Text;
using WinApi;
using Gothic.zClasses;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Gothic.zTypes
{
    public class zString : zClass, IDisposable
    {
        public zString(Process process, int position)
            : base(process, position)
        { }

        public zString()
        {

        }
        public bool memorySave = false;

        private bool disposed = false;
        public void Dispose()
        {
            Dispose(true);
        }

        public override uint ValueLength()
        {
            return 4;
        }

        public static zString strVirtualPath(Process process)
        {
            return new zString(process, 0x008C3494);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {

                //if (memorySave)
                //    Process.Free(new IntPtr(PTR - 1), (uint)this.Res + 1);
                //else
                    Process.THISCALL<NullReturnCall>((uint)Address, (uint)0x00401160, new CallValue[] { });
                Process.Free(new IntPtr(Address), 20);
                disposed = true;
            }
        }

        public String getCheckedValue()
        {
            if (this.Address == 0)
                return null;
            if (this.VTBL != 8578800)
                return null;
            //if (this.Value.Length <= 0 || this.Value.Length > 500)
            //    return null;
            String val = this.Value.Trim();
            if (val.Length == 0)
                return null;

            

            //bool found = Regex.IsMatch(val, "^[a-zA-Z0-9_\\-.:;!\\\"§$%&/()=?`´\\\\}\\]\\[ ß{³²<>|,@€ÄÖÜäöü#''*+~]+$");
            //if (!found)
            //    return null;
            return val;
        }

        //public static zString Create(Process process, String value)
        //{

        //    IntPtr charArr = process.Alloc((uint)value.Length + 2);
        //    IntPtr stringArr = process.Alloc(20);

        //    System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
        //    byte[] arr = enc.GetBytes(value);
        //    if (arr.Length > 0)
        //        process.Write(arr, charArr.ToInt32() + 1);

        //    zString str = new zString(process, stringArr.ToInt32());
        //    str.PTR = charArr.ToInt32() + 1;
        //    str.Length = value.Length;
        //    str.Res = value.Length + 1;
        //    str.memorySave = true;

        //    return str;
        //}
        public static zString Create(Process process, String value)
        {
            if (process == null)
                throw new ArgumentNullException("Process can't be null!");
            if (value == null)
                throw new ArgumentNullException("Value can't be null!");
            //IntPtr charArr = process.Alloc((uint)value.Length + 2);
            //IntPtr stringArr = process.Alloc(20);

            //System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            //byte[] arr = enc.GetBytes(value);
            //if (arr.Length > 0)
            //    process.Write(arr, charArr.ToInt32() + 1);

            //zString str = new zString(process, stringArr.ToInt32());
            //str.PTR = charArr.ToInt32() + 1;
            //str.Length = value.Length;
            //str.Res = value.Length + 1;

            //return str;

            System.Text.Encoding enc = System.Text.Encoding.Default;
            byte[] arr = enc.GetBytes(value);

            IntPtr charArr = process.Alloc((uint)arr.Length + 1);
            IntPtr stringArr = process.Alloc(20);



            if (arr.Length > 0)
                process.Write(arr, charArr.ToInt32());

            process.THISCALL<NullReturnCall>((uint)stringArr.ToInt32(), (uint)0x004010C0, new CallValue[] { new IntArg(charArr.ToInt32()) });
            process.Free(charArr, (uint)arr.Length + 1);
            zString str = new zString(process, stringArr.ToInt32());
            str.memorySave = false;
            return str;
        }

        public void Clear()
        {
            if (Address == 0)
                throw new Exception("The zString-Address can't be 0!");
            Process.THISCALL<NullReturnCall>((uint)Address, 0x0059D010, new CallValue[] { });
        }

        public void Insert(int pos, zString str)
        {
            if (Address == 0)
                throw new Exception("The zString-Address can't be 0!");
            Process.THISCALL<NullReturnCall>((uint)Address, 0x0046B400, new CallValue[] { new IntArg(pos), str });
        }

        public void Add(String str)
        {
            if (Address == 0)
                throw new Exception("The zString-Address can't be 0!");
            //if (memorySave)
            //{
                Set(Value + str);

            //}
            //else
            //{
                //IntPtr charArr = Process.Alloc((uint)str.Length + 1);
                //System.Text.Encoding enc = System.Text.Encoding.Default;
                //byte[] arr = enc.GetBytes(str);
                //if (arr.Length > 0)
                //    Process.Write(arr, charArr.ToInt32());

                //Process.THISCALL<zString>((uint)Address, (uint)0x0067A7B0, new CallValue[] { new IntArg(charArr.ToInt32()) });
                //Process.Free(charArr, (uint)str.Length + 1);
            //}

        }

        public void Set(String str)
        {
            if (Address == 0)
                throw new Exception("The zString-Address can't be 0!");
            //if (memorySave)
            //{
                //IntPtr charArr = Process.Alloc((uint)str.Length + 2);
                //System.Text.Encoding enc = System.Text.Encoding.Default;

                //byte[] arr = enc.GetBytes(str);
                //if (arr.Length > 0)
                //    Process.Write(arr, charArr.ToInt32() + 1);

                ////TODO: PTR wird nicht gelöscht
                //PTR = charArr.ToInt32() + 1;
                //Length = str.Length;
                //Res = str.Length + 1;
            //}
            //else
            //{
            System.Text.Encoding enc = System.Text.Encoding.Default;
            byte[] arr = enc.GetBytes(str);
            IntPtr charArr = Process.Alloc((uint)arr.Length + 1);
            
            
            if (arr.Length > 0)
                Process.Write(arr, charArr.ToInt32());
            Process.Write(new byte[] { 0 }, charArr.ToInt32() + arr.Length);
            Process.THISCALL<zString>((uint)Address, (uint)0x004CFAF0, new CallValue[] { new IntArg(charArr.ToInt32()) });

            Process.Free(charArr, (uint)str.Length + 1);
            //}

        }

        public void Set(zString str)
        {
            if (Address == 0)
                throw new Exception("The zString-Address can't be 0!");

            Process.THISCALL<zString>((uint)Address, (uint)0x0059CEB0, new CallValue[] { str });
        }


        public int VTBL
        {
            get { return Process.ReadInt(this.Address); }
            set { Process.Write(value, this.Address); }
        }

        public int ALLOCATER
        {
            get { return Process.ReadInt(this.Address + 4); }
            set { Process.Write(value, this.Address + 4); }
        }



        public int PTR
        {
            get { return Process.ReadInt(this.Address + 8); }
            set { Process.Write(value, this.Address + 8); }
        }

        public int Length
        {
            get { return Process.ReadInt(this.Address + 12); }
            set { Process.Write(value, this.Address + 12); }
        }

        public int Res
        {
            get { return Process.ReadInt(this.Address + 16); }
            set { Process.Write(value, this.Address + 16); }
        }

        public void CopyTo(int address)
        {
            zString str = new zString(Process, address);
            str.Dispose();

            IntPtr charArr = Process.Alloc((uint)this.Value.Length + 1);
            System.Text.Encoding enc = System.Text.Encoding.Default;
            byte[] arr = enc.GetBytes(this.Value);
            if (arr.Length > 0)
                Process.Write(arr, charArr.ToInt32());

            Process.THISCALL<NullReturnCall>((uint)address, (uint)0x004010C0, new CallValue[] { new IntArg(charArr.ToInt32()) });
            Process.Free(charArr, (uint)this.Value.Length + 1);
        }

        public String Value
        {
            get
            {
                if (Address == 0)
                    throw new Exception("The zString-Address can't be 0!");
                try
                {
                    byte[] arr = Process.ReadBytes(this.PTR, (uint)this.Length);
                    System.Text.Encoding enc = System.Text.Encoding.Default;

                    return enc.GetString(arr);
                }
                catch (Exception ex)
                {
                    //Todo vll anders bearbeiten?
                    return "";
                }
            }
            //set
            //{
            //    System.Text.Encoding enc = System.Text.Encoding.Default;
            //    byte[] arr = enc.GetBytes(value);
            //    byte[] nullB = new byte[Res];

            //    Process.Write(nullB, PTR);
            //    Process.Write(arr, PTR);
            //    Length = arr.Length;
            //}
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
