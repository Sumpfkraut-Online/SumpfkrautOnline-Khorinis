using System;
using System.Collections.Generic;
using System.Text;
using WinApi;

namespace Gothic.zClasses
{
    public class oCNpcTalent : zClass
    {
        public enum Offsets
        {
            talent = 0x0,
            skill = 0x4,
            value = 0x8
        }

        public oCNpcTalent(Process process, int address)
            : base(process, address)
        { }

        public oCNpcTalent()
        {

        }

        public int Talent
        {
            get { return Process.ReadInt(Address + (int)Offsets.talent); }
            set { Process.Write(value, Address + (int)Offsets.talent); }
        }

        public int Skill
        {
            get { return Process.ReadInt(Address + (int)Offsets.skill); }
            set { Process.Write(value, Address + (int)Offsets.skill); }
        }

        public int Value
        {
            get { return Process.ReadInt(Address + (int)Offsets.value); }
            set { Process.Write(value, Address + (int)Offsets.value); }
        }

        public override int SizeOf()
        {
            return 12;
        }
    }
}
