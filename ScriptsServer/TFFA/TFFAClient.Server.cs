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
        public static void ForEach(Action<TFFAClient> action)
        {
            GameClient.ForEach(gc => action((TFFAClient)gc.ScriptObject));
        }

        partial void pConstruct()
        {
            TFFAGame.AddToTeam(this, Team.Spec);
        }

        public void OnDisconnection()
        {
            TFFAGame.RemoveFromTeam(this);
            TFFAGame.teamMenuClients.Remove(this);
            TFFAGame.classMenuClients.Remove(this);
            TFFAGame.scoreboardClients.Remove(this);
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
                    if (team != Team.Spec)
                    {
                        int teamCount = TFFAGame.GetCount(team);
                        for (int i = 0; i < (int)Team.Max; i++)
                            if (i != (int)Team.Spec && i != (int)team)
                                if (teamCount > TFFAGame.GetCount((Team)i))
                                    return; // uneven teams, can't join
                    }

                    TFFAGame.AddToTeam(this, team);

                    PacketWriter answer = GameClient.GetMenuMsgStream();
                    answer.Write((byte)MenuMsgID.SelectTeam);
                    answer.Write((byte)team);
                    this.baseClient.SendMenuMsg(answer, PktPriority.LOW_PRIORITY, PktReliability.RELIABLE);
                    break;
                case MenuMsgID.SelectClass:
                    TFFAGame.SelectClass(this, (PlayerClass)stream.ReadByte());
                    break;
                case MenuMsgID.SetName:
                    break;
            }

        }

        public void OnReadIngameMsg(PacketReader stream)
        {
        }
    }
}
