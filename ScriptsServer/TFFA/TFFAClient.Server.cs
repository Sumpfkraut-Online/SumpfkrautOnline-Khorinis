using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.Server.Scripts.TFFA;

namespace GUC.Scripts.TFFA
{
    partial class TFFAClient : GameClient.IScriptClient
    {
        public int Kills = 0;
        public int Deaths = 0;
        public int Damage = 0;

        public static void ForEach(Action<TFFAClient> action)
        {
            GameClient.ForEach(gc => action((TFFAClient)gc.ScriptObject));
        }

        partial void pConstruct()
        {
            TFFAGame.AddToTeam(this, Team.Spec, true);
            var stream = GameClient.GetMenuMsgStream();
            stream.Write((byte)MenuMsgID.PhaseMsg);
            stream.Write((byte)TFFAGame.Status);
            this.baseClient.SendMenuMsg(stream, PktPriority.LOW_PRIORITY, PktReliability.RELIABLE);
        }

        public void OnDisconnection()
        {
            TFFAGame.RemoveFromTeam(this);
            TFFAGame.teamMenuClients.Remove(this);
            TFFAGame.classMenuClients.Remove(this);
            TFFAGame.scoreboardClients.Remove(this);
            TFFAGame.Kill(this);
        }

        public void OnReadMenuMsg(PacketReader stream)
        {
            MenuMsgID id = (MenuMsgID)stream.ReadByte();
            switch (id)
            {
                case MenuMsgID.OpenTeamMenu:
                    TFFAGame.teamMenuClients.Add(this);
                    TFFAGame.UpdateStats();
                    break;
                case MenuMsgID.CloseTeamMenu:
                    TFFAGame.teamMenuClients.Remove(this);
                    break;
                case MenuMsgID.OpenClassMenu:
                    TFFAGame.classMenuClients.Add(this);
                    TFFAGame.UpdateStats();
                    break;
                case MenuMsgID.CloseClassMenu:
                    TFFAGame.classMenuClients.Remove(this);
                    break;
                case MenuMsgID.OpenScoreboard:
                    TFFAGame.scoreboardClients.Add(this);
                    TFFAGame.UpdateStats();
                    break;
                case MenuMsgID.CloseScoreboard:
                    TFFAGame.scoreboardClients.Remove(this);
                    break;
                case MenuMsgID.SelectTeam:
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
                case MenuMsgID.SelectClass:
                    TFFAGame.SelectClass(this, (PlayerClass)stream.ReadByte());
                    break;
                case MenuMsgID.SetName:
                    string newName = stream.ReadString();
                    if (!string.IsNullOrWhiteSpace(newName))
                        this.Name = newName;
                    break;
            }

        }

        public void OnReadIngameMsg(PacketReader stream)
        {
        }
    }
}
