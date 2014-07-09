﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.WorldObjects;
using GUC.WorldObjects.Character;
using GUC.WorldObjects.Mobs;
using Gothic.zClasses;
using WinApi;

namespace GUC.Network.Messages.MobInterCommands
{
    class MobInterMessage : IMessage
    {
        public void Read(RakNet.BitStream stream, RakNet.Packet packet, Client client)
        {
            int vobID = 0, playerID = 0;
            byte mobInterTypeInt = 0;
            char mobInterKey = '0';

            stream.Read(out mobInterTypeInt);
            stream.Read(out playerID);
            stream.Read(out vobID);

            MobInterNetworkFlags mobInterFlags = (MobInterNetworkFlags)mobInterTypeInt;

            if (mobInterFlags.HasFlag(MobInterNetworkFlags.PickLock))
                stream.Read(out mobInterKey);


            if (playerID == 0 || !sWorld.VobDict.ContainsKey(playerID))
                throw new Exception("Player not found!");
            Vob plVob = sWorld.VobDict[playerID];
            if (!(plVob is NPCProto))
                throw new Exception("PlayerVob was not from type Player: " + plVob);
            NPCProto player = (NPCProto)plVob;

            if (vobID == 0 || !sWorld.VobDict.ContainsKey(vobID))
                throw new Exception("Vob not found!");
            Vob vob = sWorld.VobDict[vobID];
            if (!(vob is MobInter))
                throw new Exception("Vob was not from type MobInter: " + vob);


            MobInter mob = (MobInter)vob;
            Process process = Process.ThisProcess();

            if (mobInterFlags == MobInterNetworkFlags.PickLock)
            {
                if (!(vob is MobLockable))
                    throw new Exception("Vob was not from type MobLockable: " + vob);
                
            }
            else if (mobInterFlags == MobInterNetworkFlags.OnTrigger)
            {
                mob.State = 1;
                if (mob.Address != 0)
                {
                    oCMobInter mI = new oCMobInter(process, mob.Address);

                    mI.GetModel().StartAnimation("T_S0_2_S1");

                    mI.OnTrigger(new zCVob(process, 0), new zCVob(process, player.Address));
                    mI.State = 1;
                    
                    
                    //mI.StateAniID = mI.GetModel().GetAniIDFromAniName("S_S1");


                }
            }
            else if (mobInterFlags == MobInterNetworkFlags.OnUnTrigger)
            {
                mob.State = 0;
                if (mob.Address != 0)
                {
                    oCMobInter mI = new oCMobInter(process, mob.Address);
                    mI.GetModel().StartAnimation("T_S1_2_S0");
                    mI.OnUnTrigger(new zCVob(process, 0), new zCVob(process, player.Address));
                    mI.State = 0;
                    
                    
                    //mI.StateAniID = mI.GetModel().GetAniIDFromAniName("S_S0");
                    //mI.StateAniID = mI.GetModel().
                }
            }
            else if (mobInterFlags == MobInterNetworkFlags.StartInteraction)
            {
                if (mob.Address != 0)
                {
                    new oCMobInter(process, mob.Address).StartInteraction(new oCNpc(process, player.Address));
                }
            }
            else if (mobInterFlags == MobInterNetworkFlags.StopInteraction)
            {
                if (mob.Address != 0)
                {
                    new oCMobInter(process, mob.Address).StopInteraction(new oCNpc(process, player.Address));
                }
            }
        }
    }
}
