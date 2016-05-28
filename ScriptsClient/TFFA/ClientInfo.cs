using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;

namespace GUC.Scripts.TFFA
{
    public class ClientInfo
    {
        int id; public int ID { get { return this.id; } }
        public Team Team;
        public PlayerClass Class;
        public int CharID;
        public string Name;

        public int Kills = 0;
        public int Damage = 0;
        public int Deaths = 0;
        public int Ping = 999;

        public void ReadClientStream(PacketReader stream)
        {
            this.Team = (Team)stream.ReadByte();
            this.Class = (PlayerClass)stream.ReadByte();
            this.CharID = stream.ReadUShort();
            this.Name = stream.ReadString();
        }

        private ClientInfo(int id)
        {
            this.id = id;
            Log.Logger.Log("New Client: " + id);
        }

        public static ClientInfo Read(PacketReader stream)
        {
            int cid = stream.ReadByte();
            ClientInfo ci;
            if (!ClientInfos.TryGetValue(cid, out ci))
            {
                ci = new ClientInfo(cid);
                ClientInfos.Add(cid, ci);
            }
            ci.ReadClientStream(stream);
            return ci;
        }

        public static ClientInfo ReadScoreboardInfo(PacketReader stream)
        {
            int cid = stream.ReadByte();
            ClientInfo ci;
            if (ClientInfos.TryGetValue(cid, out ci))
            {
                ci.Kills = stream.ReadByte();
                ci.Deaths = stream.ReadByte();
                ci.Damage = stream.ReadUShort();
                ci.Ping = stream.ReadUShort();
            }
            return ci;
        }

        public static readonly Dictionary<int, ClientInfo> ClientInfos = new Dictionary<int, ClientInfo>();
    }
}
