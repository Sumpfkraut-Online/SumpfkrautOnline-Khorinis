using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RakNet;
using GUC.Enumeration;
using Gothic.zClasses;
using WinApi;
using GUC.Client.States;
using GUC.Network;

namespace GUC.Client.Network.Messages
{
    static class AccountMessage
    {
        public delegate void OnLoginHandler(AccCharInfo[] chars);
        public static OnLoginHandler OnLogin;

        public static void Login()
        {
            BitStream stream = Program.client.SetupSendStream(NetworkID.AccountLoginMessage);
            stream.mWrite(StartupState.clientOptions.name);
            stream.mWrite(StartupState.clientOptions.password);
            Program.client.SendStream(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE);
        }

        public static void Register()
        {
            BitStream stream = Program.client.SetupSendStream(NetworkID.AccountCreationMessage);
            stream.mWrite(StartupState.clientOptions.name);
            stream.mWrite(StartupState.clientOptions.password);
            Program.client.SendStream(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE);
        }

        public static void CreateNewCharacter(AccCharInfo ci)
        {
            BitStream stream = Program.client.SetupSendStream(NetworkID.AccountCharCreationMessage);
            ci.Write(stream);
            Program.client.SendStream(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE);
        }

        public static void StartInWorld(int slotNum)
        {
            if (slotNum >= 0 && slotNum < AccCharInfo.Max_Slots)
            {
                BitStream stream = Program.client.SetupSendStream(NetworkID.AccountCharLoginMessage);
                stream.mWrite((byte)slotNum);
                Program.client.SendStream(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE);
            }
        }

        public static void Error(BitStream stream)
        {
            string message = stream.mReadString();
            if (Menus.GUCMenus._ActiveMenus.Count > 0 && Menus.GUCMenus._ActiveMenus[0] is Menus.MainMenus.GUCMainMenu)
            {
                ((Menus.MainMenus.GUCMainMenu)Menus.GUCMenus._ActiveMenus[0]).SetHelpText(message);
            }
            else
            {
                //print on screen?
            }
        }

        public static void GetCharList(BitStream stream)
        {
            byte count = stream.mReadByte();

            AccCharInfo[] chars = new AccCharInfo[count];
            for (int i = 0; i < count; i++)
            {
                chars[i] = new AccCharInfo();
                chars[i].Read(stream);
            }
            
            if (OnLogin != null)
            {
                OnLogin(chars);
            }
        }
    }
}
