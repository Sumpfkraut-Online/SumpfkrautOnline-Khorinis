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
        static HordeMode()
        {
            NPCInst.sOnHit += NPCInst_sOnHit;
        }

        static void NPCInst_sOnHit(NPCInst attacker, NPCInst target, int damage)
        {
            if (attacker.IsPlayer)
            {
                if (target.IsPlayer)
                    return;

                ArenaClient player = (ArenaClient)attacker.Client;
                if (player.HordeClass == null) return;

                player.HordeScore += damage / 10.0f;
                if (target.HP <= 0)
                {
                    player.HordeKills++;
                    player.HordeScore += 5;
                    enemies.Remove(target);
                    CheckSectionClear();
                }
            }
            else if (target.IsPlayer)
            {
                ArenaClient player = (ArenaClient)target.Client;
                if (player.HordeClass == null) return;

                if (target.HP <= 1)
                {
                    player.HordeDeaths++;
                    if (Players.TrueForAll(p => p.Character.HP <= 1))
                        EndHorde(false);
                }
            }
        }

        static GUCTimer barrierTimer = new GUCTimer();

        static WorldInst activeWorld;

        static List<VobInst> barriers = new List<VobInst>(10);
        static HashSet<NPCInst> enemies = new HashSet<NPCInst>();
        public static HashSet<NPCInst> Enemies { get { return enemies; } }

        static List<ArenaClient> players = new List<ArenaClient>(10);
        public static List<ArenaClient> Players { get { return players; } }

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

            ArenaClient.ForEach(c =>
            {
                var client = (ArenaClient)c;
                client.HordeScore = 0;
                client.HordeDeaths = 0;
                client.HordeKills = 0;
                client.HordeClass = null;
            });

            players.Clear();

            var stream = ArenaClient.GetScriptMessageStream();
            stream.Write((byte)ScriptMessages.HordeStart);
            stream.Write(def.Name);
            ArenaClient.ForEach(c => c.SendScriptMessage(stream, NetPriority.Low, NetReliability.ReliableOrdered));

            var world = WorldInst.List.Find(w => w.Path == def.WorldPath);

            activeDef = def;
            activeWorld = world;
            activeSectionIndex = 0;

            foreach (var sec in activeDef.Sections)
                if (sec.barriers != null)
                    foreach (var bar in sec.barriers)
                    {
                        VobInst vob = new VobInst(VobDef.Get(bar.Definition));
                        vob.Spawn(activeWorld, bar.Position, bar.Angles);
                        barriers.Add(vob);
                    }

            SpawnSection(ActiveSection);

            SetPhase(HordePhase.Intermission);
            CheckSectionClear();
        }

        public static void ForceNextSection()
        {
            foreach (var npc in enemies)
                npc.SetHealth(0);
            enemies.Clear();
            CheckSectionClear();
        }

        static void EndHorde(bool victory)
        {
            SetPhase(victory ? HordePhase.Victory : HordePhase.Lost);
            players.ForEach(p => p.Character.LiftUnconsciousness());
            barriers.ForEach(b => b.Despawn());
            barriers.Clear();

            barrierTimer.SetCallback(StartHorde);
            barrierTimer.SetInterval(60 * TimeSpan.TicksPerSecond);
            barrierTimer.Start();
        }

        static void CheckSectionClear()
        {
            if (enemies.Count > 0 || players.Count == 0)
                return;

            players.ForEach(p => p.Character.LiftUnconsciousness());
            if (activeSectionIndex + 1 >= activeDef.Sections.Count)
            {
                EndHorde(true);
            }
            else
            {
                barrierTimer.SetCallback(NextSection);
                barrierTimer.SetInterval(ActiveSection.SecsTillNext * TimeSpan.TicksPerSecond);
                barrierTimer.Start();

                SetPhase(HordePhase.Intermission);
            }
        }

        static void NextSection()
        {
            barrierTimer.Stop();

            activeSectionIndex++;

            SpawnSection(ActiveSection);

            var oldBars = activeDef.Sections[activeSectionIndex - 1].barriers;
            if (oldBars != null)
            {
                for (int i = 0; i < oldBars.Count; i++)
                {
                    barriers[i].Despawn();
                }
                barriers.RemoveRange(0, oldBars.Count);
            }

            CheckSectionClear();
        }

        static void SpawnSection(HordeSection section)
        {
            if (section.groups != null)
            {
                var manager = Sumpfkraut.AI.SimpleAI.AIManager.aiManagers[0];
                foreach (var group in section.groups)
                {
                    List<VobInst> vobs = new List<VobInst>(group.npcs.Count);
                    foreach (var bots in group.npcs)
                    {
                        int maxNum = (int)Math.Ceiling(bots.Item2 * players.Count);
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

            OnPhaseChange?.Invoke(phase);
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

            foreach (string e in client.HordeClass.Equipment)
            {
                ItemInst item = new ItemInst(ItemDef.Get(e));
                npc.Inventory.AddItem(item);
                npc.EffectHandler.TryEquipItem(item);
            }

            if (client.HordeClass.NeedsBolts)
            {

            }

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
            else if (client.HordeScore == 0)
            {
                SpawnPlayer(client);
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
