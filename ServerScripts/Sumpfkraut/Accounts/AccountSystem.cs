using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Log;
using GUC.Server.Network.Messages;
using GUC.Server.Scripting.Objects.Character;
using GUC.Server.Scripts.Sumpfkraut.Database;

using Mono.Data.Sqlite;
using SQLiteDataReader = Mono.Data.Sqlite.SqliteDataReader;
using SQLiteCommand = Mono.Data.Sqlite.SqliteCommand;

namespace GUC.Server.Scripts.Sumpfkraut.Accounts
{
    class AccountSystem
    {
        public AccountSystem()
        {
            Logger.log(Logger.LogLevel.INFO, "################# Initalise Account-System ################");
            CreateStandardTables();

            AccountMessage.OnCreateAccount = CreateAccount;
            AccountMessage.OnLoginAccount = LoginAccount;
            AccountMessage.OnGetCharacters = GetCharacters;
            AccountMessage.OnCreateCharacter = CreateCharacter;
        }

        private List<AccountMessage.CharInfo> GetCharacters(int accID)
        {
            var chars = new List<AccountMessage.CharInfo>();

            List<List<List<object>>> res = new List<List<List<object>>>();
            DBReader.LoadFromDB(ref res, string.Format("SELECT * FROM player WHERE accountID=\"{0}\"", accID));

            if (res.Count > 0 && res[0].Count > 0)
            {
                for (int i = 0; i < res[0].Count; i++)
                {
                    var ci = new AccountMessage.CharInfo();
                    ci.Name = res[0][i][2].ToString();
                    ci.BodyMesh = Convert.ToInt32(res[0][i][3]);
                    ci.BodyTex = Convert.ToInt32(res[0][i][4]);
                    ci.HeadMesh = Convert.ToInt32(res[0][i][5]);
                    ci.HeadTex = Convert.ToInt32(res[0][i][6]);
                    ci.Fatness = Convert.ToSingle(res[0][i][7]);
                    ci.BodyHeight = Convert.ToSingle(res[0][i][8]);
                    ci.BodyWidth = Convert.ToSingle(res[0][i][9]);
                    ci.Voice = Convert.ToInt32(res[0][i][10]);
                    ci.FormerClass = Convert.ToInt32(res[0][i][11]);
                    //ci.posx = Convert.ToSingle(res[0][i][12]);
                    //ci.posy = Convert.ToSingle(res[0][i][13]);
                    //ci.posz = Convert.ToSingle(res[0][i][14]);
                    ci.SlotNum = Convert.ToInt32(res[0][i][15]);

                    chars.Add(ci);
                }
            }
            return chars;
        }

        private bool CreateCharacter(int accID, AccountMessage.CharInfo ci)
        {
            return DBReader.SaveToDB(string.Format("INSERT INTO player (id, accountID, name, bodymesh, bodytex, headmesh, headtex, fatness, bodyheight, bodywidth, voice, class, posx, posy, posz, slotnum) VALUES(NULL, \"{0}\", \"{1}\", \"{2}\", \"{3}\", \"{4}\", \"{5}\", \"{6}\", \"{7}\", \"{8}\", \"{9}\", \"{10}\", \"0\", \"0\", \"0\", \"{11}\")",
                accID, ci.Name, ci.BodyMesh, ci.BodyTex, ci.HeadMesh, ci.HeadTex, ci.Fatness.ToString(System.Globalization.CultureInfo.InvariantCulture), ci.BodyHeight.ToString(System.Globalization.CultureInfo.InvariantCulture), ci.BodyWidth.ToString(System.Globalization.CultureInfo.InvariantCulture), ci.Voice, ci.FormerClass, ci.SlotNum)) > 0;
        }

        private int CreateAccount(string name, string pw)
        {
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
