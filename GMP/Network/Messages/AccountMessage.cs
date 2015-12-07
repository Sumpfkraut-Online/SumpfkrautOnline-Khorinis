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
        // if you change these, change them in GUC.Server.Scripts.Accounts.AccountSystem too!!!
        enum LoginType
        {
            AccountLogin,
            AccountCreation,
            CharacterLogin,
            CharacterCreation
        }

        public delegate void OnLoginHandler(AccCharInfo[] chars);
        public static OnLoginHandler OnLogin;

        public static void Login()
        {
            PacketWriter stream = Program.client.SetupSendStream(NetworkID.LoginMessage);
            stream.Write((byte)LoginType.AccountLogin);
            stream.Write(StartupState.clientOptions.name);
            stream.Write(StartupState.clientOptions.password);
            Program.client.SendStream(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE);
        }

        public static void Register()
        {
            PacketWriter stream = Program.client.SetupSendStream(NetworkID.LoginMessage);
            stream.Write((byte)LoginType.AccountCreation);
            stream.Write(StartupState.clientOptions.name);
            stream.Write(StartupState.clientOptions.password);
            Program.client.SendStream(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE);
        }

        public static void CreateNewCharacter(AccCharInfo ci)
        {
            PacketWriter stream = Program.client.SetupSendStream(NetworkID.LoginMessage);
            stream.Write((byte)LoginType.CharacterCreation);
            ci.Write(stream);
            Program.client.SendStream(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE);
        }

        public static void StartInWorld(int slotNum)
        {
            if (slotNum >= 0 && slotNum < AccCharInfo.Max_Slots)
            {
                PacketWriter stream = Program.client.SetupSendStream(NetworkID.LoginMessage);
                stream.Write((byte)LoginType.CharacterLogin);
                stream.Write((byte)slotNum);
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

        public static void GetCharList(PacketReader stream)
        {
            byte count = stream.ReadByte();

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
