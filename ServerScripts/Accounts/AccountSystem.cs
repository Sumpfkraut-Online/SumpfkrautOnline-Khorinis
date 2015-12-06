using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Log;
using GUC.Server.Scripts.Database;
using GUC.Network;
using GUC.Server.Network;
using GUC.Enumeration;

namespace GUC.Server.Scripts.Accounts
{
    enum LoginType
    {
        AccountLogin,
        AccountCreation,
        CharacterLogin,
        CharacterCreation
    }

    static class AccountSystem
    {
        public static void Init()
        {
            Logger.log(Logger.LogLevel.INFO, "################# Initialise Account-System ################");
            CreateStandardTables();

            Server.Network.Server.OnLoginMessage = ReadLoginMessage;
        }

        static void ReadLoginMessage(PacketReader stream, Client client, PacketWriter answer)
        {
            LoginType type = (LoginType)stream.ReadByte();
            switch (type)
            {
                case LoginType.AccountLogin:
                    string accName = stream.ReadString();
                    string accPW = stream.ReadString();

                    client.AccountID = LoginAccount(accName, accPW);
                    if (client.AccountID >= 0)
                    {
                        answer.Write((byte)LoginType.AccountLogin);
                        AccCharInfo[] infos = GetCharacters(client.AccountID);
                        answer.Write((byte)infos.Length);
                        for (int i = 0; i < infos.Length; i++)
                            infos[i].Write(answer);

                        client.SendLoginMsg(answer);
                    }
                    else  // failed
                    {
                        client.SendErrorMsg("Ungültiger Accountname oder Passwort.");
                    }
                    break;
                case LoginType.AccountCreation:
                    accName = stream.ReadString();
                    accPW = stream.ReadString();

                    client.AccountID = CreateAccount(accName, accPW);
                    if (client.AccountID >= 0)
                    { //send empty list
                        answer.Write((byte)LoginType.AccountLogin);
                        answer.Write((byte)0);
                        client.SendLoginMsg(answer);
                    }
                    else
                    {
                        client.SendErrorMsg("Ungültiger Accountname oder Passwort.");
                    }
                    break;
                case LoginType.CharacterLogin:
                    if (client.AccountID >= 0) // logged in ?
                    {
                        byte slot = stream.ReadByte();
                        if (slot >= 0 && slot < 20)
                        {
                            StartInWorld(client, slot);
                        }
                        else // hax
                        {
                            client.Disconnect();
                        }
                    }
                    break;
                case LoginType.CharacterCreation:
                    if (client.AccountID >= 0) // logged in ?
                    {
                        AccCharInfo info = new AccCharInfo();
                        info.Read(stream);
                        Log.Logger.log(info.ToString());
                        if (CreateCharacter(client.AccountID, info))
                        {
                            StartInWorld(client, info.SlotNum);
                        }
                        else
                        {
                            client.SendErrorMsg("Ungültige Charaktereinstellungen.");
                        }
                    }
                    break;
            }
        }

        static void StartInWorld(Client client, int slotNum)
        {
            AccCharInfo info = GetCharacters(client.AccountID)[slotNum];

            var npc = GUC.Server.WorldObjects.NPC.Create("_MALE", null);
            npc.CustomName = info.Name;

            //set all the stuff from the data bank
            npc.HumanBodyTex = (HumBodyTex)info.BodyTex;
            npc.HumanHeadMesh = (HumHeadMesh)info.HeadMesh;
            npc.HumanHeadTex = (HumHeadTex)info.HeadTex;
            npc.HumanVoice = (HumVoice)info.Voice;

            client.SetControl(npc, Network.Server.GetWorld("newworld"));
            Log.Logger.log("Client joins in on npc " + npc.ID);
        }

        static int LoginAccount(string name, string pw)
        {
            return GetAccID(name, pw);
        }

        static int GetAccID(string name, string pw)
        {

            List<List<List<object>>> res = new List<List<List<object>>>();
            DBReader.LoadFromDB(ref res, string.Format("SELECT id FROM account WHERE name=\"{0}\" AND password=\"{1}\"", name, pw));

            if (res.Count == 0 || res[0].Count == 0 || res[0][0].Count == 0)
            {
                return -1;
            }

            return Convert.ToInt32(res[0][0][0]);
        }

        // for login screen
        static AccCharInfo[] GetCharacters(int accID)
        {
            List<List<List<object>>> res = new List<List<List<object>>>();
            DBReader.LoadFromDB(ref res, string.Format("SELECT * FROM player WHERE accountID=\"{0}\"", accID));

            if (res.Count > 0 && res[0].Count > 0)
            {
                AccCharInfo[] chars = new AccCharInfo[res[0].Count];
                for (int i = 0; i < chars.Length; i++)
                {
                    chars[i] = new AccCharInfo();
                    chars[i].Name = res[0][i][2].ToString();
                    chars[i].BodyMesh = Convert.ToInt32(res[0][i][3]);
                    chars[i].BodyTex = Convert.ToInt32(res[0][i][4]);
                    chars[i].HeadMesh = Convert.ToInt32(res[0][i][5]);
                    chars[i].HeadTex = Convert.ToInt32(res[0][i][6]);
                    chars[i].Fatness = Convert.ToSingle(res[0][i][7]);
                    chars[i].BodyHeight = Convert.ToSingle(res[0][i][8]);
                    chars[i].BodyWidth = Convert.ToSingle(res[0][i][9]);
                    //chars[i].Voice = Convert.ToInt32(res[0][i][10]);
                    chars[i].FormerClass = Convert.ToInt32(res[0][i][11]);
                    //ci.posx = Convert.ToSingle(res[0][i][12]);
                    //ci.posy = Convert.ToSingle(res[0][i][13]);
                    //ci.posz = Convert.ToSingle(res[0][i][14]);
                    chars[i].SlotNum = Convert.ToInt32(res[0][i][15]);
                }
                return chars;
            }
            return new AccCharInfo[0];
        }

        static bool CreateCharacter(int accID, AccCharInfo ci)
        {
            //FIXME: Check if visuals are supported (no female head on male body etc) && account has <= 20 chars 
            return DBReader.SaveToDB(string.Format("INSERT INTO player (id, accountID, name, bodymesh, bodytex, headmesh, headtex, fatness, bodyheight, bodywidth, voice, class, posx, posy, posz, slotnum) VALUES(NULL, \"{0}\", \"{1}\", \"{2}\", \"{3}\", \"{4}\", \"{5}\", \"{6}\", \"{7}\", \"{8}\", \"{9}\", \"{10}\", \"0\", \"0\", \"0\", \"{11}\")",
                accID, ci.Name, ci.BodyMesh, ci.BodyTex, ci.HeadMesh, ci.HeadTex, ci.Fatness.ToString(System.Globalization.CultureInfo.InvariantCulture), ci.BodyHeight.ToString(System.Globalization.CultureInfo.InvariantCulture), ci.BodyWidth.ToString(System.Globalization.CultureInfo.InvariantCulture), ci.Voice, ci.FormerClass, ci.SlotNum)) > 0;
        }

        static int CreateAccount(string name, string pw)
        {
            //FIXME: Check for unsupported symbols
            if (AccNameExists(name))
            {
                return -1;
            }

            if (DBReader.SaveToDB(string.Format("INSERT INTO account (id, name, password) VALUES(NULL, \"{0}\", \"{1}\")", name, pw)) <= 0)
            {
                return -1;
            }

            return GetAccID(name, pw);
        }

        static bool AccNameExists(String name)
        {
            List<List<List<object>>> res = new List<List<List<object>>>();
            DBReader.LoadFromDB(ref res, string.Format("SELECT 1 FROM account WHERE name=\"{0}\"", name));
            return res.Count > 0;
        }

        static void CreateStandardTables()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("CREATE TABLE IF NOT EXISTS account (");
            sb.Append("id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,");
            sb.Append("name text NOT NULL,");
            sb.Append("password text NOT NULL)");
            DBReader.SaveToDB(sb.ToString());

            sb.Clear();
            sb.Append("CREATE TABLE IF NOT EXISTS player (");
            sb.Append("id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,");
            sb.Append("accountID INTEGER NOT NULL,");
            sb.Append("name text NOT NULL,");
            sb.Append("bodymesh INTEGER NOT NULL,");
            sb.Append("bodytex INTEGER NOT NULL,");
            sb.Append("headmesh INTEGER NOT NULL,");
            sb.Append("headtex INTEGER NOT NULL,");
            sb.Append("fatness REAL NOT NULL,");
            sb.Append("bodyheight REAL NOT NULL,");
            sb.Append("bodywidth REAL NOT NULL,");
            sb.Append("voice INTEGER NOT NULL,");
            sb.Append("class INTEGER NOT NULL,");
            sb.Append("posx REAL NOT NULL,");
            sb.Append("posy REAL NOT NULL,");
            sb.Append("posz REAL NOT NULL,");
            sb.Append("slotnum INTEGER NOT NULL)");

            DBReader.SaveToDB(sb.ToString());
        }
    }
}
