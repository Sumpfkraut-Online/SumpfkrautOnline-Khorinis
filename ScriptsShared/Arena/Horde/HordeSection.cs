using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;
using GUC.Utilities;
using GUC.Types;

namespace GUC.Scripts.Arena
{
    class HordeBarrier
    {
        public Vec3f Position;
        public Angles Angles;
        public string Definition;
    }

    class HordeGroup
    {
        public List<string, int> npcs;
        public Vec3f Position;
        public float Range;
    }

    class HordeSection
    {
        public List<HordeBarrier> barriers;
        public List<HordeGroup> groups;

        public HordeSection Next;
        public Vec3f SpawnPos;
        public float SpawnRange;

        public int SecsTillNext = 30;

        public Vec3f SpecPos;
        public Angles SpecAng;

        public string FinishedMessage;
    }
}
