using System;
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

            short startChangeState0 = 0, startChangeState1 = 1;

            stream.Read(out mobInterTypeInt);
            stream.Read(out playerID);
            stream.Read(out vobID);

            MobInterNetwork mobInterFlags = (MobInterNetwork)mobInterTypeInt;

            if (mobInterFlags.HasFlag(MobInterNetwork.PickLock))
                stream.Read(out mobInterKey);

            if (mobInterFlags.HasFlag(MobInterNetwork.StartStateChange)){
                stream.Read(out startChangeState0);
                stream.Read(out startChangeState1);
            }

            if (playerID == 0 || !sWorld.VobDict.ContainsKey(playerID))
                throw new Exception("Player not found!");
            Vob plVob = sWorld.VobDict[playerID];
            if (!(plVob is NPC))
                throw new Exception("PlayerVob was not from type Player: " + plVob);
            NPC player = (NPC)plVob;

            if (vobID == 0 || !sWorld.VobDict.ContainsKey(vobID))
                throw new Exception("Vob not found!");
            Vob vob = sWorld.VobDict[vobID];
            if (!(vob is MobInter))
                throw new Exception("Vob was not from type MobInter: " + vob);


            MobInter mob = (MobInter)vob;
            Process process = Process.ThisProcess();

            if (mobInterFlags == MobInterNetwork.PickLock)
            {
                if (!(vob is MobLockable))
                    throw new Exception("Vob was not from type MobLockable: " + vob);
                
            }
            else if (mobInterFlags == MobInterNetwork.OnTrigger)
            {
                mob.State = 1;
                if (mob.Address != 0)
                {
                    oCMobInter mI = new oCMobInter(process, mob.Address);

                    //mI.GetModel().StartAnimation("T_S0_2_S1");

                    mI.OnTrigger(new zCVob(process, mI.Address), new zCVob(process, player.Address));
                    //mI.State = 1;
                    
                    
                    //mI.StateAniID = mI.GetModel().GetAniIDFromAniName("S_S1");


                }
            }
            else if (mobInterFlags == MobInterNetwork.OnUnTrigger)
            {
                mob.State = 0;
                if (mob.Address != 0)
                {
                    oCMobInter mI = new oCMobInter(process, mob.Address);
                    //mI.GetModel().StartAnimation("T_S1_2_S0");
                    mI.OnUnTrigger(new zCVob(process, mI.Address), new zCVob(process, player.Address));
                    //mI.State = 0;
                    

                    if (mob is MobLockable && ( ((MobLockable)mob).KeyInstance != null ||  ( ((MobLockable)mob).PickLockStr != null && ((MobLockable)mob).PickLockStr.Length != 0)) )
                    {
                        oCMobLockable ML = new oCMobLockable(process, mob.Address);
                        ML.SetLocked(1);
                    }
                    
                    //mI.StateAniID = mI.GetModel().GetAniIDFromAniName("S_S0");
                    //mI.StateAniID = mI.GetModel().
                }
            }
            else if (mobInterFlags == MobInterNetwork.StartInteraction)
            {
                if (mob.Address != 0)
                {
                    new oCMobInter(process, mob.Address).StartInteraction(new oCNpc(process, player.Address));
                }
            }
            else if (mobInterFlags == MobInterNetwork.StopInteraction)
            {
                if (mob.Address != 0)
                {
                    new oCMobInter(process, mob.Address).StopInteraction(new oCNpc(process, player.Address));
                }
            }
            else if (mobInterFlags == MobInterNetwork.StartStateChange)
            {
                if (mob.Address != 0)
                {
                    oCMobInter mI = new oCMobInter(process, mob.Address);
                    mI.StartStateChange(new oCNpc(process, player.Address), startChangeState0, startChangeState1);
                }
            }
        }
    }
}
