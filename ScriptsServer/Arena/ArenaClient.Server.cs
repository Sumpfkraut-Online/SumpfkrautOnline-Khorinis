using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.Scripts.Sumpfkraut.WorldSystem;
using GUC.Types;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;
using GUC.Log;
using GUC.Utilities;
using GUC.Scripting;
using GUC.Scripts.Sumpfkraut.Visuals;

namespace GUC.Scripts.Arena
{
    partial class ArenaClient
    {
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
            if (this.IsCharacter)
                return;

            TeamMode.JoinTeam(this, null);

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

            var item = new ItemInst(ItemDef.Get("ItMw_1h_Bau_Mace"));
            npc.Inventory.AddItem(item);
            npc.EquipItem(item);

            item = new ItemInst(ItemDef.Get("ITAR_Prisoner"));
            npc.Inventory.AddItem(item);
            npc.EquipItem(item);

            ScriptOverlay ov;
            if (npc.ModelDef.TryGetOverlay("1HST1", out ov))
                npc.ModelInst.ApplyOverlay(ov);

            npc.Spawn(WorldInst.Current);
            this.SetControl(npc);
        }

        void Spectate()
        {
            if (this.IsSpecating)
                return;

            TeamMode.JoinTeam(this, null);
            if (this.IsCharacter)
            {
                var npc = this.Character;
                this.SetToSpectator(npc.World, npc.GetPosition(), npc.GetDirection());
                npc.Despawn();
            }
            else
            {
                this.SetToSpectator(WorldInst.Current, new Vec3f(), new Vec3f());
            }
        }
    }
}
