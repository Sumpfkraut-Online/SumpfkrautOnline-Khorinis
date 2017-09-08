using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;

namespace GUC.Scripts.Arena
{
    class PlayerInfo
    {
        static Dictionary<int, PlayerInfo> players = new Dictionary<int, PlayerInfo>();

        static int heroID = -1;
        public static int HeroID { get { return heroID; } }
        public static bool TryGetHeroInfo(out PlayerInfo info)
        {
            return TryGetInfo(heroID, out info);
        }

        public static bool TryGetInfo(int id, out PlayerInfo info)
        {
            return players.TryGetValue(id, out info);
        }

        int id;
        public int ID { get { return id; } }

        string name;
        public string Name { get { return name; } }

        public static void ReadHeroInfo(PacketReader stream)
        {
            heroID = stream.ReadByte();
        }

        public static void ReadPlayerInfoMessage(PacketReader stream)
        {
            int id = stream.ReadByte();
            PlayerInfo pi;
            if (!players.TryGetValue(id, out pi))
            {
                pi = new PlayerInfo();
                pi.id = id;
                players.Add(id, pi);
            }

            pi.name = stream.ReadString();
        }

        public static void ReadPlayerQuitMessage(PacketReader stream)
        {
            players.Remove(stream.ReadByte());
        }
    }
}
