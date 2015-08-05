using System;
using System.Collections.Generic;
using System.Text;
using WinApi;
using Gothic.zTypes;

namespace Gothic.zClasses
{
    public class oCObjectFactory : zClass
    {
        public oCObjectFactory()
        {

        }

        public oCObjectFactory(Process process, int address)
            : base(process, address)
        {

        }


        public oCCSManager CreateCSManager()
        {
            return Process.THISCALL<oCCSManager>((uint)Address, (uint)0x0076FB80, new CallValue[] { });
        }

        public oCItem CreateItem(int id)
        {
            return Process.THISCALL<oCItem>((uint)Address, (uint)0x0076FDE0, new CallValue[] { new IntArg(id) });
        }

        public oCItem CreateItem(String instance)
        {
            zString str = zString.Create(Process, instance);
            int id = zCParser.getParser(Process).GetIndex(str);
            str.Dispose();

            return CreateItem(id);
        }

        public oCNpc CreateNPC(string instance)
        {
            zString str = zString.Create(Process, instance);
            oCNpc npc = null;
            npc = CreateNPC(zCParser.getParser(Process).GetIndex(str));
            str.Dispose();

            return npc;
        }

        public oCNpc CreateNPC(int id)
        {
            return Process.THISCALL<oCNpc>((uint)Address, (uint)0x0076FD20, new CallValue[] { new IntArg(id) });
        }

        public zCWorld CreateWorld()
        {
            return Process.THISCALL<zCWorld>((uint)Address, (uint)0x0076FCA0, new CallValue[] { });
        }

        public zCWay CreateWay()
        {
            return Process.THISCALL<zCWay>((uint)Address, (uint)0x0076FF00, new CallValue[] { });
        }

        public zCWaypoint CreateWaypoint()
        {
            return Process.THISCALL<zCWaypoint>((uint)Address, (uint)0x0076FEA0, new CallValue[] { });
        }

        public zCPlayerInfo CreatePlayerInfo()
        {
            return Process.THISCALL<zCPlayerInfo>((uint)Address, (uint)0x0076FC40, new CallValue[] { });
        }



        public override uint ValueLength()
        {
            return 4;
        }

        public static oCObjectFactory GetFactory(Process process)
        {
            return new oCObjectFactory(process, process.ReadInt(0x008D8DF0));
        }
    }
}
