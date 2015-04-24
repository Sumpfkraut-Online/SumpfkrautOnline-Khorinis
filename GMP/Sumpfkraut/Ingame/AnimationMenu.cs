using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GUC.Sumpfkraut.Ingame.GUI;
using GUC.GUI;

using WinApi;
using WinApi.User.Enumeration;
using GUC.Types;
using GUC.Sumpfkraut.Ingame.GUI;
using GUC.Enumeration;
using GUC.WorldObjects.Character;
using GUC.WorldObjects;
using GUC.Network;
using RakNet;
using Gothic.mClasses;
using Gothic.zClasses;

namespace GUC.Sumpfkraut.Ingame
{
    class AnimationMenu : GUCMInputReceiver
    {
        GUCMenuTexture background;
        List<GUC.GUI.Button> AniButtons = new List<GUC.GUI.Button>();

        bool inputEnabled = false;

        public AnimationMenu()
        {
            if (!Program.client.messageListener.ContainsKey((byte)NetworkID.AnimationMenuMessage))
            {

                Program.client.messageListener.Add((byte)NetworkID.AnimationMenuMessage, new AnimationMenuMessage());
            }

            background = new GUCMenuTexture("Menu_Ingame.tga", 50, 50, 500, 500);
            IngameInput.menus.Add(this);
        }

        public void KeyPressed(int key)
        {
            if (!inputEnabled)
            {
                if (key == (int)VirtualKeys.M) //Open Animation Menu
                {
                    background.Show();
                    inputEnabled = true;
                    //IngameInput.activateFullControl(this);
                }
            }
        }

        public void Update(long ticks)
        {
        }
    }

    class AnimationMenuMessage : IMessage
    {
        public void Read(RakNet.BitStream stream, RakNet.Packet packet, Client client)
        {
        }

        public void StartAnimation(String animation)
        {
            BitStream stream = Program.client.sentBitStream;
            stream.Reset();
            stream.Write((byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.AnimationMenuMessage);
            stream.Write(Player.Hero.ID);
            stream.Write(animation);
            stream.Write(1);
            Program.client.client.Send(stream, PacketPriority.IMMEDIATE_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }
        public void StopAnimation(String animation)
        {
            BitStream stream = Program.client.sentBitStream;
            stream.Reset();
            stream.Write((byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.AnimationMenuMessage);
            stream.Write(Player.Hero.ID);
            stream.Write(animation);
            stream.Write(0);
            Program.client.client.Send(stream, PacketPriority.IMMEDIATE_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }
    }
}
