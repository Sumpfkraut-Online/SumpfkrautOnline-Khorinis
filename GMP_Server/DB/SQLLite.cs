using System;
using System.Collections.Generic;
using System.Text;
//using System.Data.SQLite;
using Mono.Data.Sqlite;
namespace GMP_Server.DB
{
    public class SQLLite
    {
        static string dataSource = "save.db";
        public SqliteConnection connection;
        
        public SQLLite()
        {
            connection = new SqliteConnection();
            connection.ConnectionString = "Data Source=" + dataSource;


        }

        public void Open()
        {
            connection.Open();
            CreateStandardTables();
        }

        public void CreateStandardTables()
        {
            SqliteCommand command = new SqliteCommand(connection);
            

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
