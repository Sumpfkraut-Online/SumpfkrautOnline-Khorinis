using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.Client.Scripts.TFFA;

namespace GUC.Scripts.TFFA
{
    partial class TFFAClient : GameClient.IScriptClient
    {
        public static TFFAClient Client { get { return GameClient.Client == null ? null : (TFFAClient)GameClient.Client.ScriptObject; } }

        public void ReadScriptMsg(PacketReader stream)
        {
            MenuMsgID id = (MenuMsgID)stream.ReadByte();
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
                    if (this.Team != Team.Spec)
                    {
                        ClassMenu.Menu.Open();
                    }
                    break;
                case MenuMsgID.SelectClass:
                    break;
                case MenuMsgID.SetName:
                    break;

                case MenuMsgID.OpenScoreboard:
                    Scoreboard.Menu.SetTime(stream.ReadInt());
                    break;
            }
        }
    }
}
