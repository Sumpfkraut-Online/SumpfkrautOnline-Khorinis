using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.Types;
using GUC.WorldObjects.Instances;

namespace GUC.WorldObjects.VobGuiding
{
    public abstract partial class GuideCmd
    {
        public abstract byte CmdType { get; }

#if D_SERVER
        public abstract void WriteStream(PacketWriter stream);
#endif

#if D_CLIENT
        public abstract void Start(GuidedVob vob);
        public abstract void Update(GuidedVob vob, long now);
        public abstract void Stop(GuidedVob vob);

        public abstract void ReadStream(PacketReader stream);
#endif
    }

    public abstract partial class TargetCmd : GuideCmd
    {
        float distance;
        public float Distance { get { return distance; } }

        GUCBaseVobInst target;
        public GUCBaseVobInst Target { get { return this.target; } }

        public TargetCmd()
        {

        }

        public TargetCmd(GUCBaseVobInst target, float distance)
        {
            if (target == null)
                throw new ArgumentNullException("Target is null!");
            if (!target.IsSpawned)
                throw new ArgumentException("Target is not spawned!");

            this.target = target;
            this.distance = distance;
        }

#if D_SERVER
        public override void WriteStream(PacketWriter stream)
        {
            stream.Write((ushort)target.ID);
            stream.Write(target.Position);
            stream.Write(distance);
        }
#endif

#if D_CLIENT
        static Dictionary<int, List<TargetCmd>> Commands = new Dictionary<int, List<TargetCmd>>();

        static TargetCmd()
        {
            GUCBaseVobInst.sOnSpawn += CheckSpawn;
            GUCBaseVobInst.sOnDespawn += CheckDespawn;
        }

        static void CheckSpawn(GUCBaseVobInst vob, World world, Vec3f pos, Angles ang)
        {
            if (Commands.TryGetValue(vob.ID, out List<TargetCmd> cmdList))
                for (int i = 0; i < cmdList.Count; i++)
                    cmdList[i].target = vob;
        }

        static void CheckDespawn(GUCBaseVobInst vob)
        {
            if (Commands.TryGetValue(vob.ID, out List<TargetCmd> cmdList))
                for (int i = 0; i < cmdList.Count; i++)
                {
                    cmdList[i].sentDest = vob.Position;
                    cmdList[i].target = null;
                }
        }

        internal static void CheckPos(int id, Vec3f pos)
        {
            if (Commands.TryGetValue(id, out List<TargetCmd> cmdList))
                for (int i = 0; i < cmdList.Count; i++)
                    cmdList[i].sentDest = pos;
        }

        int targetID = -1;

        Vec3f sentDest;
        public Vec3f Destination { get { return this.target == null ? this.sentDest : this.target.Position; } }

        public override void ReadStream(PacketReader stream)
        {
            this.targetID = stream.ReadUShort();
            this.sentDest = stream.ReadVec3f();
            this.distance = stream.ReadFloat();
        }

        public override void Start(GuidedVob vob)
        {
            if (!Commands.TryGetValue(targetID, out List<TargetCmd> cmdList))
            {
                cmdList = new List<TargetCmd>();
                Commands.Add(targetID, cmdList);
            }
            cmdList.Add(this);

            World.Current.TryGetVob(targetID, out target);
        }

        public override void Stop(GuidedVob vob)
        {
            if (Commands.TryGetValue(targetID, out List<TargetCmd> cmdList))
            {
                cmdList.Remove(this);
                if (cmdList.Count == 0)
                    Commands.Remove(targetID);
            }

            this.target = null;
        }
#endif
    }
}
