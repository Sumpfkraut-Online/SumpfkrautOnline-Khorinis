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

            // FOR TESTING PURPOSES
            AccCharInfo[] chars = new AccCharInfo[1];
            for (int i = 0; i < chars.Length; i++)
            {
                chars[i] = new AccCharInfo();
                chars[i].Name = "WURST";
                chars[i].BodyMesh = 0;
                chars[i].BodyTex = 2;
                chars[i].HeadMesh = 4;
                chars[i].HeadTex = 121;
                chars[i].Fatness = 3f;
                chars[i].BodyHeight = 1f;
                chars[i].BodyWidth = 1f;
                //chars[i].Voice = Convert.ToInt32(res[0][i][10]);
                chars[i].FormerClass = 0;
                //ci.posx = Convert.ToSingle(res[0][i][12]);
                //ci.posy = Convert.ToSingle(res[0][i][13]);
                //ci.posz = Convert.ToSingle(res[0][i][14]);
                chars[i].SlotNum = 0;
            }
            CreateNewCharacter(chars[0]);
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
