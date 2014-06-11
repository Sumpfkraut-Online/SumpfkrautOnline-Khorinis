#if SSM_WEB

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;
using GUC.Types;

using Mono.Data.Sqlite;
using SQLiteDataReader = Mono.Data.Sqlite.SqliteDataReader;
using SQLiteCommand = Mono.Data.Sqlite.SqliteCommand;

namespace GUC.Server.Scripts.Web.Actions
{
    public class GetAccountLogAction: Action
    {
        public class AccountLogChar
        {
            public int type;
            public int value;
            public long time;
        }

        public class AccountEvents
        {
            public GUC.Server.Scripts.Accounts.Logs.SQLiteLogger.LOGEventTypes eventType;
            public int lastPing;
            public int averagePing;
            public long time;
            public String Message;

            public String interactName;
        }

        protected AccountLogChar[] charStatList = new AccountLogChar[0];
        private String name;
        protected bool accExists = false;



        public AccountLogChar[] CharStatList
        {
            get
            { 
            if(!this.IsFinished)
                throw new FieldAccessException("Task has to be finished!");
            return charStatList;
        }}

        public String Name
        {
            get
            {
                if (!this.IsFinished)
                    throw new FieldAccessException("Task has to be finished!");
                return name;
            }
        }


        protected int accID = 0;
        public GetAccountLogAction(int aid)
        {
            accID = aid;
        }


        public override void update(ActionTimer timer)
        {

            using (SQLiteCommand command = new SQLiteCommand(Sqlite.getSqlite().connection))
            {
                command.CommandText = "SELECT name FROM `account` WHERE `id`=@id";
                command.Parameters.AddWithValue("@id", this.accID);
                using (SQLiteDataReader sdrITM = command.ExecuteReader())
                {
                    if (sdrITM.HasRows)
                    {
                        sdrITM.Read();
                        
                        this.name = Convert.ToString(sdrITM["name"]);
                        accExists = true;
                        
                    }
                }
            }


            if (accExists)
            {
                using (SQLiteCommand command = new SQLiteCommand(Accounts.Logs.SQLiteLogger.connection))
                {
                    command.CommandText = "SELECT * FROM `logStats` WHERE `accountID`=@id";
                    command.Parameters.AddWithValue("@id", this.accID);
                    using (SQLiteDataReader sdrITM = command.ExecuteReader())
                    {
                        if (sdrITM.HasRows)
                        {
                            List<AccountLogChar> alcList = new List<AccountLogChar>();
                            while (sdrITM.Read())
                            {
                                AccountLogChar alc = new AccountLogChar();
                                alc.type = Convert.ToInt32(sdrITM["type"]);
                                alc.value = Convert.ToInt32(sdrITM["value"]);
                                alc.time = Convert.ToInt64(sdrITM["time"]);


                                alcList.Add(alc);
                            }
                            charStatList = alcList.ToArray();
                        }
                    }
                }

            }




            isFinished = true;
            timer.removeAction(this);
        }
    }
}

#endif