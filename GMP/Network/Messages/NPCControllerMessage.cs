using System;
using System.Collections.Generic;
using System.Text;
using GMP.Net;
using Network;
using GMP.Modules;
using Injection;
using Gothic.zClasses;
using Gothic.zTypes;
using WinApi;
using RakNet;

namespace GMP.Network.Messages
{
    public class NPCControllerMessage : Message
    {
        public override void Read(RakNet.BitStream stream, RakNet.Packet packet, Client client)
        {
            int npcid;
            bool enabled;
            stream.Read(out npcid);
            stream.Read(out enabled);

            NPC npcafter = null;
            try
            {
                foreach (NPC npc in StaticVars.npcList)
                {
                    if (npc.npcPlayer.id == npcid)
                    {
                        npcafter = npc;
                        if (enabled)
                        {
                            npc.controller = Program.Player;
                            StaticVars.npcControlList.Add(npc);
                            Program.Player.NPCList.Add(npc);


                            Process process = Process.ThisProcess();
                            oCNpc n = new oCNpc(process, npc.npcPlayer.NPCAddress);
                            oCRtnManager.GetRtnManager(process).UpdateSingleRoutine(n);

                            //TODO: Problematisch, da hp resettet wird...
                            //zVec3 pos = n.GetPosition();
                            //int hp = n.HP;
                            //n.ResetPos(pos);
                            //n.HP = hp;
                            //pos.Dispose();

                            //n.NpcStates.StartRtnState(1);
                            //n.NpcStates.ActivateRtnState(1);
                            //TimeMessage.firstTimeUpdate = true;
                            //oCRtnManager.GetRtnManager(process).SetDailyRoutinePos(1);
                        }
                        else
                        {
                            npc.controller = null;
                            StaticVars.npcControlList.Remove(npc);
                            Program.Player.NPCList.Remove(npc);
                        }
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                if (npcafter != null)
                    npcafter.controller = null;
                npcafter = null;
            }
            if (npcafter == null && enabled)
            {
                stream.Reset();
                stream.Write((byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
                stream.Write((byte)NetWorkIDS.NPCControllerMessage);
                stream.Write(npcid);
                stream.Write(enabled);



                client.client.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);

                if (!StaticVars.sStats.ContainsKey((int)NetWorkIDS.NPCControllerMessage))
                    StaticVars.sStats[(int)NetWorkIDS.NPCControllerMessage] = 0;
                StaticVars.sStats[(int)NetWorkIDS.NPCControllerMessage] += 1;
            }
             
        }
    }
}
