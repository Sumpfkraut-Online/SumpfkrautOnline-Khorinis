using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.Scripts.Sumpfkraut.Networking;
using GUC.Client.Scripts.TFFA;

namespace GUC.Scripts.TFFA
{
    partial class TFFAClient
    {
        public static TFFAPhase Status { get; private set; }
        public static Team Winner { get; private set; }

        new public static TFFAClient Client { get { return (TFFAClient)ScriptClient.Client; } }

        public static ClientInfo Info { get; private set; }

        public static long PhaseEndTime { get; private set; }

        public override void ReadScriptMsg(PacketReader stream)
        {
            try
            {
                MenuMsgID id = (MenuMsgID)stream.ReadByte();
                Log.Logger.Log("Menu MSG: " + id);
                switch (id)
                {
                    case MenuMsgID.ClientInfoGroup:
                        int thisID = stream.ReadByte();
                        Status = (TFFAPhase)stream.ReadByte();
                        if (Status != TFFAPhase.Fight)
                            StatusMenu.Menu.StatusShow = true;

                        PhaseEndTime = stream.ReadUInt() * TimeSpan.TicksPerSecond + GameTime.Ticks;

                        int count = stream.ReadByte();
                        for (int i = 0; i < count; i++)
                        {
                            var c = ClientInfo.Read(stream);
                            if (c.ID == thisID)
                            {
                                Info = c;
                            }
                        }
                        ClassMenu.Menu.UpdateCounts();
                        TeamMenu.Menu.UpdateCounts();
                        Scoreboard.Menu.UpdateStats();
                        break;
                    case MenuMsgID.ClientConnect:
                        ClientInfo.Read(stream);
                        ClassMenu.Menu.UpdateCounts();
                        TeamMenu.Menu.UpdateCounts();
                        Scoreboard.Menu.UpdateStats();
                        break;

                    case MenuMsgID.ClientDisconnect:
                        ClientInfo.ClientInfos.Remove(stream.ReadByte());
                        ClassMenu.Menu.UpdateCounts();
                        TeamMenu.Menu.UpdateCounts();
                        Scoreboard.Menu.UpdateStats();
                        break;

                    case MenuMsgID.ClientTeam:
                        int clientID = stream.ReadByte();
                        ClientInfo ci;
                        if (ClientInfo.ClientInfos.TryGetValue(clientID, out ci))
                        {
                            ci.Team = (Team)stream.ReadByte();
                            ci.Class = PlayerClass.None;
                            if (ci == Info)
                            {
                                if (ci.Team != Team.Spec)
                                {
                                    ClassMenu.Menu.Open();
                                }
                                else
                                {
                                    TeamMenu.Menu.Open();
                                }
                            }
                            TeamMenu.Menu.UpdateCounts();
                            Scoreboard.Menu.UpdateStats();
                        }
                        break;
                    case MenuMsgID.ClientClass:
                        clientID = stream.ReadByte();
                        if (ClientInfo.ClientInfos.TryGetValue(clientID, out ci))
                        {
                            ci.Class = (PlayerClass)stream.ReadByte();
                            ClassMenu.Menu.UpdateCounts();
                        }
                        break;
                    case MenuMsgID.ClientName:
                        clientID = stream.ReadByte();
                        if (ClientInfo.ClientInfos.TryGetValue(clientID, out ci))
                        {
                            ci.Name = stream.ReadString();
                            Scoreboard.Menu.UpdateNames();

                            WorldObjects.NPC npc;
                            if (WorldObjects.World.Current.TryGetVob(ci.CharID, out npc))
                            {
                                npc.gVob.Name.Set(InputControl.ClientsShown ? string.Format("({0}){1}", ci.ID, ci.Name) : ci.Name);
                            }
                        }
                        break;
                    case MenuMsgID.ClientNPC:
                        clientID = stream.ReadByte();
                        if (ClientInfo.ClientInfos.TryGetValue(clientID, out ci))
                        {
                            ci.CharID = stream.ReadUShort();
                            
                            WorldObjects.NPC npc;
                            if (WorldObjects.World.Current.TryGetVob(ci.CharID, out npc))
                            {
                                npc.gVob.Name.Set(InputControl.ClientsShown ? string.Format("({0}){1}", ci.ID, ci.Name) : ci.Name);
                            }
                        }
                        break;

                    case MenuMsgID.OpenScoreboard:
                        int alKills = stream.ReadByte();
                        int nlKills = stream.ReadByte();
                        count = stream.ReadByte();
                        for (int i = 0; i < count; i++)
                        {
                            ClientInfo.ReadScoreboardInfo(stream);
                        }
                        Scoreboard.Menu.UpdateStats(alKills, nlKills);
                        break;

                    case MenuMsgID.WinMsg:
                        Status = TFFAPhase.End;
                        Winner = (Team)stream.ReadByte();
                        PhaseEndTime = stream.ReadUInt() * TimeSpan.TicksPerSecond + GameTime.Ticks;
                        StatusMenu.Menu.StatusShow = true;
                        break;
                    case MenuMsgID.PhaseMsg:
                        Status = (TFFAPhase)stream.ReadByte();
                        PhaseEndTime = stream.ReadUInt() * TimeSpan.TicksPerSecond + GameTime.Ticks;
                        Winner = Team.Spec;
                        if (Status == TFFAPhase.Fight)
                        {
                            StatusMenu.Menu.StatusShow = false;
                        }
                        else
                        {
                            StatusMenu.Menu.StatusShow = true;
                        }
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
