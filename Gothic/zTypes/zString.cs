using System;
using System.Collections.Generic;
using System.Text;
using WinApi;
using Gothic.zClasses;
using System.Runtime.InteropServices;

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
            Process.THISCALL<NullReturnCall>((uint)Address, 0x0059D010, new CallValue[] { });
        }

        public void Insert(int pos, zString str)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, 0x0046B400, new CallValue[] { new IntArg(pos), str });
        }

        public void Add(String str)
        {
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
            IntPtr charArr = Process.Alloc((uint)str.Length + 1);
            System.Text.Encoding enc = System.Text.Encoding.Default;
            byte[] arr = enc.GetBytes(str);
            if (arr.Length > 0)
                Process.Write(arr, charArr.ToInt32());

            Process.THISCALL<zString>((uint)Address, (uint)0x004CFAF0, new CallValue[] { new IntArg(charArr.ToInt32()) });

            Process.Free(charArr, (uint)str.Length + 1);
            //}

        }

        public void Set(zString str)
        {
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
