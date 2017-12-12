using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripting;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;
using GUC.Scripts.Sumpfkraut.WorldSystem;
using GUC.Types;
using GUC.Utilities;
using GUC.Network;
using GUC.Scripts.Sumpfkraut.Visuals;

namespace GUC.Scripts.Arena
{
    static partial class HordeMode
    {
        static GUCTimer barrierTimer = new GUCTimer();
        
        static WorldInst activeWorld;

        static List<VobInst> barriers = new List<VobInst>(10);
        static HashSet<NPCInst> enemies = new HashSet<NPCInst>();

        static List<ArenaClient> players = new List<ArenaClient>(10);

        static int currentIndex = 0;
        public static void StartHorde()
        {
            currentIndex++;
            if (currentIndex >= HordeDef.GetAll().Count())
                currentIndex = 0;

            StartHorde(HordeDef.GetAll().ElementAt(currentIndex));
        }

        public static bool StartHorde(string name)
        {
            HordeDef def = HordeDef.GetDef(name);
            if (def == null) return false;

            StartHorde(def);

            return true;
        }

        public static void StartHorde(HordeDef def)
        {
            if (def == null) return;

            var stream = ArenaClient.GetScriptMessageStream();
            stream.Write((byte)ScriptMessages.HordeStart);
            stream.Write(def.Name);
            ArenaClient.ForEach(c => c.SendScriptMessage(stream, NetPriority.Low, NetReliability.ReliableOrdered));

            var world = WorldInst.List.Find(w => w.Path == def.WorldPath);

            activeDef = def;
            activeWorld = world;
            activeSectionIndex = 0;

            SpawnSection(ActiveSection);
            
            SetPhase(HordePhase.Intermission);
            CheckSectionClear();
        }

        static void EndHorde()
        {

        }

        static void CheckSectionClear()
        {
            if (enemies.Count > 0 || players.Count == 0)
                return;

            barrierTimer.SetCallback(NextSection);
            barrierTimer.SetInterval(ActiveSection.SecsTillNext * TimeSpan.TicksPerSecond);
            barrierTimer.Start();

            SetPhase(HordePhase.Intermission);
        }

        static void NextSection()
        {
            barrierTimer.Stop();

            activeSectionIndex++;

            List<VobInst> oldBarriers = new List<VobInst>(barriers);
            if (activeSectionIndex >= activeDef.Sections.Count)
            {
                // Victory
            }
            else
            {
                SpawnSection(ActiveSection);
                CheckSectionClear();
            }

            oldBarriers.ForEach(b => b.Despawn());
        }

        static void SpawnSection(HordeSection section)
        {
            if (section.barriers != null)
                foreach (var bar in section.barriers)
                {
                    VobInst vob = new VobInst(VobDef.Get(bar.Definition));
                    vob.Spawn(activeWorld, bar.Position, bar.Angles);
                    barriers.Add(vob);
                }

            if (section.groups != null)
            {
                var manager = Sumpfkraut.AI.SimpleAI.AIManager.aiManagers[0];
                foreach (var group in section.groups)
                {
                    List<VobInst> vobs = new List<VobInst>(group.npcs.Count);
                    foreach (var bots in group.npcs)
                    {
                        int maxNum = bots.Item2 * players.Count;
                        for (int i = 0; i < maxNum; i++)
                        {
                            NPCInst npc = new NPCInst(NPCDef.Get(bots.Item1));
                            Vec3f spawnPos = Randomizer.GetVec3fRad(group.Position, group.Range);
                            Angles spawnAng = new Angles(0, Randomizer.GetFloat(-Angles.PI, Angles.PI), 0);
                            npc.BaseInst.SetNeedsClientGuide(true);
                            npc.Spawn(activeWorld, spawnPos, spawnAng);
                            enemies.Add(npc);
                            vobs.Add(npc);
                        }
                    }
                    var pers = new Sumpfkraut.AI.SimpleAI.AIPersonalities.SimpleAIPersonality(800, 1);
                    pers.Init(null, null);
                    var agent = new Sumpfkraut.AI.SimpleAI.AIAgent(vobs, pers);
                    manager.SubscribeAIAgent(agent);
                }
            }

            if (enemies.Count > 0)
                SetPhase(HordePhase.Fight);
        }

        static void SetPhase(HordePhase phase)
        {
            if (Phase == phase) return;

            Phase = phase;

            var stream = ArenaClient.GetScriptMessageStream();
            stream.Write((byte)ScriptMessages.HordePhase);
            stream.Write((byte)activeSectionIndex);
            stream.Write((byte)Phase);
            ArenaClient.ForEach(c => c.SendScriptMessage(stream, NetPriority.Low, NetReliability.ReliableOrdered));
        }

        static void SpawnPlayer(ArenaClient client)
        {
            var charInfo = client.CharInfo;
            NPCInst npc = new NPCInst(NPCDef.Get(charInfo.BodyMesh == HumBodyMeshs.HUM_BODY_NAKED0 ? "maleplayer" : "femaleplayer"));
            npc.UseCustoms = true;
            npc.CustomBodyTex = charInfo.BodyTex;
            npc.CustomHeadMesh = charInfo.HeadMesh;
            npc.CustomHeadTex = charInfo.HeadTex;
            npc.CustomVoice = charInfo.Voice;
            npc.CustomFatness = charInfo.Fatness;
            npc.CustomScale = new Vec3f(charInfo.BodyWidth, 1.0f, charInfo.BodyWidth);
            npc.CustomName = charInfo.Name;

            Vec3f spawnPos = Randomizer.GetVec3fRad(ActiveSection.SpawnPos, ActiveSection.SpawnRange);
            Angles spawnAng = new Angles(0, Randomizer.GetFloat(-Angles.PI, Angles.PI), 0);
            npc.Spawn(activeWorld, spawnPos, spawnAng);
            client.SetControl(npc);
        }

        public static void WriteGameInfo(PacketWriter stream)
        {
            stream.Write(activeDef.Name);
            stream.Write((byte)activeSectionIndex);
            stream.Write((byte)Phase);
        }

        public static void KillEnemy(NPCInst enemy, NPCInst player)
        {
            if (enemies.Remove(enemy))
                CheckSectionClear();

            if (!player.IsPlayer)
                return;
        }

        public static void JoinClass(PacketReader stream, ArenaClient client)
        {
            int index = stream.ReadByte();

            HordeClassDef classDef = activeDef.Classes.ElementAtOrDefault(index);
            if (classDef == null)
                return;

            client.HordeClass = classDef;
            players.Add(client);

            if (activeDef == null)
            {
                StartHorde();
            }
            else if (enemies.Count == 0)
            {
                SpawnPlayer(client);
                CheckSectionClear();
            }
        }

        public static void LeaveHorde(ArenaClient client)
        {
            client.HordeClass = null;
            if (players.Remove(client) && players.Count == 0)
            {
                barrierTimer.Stop();
            }
        }

        public static void JoinSpectate(ArenaClient client)
        {
            client.SetToSpectator(activeWorld, ActiveSection.SpecPos, ActiveSection.SpecAng);
        }
    }
}
