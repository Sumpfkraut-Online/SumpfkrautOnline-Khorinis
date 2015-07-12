using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Log;
using GUC.Server.Network.Messages;
using GUC.Server.Scripts.Database;
using GUC.Enumeration;
using GUC.Network;

using Mono.Data.Sqlite;
using SQLiteDataReader = Mono.Data.Sqlite.SqliteDataReader;
using SQLiteCommand = Mono.Data.Sqlite.SqliteCommand;

namespace GUC.Server.Scripts.Accounts
{
    class AccountSystem
    {
        private static AccountSystem acs;

        public static AccountSystem Get()
        {
            if (acs == null)
            {
                acs = new AccountSystem();
            }
            return acs;
        }

        public AccountSystem()
        {
            Logger.log(Logger.LogLevel.INFO, "################# Initalise Account-System ################");
            CreateStandardTables();

            AccountMessage.OnCreateAccount = CreateAccount;
            AccountMessage.OnLoginAccount = LoginAccount;
            AccountMessage.OnGetCharacters = GetCharacters;
            AccountMessage.OnCreateCharacter = CreateCharacter;
        }

        private AccCharInfo[] GetCharacters(int accID)
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

        private bool CreateCharacter(int accID, AccCharInfo ci)
        {
            //FIXME: Check if visuals are supported (no female head on male body etc) && account has <= 20 chars 
            return DBReader.SaveToDB(string.Format("INSERT INTO player (id, accountID, name, bodymesh, bodytex, headmesh, headtex, fatness, bodyheight, bodywidth, voice, class, posx, posy, posz, slotnum) VALUES(NULL, \"{0}\", \"{1}\", \"{2}\", \"{3}\", \"{4}\", \"{5}\", \"{6}\", \"{7}\", \"{8}\", \"{9}\", \"{10}\", \"0\", \"0\", \"0\", \"{11}\")",
                accID, ci.Name, ci.BodyMesh, ci.BodyTex, ci.HeadMesh, ci.HeadTex, ci.Fatness.ToString(System.Globalization.CultureInfo.InvariantCulture), ci.BodyHeight.ToString(System.Globalization.CultureInfo.InvariantCulture), ci.BodyWidth.ToString(System.Globalization.CultureInfo.InvariantCulture), ci.Voice, ci.FormerClass, ci.SlotNum)) > 0;
        }

        private int CreateAccount(string name, string pw)
        {
            //FIXME: Check for unsupported symbols
            if (AccNameExists(name))
            {
                return -1;
            }

            if (DBReader.SaveToDB(string.Format("INSERT INTO account (id, name, password) VALUES(NULL, \"{0}\", \"{1}\")",name,pw)) <= 0)
            {
                return -1;
            }

            return GetAccID(name, pw);
        }

        private int LoginAccount(string name, string pw)
        {
            return GetAccID(name, pw);
        }

        private int GetAccID(string name, string pw)
        {
            
            List<List<List<object>>> res = new List<List<List<object>>>();
            DBReader.LoadFromDB(ref res, string.Format("SELECT id FROM account WHERE name=\"{0}\" AND password=\"{1}\"", name,pw));

            if (res.Count == 0 || res[0].Count == 0 || res[0][0].Count == 0)
            {
                return -1;
            }
            
            return Convert.ToInt32(res[0][0][0]);
        }

        private bool AccNameExists(String name)
        {
            List<List<List<object>>> res = new List<List<List<object>>>();
            DBReader.LoadFromDB(ref res, string.Format("SELECT 1 FROM account WHERE name=\"{0}\"",name));
            return res.Count > 0;
        }

        private void CreateStandardTables()
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
