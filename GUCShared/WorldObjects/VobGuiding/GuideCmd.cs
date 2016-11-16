using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.Types;

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
        BaseVob target;
        public BaseVob Target { get { return this.target; } }

        public TargetCmd()
        {

        }

        public TargetCmd(BaseVob target)
        {
            if (target == null)
                throw new ArgumentNullException("Target is null!");
            if (!target.IsSpawned)
                throw new ArgumentException("Target is not spawned!");

            this.target = target;
        }

#if D_SERVER
        public override void WriteStream(PacketWriter stream)
        {
            stream.Write((ushort)target.ID);
            stream.Write(target.GetPosition());
        }
#endif

#if D_CLIENT
        static Dictionary<int, List<TargetCmd>> Commands = new Dictionary<int, List<TargetCmd>>();

        static TargetCmd()
        {
            BaseVob.sOnSpawn += CheckSpawn;
            BaseVob.sOnDespawn += CheckDespawn;
        }

        static void CheckSpawn(BaseVob vob, World world, Vec3f pos, Vec3f dir)
        {
            List<TargetCmd> cmdList;
            if (Commands.TryGetValue(vob.ID, out cmdList))
                for (int i = 0; i < cmdList.Count; i++)
                    cmdList[i].target = vob;
        }

        static void CheckDespawn(BaseVob vob)
        {
            List<TargetCmd> cmdList;
            if (Commands.TryGetValue(vob.ID, out cmdList))
                for (int i = 0; i < cmdList.Count; i++)
                {
                    cmdList[i].sentDest = vob.GetPosition();
                    cmdList[i].target = null;
                }
        }

        internal static void CheckPos(int id, Vec3f pos)
        {
            List<TargetCmd> cmdList;
            if (Commands.TryGetValue(id, out cmdList))
                for (int i = 0; i < cmdList.Count; i++)
                    cmdList[i].sentDest = pos;
        }

        int targetID = -1;

        Vec3f sentDest;
        public Vec3f Destination { get { return this.target == null ? this.sentDest : this.target.GetPosition(); } }

        public override void ReadStream(PacketReader stream)
        {
            this.targetID = stream.ReadUShort();
            this.sentDest = stream.ReadVec3f();
        }

        public override void Start(GuidedVob vob)
        {
            List<TargetCmd> cmdList;
            if (!Commands.TryGetValue(targetID, out cmdList))
            {
                cmdList = new List<TargetCmd>();
                Commands.Add(targetID, cmdList);
            }
            cmdList.Add(this);

            World.Current.TryGetVob(targetID, out target);
        }

        public override void Stop(GuidedVob vob)
        {
            List<TargetCmd> cmdList;
            if (Commands.TryGetValue(targetID, out cmdList))
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
