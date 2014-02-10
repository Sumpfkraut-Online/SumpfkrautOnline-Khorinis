using System;
using System.Collections.Generic;
using System.Text;
using GMP.Net;
using Network;
using Gothic.zClasses;
using WinApi;
using Injection;
using RakNet;

namespace GMP.Network.Messages.update
{
    public class MobInteractDiffMessage
    {
        public void Write(Player pl ){
            if (!pl.isSpawned || pl.NPCAddress == 0)
                return;
            Process process = Process.ThisProcess();
            oCNpc npc = new oCNpc(process, pl.NPCAddress);

            bool sameMobInteract = true;
            oCMobInter mobInter = npc.GetInteractMob();
            if (mobInter == null || mobInter.Address == 0)
            {
                if (pl.mobInteract != null)
                    sameMobInteract = false;
            }
            else
            {
                if (pl.mobInteract == null)
                    sameMobInteract = false;
                else if (pl.mobInteract.name != mobInter.Name.Value.Trim().ToLower() || pl.mobInteract.objectName != mobInter.ObjectName.Value.Trim().ToLower() ||
                    pl.mobInteract.onStateFunc != mobInter.OnStateFunc.Value.Trim().ToLower())
                {
                    sameMobInteract = false;
                }
            }
            

            if (sameMobInteract)
                return;

            

            RakNet.BitStream stream = Program.client.sentBitStream;
            stream.Reset();
            stream.Write((byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetWorkIDS.MobInteractDiffMessage);
            stream.Write(pl.id);

            if (mobInter == null || mobInter.Address == 0)
            {
                pl.mobInteract = null;
                stream.Write(false);
            }
            else
            {
                pl.mobInteract = new MobInteract();
                pl.mobInteract.onStateFunc = mobInter.OnStateFunc.Value.Trim().ToLower();
                pl.mobInteract.modelName = mobInter.GetModel().ObjectName.Value.Trim().ToLower();
                pl.mobInteract.name = mobInter.Name.Value.Trim().ToLower();
                pl.mobInteract.objectName = mobInter.ObjectName.Value.Trim().ToLower();
                pl.mobInteract.pos = mobInter.TrafoObjToWorld.getPosition();

                stream.Write(true);
                stream.Write(pl.mobInteract.objectName);
                stream.Write(pl.mobInteract.name);
                stream.Write(pl.mobInteract.onStateFunc);
                stream.Write(pl.mobInteract.modelName);
                stream.Write(pl.mobInteract.pos[0]);
                stream.Write(pl.mobInteract.pos[1]);
                stream.Write(pl.mobInteract.pos[2]);
            }

            Program.client.client.Send(stream, RakNet.PacketPriority.LOW_PRIORITY, RakNet.PacketReliability.RELIABLE, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }
    }
}
