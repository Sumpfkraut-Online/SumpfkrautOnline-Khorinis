using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.zClasses;
using WinApi;

namespace Gothic.zStruct
{
    public class oCMsgWeapon : zCObject, IDisposable
    {
        public oCMsgWeapon()
            : base()
        {

        }

        public oCMsgWeapon(Process process, int address)
            : base(process, address)
        {

        }



        #region statics

        public static oCMsgWeapon Create(Process process, int subType, int arg1, int arg2)
        {
            oCMsgWeapon rVal = null;

            IntPtr address = process.Alloc(0x50);
            zCClassDef.ObjectCreated(process, address.ToInt32(), 0x00AB2CC0);
            
            //Konstruktor...
            process.THISCALL<NullReturnCall>((uint)address.ToInt32(), 0x007667D0, new CallValue[] { (IntArg)subType, (IntArg)arg1, (IntArg)arg2 });
            rVal = new oCMsgWeapon(process, address.ToInt32());


            
            return rVal;
        }
        #endregion



        private bool disposed = false;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!this.disposed)
            {
                if (disposing)
                {
                    Process.Free(new IntPtr(this.Address), 0x50);
                }
                disposed = true;

            }
        }
    }
}
