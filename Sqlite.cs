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
using SQLiteConnection = Mono.Data.Sqlite.SqliteConnection;
namespace GUC.Server.Scripts
{
	public class Sqlite
	{
		protected static Sqlite sSqlite = null;

		public static Sqlite getSqlite() {
			if(sSqlite == null)
			{
				sSqlite = new Sqlite();
				sSqlite.Init();
			}
			
			return sSqlite;
		}
		
		public SQLiteConnection connection;
		public void Init()
		{
			connection = new SQLiteConnection ();
			connection.ConnectionString = "Data Source=" + "save.db";
			
			Open();
		}
		
		public void Open()
        {
            connection.Open();
            //CreateStandardTables();
        }

        public void CreateStandardTables()
        {
            SQLiteCommand command = new SQLiteCommand(connection);
            

            command.CommandText = "CREATE TABLE IF NOT EXISTS `world_container` (";
            command.CommandText += "  `id` INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,";
             command.CommandText += "  `name` text NOT NULL,";
             command.CommandText += " `posx` REAL  NOT NULL,";
             command.CommandText += " `posy` REAL  NOT NULL,";
             command.CommandText += " `posz` REAL  NOT NULL,";
             command.CommandText += " `world` text NOT NULL,";
             command.CommandText += " `opened` INTEGER NOT NULL";
            command.CommandText += ")";
            command.ExecuteNonQuery();

            command.CommandText = "CREATE TABLE IF NOT EXISTS `world_items` (";
            command.CommandText += "  `id` INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,";
            command.CommandText += "  `iteminstance` text NOT NULL,";
            command.CommandText += "  `amount` INTEGER NOT NULL,";
            command.CommandText += "  `posx` REAL NOT NULL,";
            command.CommandText += "  `posy` REAL NOT NULL,";
            command.CommandText += "  `posz` REAL NOT NULL,";
            command.CommandText += "  `world` text NOT NULL";
            command.CommandText += ")";
            command.ExecuteNonQuery();


            command.CommandText = "CREATE TABLE IF NOT EXISTS `world_mobinteract` (";
            command.CommandText += " `id` INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,";
             command.CommandText += " `vobType` INTEGER NOT NULL,";
             command.CommandText += " `name` text NOT NULL,";
             command.CommandText += " `posx` REAL NOT NULL,";
             command.CommandText += " `posy` REAL NOT NULL,";
             command.CommandText += " `posz` REAL NOT NULL,";
             command.CommandText += " `world` text NOT NULL,";
             command.CommandText += " `triggered` INTEGER NOT NULL";
            command.CommandText += ")";
            command.ExecuteNonQuery();


            command.CommandText = "CREATE TABLE IF NOT EXISTS `world_container_items` (";
            command.CommandText += " `id` INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,";
            command.CommandText += " `containerid` INTEGER NOT NULL,";
            command.CommandText += " `item` text NOT NULL,";
            command.CommandText += " `amount` INTEGER NOT NULL";
            command.CommandText += ")";
            command.ExecuteNonQuery();

            command.Dispose();
            
        }

		
		
    }
}
