using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripts.AI.Waypoints;
using GUC.Server.Scripting.Objects.Character;
using GUC.Server.Scripting;
using GUC.Server.Scripts.AI.Enumeration;

namespace GUC.Server.Scripts.AI
{
    public static class AISystem
    {
        public static Dictionary<String, WayNet> WayNets = new Dictionary<string, WayNet>();
        public static Dictionary<Guilds, Dictionary<Guilds, GuildsAttitude>> GuildAttitudes = new Dictionary<Guilds, Dictionary<Guilds, GuildsAttitude>>();
        
        /**
         * 
         */
        public static void Init()
        {
            WayNets.Add("NEWWORLD\\NEWWORLD.ZEN", WayNet.loadFromFile("newworld"));

            Player.sOnPlayerConnects += new Events.PlayerEventHandler(playerConnects);

            AI_Events.Init();
            AISystem.initGuildAttitudes();
        }

        public static FreeOrWayPoint getWaypoint(String map, String wp)
        {
            WayNet wn = null;
            WayNets.TryGetValue(map.ToUpper(), out wn);

            if (wn == null)
                return null;

            return wn[wp];
        }

        public static void playerConnects(Player player)
        {
            player.InitNPCAI();
        }

        //TODO: siehe ods datei mit tieren
        public static void initGuildAttitudes()
        {
            //humans:
            setGuildAttitude(Guilds.HUM_NONE, Guilds.MON_NONE, GuildsAttitude.HOSTILE);
            setGuildAttitude(Guilds.HUM_NONE, Guilds.MON_WOLF, GuildsAttitude.HOSTILE);
            setGuildAttitude(Guilds.HUM_NONE, Guilds.ORC_NONE, GuildsAttitude.HOSTILE);
            setGuildAttitude(Guilds.MON_NONE, Guilds.HUM_NONE, GuildsAttitude.HOSTILE);

            setGuildAttitude(Guilds.ORC_NONE, Guilds.HUM_NONE, GuildsAttitude.HOSTILE);

            //wolf:
            setGuildAttitude(Guilds.MON_WOLF, Guilds.HUM_NONE, GuildsAttitude.HOSTILE);
            setGuildAttitude(Guilds.MON_WOLF, Guilds.MON_SCAVENGER, GuildsAttitude.HOSTILE);

            //scavenger:
            setGuildAttitude(Guilds.MON_SCAVENGER, Guilds.MON_WOLF, GuildsAttitude.NEUTRAL);

            //bloodfly:
            setGuildAttitude(Guilds.MON_BLOODFLY, Guilds.MON_WOLF, GuildsAttitude.HOSTILE);
            setGuildAttitude(Guilds.MON_BLOODFLY, Guilds.MON_SCAVENGER, GuildsAttitude.HOSTILE);

            //sheep:
            setGuildAttitude(Guilds.MON_SHEEP, Guilds.HUM_NONE, GuildsAttitude.FRIENDLY);

            //TODO: continue SOK relationships and delete standard ones; 
            //friendly will be replaced by packs
            //setGuildAttitude(Guilds.MON_MEATBUG, Guilds.MON_MEATBUG, GuildsAttitude.FRIENDLY);
            //setGuildAttitude(Guilds.MON_MEATBUG, Guilds.MON_SHEEP, GuildsAttitude.NEUTRAL);
            //setGuildAttitude(Guilds.MON_MEATBUG, Guilds.MON_, GuildsAttitude.NEUTRAL);
            //setGuildAttitude(Guilds.MON_MEATBUG, Guilds.MON_, GuildsAttitude.NEUTRAL);
            //setGuildAttitude(Guilds.MON_MEATBUG, Guilds.MON_WOLF, GuildsAttitude.NEUTRAL);
            //setGuildAttitude(Guilds.MON_MEATBUG, Guilds.MON_BLOODFLY, GuildsAttitude.NEUTRAL);
            
        }

        /**
         * If not otherwise stated with setGuildAttitude, default values will be returned based on humans vs non-humans
         * @see Guilds
         * @return The GuildsAttitude for the two guilds if they are set explicit, or a default attitude (humans are friendly to another and hostile to monsters and orcs)
         */
        public static GuildsAttitude getGuildAttitude(Guilds _Guild1, Guilds _Guild2)
        {
            if (GuildAttitudes.ContainsKey(_Guild1) &&
                GuildAttitudes[_Guild1].ContainsKey(_Guild2))
            {
                return GuildAttitudes[_Guild1][_Guild2];
            }
            else if (GuildAttitudes.ContainsKey(_Guild2) &&
                     GuildAttitudes[_Guild2].ContainsKey(_Guild1))
            {
                return GuildAttitudes[_Guild2][_Guild1];
            }

            //default attitudes: humans are friendly to each other
            if (_Guild1 < Guilds.HUM_SEPERATOR && _Guild2 < Guilds.HUM_SEPERATOR)
                return GuildsAttitude.FRIENDLY;

            //Humans are hostile to monsters and orcs
            if (_Guild1 < Guilds.HUM_SEPERATOR && _Guild2 > Guilds.HUM_SEPERATOR)
                return GuildsAttitude.HOSTILE;

            //Humans are hostile to monsters and orcs
            if (_Guild1 > Guilds.HUM_SEPERATOR && _Guild2 < Guilds.HUM_SEPERATOR)
                return GuildsAttitude.HOSTILE;

            //monster vs monster are hostile unless stated otherwise
            if (_Guild1 > Guilds.HUM_SEPERATOR && _Guild2 > Guilds.HUM_SEPERATOR)
                return GuildsAttitude.HOSTILE;

            return GuildsAttitude.NEUTRAL;
        }

        public static void setGuildAttitude(Guilds _Guild1, Guilds _Guild2, GuildsAttitude _GuildAttitude)
        {
            if (!GuildAttitudes.ContainsKey(_Guild1))
            {
                GuildAttitudes.Add(_Guild1, new Dictionary<Guilds, GuildsAttitude>());
            }

            if (GuildAttitudes[_Guild1].ContainsKey(_Guild2))
                GuildAttitudes[_Guild1][_Guild2] = _GuildAttitude;
            else
                GuildAttitudes[_Guild1].Add(_Guild2, _GuildAttitude);
        }

    }
}
