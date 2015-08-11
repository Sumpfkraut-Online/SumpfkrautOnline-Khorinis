using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RakNet;
using GUC.Enumeration;
using GUC.Network;

namespace GUC.Server.Network.Messages
{
    public class AccountMessage
    {
        public delegate int LoginHandler(string accName, string accPW);
        public static LoginHandler OnCreateAccount;
        public static LoginHandler OnLoginAccount;

        public delegate bool CreateCharacterHandler(int accID, AccCharInfo ci);
        public static CreateCharacterHandler OnCreateCharacter;

        public delegate AccCharInfo[] GetCharactersHandler(int accID);
        public static GetCharactersHandler OnGetCharacters;

        internal static void Login(BitStream stream, Client client)
        {
            string accName = stream.mReadString();
            string accPW = stream.mReadString();

            //basic login, sends a list of characters for the character selection menu
            if (OnLoginAccount != null)
            {
                client.accountID = OnLoginAccount(accName, accPW);
                if (client.accountID != -1 && OnGetCharacters != null)
                {
                    SendCharList(client, OnGetCharacters(client.accountID));
                }
                else
                {
                    SendFailed(client, "Ungültiger Accountname oder Passwort.");
                }
            }
        }

        internal static void Register(BitStream stream, Client client)
        {
            string accName = stream.mReadString();
            string accPW = stream.mReadString();

            //creates a new account and does login process
            if (OnCreateAccount != null)
            {
                client.accountID = OnCreateAccount(accName, accPW);
                if (client.accountID != -1)
                { //send empty list
                    SendCharList(client, new AccCharInfo[0]);
                }
                else
                {
                    SendFailed(client, "Ungültiger Accountname oder Passwort.");
                }
            }
        }

        internal static void CreateCharacter(BitStream stream, Client client)
        {
            AccCharInfo ci = new AccCharInfo();
            ci.Read(stream);

            Log.Logger.log(ci.ToString());

            if (OnCreateCharacter != null)
            {
                if (OnCreateCharacter(client.accountID, ci))
                {
                    StartInWorld(client, ci);
                }
                else
                {
                    SendFailed(client, "Ungültige Charaktereinstellung.");
                }
            }
        }

        internal static void LoginCharacter(BitStream stream, Client client)
        {
            byte id = stream.mReadByte();

            if (OnGetCharacters != null)
            {
                //FIXME: Read the whole information of the single character
                AccCharInfo[] chars = OnGetCharacters(client.accountID);
                if (id >= 0 && id < chars.Length)
                {
                    StartInWorld(client, chars[id]);
                }
                else
                {
                    SendFailed(client, "Ungültiger Charakter.");
                }
            }
        }

        private static void SendCharList(Client client, AccCharInfo[] chars)
        {
            BitStream stream = Program.server.SetupStream(NetworkID.AccountLoginMessage);
            stream.mWrite((byte)chars.Length);
            for (int i = 0; i < chars.Length; i++)
            {
                chars[i].Write(stream);
            }
            Program.server.ServerInterface.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE, 'M', client.guid, false);
        }

        private static void SendFailed(Client client, string message)
        {
            BitStream stream = Program.server.SetupStream(NetworkID.AccountErrorMessage);
            stream.mWrite(message);
            Program.server.ServerInterface.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'M', client.guid, false);
        }

        private static void StartInWorld(Client client, AccCharInfo ci)
        {
            WorldObjects.NPC npc = new WorldObjects.NPC();
            npc.World = WorldObjects.World.NewWorld;
            //set all the stuff from the data bank
            client.SetControl(npc);
            Log.Logger.log("Client joins in on npc " + npc.ID);
        }
    }
}
