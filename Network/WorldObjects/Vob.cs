using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.Types;
using GUC.WorldObjects.Instances;
using GUC.WorldObjects.Collections;
using GUC.Network;

namespace GUC.WorldObjects
{
    public partial interface IScriptVob : IScriptWorldObject
    {
        void OnWriteSpawnProperties(PacketWriter stream);
        void OnReadSpawnProperties(PacketReader stream);
    }

    public class Vob : WorldObject, IVobObj<uint>
    {
        public static readonly VobCollection AllVobs = new VobCollection();
        public static readonly VobDictionary StaticVobs = AllVobs.GetDict(VobTypes.Vob);


        public uint ID { get; protected set; }
        public VobInstance Instance { get; protected set; }
        new public IScriptVob ScriptObj { get; protected set; }

        public VobTypes VobType { get { return Instance.VobType; } }

        public string Visual { get { return Instance.Visual; } }
        public bool CDDyn { get { return Instance.CDDyn; } }
        public bool CDStatic { get { return Instance.CDStatic; } }

        protected Vec3f pos = new Vec3f(0, 0, 0);
        protected Vec3f dir = new Vec3f(0, 0, 1);

        public World World { get; internal set; }
        public bool IsSpawned { get { return World != null; } }

        #region Creation

        internal Vob()
        {
        }

        public override void Create()
        {
            Vob.AllVobs.Add(this);
            base.Create();
        }
        
        public override void Delete()
        {
            this.Despawn();
            Vob.AllVobs.Remove(this);
            base.Delete();
        }

        #endregion

        #region Spawn

        public void Spawn(World world)
        {
            Spawn(world, this.pos, this.dir);
        }

        public void Spawn(World world, Vec3f position)
        {
            Spawn(world, position, this.dir);
        }

        public virtual void Spawn(World world, Vec3f position, Vec3f direction)
        {
            this.pos = position;
            this.dir = direction;
            world.SpawnVob(this);
        }

        public virtual void Despawn()
        {
            if (this.IsSpawned)
            {
                this.World.DespawnVob(this);
            }
        }
        #endregion

        internal virtual void WriteSpawnProperties(PacketWriter stream)
        {
            stream.Write(this.ID);
            stream.Write(this.Instance.ID);
            stream.Write(this.pos);
            stream.Write(this.dir);

            this.ScriptObj.OnWriteSpawnProperties(stream);
        }

        internal virtual void ReadSpawnProperties(PacketReader stream)
        {
            this.ID = stream.ReadUInt();
            ushort instanceid = stream.ReadUShort();
            this.Instance = VobInstance.AllInstances.Get(instanceid);
            if (this.Instance == null)
            {
                throw new Exception("Vob.ReadSpawnProperties failed: Instance-ID not found!");
            }
            this.pos = stream.ReadVec3f();
            this.dir = stream.ReadVec3f();

            this.ScriptObj.OnReadSpawnProperties(stream);
        }

        internal static Vob CreateVobByType(VobTypes type)
        {
            Vob result;
            switch (type)
            {
                case VobTypes.Vob:
                    result = new Vob();
                    break;
                case VobTypes.Item:
                    result = new Item();
                    break;
                case VobTypes.NPC:
                    result = new NPC();
                    break;
                case VobTypes.Mob:
                    result = new Mob();
                    break;
                case VobTypes.MobInter:
                    result = new MobInter();
                    break;
                case VobTypes.MobFire:
                    result = new MobFire();
                    break;
                case VobTypes.MobLadder:
                    result = new MobLadder();
                    break;
                case VobTypes.MobSwitch:
                    result = new MobSwitch();
                    break;
                case VobTypes.MobWheel:
                    result = new MobWheel();
                    break;
                case VobTypes.MobContainer:
                    result = new MobContainer();
                    break;
                case VobTypes.MobDoor:
                    result = new MobDoor();
                    break;
                default:
                    return null;
            }

            result.ScriptObj = Scripting.ScriptManager.si.CreateScriptVob(type);

            return result;
        }
    }
}
