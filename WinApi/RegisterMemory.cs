using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinApi
{
    public struct RegisterMemory
    {
        int address;
        public int Address { get { return this.address; } }
        Hook hook;
        public Hook Hook { get { return this.hook; } }

        internal RegisterMemory(int stackAddr, Hook hook)
        {
            this.address = stackAddr;
            this.hook = hook;
        }

        /// <summary>
        /// Gets or sets the Registers.
        /// </summary>
        public int this[Registers i]
        {
            get
            {
                if (!Enum.IsDefined(typeof(Registers), i))
                    throw new ArgumentException("Read: Undefined register! " + i);
                return Process.ReadInt(address + 2 + 4 * (int)i);
            }
            set
            {
                if (!Enum.IsDefined(typeof(Registers), i))
                    throw new ArgumentException("Write: Undefined register! " + i);

                Process.Write(address + 2 + 4 * (int)i, value);
            }
        }

        /// <summary>
        /// Gets or sets the Arguments. Writes directly into the stack!
        /// </summary>
        public int this[int i]
        {
            get
            {
                if (i < 0)
                    throw new ArgumentOutOfRangeException("Argument can't be smaller than zero!");

                return Process.ReadInt(address + 38 + i * 4);
            }
            set
            {
                if (i < 0)
                    throw new ArgumentOutOfRangeException("Argument can't be smaller than zero!");

                Process.Write(address + 38 + i * 4, value);
            }
        }


        /// <summary>
        /// Gets or sets the RegisterFlags
        /// </summary>
        public bool this[FlagRegisters i]
        {
            get
            {
                if (!Enum.IsDefined(typeof(FlagRegisters), i))
                    throw new ArgumentException("Read: Undefined register flag! " + i);

                throw new NotImplementedException();

                /*byte[] arr = new byte[2];
                Process.Read(address, arr, 2);
                short flags = (short)(arr[1] | (arr[0] << 8));

                return (flags & (1 << (int)i)) != 0;*/
            }
            set
            {
                if (!Enum.IsDefined(typeof(FlagRegisters), i))
                    throw new ArgumentException("Write: Undefined register flag! " + i);

                throw new NotImplementedException();

                /*byte[] arr = new byte[2];
                Process.Read(address, arr, 2);
                short flags = (short)(arr[1] | (arr[0] << 8));

                if (value)
                {
                    flags |= (short)(1 << (int)i);
                }
                else
                {
                    flags &= (short)~(1 << (int)i);
                }
                Process.Write(address, flags);*/
            }
        }
    }
}
