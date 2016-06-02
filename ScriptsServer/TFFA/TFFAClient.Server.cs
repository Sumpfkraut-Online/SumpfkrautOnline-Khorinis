using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.Server.Scripts.TFFA;

namespace GUC.Scripts.TFFA
{
    partial class TFFAClient
    {
        public PlayerClass Class = PlayerClass.None;
        public Team Team = Team.Spec;
        public string Name = "Spieler";

        public int Kills = 0;
        public int Deaths = 0;
        public int Damage = 0;

        public static void ForEach(Action<TFFAClient> action)
        {
            GameClient.ForEach(gc => action((TFFAClient)gc.ScriptObject));
        }

        public override void OnConnection()
        {
            TFFAGame.AddToTeam(this, Team.Spec, true);
            this.SendClientInfosToThis();
            this.SendThisClientInfo();
        }

        public override void OnDisconnection()
        {
            this.SendDisconnect();
            TFFAGame.scoreboardClients.Remove(this);
            TFFAGame.RemoveFromTeam(this);
            TFFAGame.Kill(this);
        }

        public override void OnReadMenuMsg(PacketReader stream)
        {
            MenuMsgID id = (MenuMsgID)stream.ReadByte();
            switch (id)
            {
                case MenuMsgID.OpenScoreboard:
                    TFFAGame.scoreboardClients.Add(this);
                    TFFAGame.UpdateStats(this);
                    break;
                case MenuMsgID.CloseScoreboard:
                    TFFAGame.scoreboardClients.Remove(this);
                    break;
                case MenuMsgID.ClientTeam:
                    Team team = (Team)stream.ReadByte();
                    if (team != this.Team)
                    {
                        if (team != Team.Spec)
                        {
                            int alCount = TFFAGame.GetCount(Team.AL);
                            int nlCount = TFFAGame.GetCount(Team.NL);

                            if (this.Team == Team.AL)
                                alCount--;
                            else if (this.Team == Team.NL)
                                nlCount--;

                            if (team == Team.AL)
                            {
                                if (alCount > nlCount)
                                    return;
                            }
                            else if (team == Team.NL)
                            {
                                if (nlCount > alCount)
                                    return;
                            }
                        }

                        TFFAGame.AddToTeam(this, team);
                    }
                    break;
                case MenuMsgID.ClientClass:
                    TFFAGame.SelectClass(this, (PlayerClass)stream.ReadByte());
                    break;
                case MenuMsgID.ClientName:
                    string newName = stream.ReadString();
                    if (!string.IsNullOrWhiteSpace(newName))
                        this.Name = newName;
                    this.SendNameChanged();
                    break;
            }

        }

        public override void OnReadIngameMsg(PacketReader stream)
        {
        }

        void WriteClientSteam(PacketWriter stream)
        {
            stream.Write((byte)this.ID);
            stream.Write((byte)this.Team);
            stream.Write((byte)this.Class);
            stream.Write(this.Character == null ? ushort.MaxValue : (ushort)this.Character.ID);
            stream.Write(this.Name);
        }

        void SendThisClientInfo()
        {
            PacketWriter stream = GameClient.GetMenuMsgStream();
            stream.Write((byte)MenuMsgID.ClientConnect);
            this.WriteClientSteam(stream);
            TFFAClient.ForEach(c => { if (c != this) c.BaseClient.SendMenuMsg(stream, PktPriority.LOW_PRIORITY, PktReliability.RELIABLE_ORDERED); });
        }

        void SendClientInfosToThis()
        {
            PacketWriter stream = GameClient.GetMenuMsgStream();
            stream.Write((byte)MenuMsgID.ClientInfoGroup);
            stream.Write((byte)this.ID);
            stream.Write((byte)TFFAGame.Status);
            stream.Write(TFFAGame.GetPhaseSecsLeft());
            stream.Write((byte)TFFAClient.GetCount());
            TFFAClient.ForEach(c => c.WriteClientSteam(stream));
            this.BaseClient.SendMenuMsg(stream, PktPriority.LOW_PRIORITY, PktReliability.RELIABLE_ORDERED);
        }

        void SendDisconnect()
        {
            PacketWriter stream = GameClient.GetMenuMsgStream();
            stream.Write((byte)MenuMsgID.ClientDisconnect);
            stream.Write((byte)this.ID);
            TFFAClient.ForEach(c => c.BaseClient.SendMenuMsg(stream, PktPriority.LOW_PRIORITY, PktReliability.RELIABLE_ORDERED));
        }

        public void SendTeamChanged()
        {
            PacketWriter stream = GameClient.GetMenuMsgStream();
            stream.Write((byte)MenuMsgID.ClientTeam);
            stream.Write((byte)this.ID);
            stream.Write((byte)this.Team);
            TFFAClient.ForEach(c => c.BaseClient.SendMenuMsg(stream, PktPriority.LOW_PRIORITY, PktReliability.RELIABLE_ORDERED));
        }

        public void SendClassChanged()
        {
            PacketWriter stream = GameClient.GetMenuMsgStream();
            stream.Write((byte)MenuMsgID.ClientClass);
            stream.Write((byte)this.ID);
            stream.Write((byte)this.Class);
            TFFAClient.ForEach(c => c.BaseClient.SendMenuMsg(stream, PktPriority.LOW_PRIORITY, PktReliability.RELIABLE_ORDERED));
        }

        public void SendNameChanged()
        {
            PacketWriter stream = GameClient.GetMenuMsgStream();
            stream.Write((byte)MenuMsgID.ClientName);
            stream.Write((byte)this.ID);
            stream.Write(this.Name);
            TFFAClient.ForEach(c => c.BaseClient.SendMenuMsg(stream, PktPriority.LOW_PRIORITY, PktReliability.RELIABLE_ORDERED));
        }

        public void SendNPCChanged()
        {
            PacketWriter stream = GameClient.GetMenuMsgStream();
            stream.Write((byte)MenuMsgID.ClientNPC);
            stream.Write((byte)this.ID);
            stream.Write((ushort)this.Character.ID);
            TFFAClient.ForEach(c => c.BaseClient.SendMenuMsg(stream, PktPriority.LOW_PRIORITY, PktReliability.RELIABLE_ORDERED));
        }
    }
}
