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
using GUC.Scripts.Arena.Duel;
using GUC.Scripts.Arena.GameModes;
using GUC.Scripts.Arena.GameModes.TDM;
using GUC.Scripts.Arena.GameModes.Horde;
using GUC.Scripts.Arena.GameModes.BattleRoyale;

namespace GUC.Scripts.Arena
{
    partial class ArenaClient
    {
        public static void ForEach(Action<ArenaClient> action)
        {
            GameClient.ForEach(c => action((ArenaClient)c.ScriptObject));
        }

        #region GameModes

        public NPCClass GMClass;
        public bool GMJoined { get { return GMTeamID >= TeamIdent.GMSpectator; } }
        public TeamIdent GMTeamID = TeamIdent.None;
        public void SetTeamID(TeamIdent id)
        {
            if (GMTeamID == id)
                return;

            GMTeamID = id;
            var stream = GetStream(ScriptMessages.PlayerInfoTeam);
            stream.Write((byte)this.ID);
            stream.Write((sbyte)this.GMTeamID);
            ForEach(c => c.SendScriptMessage(stream, NetPriority.Low, NetReliability.ReliableOrdered));
        }

        public float GMScore;
        public int GMKills;
        public int GMDeaths;

        // TDM
        public TDMTeamInst TDMTeam;

        #endregion

        #region DuelMode

        public List<ArenaClient, GUCTimer> DuelRequests = new List<ArenaClient, GUCTimer>(3);
        public ArenaClient DuelEnemy;
        public bool IsDueling { get { return this.DuelEnemy != null; } }

        public int DuelScore;
        public int DuelKills;
        public int DuelDeaths;

        #endregion

        public static PacketWriter GetStream(ScriptMessages id)
        {
            var s = GameClient.GetScriptMessageStream();
            s.Write((byte)id);
            return s;
        }

        static void SendGameInfo(ArenaClient client)
        {
            var stream = ArenaClient.GetScriptMessageStream();
            stream.Write((byte)ScriptMessages.GameInfo);
            stream.Write((byte)client.ID);

            stream.Write((byte)ArenaClient.GetCount());
            ArenaClient.ForEach(c => c.WritePlayerInfo(stream));

            GameMode.WriteGameInfo(stream);

            client.SendScriptMessage(stream, NetPriority.Low, NetReliability.ReliableOrdered);
        }

        void WritePlayerInfo(PacketWriter stream)
        {
            stream.Write((byte)this.ID);
            stream.Write(this.CharInfo.Name);
            stream.Write((sbyte)this.GMTeamID);
        }

        void SendPlayerInfoMessage()
        {
            var stream = GetScriptMessageStream();
            stream.Write((byte)ScriptMessages.PlayerInfo);
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
        }

        partial void pOnDisconnect(int id)
        {
            if (!GameMode.IsActive || !GameMode.ActiveMode.Leave(this))
                KillCharacter();

            var stream = GetStream(ScriptMessages.PlayerQuit);
            stream.Write((byte)id);
            ForEach(c => c.SendScriptMessage(stream, NetPriority.Low, NetReliability.ReliableOrdered));
            DuelBoard.Instance.Remove(this);
            TDMScoreBoard.Instance.Remove(this);
            HordeBoard.Instance.Remove(this);
        }

        public override void ReadScriptMessage(PacketReader stream)
        {
            ScriptMessages id = (ScriptMessages)stream.ReadByte();
            switch (id)
            {
                case ScriptMessages.JoinGame:
                    FMSpawn();
                    break;
                case ScriptMessages.Spectate:
                    FMSpectate();
                    break;
                case ScriptMessages.CharEdit:
                    string oldName = charInfo.Name;
                    charInfo.Read(stream);
                    if (oldName != charInfo.Name)
                        SendPlayerInfoMessage();
                    if (GMTeamID == TeamIdent.FFAPlayer)
                        FMSpawn();
                    break;
                case ScriptMessages.DuelRequest:
                    DuelMode.ReadRequest(this, stream);
                    break;
                case ScriptMessages.ChatMessage:
                    Chat.ReadMessage(this, stream);
                    break;
                case ScriptMessages.DuelScoreBoard:
                    DuelBoard.Instance.Toggle(this, stream.ReadBit());
                    break;

                case ScriptMessages.ModeSpectate:
                    if (GameMode.IsActive)
                        GameMode.ActiveMode.JoinAsSpectator(this);
                    break;

                case ScriptMessages.TDMTeamSelect:
                    if (TDMMode.IsActive)
                        TDMMode.ActiveMode.JoinTeam(this, stream.ReadByte());
                    break;
                case ScriptMessages.TDMScoreBoard:
                    TDMScoreBoard.Instance.Toggle(this, TDMMode.IsActive ? stream.ReadBit() : false);
                    break;

                case ScriptMessages.ModeClassSelect:
                    if (GameMode.IsActive)
                        GameMode.ActiveMode.SelectClass(this, stream.ReadByte());
                    break;

                case ScriptMessages.BRJoin:
                    if (BRMode.IsActive)
                        BRMode.ActiveMode.Join(this);
                    break;

                case ScriptMessages.HordeScoreBoard:
                    HordeBoard.Instance.Toggle(this, HordeMode.IsActive ? stream.ReadBit() : false);
                    break;
            }
        }

        CharCreationInfo charInfo = new CharCreationInfo();
        public CharCreationInfo CharInfo { get { return charInfo; } }

        public void FMSpectate()
        {
            if (GameMode.IsActive)
                GameMode.ActiveMode.Leave(this);

            KillCharacter();
            SetTeamID(TeamIdent.FFASpectator);
            this.SetToSpectator(WorldInst.List[0], new Vec3f(-6489, -480, 3828), new Angles(0.1151917f, -2.104867f, 0f));
        }

        public void FMSpawn()
        {
            if (GameMode.IsActive)
                GameMode.ActiveMode.Leave(this);

            KillCharacter();
            SetTeamID(TeamIdent.FFAPlayer);

            NPCDef def = NPCDef.Get(charInfo.BodyMesh == HumBodyMeshs.HUM_BODY_NAKED0 ? "maleplayer" : "femaleplayer");
            NPCInst npc = new NPCInst(def)
            {
                UseCustoms = true,
                CustomBodyTex = charInfo.BodyTex,
                CustomHeadMesh = charInfo.HeadMesh,
                CustomHeadTex = charInfo.HeadTex,
                CustomVoice = charInfo.Voice,
                CustomFatness = charInfo.Fatness,
                CustomScale = new Vec3f(charInfo.BodyWidth, 1.0f, charInfo.BodyWidth),
                CustomName = charInfo.Name,
                DropUnconsciousOnDeath = true,
                UnconsciousDuration = 15 * TimeSpan.TicksPerSecond,
            };

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

        public void KillCharacter()
        {
            if (this.Character == null || this.Character.IsDead)
                return;

            if (GameMode.IsActive && this.GMTeamID >= TeamIdent.GMPlayer)
            {
                GameMode.ActiveMode.OnSuicide(this);
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
