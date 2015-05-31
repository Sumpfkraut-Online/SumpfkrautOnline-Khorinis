using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Character;
using GUC.WorldObjects;
using Gothic.zClasses;
using WinApi;
using Gothic.zTypes;
using RakNet;
using GUC.Enumeration;
using GUC.States;
using GUC.Types;

namespace GUC.Network.Messages.Connection
{
    class NPCSpawnMessage : IMessage
    {
        public void Read(RakNet.BitStream stream, RakNet.Packet packet, Client client)
        {
            

            int plID;
            String levelName;
            Vec3f pos, dir;
            stream.Read(out plID);
            stream.Read(out levelName);
            stream.Read(out pos);
            stream.Read(out dir);

            if (plID == 0 || !sWorld.VobDict.ContainsKey(plID))
                throw new Exception("Vob not found!");
            Vob vob = sWorld.VobDict[plID];
            if (!(vob is NPCProto))
                throw new Exception("Vob is not an NPC!");

            NPCProto playerVob = (NPCProto)vob;

            

            Process process = Process.ThisProcess();
            oCNpc npc = new oCNpc(process, vob.Address);
            if (vob is Player && vob == Player.Hero)
            {
                if (Sumpfkraut.Menus.MainMenus._Background != null)
                    Sumpfkraut.Menus.MainMenus._Background.Hide();
                if (Sumpfkraut.Menus.MainMenus.ActiveMenu != null)
                    Sumpfkraut.Menus.MainMenus.ActiveMenu.Close();

                ((Player)vob).spawned();
                playerVob.Map = levelName;
                zString level = zString.Create(process, playerVob.Map);
                oCGame.Game(process).ChangeLevel(level, level);
                level.Dispose();
                                
                vob.setDirection(dir);
                vob.setPosition(pos);
                Program._state = new GameState();
            }
        }


        public static void Write()
        {
            sWorld.getWorld(Player.Hero.Map).addVob(Player.Hero);
            sWorld.SpawnedVobDict.Add(Player.Hero.Address, Player.Hero);

            BitStream stream = Program.client.sentBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.NPCSpawnMessage);
            stream.Write(Player.Hero.ID);
            Program.client.client.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }
    }
}
