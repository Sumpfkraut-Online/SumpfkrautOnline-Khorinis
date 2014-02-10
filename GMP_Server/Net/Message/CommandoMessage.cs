using System;
using System.Collections.Generic;
using System.Text;
using Network;
using RakNet;

namespace GMP_Server.Net.Message
{
    public class CommandoMessage : Message
    {
        public override void Read(RakNet.BitStream stream, RakNet.Packet packet, Server server)
        {
            byte type = 0; byte commandoFlag = 0; byte argFlag = 0;
            int playerid = -1;
            String[] arguments1 = null; int[] arguments2 = null; float[] arguments3 = null;


            stream.Read(out type);
            stream.Read(out commandoFlag);
            if (commandoFlag == (int)CommandoFlags.sentToPlayer)
                stream.Read(out playerid);
            stream.Read(out argFlag);
            if ((argFlag & (byte)CommandoArgumentsFlags.Arg1) == (byte)CommandoArgumentsFlags.Arg1)
            {
                int argLength = 0;
                stream.Read(out argLength);
                arguments1 = new string[argLength];
                for (int i = 0; i < argLength; i++ )
                    stream.Read(out arguments1[i]);
            }
            if ((argFlag & (byte)CommandoArgumentsFlags.Arg2) == (byte)CommandoArgumentsFlags.Arg2)
            {
                int argLength = 0;
                stream.Read(out argLength);
                arguments2 = new int[argLength];
                for (int i = 0; i < argLength; i++)
                    stream.Read(out arguments2[i]);
            }
            if ((argFlag & (byte)CommandoArgumentsFlags.Arg3) == (byte)CommandoArgumentsFlags.Arg3)
            {
                int argLength = 0;
                stream.Read(out argLength);
                arguments3 = new float[argLength];
                for (int i = 0; i < argLength; i++)
                    stream.Read(out arguments3[i]);
            }



            //Daten bearbeiten

            Player pl = null;
            if (commandoFlag == (int)CommandoFlags.sentToPlayer)
            {
                pl = Player.getPlayerSort(playerid, Program.playerList);

                if (pl.isNPC)
                {
                    //TODO: Controller finden
                }
            }
            if (type == (byte)CommandoType.GiveItems)
            {
                Player Otherpl = Player.getPlayerSort(arguments2[0], Program.playerList);
                if (Otherpl != null)
                {
                    Otherpl.InsertItem(new item() { code = arguments1[0], Amount = arguments2[1] });
                    Console.WriteLine("Commando: GiveItem Player" + Otherpl.name + " item:" + arguments1[0] + " " + arguments2[1]);
                }
                
            }

            if (type == (byte)CommandoType.SetInventory)
            {
                Player pl2 = Player.getPlayerSort(arguments2[arguments2.Length - 1], Program.playerList);
                if (pl2 == null)
                    return;
                pl2.itemList.Clear();
                for (int i = 0; i < arguments1.Length; i++)
                {
                    pl2.InsertItem(new item() { code = arguments1[0], Amount = arguments2[1] }); 
                }
            }


            if (type == (byte)CommandoType.RemoveNPC)
            {
                Player pl2 = Player.getPlayerSort(arguments2[0], Program.playerList);
                NPC npc = pl2.NPC;
                if (Program.playerDict.ContainsKey(npc.npcPlayer.id))
                    Program.playerDict.Remove(npc.npcPlayer.id);
                if (Program.playerList.Contains(npc.npcPlayer))
                    Program.playerList.Remove(npc.npcPlayer);
                if (Program.npcList.Contains(npc))
                    Program.npcList.Remove(npc);
            }





            //weiter senden
            stream.Reset();
            stream.Write((byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetWorkIDS.CommandoMessage);
            stream.Write((byte)type);

            if (commandoFlag == (int)CommandoFlags.sentToAll)
            {
                stream.Write(commandoFlag);
            }
            else
            {
                stream.Write(commandoFlag);
                stream.Write(playerid);
            }

            if ((arguments1 != null && arguments1.Length != 0))
                argFlag |= (byte)CommandoArgumentsFlags.Arg1;
            if ((arguments2 != null && arguments2.Length != 0))
                argFlag |= (byte)CommandoArgumentsFlags.Arg2;
            if ((arguments3 != null && arguments3.Length != 0))
                argFlag |= (byte)CommandoArgumentsFlags.Arg3;

            stream.Write((byte)argFlag);
            if ((argFlag & (byte)CommandoArgumentsFlags.Arg1) == (byte)CommandoArgumentsFlags.Arg1)
            {
                stream.Write(arguments1.Length);
                foreach (String str in arguments1)
                    stream.Write(str);
            }
            if ((argFlag & (byte)CommandoArgumentsFlags.Arg2) == (byte)CommandoArgumentsFlags.Arg2)
            {
                stream.Write(arguments2.Length);
                foreach (int str in arguments2)
                    stream.Write(str);
            }
            if ((argFlag & (byte)CommandoArgumentsFlags.Arg3) == (byte)CommandoArgumentsFlags.Arg3)
            {
                stream.Write(arguments3.Length);
                foreach (float str in arguments3)
                    stream.Write(str);
            }
            if (commandoFlag == (int)CommandoFlags.sentToAll)
                server.server.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
            else
                server.server.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, pl.guid, false);
        }

        public void Write(RakNet.BitStream stream, Server server, int playerid, byte type, String[] arguments1, int[] arguments2, float[] arguments3)
        {
            Write(stream, server, playerid, type, arguments1, arguments2, arguments3, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED);
        }


        public void Write(RakNet.BitStream stream, Server server,  int playerid, byte type, String[] arguments1, int[] arguments2, float[] arguments3, PacketPriority prior, PacketReliability rel)
        {
            if (Program.playList.Count == 0)
                return;

            Player pl = null;

            byte commandoFlag = (byte)CommandoFlags.sentToPlayer;


            if (playerid < 0)
                commandoFlag = (byte)CommandoFlags.sentToAll;


            stream.Reset();
            stream.Write((byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetWorkIDS.CommandoMessage);
            stream.Write((byte)type);

            if (commandoFlag == (int)CommandoFlags.sentToAll)
            {
                stream.Write(commandoFlag);
            }
            else
            {
                stream.Write(commandoFlag);
                stream.Write(playerid);
                pl = Player.getPlayerSort(playerid, Program.playerList);
            }

            CommandoArgumentsFlags argFlag = CommandoArgumentsFlags.None;
            if ((arguments1 != null && arguments1.Length != 0))
                argFlag |= CommandoArgumentsFlags.Arg1;
            if ((arguments2 != null && arguments2.Length != 0))
                argFlag |= CommandoArgumentsFlags.Arg2;
            if ((arguments3 != null && arguments3.Length != 0))
                argFlag |= CommandoArgumentsFlags.Arg3;

            stream.Write((byte)argFlag);
            if ((argFlag & CommandoArgumentsFlags.Arg1) == CommandoArgumentsFlags.Arg1)
            {
                stream.Write(arguments1.Length);
                foreach (String str in arguments1)
                    stream.Write(str);
            }
            if ((argFlag & CommandoArgumentsFlags.Arg2) == CommandoArgumentsFlags.Arg2)
            {
                stream.Write(arguments2.Length);
                foreach (int str in arguments2)
                    stream.Write(str);
            }
            if ((argFlag & CommandoArgumentsFlags.Arg3) == CommandoArgumentsFlags.Arg3)
            {
                stream.Write(arguments3.Length);
                foreach (float str in arguments3)
                    stream.Write(str);
            }
            if (commandoFlag == (int)CommandoFlags.sentToAll)
                server.server.Send(stream, prior, rel, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
            else
                server.server.Send(stream, prior, rel, (char)0, pl.guid, false);
        }


        public static void RemoveNPC(NPC npc)
        {
            if (Program.playerDict.ContainsKey(npc.npcPlayer.id))
                Program.playerDict.Remove(npc.npcPlayer.id);
            if (Program.playerList.Contains(npc.npcPlayer))
                Program.playerList.Remove(npc.npcPlayer);
            if (Program.npcList.Contains(npc))
                Program.npcList.Remove(npc);
            new CommandoMessage().Write(Program.server.receiveBitStream, Program.server, -1, (byte)CommandoType.RemoveNPC, null, new int[] { npc.npcPlayer.id }, null);
        }
    }
}
