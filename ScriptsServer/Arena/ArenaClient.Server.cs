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
        public int DuelWins;

        #endregion

        #region TeamObjective

        public TOTeamInst Team;

        #endregion

        partial void pOnConnect()
        {
            Spectate();

            TeamMode.CheckStartTO();
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
                    charInfo.Read(stream);
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
                    DuelMode.ReadScoreMessage(this, stream);
                    break;
            }
        }

        CharCreationInfo charInfo = new CharCreationInfo();
        public CharCreationInfo CharInfo { get { return charInfo; } }

        void JoinGame()
        {
            if (this.IsCharacter)
                return;

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
