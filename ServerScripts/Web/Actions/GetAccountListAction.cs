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
    public class GetAccountListAction: Action
    {
        public class Account
        {
            public String name;
            public int accountID;
        }
        public Account[] list;

        public Account[] List
        {
            get
            { 
            if(!this.IsFinished)
                throw new FieldAccessException("Task has to be finished!");
            return list;
        }}

        public override void update(ActionTimer timer)
        {
            List<Account> _list = new List<Account>();
            using (SQLiteCommand command = new SQLiteCommand(Sqlite.getSqlite().connection))
            {
                command.CommandText = "SELECT id, name FROM `account`";
                
                using (SQLiteDataReader sdrITM = command.ExecuteReader())
                {
                    if (sdrITM.HasRows)
                    {
                        while (sdrITM.Read())
                        {
                            int accID = Convert.ToInt32(sdrITM["id"]);
                            string name = Convert.ToString(sdrITM["name"]);
                            _list.Add(new Account() { accountID = accID, name = name });
                        }
                    }
                }
            }
            list = _list.ToArray();


            isFinished = true;
            timer.removeAction(this);
        }
    }
}

#endif