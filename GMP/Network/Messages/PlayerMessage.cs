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
        public static void ReadControl(PacketReader stream)
        {
            Player.ID = stream.ReadUInt();
            string newMap = stream.ReadString();

            Menus.GUCMenus.CloseActiveMenus();
            Menus.GUCMenus._Background.Hide();

            if (World.MapName == null)
            {
                // the map which is used for NEW_GAME
                Program.Process.Write(System.Text.Encoding.UTF8.GetBytes(newMap), 0x008907B0);
                Program.Process.Write(new byte[] { 0 }, 0x008907B0 + newMap.Length);

                zCMenu activeGothicMenu = zCMenu.GetActive(Program.Process);
                using (zString z = zString.Create(Program.Process, "RESUME_GAME"))
                {
                    activeGothicMenu.HandleSelAction(4, z, new zCMenuItem()); //SEL_ACTION_CLOSE, menuAction = NEW_GAME
                }
                Program.Process.Write(1, activeGothicMenu.Address + 0xC24); //escape zCMenu::Run() loop        
            }

            if (World.MapName != newMap)
            {
                World.ChangeLevel(newMap);
            }

            WriteControl();

            if (!(Program._state is GameState))
            {
                Program._state = new GameState();
            }
        }

        private static void WriteControl() //for confirmation
        {
            PacketWriter stream = Program.client.SetupSendStream(NetworkID.PlayerControlMessage);
            Program.client.SendStream(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED);
        }

        public static void WritePickUpItem(Vob vob)
        {
            PacketWriter stream = Program.client.SetupSendStream(NetworkID.PlayerPickUpItemMessage);
            stream.Write(vob.ID);
            Program.client.SendStream(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.UNRELIABLE);
        }
    }
}
