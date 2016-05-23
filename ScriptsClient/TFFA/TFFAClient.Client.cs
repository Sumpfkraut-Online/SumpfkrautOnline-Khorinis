using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.Client.Scripts.TFFA;
using GUC.Scripts.Sumpfkraut.WorldSystem;
using GUC.Types;

namespace GUC.Scripts.TFFA
{
    partial class TFFAClient : GameClient.IScriptClient
    {
        public static TFFAPhase Status { get; private set; }

        public static TFFAClient Client { get { return GameClient.Client == null ? null : (TFFAClient)GameClient.Client.ScriptObject; } }

        public void ReadScriptMsg(PacketReader stream)
        {
            try
            {
                MenuMsgID id = (MenuMsgID)stream.ReadByte();
                Log.Logger.Log("Menu MSG: " + id);
                switch (id)
                {
                    case MenuMsgID.OpenTeamMenu:
                        int countSpec = stream.ReadByte();
                        int countAL = stream.ReadByte();
                        int countNL = stream.ReadByte();
                        TeamMenu.Menu.SetCounts(countSpec, countAL, countNL);
                        break;
                    case MenuMsgID.OpenClassMenu:
                        int tLight = stream.ReadByte();
                        int tHeavy = stream.ReadByte();
                        ClassMenu.Menu.SetCounts(tLight, tHeavy);
                        break;
                    case MenuMsgID.SelectTeam:
                        this.Team = (Team)stream.ReadByte();
                        this.Class = PlayerClass.None;
                        if (this.Team != Team.Spec)
                        {
                            ClassMenu.Menu.Open();
                        }
                        else
                        {
                            TeamMenu.Menu.Open();
                        }
                        break;
                    case MenuMsgID.SelectClass:
                        break;
                    case MenuMsgID.SetName:
                        break;

                    case MenuMsgID.OpenScoreboard:
                        int secs = stream.ReadInt();
                        Scoreboard.Menu.SetTime(secs);
                        int alKills = stream.ReadByte();
                        int count = stream.ReadByte();
                        for (int i = 0; i < count; i++)
                        {
                            string name = stream.ReadString();
                            int kills = stream.ReadSByte();
                            int deaths = stream.ReadByte();
                            int damage = stream.ReadUShort();
                            Scoreboard.Menu.AddPlayer(Team.AL, name, kills, deaths, damage);
                        }
                        int nlKills = stream.ReadByte();
                        count = stream.ReadByte();
                        for (int i = 0; i < count; i++)
                        {
                            string name = stream.ReadString();
                            int kills = stream.ReadSByte();
                            int deaths = stream.ReadByte();
                            int damage = stream.ReadUShort();
                            Scoreboard.Menu.AddPlayer(Team.NL, name, kills, deaths, damage);
                        }
                        Scoreboard.Menu.SetKills(alKills, nlKills);
                        break;

                    case MenuMsgID.WinMsg:
                        Team winner = (Team)stream.ReadByte();
                        Scoreboard.Menu.OpenWinner(winner);
                        break;
                    case MenuMsgID.PhaseMsg:
                        Status = (TFFAPhase)stream.ReadByte();
                        PhaseInfo.info.SetState(Status);
                        /*if (Status == TFFAPhase.Fight)
                            GUC.Client.SoundHandler.SetPlayFightMusic(true);
                        else
                            GUC.Client.SoundHandler.SetPlayFightMusic(false);*/
                        break;
                }
            }
            catch (Exception e)
            {
                Log.Logger.LogError("ReadScriptMsg: " + e);
            }
        }
    }
}
