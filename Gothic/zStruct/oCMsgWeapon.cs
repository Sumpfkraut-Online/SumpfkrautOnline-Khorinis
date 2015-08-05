using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.zClasses;
using WinApi;

namespace Gothic.zStruct
{
    public class oCMsgWeapon : oCNpcMessage, IDisposable
    {
        public enum SubTypes
        {
            DrawWeapon,
            DrawWeapon1,
            DrawWeapon2,

            RemoveWeapon,
            RemoveWeapon1,
            RemoveWeapon2,

            ChooseWeapon,
            ForceRemoveWeapon,
            Attack, //wat
            EquipBestWeapon,
            EquipBestArmor,
            UnequipWeapons,
            UnEquipArmor,//guessed
            EquipArmor
        }

        public oCMsgWeapon()
            : base()
        {

        }

        public oCMsgWeapon(Process process, int address)
            : base(process, address)
        {

        }


        public SubTypes SubType
        {
            get { return (SubTypes)Process.ReadUShort(Address + 0x24); }
        }

        public int WpType
        {
            get { return Process.ReadUShort(Address + 0x44); }
        }

        #region statics

        public static oCMsgWeapon Create(Process process, SubTypes subType, int arg1, int arg2)
        {
            int address = process.CDECLCALL<IntArg>(0x7636E0, null);
            
            //Konstruktor...
            process.THISCALL<NullReturnCall>((uint)address, 0x007667D0, new CallValue[] { (IntArg)(int)subType, (IntArg)arg1, (IntArg)arg2 });

            return new oCMsgWeapon(process, address);
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
