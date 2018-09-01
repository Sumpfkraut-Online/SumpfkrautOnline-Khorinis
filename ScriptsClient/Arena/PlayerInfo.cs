using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.Scripts.Arena.GameModes;
using GUC.Scripts.Arena.GameModes.TDM;

namespace GUC.Scripts.Arena
{
    class PlayerInfo
    {
        static Dictionary<int, PlayerInfo> players = new Dictionary<int, PlayerInfo>();
        public static IEnumerable<PlayerInfo> GetInfos() { return players.Values; }
        
        static readonly PlayerInfo heroInfo = new PlayerInfo();
        public static PlayerInfo HeroInfo { get { return heroInfo; } }

        public static bool TryGetInfo(int id, out PlayerInfo info)
        {
            return players.TryGetValue(id, out info);
        }

        int id = -1;
        public int ID { get { return id; } }

        string name;
        public string Name { get { return name; } }

        TeamIdent teamID = TeamIdent.None;
        public TeamIdent TeamID { get { return teamID; } }

        public static void ReadHeroInfo(PacketReader stream)
        {
            heroInfo.id = stream.ReadByte();
        }

        public static event Action OnPlayerListChange;
        public static void ReadPlayerInfoMessage(PacketReader stream)
        {
            int id = stream.ReadByte();
            if (!players.TryGetValue(id, out PlayerInfo pi))
            {
                pi = id == heroInfo.ID ? heroInfo : new PlayerInfo { id = id };
                players.Add(id, pi);
            }

            pi.name = stream.ReadString();
            pi.teamID = (TeamIdent)stream.ReadSByte();

            if (id == heroInfo.ID && pi.teamID < TeamIdent.GMPlayer)
            {
                NPCClass.Hero = null;
            }

            OnPlayerListChange?.Invoke();
        }

        public static void ReadPlayerQuitMessage(PacketReader stream)
        {
            players.Remove(stream.ReadByte());
        }
        
        public static void ReadPlayerInfoTeam(PacketReader stream)
        {
            int id = stream.ReadByte();
            if (!players.TryGetValue(id, out PlayerInfo pi))
                return;
            
            pi.teamID = (TeamIdent)stream.ReadSByte();
            OnPlayerListChange?.Invoke();

            if (pi == heroInfo && TDMMode.IsActive)
                TDMMode.HeroTeamChange();
        }
    }
}
