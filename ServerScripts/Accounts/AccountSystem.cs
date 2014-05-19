using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using GUC.Server.Log;
using GUC.Server.Scripting.Listener;
using GUC.Server.Scripting;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;
using GUC.Server.Scripting.Objects.Mob;
using GUC.Enumeration;
using GUC.Server.Scripting.GUI;
using GUC.Types;

using Mono.Data.Sqlite;
using SQLiteDataReader = Mono.Data.Sqlite.SqliteDataReader;
using SQLiteCommand = Mono.Data.Sqlite.SqliteCommand;

using GUC.Server.Scripts.StartModules;

namespace GUC.Server.Scripts.Accounts
{
	public class AccountSystem
	{
		public void Init()
		{
            Logger.log(Logger.LogLevel.INFO, "################# Initalise Account-System ################");
            
			
			Open();

			Modules.addModule(new AccountStartModule());
		}

		public void Open()
		{
			CreateStandardTables();
		}

		public void CreateStandardTables()
		{
			using(SQLiteCommand command = new SQLiteCommand(Sqlite.getSqlite().connection)) {
				command.CommandText = "CREATE TABLE IF NOT EXISTS `account` (";
				command.CommandText += "  `id` INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,";
				command.CommandText += "  `name` text NOT NULL,";
				command.CommandText += "  `password` text NOT NULL,";
				command.CommandText += " `posx` REAL  NOT NULL,";
				command.CommandText += " `posy` REAL  NOT NULL,";
				command.CommandText += " `posz` REAL  NOT NULL,";
				command.CommandText += " `world` text NOT NULL";
				command.CommandText += ")";
				command.ExecuteNonQuery();

				command.CommandText = "CREATE TABLE IF NOT EXISTS `account_hitchances` (";
				command.CommandText += "  `id` INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,";
				command.CommandText += "  `accountID` INTEGER NOT NULL,";
				command.CommandText += "  `type` INTEGER NOT NULL,";
				command.CommandText += "  `value` INTEGER NOT NULL";
				command.CommandText += ")";
				command.ExecuteNonQuery();

				command.CommandText = "CREATE TABLE IF NOT EXISTS `account_attributes` (";
				command.CommandText += "  `id` INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,";
				command.CommandText += "  `accountID` INTEGER NOT NULL,";
				command.CommandText += "  `type` INTEGER NOT NULL,";
				command.CommandText += "  `value` INTEGER NOT NULL";
				command.CommandText += ")";
				command.ExecuteNonQuery();

				command.CommandText = "CREATE TABLE IF NOT EXISTS `account_talents` (";
				command.CommandText += "  `id` INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,";
				command.CommandText += "  `accountID` INTEGER NOT NULL,";
				command.CommandText += "  `type` INTEGER NOT NULL,";
				command.CommandText += "  `value` INTEGER NOT NULL,";
				command.CommandText += "  `skill` INTEGER NOT NULL";
				command.CommandText += ")";
				command.ExecuteNonQuery();
				
				command.CommandText = "CREATE TABLE IF NOT EXISTS `account_items` (";
				command.CommandText += "  `id` INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,";
				command.CommandText += "  `accountID` INTEGER NOT NULL,";
				command.CommandText += "  `instanceID` text NOT NULL,";
				command.CommandText += "  `amount` INTEGER NOT NULL";
				command.CommandText += ")";
				command.ExecuteNonQuery();
            }
        }

		
		
    }
}
