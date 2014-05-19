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

namespace GUC.Server.Scripts
{
	public class Account
	{
		public enum State {
			Nothing = 0,
			Login = 1,
			Register = 2,
			Name = 4,
			Password = 8,
			LoggedIn = 16
		}

		Player player = null;
		public State state = State.Nothing;
		public long accountID = 0;

		public Account(Player player)
		{
			this.player = player;

			this.player.Disconnected += new Events.PlayerEventHandler(disconnect);
		}

		public static bool existsName(String name) {
			using(SQLiteCommand command = new SQLiteCommand(Sqlite.getSqlite().connection)) {
				command.CommandText = "SELECT name FROM `account` WHERE `name`=@name";
				command.Parameters.AddWithValue("@name", name);
				using(SQLiteDataReader sdrITM = command.ExecuteReader()) {
					if (!sdrITM.HasRows) {
						return false;
					}

					return true;
				}
			}
		}

		public bool login(String name, String password) {
			using(SQLiteCommand command = new SQLiteCommand(Sqlite.getSqlite().connection)) {
				command.CommandText = "SELECT * FROM `account` WHERE `name`=@name AND `password`=@password";
				command.Parameters.AddWithValue("@name", name);
				command.Parameters.AddWithValue("@password", password);
				using(SQLiteDataReader sdrITM = command.ExecuteReader()) {
					if (!sdrITM.HasRows) {
						return false;
					}

					sdrITM.Read();
					accountID = Convert.ToInt32(sdrITM["id"]);

					String world = Convert.ToString(sdrITM["world"]);

					player.setSpawnInfos(world, new Types.Vec3f(Convert.ToSingle(sdrITM["posx"]), Convert.ToSingle(sdrITM["posy"]), Convert.ToSingle(sdrITM["posz"])), null);
				}

				//Attributes:
                command.CommandText = "SELECT * FROM `account_attributes` WHERE `accountID`=@accountID ORDER BY `type` DESC";
				command.Parameters.AddWithValue("@accountID", accountID);
				using(SQLiteDataReader sdrITM = command.ExecuteReader()) {
					if(sdrITM.HasRows) {
						while(sdrITM.Read()) {
							player.setAttribute((NPCAttributeFlags)Convert.ToInt32(sdrITM["type"]), Convert.ToInt32(sdrITM["value"]));
						}
					}
				}

				//Talents:
				command.CommandText = "SELECT * FROM `account_talents` WHERE `accountID`=@accountID";
				command.Parameters.AddWithValue("@accountID", accountID);
				using(SQLiteDataReader sdrITM = command.ExecuteReader()) {
					if(sdrITM.HasRows) {
						while(sdrITM.Read()) {
							player.setTalentValues((NPCTalents)Convert.ToInt32(sdrITM["type"]), Convert.ToInt32(sdrITM["value"]));
							player.setTalentSkills((NPCTalents)Convert.ToInt32(sdrITM["type"]), Convert.ToInt32(sdrITM["skill"]));
						}
					}
				}

				//Hitchances
				command.CommandText = "SELECT * FROM `account_hitchances` WHERE `accountID`=@accountID";
				command.Parameters.AddWithValue("@accountID", accountID);
				using(SQLiteDataReader sdrITM = command.ExecuteReader()) {
					if(sdrITM.HasRows) {
						while(sdrITM.Read()) {
							player.setHitchances((NPCTalents)Convert.ToInt32(sdrITM["type"]), Convert.ToInt32(sdrITM["value"]));
						}
					}
				}

				//Items
				command.CommandText = "SELECT * FROM `account_items` WHERE `accountID`=@accountID";
				command.Parameters.AddWithValue("@accountID", accountID);
				using(SQLiteDataReader sdrITM = command.ExecuteReader()) {
					if(sdrITM.HasRows) {
						while(sdrITM.Read()) {
							player.addItem(ItemInstance.getItemInstance(Convert.ToString(sdrITM["instanceID"])), Convert.ToInt32(sdrITM["amount"]));
						}
					}
				}
                
				return true;
			}

            
		}

		public bool register(String name, String password)
		{
			if(existsName(name))
				return false;

			//Setting default parameter! Can be overwritten by other modules, after registration!
			player.setSpawnInfos(@"NEWWORLD\NEWWORLD.ZEN", null, null);
			player.HPMax = 10;
			player.HP = 10;
			
			

			using(SQLiteCommand command = new SQLiteCommand(Sqlite.getSqlite().connection)) {
				command.CommandText = "INSERT INTO `account` (";
				command.CommandText += "  `id`, `name`, `password`, `posx`, `posy`, `posz`, `world`)";
				command.CommandText += "VALUES( NULL, @name, @password, @posx, @posy, @posz, @world)";

				command.Parameters.AddWithValue("@name", name);
				command.Parameters.AddWithValue("@password", password);
				command.Parameters.AddWithValue("@posx", 0);
				command.Parameters.AddWithValue("@posy", 0);
				command.Parameters.AddWithValue("@posz", 0);
				command.Parameters.AddWithValue("@world", player.Map);

				command.ExecuteNonQuery();

				command.CommandText = @"select last_insert_rowid()";
				accountID = (long)command.ExecuteScalar();
			}

			state = State.LoggedIn;
			return true;
		}

		public void load()
		{
		}

		public void save()
		{
		}

		protected void disconnect(Player player) {
			if(!player.IsSpawned())
				return;
            
			using(SQLiteCommand command = new SQLiteCommand(Sqlite.getSqlite().connection)) {
				command.CommandText = "UPDATE `account` ";
				command.CommandText += " SET `posx`=@posx, `posy`=@posy, `posz`=@posz, `world`=@world";
				command.CommandText += " WHERE `id`=@id";

				command.Parameters.AddWithValue("@id", accountID);

				command.Parameters.AddWithValue("@posx", player.Position.X);
				command.Parameters.AddWithValue("@posy", player.Position.Y);
				command.Parameters.AddWithValue("@posz", player.Position.Z);
				command.Parameters.AddWithValue("@world", player.Map);

				command.ExecuteNonQuery();

				//Saving Attributes!
				for(int i = 0; i < (int)NPCAttributeFlags.ATR_MAX; i++) {
					command.CommandText = "UPDATE `account_attributes` ";
					command.CommandText += " SET `value`=@value";
					command.CommandText += " WHERE `accountID`=@accountID AND `type`=@type";
					command.Parameters.AddWithValue("@value", player.getAttribute((NPCAttributeFlags)i));
					command.Parameters.AddWithValue("@accountID", accountID);
					command.Parameters.AddWithValue("@type", i);

					int update = command.ExecuteNonQuery();
					if(update == 0) {
						command.CommandText = "INSERT INTO `account_attributes` (";
						command.CommandText += "  `id`, `accountID`, `type`, `value`)";
						command.CommandText += "VALUES( NULL, @accountID, @type, @value)";
						command.Parameters.AddWithValue("@value", player.getAttribute((NPCAttributeFlags)i));
						command.Parameters.AddWithValue("@accountID", accountID);
						command.Parameters.AddWithValue("@type", i);
						command.ExecuteNonQuery();
					}
				}

				//Saving Talents:
				for(int i = (int)NPCTalents.H1; i < (int)NPCTalents.MaxTalents; i++) {
					command.CommandText = "UPDATE `account_talents` ";
					command.CommandText += " SET `value`=@value, `skill`=@skill";
					command.CommandText += " WHERE `accountID`=@accountID AND `type`=@type";
					command.Parameters.AddWithValue("@value", player.getTalentValues((NPCTalents)i));
					command.Parameters.AddWithValue("@skill", player.getTalentSkills((NPCTalents)i));
					command.Parameters.AddWithValue("@accountID", accountID);
					command.Parameters.AddWithValue("@type", i);

					int update = command.ExecuteNonQuery();
					if(update == 0) {
						command.CommandText = "INSERT INTO `account_talents` (";
						command.CommandText += "  `id`, `accountID`, `type`, `value`, `skill`)";
						command.CommandText += "VALUES( NULL, @accountID, @type, @value, @skill)";
						command.Parameters.AddWithValue("@value", player.getTalentValues((NPCTalents)i));
						command.Parameters.AddWithValue("@skill", player.getTalentSkills((NPCTalents)i));
						command.Parameters.AddWithValue("@accountID", accountID);
						command.Parameters.AddWithValue("@type", i);
						command.ExecuteNonQuery();
					}
				}

				//Saving Hitchances:
				for(int i = (int)NPCTalents.H1; i <= (int)NPCTalents.CrossBow; i++) {
					command.CommandText = "UPDATE `account_hitchances` ";
					command.CommandText += " SET `value`=@value";
					command.CommandText += " WHERE `accountID`=@accountID AND `type`=@type";
					command.Parameters.AddWithValue("@value", player.getTalentValues((NPCTalents)i));
					command.Parameters.AddWithValue("@accountID", accountID);
					command.Parameters.AddWithValue("@type", i);

					int update = command.ExecuteNonQuery();
					if(update == 0) {
						command.CommandText = "INSERT INTO `account_hitchances` (";
						command.CommandText += "  `id`, `accountID`, `type`, `value`)";
						command.CommandText += "VALUES( NULL, @accountID, @type, @value)";
						command.Parameters.AddWithValue("@value", player.getTalentValues((NPCTalents)i));
						command.Parameters.AddWithValue("@accountID", accountID);
						command.Parameters.AddWithValue("@type", i);
						command.ExecuteNonQuery();
					}
				}

				//Saving Items:
				command.CommandText = "DELETE FROM `account_items` WHERE `accountID`=@accountID";
				command.Parameters.AddWithValue("@accountID", accountID);
				command.ExecuteNonQuery();
				foreach(Item item in player.getItemList()) {
					command.CommandText = "INSERT INTO `account_items` (";
					command.CommandText += "  `id`, `accountID`, `instanceID`, `amount`)";
					command.CommandText += "VALUES( NULL, @accountID, @instanceID, @amount)";
					command.Parameters.AddWithValue("@instanceID", item.ItemInstance.InstanceName);
					command.Parameters.AddWithValue("@accountID", accountID);
					command.Parameters.AddWithValue("@amount", item.Amount);
					command.ExecuteNonQuery();
				}
			}
		}

		
		
    }
}
