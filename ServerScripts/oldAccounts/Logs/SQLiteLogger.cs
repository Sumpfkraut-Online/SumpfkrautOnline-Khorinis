
#if (SSM_ACCOUNT && SSM_ACCOUNT_LOGGING)

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects.Character;
using GUC.Server.Scripting.Objects;
using GUC.Enumeration;

using Mono.Data.Sqlite;
using SQLiteDataReader = Mono.Data.Sqlite.SqliteDataReader;
using SQLiteCommand = Mono.Data.Sqlite.SqliteCommand;
using SQLiteConnection = Mono.Data.Sqlite.SqliteConnection;

namespace GUC.Server.Scripts.Accounts.Logs
{
    public static class SQLiteLogger
    {
        #region Inits
        public static SQLiteConnection connection;


        public static void Init()
        {
            connection = new SQLiteConnection();
            connection.ConnectionString = "Data Source=" + "logger.db";
            connection.Open();

            InitDefaultTables();

            NPC.sOnAttributeChanged += _lcs_Attribute;
            NPC.sOnTalentValueChanged += _lcs_TalentValue;
            NPC.sOnTalentSkillChanged += _lcs_TalentSkill;
            NPC.sOnHitchancesChanged += _lcs_Hitchances;

            //Events:
            Player.sOnPlayerSpawns += _le_Spawn;
            Player.sOnPlayerDisconnects += _le_Disconnection;
            DamageScript.Damages += _le_Damage;

        }

        private static void InitDefaultTables()
        {
            using (SQLiteCommand command = new SQLiteCommand(connection))
            {
                command.CommandText = "CREATE TABLE IF NOT EXISTS `logStats` (";
                command.CommandText += "  `id` INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,";
                command.CommandText += "  `accountID` INTEGER NOT NULL,";
                command.CommandText += "  `type` INTEGER NOT NULL,";
                command.CommandText += "  `value` INTEGER NOT NULL,";
                command.CommandText += "  `time` INTEGER NOT NULL";
                command.CommandText += ")";
                command.ExecuteNonQuery();

                command.CommandText = "CREATE TABLE IF NOT EXISTS `logEvents` (";
                command.CommandText += "  `id` INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,";
                command.CommandText += "  `accountID` INTEGER NOT NULL,";
                command.CommandText += "  `InteractAccountID` INTEGER NOT NULL,";
                command.CommandText += "  `type` INTEGER NOT NULL,";
                command.CommandText += "  `lastPing` INTEGER NOT NULL,";
                command.CommandText += "  `averagePing` INTEGER NOT NULL,";
                command.CommandText += "  `time` INTEGER NOT NULL,";
                command.CommandText += "  `message` TEXT NOT NULL";
                command.CommandText += ")";
                command.ExecuteNonQuery();
            }
        }



        #endregion


        #region Events
        public enum LOGEventTypes
        {
            Die,
            Chat,
            TakeItem,
            DropItem,
            OnDamage,
            OnHit,

            Spawn,
            Disconnection


        }

        public static void log_Chat(Player player, String message)
        {
            log_Event(LOGEventTypes.Chat, player, null, message);
        }

        private static void _le_Disconnection(Player player)
        {
            log_Event(LOGEventTypes.Disconnection, player, null, "");
        }

        private static void _le_Spawn(Player player)
        {
            log_Event(LOGEventTypes.Spawn, player, null, "");
        }

        private static void _le_Damage(NPC victim, NPC attacker, int damage, bool dropUnconscious, bool dropDead)
        {
            if (victim is Player)
            {
                log_Event(LOGEventTypes.OnDamage, (Player)victim, attacker, "Damage: " + damage + " " + dropUnconscious + " " + dropDead);
            }

            if (attacker != null && attacker is Player)
            {
                log_Event(LOGEventTypes.OnHit, (Player)attacker, victim, "Hit: " + damage + " " + dropUnconscious + " " + dropDead);
            }
        }
        

        public static void log_Event(LOGEventTypes let, Player player, Vob interact, String message)
        {
            if (player.getAccount() == null || !player.IsSpawned())
                return;

            long accountID = player.getAccount().accountID;
            long interactID = 0;
            short lastPing = (short)player.LastPing;
            short averagePing = (short)player.AveragePing;
            long now = DateTime.Now.Ticks;

            if (message == null)
                message = "";

            if (interact != null && !(interact is Player))
            {
                message += "Interact-Vob: " + interact.ID + ": " + interact.Visual + ", " + interact;
            }
            else if (interact != null && interact is Player && ((Player)interact).getAccount() != null)
            {
                interactID = ((Player)interact).getAccount().accountID;
            }


            lock (connection)
            {
                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText = "INSERT INTO `logEvents` (";
                    command.CommandText += "  `id`, `accountID`, `InteractAccountID`, `type`, `lastPing`, `averagePing`, `time`, `message`)";
                    command.CommandText += "VALUES( NULL, @accID, @interID, @type, @lastPing, @aPing, @time, @message)";

                    command.Parameters.AddWithValue("@accID", accountID);
                    command.Parameters.AddWithValue("@interID", interactID);
                    command.Parameters.AddWithValue("@type", (int)let);
                    command.Parameters.AddWithValue("@lastPing", lastPing);
                    command.Parameters.AddWithValue("@aPing", averagePing);
                    command.Parameters.AddWithValue("@time", now);
                    command.Parameters.AddWithValue("@message", message);

                    command.ExecuteNonQuery();
                }
            }

        }


        
        #endregion

        #region LogCharacterStats
        

        private static void _lcs_Attribute(NPC proto, NPCAttribute attrib, int oldValue, int newValue)
        {
            CharStat cs = CharStat.AttributeStart + (int)attrib;
            log_CharacterStat(proto, cs, newValue);
        }

        private static void _lcs_TalentValue(NPC proto, NPCTalent talent, int oldValue, int newValue)
        {
            CharStat cs = CharStat.TalentValuesStart + (int)talent;
            log_CharacterStat(proto, cs, newValue);
        }

        private static void _lcs_TalentSkill(NPC proto, NPCTalent talent, int oldValue, int newValue)
        {
            CharStat cs = CharStat.TalentSkillStart + (int)talent;
            log_CharacterStat(proto, cs, newValue);
        }

        private static void _lcs_Hitchances(NPC proto, NPCTalent talent, int oldValue, int newValue)
        {
            CharStat cs = CharStat.HitChances + (int)talent;
            log_CharacterStat(proto, cs, newValue);
        }

        public enum CharStat
        {
            None = 0,
            AttributeStart = 1,
            TalentValuesStart = AttributeStart + NPCAttribute.ATR_MAX + 1,
            TalentSkillStart = TalentValuesStart + NPCTalent.MaxTalents + 1,
            HitChances = TalentSkillStart + NPCTalent.MaxTalents + 1,

        }

        public static void log_CharacterStat(NPC proto, CharStat stat, int value)
        {
            if (proto is NPC)
                return;
            Player player = (Player)proto;
            if (player.getAccount() == null || !player.IsSpawned())
                return;

            lock (connection)
            {
                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText = "INSERT INTO `logStats` (";
                    command.CommandText += "  `id`, `accountID`, `type`, `value`, `time`)";
                    command.CommandText += "VALUES( NULL, @accID, @type, @value, @time)";

                    command.Parameters.AddWithValue("@accID", player.getAccount().accountID);
                    command.Parameters.AddWithValue("@type", (int)stat);
                    command.Parameters.AddWithValue("@value", value);
                    command.Parameters.AddWithValue("@time", DateTime.Now.Ticks);

                    command.ExecuteNonQuery();
                }
            }
        }

        #endregion
    }
}


#endif