using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RakNet;
using GUC.Network;
using GUC.Enumeration;
using GUC.Client.Network.Messages;
using Gothic.zStruct;
using GUC.Client.Hooks;

namespace GUC.Client.WorldObjects
{
    static class Player
    {
        public static uint ID;
        public static NPC Hero = null;
        public static List<AbstractVob> VobControlledList = new List<AbstractVob>();

        public static Item lastUsedWeapon = null;

        public static void DoFists()
        {
            if (Player.Hero.DrawnItem == null)
            {
                NPCMessage.WriteDrawItem(0/*Item.Fists.Slot*/);
            }
            else if (Player.Hero.DrawnItem == Item.Fists)
            {
                NPCMessage.WriteUndrawItem();
            }
        }

        public static Dictionary<uint, Item> Inventory = new Dictionary<uint, Item>();

        public static void ReadVobControlCmd(BitStream stream)
        {
            return;
            Gothic.zClasses.zERROR.GetZErr(Program.Process).Report(2, 'G', "Read Vob Control Cmd", 0, "hModelAni.cs", 0);

            uint ID = stream.mReadUInt();
            AbstractVob vob = VobControlledList.Find(v => v.ID == ID);
            if (vob == null || !(vob is NPC))
                return;

            Gothic.zClasses.zERROR.GetZErr(Program.Process).Report(2, 'G', "CmdVob found", 0, "hModelAni.cs", 0);

            NPC npc = (NPC)vob;


            ControlCmd cmd = (ControlCmd)stream.mReadByte();

            switch (cmd)
            {
                case ControlCmd.GoToPos:
                    break;

                case ControlCmd.GoToVob:
                    Gothic.zClasses.zERROR.GetZErr(Program.Process).Report(2, 'G', "GoToVob", 0, "hModelAni.cs", 0);
                    ID = stream.mReadUInt();
                    AbstractVob target = Player.Hero;
                    if (target != null)
                    {
                        Gothic.zClasses.zERROR.GetZErr(Program.Process).Report(2, 'G', "TargetVob found", 0, "hModelAni.cs", 0);
                        oCMsgMovement msg = oCMsgMovement.Create(Program.Process, oCMsgMovement.SubTypes.GotoVob, target.gVob);
                        npc.gVob.GetEM(0).StartMessage(msg, npc.gVob);
                        Gothic.zClasses.zERROR.GetZErr(Program.Process).Report(2, 'G', "Started Msg", 0, "hModelAni.cs", 0);
                    }
                    break;

                case ControlCmd.Stop:
                    break;
            }
        }
    }
}
