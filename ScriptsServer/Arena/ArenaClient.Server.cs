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
                    if (client.ClassDef != null && TeamMode.Phase != TOPhases.None && TeamMode.Phase != TOPhases.Finish
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

        static void SendGameInfo(ArenaClient client)
        {
            var stream = ArenaClient.GetScriptMessageStream();
            stream.Write((byte)ScriptMessages.GameInfo);
            stream.Write((byte)client.ID);

            stream.Write((byte)ArenaClient.GetCount());
            ArenaClient.ForEach(c => ((ArenaClient)c).WritePlayerInfo(stream));

            TeamMode.WriteGameInfo(stream);

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
            TeamMenu.Remove(this);

            if (this.Team != null)
                this.Team.Players.Remove(this);
            this.Team = null;
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
                    npc.EquipItem(item);
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
                this.SetToSpectator(WorldInst.List[0], new Vec3f(-6489, -480, 3828), new Vec3f(0.910f, -0.063f, -0.409f));
            }
            else
            {
                this.SetToSpectator(TeamMode.World, TeamMode.ActiveTODef.SpecPos.Item1, TeamMode.ActiveTODef.SpecPos.Item2);
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
                }
            }
            else if (this.DuelEnemy != null)
            {
                DuelMode.DuelWin(this.DuelEnemy);
            }

            this.Character.SetHealth(0);
        }

        static List<Vec3f, Vec3f> spawnPositions = new List<Vec3f, Vec3f>()
        {
            { new Vec3f(-3309.678f, -611.897f, 3652.568f), new Vec3f(0.4438523f, 0f, -0.8961f) },
            { new Vec3f(-3748.823f, -631.83f, 4098.888f), new Vec3f(-0.9585717f, 0f, -0.2848513f) },
            { new Vec3f(-4857.682f, -861.8788f, 4186.245f), new Vec3f(-0.2697614f, 0f, -0.9629272f) },
            { new Vec3f(-5558.795f, -881.8917f, 4982.458f), new Vec3f(-0.7366884f, 0f, -0.6762323f) },
            { new Vec3f(-5431.704f, -1086.897f, 2389.581f), new Vec3f(-0.6826372f, 0f, -0.7307576f) },
            { new Vec3f(-7645.023f, -611.8591f, 3334.188f), new Vec3f(0.4344447f, 0f, -0.9006985f) },
            { new Vec3f(-1778.532f, -1021.838f, 2775.348f), new Vec3f(-0.2376849f, 0f, 0.9713424f) },
        };
    }
}
