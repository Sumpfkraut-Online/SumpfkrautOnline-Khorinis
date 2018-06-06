using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripts.Sumpfkraut.WorldSystem;
using System.IO;
using GUC.Log;
using GUC.Types;
using System.Globalization;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;

namespace GUC.Scripts.Arena.GameModes.BattleRoyale
{
    static class BRWorldLoader
    {
        const string ZENVobSpotString = "		[% zCVobSpot:zCVob";
        const string ZENItemString = "		[% oCItem:zCVob";
        const string ZENPositionString = "			trafoOSToWSPos=vec3:";


        public static void Load(WorldInst world)
        {
            if (!File.Exists(world.Path))
            {
                Logger.LogWarning("Battle Royale World '{0}' not found!", world.Path);
            }

            using (FileStream fs = new FileStream(world.Path, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (StreamReader sr = new StreamReader(fs))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    Action<WorldInst, PosAng> spawnFunc;
                    if (line.StartsWith(ZENVobSpotString))
                        spawnFunc = SpawnSomething;
                    else if (line.StartsWith(ZENItemString))
                        spawnFunc = SpawnItem;
                    else continue;

                    for (int i = 0; i < 5; i++)
                        line = sr.ReadLine();

                    if (line == null || !line.StartsWith(ZENPositionString))
                        continue;

                    if (!ReadPosition(line, out Vec3f pos))
                        continue;

                    spawnFunc(world, new PosAng(pos, Angles.TwoPI * Randomizer.GetFloat()));

                    for (int i = 0; i < 19; i++)
                        sr.ReadLine();
                }
            }
        }

        static bool ReadPosition(string line, out Vec3f position)
        {
            float value;
            string valueStr;

            position = new Vec3f();

            int start = ZENPositionString.Length;
            for (int i = 0; i < 2; i++)
            {
                int index = line.IndexOf(' ', start);
                if (index < 0)
                    return false;

                valueStr = line.Substring(start, index - start);
                if (!float.TryParse(valueStr, NumberStyles.Float, CultureInfo.InvariantCulture, out value))
                    return false;

                position[i] = value;
                start = index + 1;
            }

            valueStr = line.Substring(start);
            if (!float.TryParse(valueStr, NumberStyles.Float, CultureInfo.InvariantCulture, out value))
                return false;

            position[2] = value;
            return true;
        }

        static void SpawnSomething(WorldInst world, PosAng posAng)
        {
            float prob = Randomizer.GetFloat();
            if (prob < BRScenario.VobSpotNPCProb)
            {
                SpawnNPC(world, posAng);
            }
            else if (prob < BRScenario.VobSpotNPCProb + BRScenario.VobSpotItemProb)
            {
                SpawnItem(world, posAng);
            }
        }

        static void SpawnItem(WorldInst world, PosAng posAng)
        {
            var item = BRScenario.Items.GetRandom();

            ItemDef def = ItemDef.Get(item.Definition);
            if (def == null)
                return;

            ItemInst inst = new ItemInst(def);
            inst.SetAmount(item.Amount);
            inst.Spawn(world, posAng.Position, posAng.Angles);
        }

        static void SpawnNPC(WorldInst world, PosAng posAng)
        {
            var npc = BRScenario.NPCs.GetRandom();

            NPCDef def = NPCDef.Get(npc.Definition);
            if (def == null) return;

            NPCInst inst = new NPCInst(def);
            inst.Spawn(world, posAng.Position, posAng.Angles);
        }
    }
}
