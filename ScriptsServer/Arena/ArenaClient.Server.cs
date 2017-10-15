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
            Spectate();

            TeamMode.CheckStartTO();
        }

        partial void pOnDisconnect()
        {
            KillCharacter();

            var stream = GetScriptMessageStream();
            stream.Write((byte)ScriptMessages.PlayerQuitMessage);
            stream.Write((byte)this.ID);
            ForEach(c => c.SendScriptMessage(stream, NetPriority.Low, NetReliability.ReliableOrdered));
            DuelBoard.Instance.Remove(this);
            TOBoard.Instance.Remove(this);

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
            npc.Spawn(WorldInst.Current, pair.Item1, pair.Item2);
            this.SetControl(npc);
        }

        public void Spectate()
        {
            KillCharacter();
            TeamMode.JoinTeam(this, null);
            if (this.IsCharacter)
            {
                var npc = this.Character;
                this.SetToSpectator(npc.World, npc.GetPosition(), npc.GetDirection());
            }
            else if (!this.IsSpecating)
            {
                this.SetToSpectator(WorldInst.Current, new Vec3f(0, 1000, 0), new Vec3f(0, 0, 1));
            }
        }

        public void KillCharacter()
        {
            if (this.Character == null || this.Character.IsDead)
                return;

            if (this.Team != null)
            {
                this.TODeaths++;
                this.TOScore--;
                this.Team.Score--;
            }
            else if (this.DuelEnemy != null)
            {
                DuelMode.DuelWin(this.DuelEnemy);
            }

            this.Character.SetHealth(0);
        }

        static List<Vec3f, Vec3f> spawnPositions = new List<Vec3f, Vec3f>()
        {
            { new Vec3f(1103.581f, 247.9452f, 789.3878f), new Vec3f(-0.2739599f, 0f, -0.9617411f) },
            { new Vec3f(1103.581f, 247.9272f, 789.3878f), new Vec3f(-0.2739599f, 0f, -0.9617411f) },
            { new Vec3f(3125.09f, 248.0491f, -149.3765f), new Vec3f(-0.9181013f, 0f, -0.3963461f) },
            { new Vec3f(3125.09f, 248.0578f, -149.3765f), new Vec3f(-0.9181013f, 0f, -0.3963461f) },
            { new Vec3f(2819.372f, 247.8344f, -1710.958f), new Vec3f(-0.4671577f, 0f, 0.884174f) },
            { new Vec3f(2819.372f, 247.8455f, -1710.958f), new Vec3f(-0.4671577f, 0f, 0.884174f) },
            { new Vec3f(-79.50902f, 234.1128f, -1803.163f), new Vec3f(0.08106972f, 0f, 0.9967085f) },
            { new Vec3f(-79.50902f, 234.0981f, -1803.163f), new Vec3f(0.08106972f, 0f, 0.9967085f) },
            { new Vec3f(-1675.428f, -95.8408f, 1258.567f), new Vec3f(-0.4602006f, 0f, -0.8878149f) },
            { new Vec3f(-1675.428f, -95.82971f, 1258.567f), new Vec3f(-0.4602006f, 0f, -0.8878149f) },
            { new Vec3f(-4405.862f, -151.877f, -450.7282f), new Vec3f(0.9719613f, 0f, 0.235141f) },
            { new Vec3f(-4405.862f, -151.8656f, -450.7282f), new Vec3f(0.9719613f, 0f, 0.235141f) },
            { new Vec3f(-4092.241f, -284.683f, -2645.511f), new Vec3f(-0.02792043f, 0f, 0.9996101f) },
            { new Vec3f(-4092.241f, -284.6982f, -2645.511f), new Vec3f(-0.02792043f, 0f, 0.9996101f) },
        };
    }
}
