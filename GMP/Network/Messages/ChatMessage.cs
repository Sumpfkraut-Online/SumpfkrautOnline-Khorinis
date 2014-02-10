using System;
using System.Collections.Generic;
using System.Text;
using Gothic.mClasses;
using RakNet;
using Network;
using Injection;
using GMP.Modules;
using GMP.Helper;
using Gothic.zClasses;
using WinApi;

namespace GMP.Net.Messages
{
    public class ChatMessage : Message
    {
        Chatbox pChat;
        public ChatMessage(Chatbox chat)
        {
            pChat = chat;
        }



        public override void Read(RakNet.BitStream stream, RakNet.Packet packet, Client client)
        {
            byte type;
            int id;
            String name;
            String content;

            stream.Read(out id);
            stream.Read(out type);
            stream.Read(out name);
            BitStreamFunc.ReadNames(stream, out content);
            if (type == 2)
            {
                Player player = Player.getPlayer(id, StaticVars.playerlist);

                float[] distVec = new float[]{player.pos[0] - Program.Player.pos[0],player.pos[1] - Program.Player.pos[1],player.pos[2] - Program.Player.pos[2]};
                float distanceToPlayer = NPCHelper.GetDistance(Program.Player, player);
                
                if (Player.isSameMap(player.actualMap, Program.Player.actualMap)
                    && distanceToPlayer <= StaticVars.serverConfig.chatOptions.DistanceRange)
                {
                    if (StaticVars.serverConfig.HideNames)
                    {
                        foreach (Player pl in StaticVars.playerlist)
                        {
                            if (!pl.isSpawned || pl.NPCAddress == 0 || pl.knowName)
                                continue;
                            if(content.ToLower().Contains(pl.name.ToLower().Trim()))
                            {
                                //Spieler gefunden... Name freischalten?
                                //Spieler hat sich selbst benannt, also automatisch...
                                if (pl.name.Trim().ToLower() == name.ToLower().Trim())
                                {
                                    pl.knowName = true;
                                    NPCHelper.SetStandards(pl);
                                    continue;
                                }

                                //Wenn Der Frager (player) ich bin und der mit dem namen (pl) in reichweite...
                                Process process = Process.ThisProcess();
                                oCNpc hero = new oCNpc(process, Program.Player.NPCAddress);
                                oCNpc senderPlayerNPC = new oCNpc(process, player.NPCAddress);
                                oCNpc otherPlayerNPC = new oCNpc(process, pl.NPCAddress);
                                if(player.id == Program.Player.id)
                                {
                                    
                                    if ((distanceToPlayer == 50 && hero.CanSee(otherPlayerNPC, 0) == 1) || 
                                        hero.Enemy.Address == otherPlayerNPC.Address)
                                    {
                                        pl.knowName = true;
                                        NPCHelper.SetStandards(pl);
                                        continue;
                                    }
                                }

                                //Wenn sowohl Frager wie auch Person mit Name im blickwinkel sind...
                                if (hero.CanSee(senderPlayerNPC, 0) == 1 && hero.CanSee(otherPlayerNPC, 0) == 1)
                                {
                                    pl.knowName = true;
                                    NPCHelper.SetStandards(pl);
                                    continue;
                                }

                            }
                        }
                    }
                    if (!StaticVars.serverConfig.HideNames || player.knowName)
                        pChat.addRow(type, name + ": " + content);
                    else
                        pChat.addRow(type, "Unbekannter: " +content);
                }

                
            }
            else if (type == 3)
            {
                Player player = Player.getPlayer(id, StaticVars.playerlist);
                if(player.isFriend == 1)
                    pChat.addRow(type, name + ": " + content);
            }
            else if (type == 7)
            {
                pChat.addRow(type, content);
            }
            else
            {
                pChat.addRow(type, name + ": " + content);
            }
        }

        public void Write(RakNet.BitStream stream, Client client, byte type, string content)
        {
            stream.Reset();
            stream.Write((byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetWorkIDS.ChatMessage);
            stream.Write(Program.Player.id);
            stream.Write((byte)type);
            stream.Write(Program.Player.name);


            BitStreamFunc.WriteNames(stream, content);

            client.client.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
            if (!StaticVars.sStats.ContainsKey((int)NetWorkIDS.ChatMessage))
                StaticVars.sStats[(int)NetWorkIDS.ChatMessage] = 0;
            StaticVars.sStats[(int)NetWorkIDS.ChatMessage] += 1;
        }

        
    }
}
