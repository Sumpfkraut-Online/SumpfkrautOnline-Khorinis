using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RakNet;
using GUC.Enumeration;
using GUC.Client.WorldObjects;
using GUC.Client.States;
using GUC.Network;
using GUC.Types;
using Gothic.zClasses;
using Gothic.zTypes;

namespace GUC.Client.Network.Messages
{
    static class PlayerMessage
    {
        public static void ReadControl(BitStream stream)
        {
            uint id = stream.mReadUInt();
            string name = "Ich";
            string newMap = stream.mReadString();
            Vec3f pos = stream.mReadVec();
            Vec3f dir = stream.mReadVec();

            Menus.GUCMenus.CloseActiveMenus();
            Menus.GUCMenus._Background.Hide();

            if (World.MapName != newMap)
            {
                World.ChangeLevel(newMap);
            }
            
            if (Player.Hero == null)
            {
                //we have not been ingame yet
                Player.Hero = new NPC(id, oCNpc.Player(Program.Process));
                Player.Hero.Name = name;
                Player.Hero.Spawn(pos, dir);
            }
            else
            {
                /*if (id == Player.Hero.ID)
                    return; //nothing to do here...

                if (!World.AllVobs.ContainsKey(id))
                { //no information about this NPC
                    Player.Hero = new NPC(id);
                    Player.Hero.Name = name;
                    Player.Hero.Spawn(pos, dir);
                }
                Program.Process.Write(Player.Hero.gVob.Address, 0xAB2684);*/
            }

            WriteControl();

            if (!(Program._state is GameState))
            {
                Program._state = new GameState();
            }
        }

        private static void WriteControl() //for confirmation
        {
            BitStream stream = Program.client.SetupSendStream(NetworkID.PlayerControlMessage);
            Program.client.SendStream(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED);
        }

        public static void WritePickUpItem(Vob vob)
        {
            BitStream stream = Program.client.SetupSendStream(NetworkID.PlayerPickUpItemMessage);
            stream.mWrite(vob.ID);
            Program.client.SendStream(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.UNRELIABLE);
        }
    }
}
