using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.Scripts.Sumpfkraut.WorldSystem;
using GUC.Types;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;
using GUC.Utilities;
using GUC.Scripting;
using GUC.Scripts.Sumpfkraut.Visuals;

namespace GUC.Scripts.Arena
{
    partial class ArenaClient
    {
        #region Respawn

        const long RespawnInterval = 10 * TimeSpan.TicksPerSecond;
        static GUCTimer respawnTimer = new GUCTimer(RespawnInterval, RespawnPlayers);

        static ArenaClient()
        {
            respawnTimer.Start();
        }

        static void RespawnPlayers()
        {
            ArenaClient.ForEach(s =>
            {
                ArenaClient client = (ArenaClient)s;
                if (client.Team != null)
                {
                    if (client.TOClass != null && TeamMode.Phase != TOPhases.None && TeamMode.Phase != TOPhases.Finish
                        && (client.Character == null || client.Character.IsDead))
                        TeamMode.SpawnCharacter(client);
                }
                else
                {
                    if (!client.IsSpecating && client.Character != null && client.Character.IsDead)
                        client.SpawnCharacter();
                }
            });
        }

        #endregion

        #region DuelMode

        public List<ArenaClient, GUCTimer> DuelRequests = new List<ArenaClient, GUCTimer>(3);
        public ArenaClient DuelEnemy;
        public bool IsDueling { get { return this.DuelEnemy != null; } }

        public int DuelScore;
        public int DuelKills;
        public int DuelDeaths;

        #endregion

        #region TeamObjective

        public TOTeamInst Team;

        public int TOScore;
        public int TOKills;
        public int TODeaths;

        #endregion

        #region Horde

        public float HordeScore;
        public int HordeKills;
        public int HordeDeaths;

        #endregion

        static void SendGameInfo(ArenaClient client)
        {
            var stream = ArenaClient.GetScriptMessageStream();
            stream.Write((byte)ScriptMessages.GameInfo);
            stream.Write((byte)client.ID);

            stream.Write((byte)ArenaClient.GetCount());
            ArenaClient.ForEach(c => ((ArenaClient)c).WritePlayerInfo(stream));

            TeamMode.WriteGameInfo(stream);
            HordeMode.WriteGameInfo(stream);

            stream.Write((uint)respawnTimer.GetRemainingTicks());

            client.SendScriptMessage(stream, NetPriority.Low, NetReliability.ReliableOrdered);
        }

        void WritePlayerInfo(PacketWriter stream)
        {
            stream.Write((byte)this.ID);
            stream.Write(this.CharInfo.Name);
        }

        void SendPlayerInfoMessage()
        {
            var stream = GetScriptMessageStream();
            stream.Write((byte)ScriptMessages.PlayerInfoMessage);
            WritePlayerInfo(stream);
            ForEach(c => c.SendScriptMessage(stream, NetPriority.Low, NetReliability.ReliableOrdered));
        }

        public void SendPointsMessage(sbyte points)
        {
            var stream = ArenaClient.GetScriptMessageStream();
            stream.Write((byte)ScriptMessages.PointsMessage);
            stream.Write(points);
            this.SendScriptMessage(stream, NetPriority.Low, NetReliability.Unreliable);
        }

        partial void pOnConnect()
        {
            SendGameInfo(this);
            SendPlayerInfoMessage();
            TeamMode.CheckStartTO();
        }

        partial void pOnDisconnect(int id)
        {
            KillCharacter();

            var stream = GetScriptMessageStream();
            stream.Write((byte)ScriptMessages.PlayerQuitMessage);
            stream.Write((byte)id);
            ForEach(c => c.SendScriptMessage(stream, NetPriority.Low, NetReliability.ReliableOrdered));
            DuelBoard.Instance.Remove(this);
            TOBoard.Instance.Remove(this);
            HordeBoard.Instance.Remove(this);
            TeamMenu.Remove(this);

            if (this.Team != null)
                this.Team.Players.Remove(this);
            this.Team = null;

            HordeMode.LeaveHorde(this);
        }

        public override void ReadScriptMessage(PacketReader stream)
        {
            ScriptMessages id = (ScriptMessages)stream.ReadByte();
            switch (id)
            {
                case ScriptMessages.JoinGame:
                    JoinGame();
                    break;
                case ScriptMessages.Spectate:
                    Spectate();
                    break;
                case ScriptMessages.CharEdit:
                    string oldName = charInfo.Name;
                    charInfo.Read(stream);
                    if (oldName != charInfo.Name)
                        SendPlayerInfoMessage();
                    break;
                case ScriptMessages.DuelRequest:
                    DuelMode.ReadRequest(this, stream);
                    break;
                case ScriptMessages.TOJoinTeam:
                    TeamMode.ReadJoinTeam(this, stream);
                    break;
                case ScriptMessages.TOSelectClass:
                    TeamMode.ReadSelectClass(this, stream);
                    break;
                case ScriptMessages.ChatMessage:
                    Chat.ReadMessage(this, stream);
                    break;
                case ScriptMessages.ChatTeamMessage:
                    Chat.ReadTeamMessage(this, stream);
                    break;
                case ScriptMessages.ScoreDuelMessage:
                    DuelBoard.Instance.Toggle(this);
                    break;
                case ScriptMessages.ScoreTOMessage:
                    TOBoard.Instance.Toggle(this);
                    break;
                case ScriptMessages.TOTeamCount:
                    TeamMenu.Toggle(this);
                    break;
                case ScriptMessages.SpectateTeam:
                    Spectate(true);
                    break;
                case ScriptMessages.HordeJoin:
                    HordeMode.JoinClass(stream, this);
                    break;
                case ScriptMessages.HordeSpectate:
                    HordeMode.JoinSpectate(this);
                    break;
                case ScriptMessages.ScoreHordeMessage:
                    HordeBoard.Instance.Toggle(this);
                    break;
            }
        }

        CharCreationInfo charInfo = new CharCreationInfo();
        public CharCreationInfo CharInfo { get { return charInfo; } }

        void JoinGame()
        {
            if (this.Character != null)
            {
                if (this.Team != null)
                {
                    TeamMode.JoinTeam(this, null);
                }
                else
                {
                    return;
                }
            }

            SpawnCharacter();
        }

        public void SpawnCharacter()
        {
            KillCharacter();

            NPCDef def = NPCDef.Get(charInfo.BodyMesh == HumBodyMeshs.HUM_BODY_NAKED0 ? "maleplayer" : "femaleplayer");
            NPCInst npc = new NPCInst(def);
            npc.UseCustoms = true;
            npc.CustomBodyTex = charInfo.BodyTex;
            npc.CustomHeadMesh = charInfo.HeadMesh;
            npc.CustomHeadTex = charInfo.HeadTex;
            npc.CustomVoice = charInfo.Voice;
            npc.CustomFatness = charInfo.Fatness;
            npc.CustomScale = new Vec3f(charInfo.BodyWidth, 1.0f, charInfo.BodyWidth);
            npc.CustomName = charInfo.Name;

            ItemDef.ForEach(itemDef =>
            {
                var item = new ItemInst(itemDef);
                npc.Inventory.AddItem(item);
                if (string.Equals(itemDef.CodeName, "ItMw_1h_Bau_Mace", StringComparison.OrdinalIgnoreCase)
                 || string.Equals(itemDef.CodeName, "ITAR_Prisoner", StringComparison.OrdinalIgnoreCase))
                {
                    npc.EffectHandler.TryEquipItem(item);
                }
            });

            if (npc.ModelDef.TryGetOverlay("1HST1", out ScriptOverlay ov))
                npc.ModelInst.ApplyOverlay(ov);
            if (npc.ModelDef.TryGetOverlay("2HST1", out ov))
                npc.ModelInst.ApplyOverlay(ov);

            var pair = spawnPositions.GetRandom();
            npc.Spawn(WorldInst.List[0], pair.Item1, pair.Item2);
            this.SetControl(npc);
        }

        public void Spectate(bool team = false)
        {
            if (team && (!TeamMode.IsRunning || TeamMode.Phase == TOPhases.None))
                return;

            KillCharacter();
            TeamMode.JoinTeam(this, null);

            if (!team)
            {
                this.SetToSpectator(WorldInst.List[0], new Vec3f(-6489, -480, 3828), new Angles(0.1151917f, -2.104867f, 0f));
            }
            else
            {
                var specPos = TeamMode.ActiveTODef.SpecPos;
                this.SetToSpectator(TeamMode.World, specPos.Item1, specPos.Item2);
            }
        }

        public void KillCharacter()
        {
            if (this.Character == null || this.Character.IsDead)
                return;

            if (this.Team != null)
            {
                if (TeamMode.Phase == TOPhases.Battle)
                {
                    this.TODeaths++;
                    this.TOScore--;
                    this.Team.Score--;
                    if (this.ID != -1)
                        SendPointsMessage(-1);
                }
            }
            else if (this.DuelEnemy != null)
            {
                DuelMode.DuelWin(this.DuelEnemy);
            }

            this.Character.SetHealth(0);
        }

        static List<Vec3f, Angles> spawnPositions = new List<Vec3f, Angles>()
        {
            { new Vec3f(-3309.678f, -611.8168f, 3652.568f), new Angles(0f, -2.81347f, 0f) },
            { new Vec3f(-4083.023f, -631.8246f, 4190.007f), new Angles(0f, -2.680825f, 0f) },
            { new Vec3f(-4813.559f, -861.8536f, 4222.456f), new Angles(0f, 2.823943f, 0f) },
            { new Vec3f(-5527.228f, -881.897f, 4944.889f), new Angles(0f, 2.035054f, 0f) },
            { new Vec3f(-7512.823f, -611.8413f, 3399.999f), new Angles(0f, 2.600541f, 0f) },
            { new Vec3f(-5432.846f, -1086.882f, 2386.581f), new Angles(0f, 2.104867f, 0f) },
        };
    }
}
