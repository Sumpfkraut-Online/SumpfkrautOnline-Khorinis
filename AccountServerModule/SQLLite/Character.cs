using System;
using System.Collections.Generic;
using System.Text;
using Network;
using System.Security.Cryptography;
using Mono.Data.Sqlite;

namespace AccountServerModule.SqlLite
{
    class Character
    {
        public int mID;
        public ulong mGUID;


        //Nur Nutzbar wenn Load und Save-Character Values genutzt wird!
        public String name;
        public String world;
        public int hp;
        public int hp_max;
        public int mp;
        public int mp_max;
        public float posX, posY, posZ;
        public int str, dex;

        public int[] talents = new int[22];
        public int[] hitChances = new int[4];
        public List<item> itemList  = new List<item>();

        public bool saved;
        public bool saveStarted;

        public Character(int id)
        {
            mID = id;
        }


        public void LoadCharacter()
        {
            SqliteCommand command = new SqliteCommand(GMP_Server.Program.sqlite.connection);
            command.CommandText = "SELECT * FROM accounts WHERE id=:id";
            command.Parameters.Add(new SqliteParameter("id", mID));
            command.Prepare();
            SqliteDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                reader.Read();
                this.name = reader["name"].ToString();
                this.world = reader["world"].ToString();
                this.hp = Convert.ToInt32(reader["hp"]);
                this.hp_max = Convert.ToInt32(reader["hp_max"]);
                this.mp = Convert.ToInt32(reader["mp"]);
                this.mp_max = Convert.ToInt32(reader["mp_max"]);
                this.str = Convert.ToInt32(reader["str"]);
                this.dex = Convert.ToInt32(reader["dex"]);
                this.posX = Convert.ToSingle(reader["posX"]);
                this.posY = Convert.ToSingle(reader["posY"]);
                this.posZ = Convert.ToSingle(reader["posZ"]);
                this.saved = Convert.ToBoolean(reader["saved"]);

                for (int i = 0; i < 22; i++)
                    this.talents[i] = Convert.ToInt32(reader["talents_"+i]);
                for (int i = 0; i < 4; i++)
                    this.hitChances[i] = Convert.ToInt32(reader["hitchances_" + i]);
            }


            reader.Close();
            reader.Dispose();
            command.Dispose();


            command = new SqliteCommand(GMP_Server.Program.sqlite.connection);
            command.CommandText = "SELECT name, count FROM accounts_equipment WHERE accID=:id";
            command.Prepare();
            command.Parameters.Add(new SqliteParameter("id", mID));
            
            reader = command.ExecuteReader();
            itemList.Clear();
            while (reader.Read())
            {
                item itm = new item();
                itm.code = reader[0].ToString();
                itm.Amount = Convert.ToInt32(reader[1]);

                itemList.Add(itm);
            }


            reader.Close();
            reader.Dispose();
            command.Dispose();
        }

        public void SaveCharacter()
        {
            SqliteCommand command = new SqliteCommand(GMP_Server.Program.sqlite.connection);
            command.CommandText = "UPDATE accounts Set world=:world, hp=:hp,hp_max=:hp_max,mp=:mp,mp_max=:mp_max,str=:str,dex=:dex,posX=:posX,posY=:posY,posZ=:posZ, saved=:saved ";
            for (int i = 0; i < 22; i++)
                command.CommandText += ", talents_" + i + "=:talents_"+i;
            for (int i = 0; i < 4; i++)
                command.CommandText += ", hitchances_" + i + "=:hitchances_" + i;
            command.CommandText += " WHERE id=:id";
            command.Parameters.Add(new SqliteParameter("id", mID));
            command.Parameters.Add(new SqliteParameter("world", world));
            command.Parameters.Add(new SqliteParameter("hp", hp));
            command.Parameters.Add(new SqliteParameter("hp_max", hp_max));
            command.Parameters.Add(new SqliteParameter("mp", mp));
            command.Parameters.Add(new SqliteParameter("mp_max", mp_max));
            command.Parameters.Add(new SqliteParameter("str", str));
            command.Parameters.Add(new SqliteParameter("dex", dex));
            command.Parameters.Add(new SqliteParameter("posX", posX));
            command.Parameters.Add(new SqliteParameter("posY", posY));
            command.Parameters.Add(new SqliteParameter("posZ", posZ));
            command.Parameters.Add(new SqliteParameter("saved", true));
            

            for (int i = 0; i < 22; i++)
                command.Parameters.Add(new SqliteParameter("talents_" + i, talents[i]));
            for (int i = 0; i < 4; i++)
                command.Parameters.Add(new SqliteParameter("hitchances_" + i, hitChances[i]));
            command.Prepare();
            command.ExecuteNonQuery();
            command.Dispose();



            command = new SqliteCommand(GMP_Server.Program.sqlite.connection);
            command.CommandText = "DELETE FROM accounts_equipment WHERE accID=:id";
            command.Prepare();
            command.Parameters.Add(new SqliteParameter("id", mID));
            command.ExecuteNonQuery();
            command.Dispose();

            foreach (item Item in itemList)
            {
                if (Item.code == null || Item.code.Trim().Length == 0)
                    continue;
                command = new SqliteCommand(GMP_Server.Program.sqlite.connection);
                command.CommandText = "INSERT INTO accounts_equipment (id, accID, name,count) VALUES (NULL, :id, :name, :count)";
                command.Prepare();


                command.Parameters.Add(new SqliteParameter("id", mID));
                command.Parameters.Add(new SqliteParameter("name", Item.code));
                command.Parameters.Add(new SqliteParameter("count", Item.Amount));
                command.ExecuteNonQuery();
                command.Dispose();
            }


        }

        public static Character getCharacter(String username, String password)
        {
            SqliteCommand command = new SqliteCommand(GMP_Server.Program.sqlite.connection);
            command.CommandText = "SELECT id, world FROM accounts WHERE name=:name AND password=:password";
            command.Parameters.Add(new SqliteParameter("name", username));
            command.Parameters.Add(new SqliteParameter("password", password));
            command.Prepare();
            SqliteDataReader reader = command.ExecuteReader();

            Character chara = null;
            if (reader.HasRows)
            {
                reader.Read();
                chara = new Character(Convert.ToInt32(reader[0]));
                chara.world = reader["world"].ToString();
            }
            reader.Close();
            reader.Dispose();
            command.Dispose();
            return chara;
        }


        public static bool isCharacterAvailable(String name)
        {
            SqliteCommand command = new SqliteCommand(GMP_Server.Program.sqlite.connection);
            command.CommandText = "SELECT id FROM accounts WHERE name=:name";
            command.Parameters.Add(new SqliteParameter("name", name));
            command.Prepare();
            SqliteDataReader reader = command.ExecuteReader();

            bool available = false;
            if (reader.HasRows)
            {
                available = false;
            }
            else
            {
                available = true;
            }
            reader.Close();
            reader.Dispose();
            command.Dispose();
            return available;
        }

        public static Character Create(String username, String password)
        {
            SqliteCommand command = new SqliteCommand(GMP_Server.Program.sqlite.connection);
            command.CommandText = "INSERT INTO accounts (id, name, password, world, hp,hp_max,mp,mp_max,str,dex,posX,posY,posZ, saved";

            for (int i = 0; i < 22; i++)
                command.CommandText += ", talents_" + i;
            for (int i = 0; i < 4; i++)
                command.CommandText += ", hitchances_" + i;

            command.CommandText += ") VALUES (NULL, :name, :password, :world, :hp, :hp_max, :mp, :mp_max, :str, :dex, :posX, :posY, :posZ, :saved";
            for (int i = 0; i < 22; i++)
                command.CommandText += ", :talents_" + i;
            for (int i = 0; i < 4; i++)
                command.CommandText += ", :hitchances_" + i;
            command.CommandText += ")";
            command.Parameters.Add(new SqliteParameter("name", username));
            command.Parameters.Add(new SqliteParameter("password", password));
            command.Parameters.Add(new SqliteParameter("world", ""));
            command.Parameters.Add(new SqliteParameter("hp", -1)); command.Parameters.Add(new SqliteParameter("hp_max", -1)); command.Parameters.Add(new SqliteParameter("mp", -1)); command.Parameters.Add(new SqliteParameter("mp_max", -1));
            command.Parameters.Add(new SqliteParameter("str", -1)); command.Parameters.Add(new SqliteParameter("dex", -1)); command.Parameters.Add(new SqliteParameter("saved", false));
            command.Parameters.Add(new SqliteParameter("posX", -1)); command.Parameters.Add(new SqliteParameter("posY", -1)); command.Parameters.Add(new SqliteParameter("posZ", -1));

            for (int i = 0; i < 22; i++)
                command.Parameters.Add(new SqliteParameter("talents_" + i, -1));
            for (int i = 0; i < 4; i++)
                command.Parameters.Add(new SqliteParameter("hitchances_" + i, -1));

            command.Prepare();
            command.ExecuteNonQuery();


            command.Dispose();

            return getCharacter(username, password);
        }


        

        static Character()
        {
            Console.WriteLine("Erstelle nötige Tabellen....");
            SqliteCommand command = new SqliteCommand(GMP_Server.Program.sqlite.connection);

            // Erstellen der Tabelle, sofern diese noch nicht existiert.
            command.CommandText = "CREATE TABLE IF NOT EXISTS accounts( id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, name VARCHAR(100) NOT NULL, password VARCHAR(100) NOT NULL,world TEXT NOT NULL, hp INTEGER NOT NULL, hp_max INTEGER NOT NULL, mp INTEGER NOT NULL, mp_max INTEGER NOT NULL, str INTEGER NOT NULL, dex INTEGER NOT NULL, posX FLOAT NOT NULL, posY FLOAT NOT NULL, posZ FLOAT NOT NULL, saved BOOLEAN NOT NULL";
            for (int i = 0; i < 22; i++)
                command.CommandText += ", talents_"+i+" INTEGER NOT NULL";
            for (int i = 0; i < 4; i++)
                command.CommandText += ", hitchances_" + i + " INTEGER NOT NULL";
            command.CommandText += ");";
            command.ExecuteNonQuery();
            command.Dispose();


            command = new SqliteCommand(GMP_Server.Program.sqlite.connection);
            command.CommandText = "CREATE TABLE IF NOT EXISTS accounts_equipment( id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,accID INTEGER NOT NULL, name VARCHAR(100) NOT NULL, count INTEGER NOT NULL);";
            command.ExecuteNonQuery();

            command.Dispose();
            Console.WriteLine("Nötige Tabellen erstellt");
        }
    }

    class CharacterList
    {
        
    }
}
