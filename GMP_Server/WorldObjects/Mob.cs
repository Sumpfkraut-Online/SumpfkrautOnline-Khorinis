using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.Types;
using GUC.Server.Network;
using RakNet;
using GUC.Network;

namespace GUC.Server.WorldObjects
{
    public class VobMob : AbstractDropVob
    {
        public MobInstance Instance { get; protected set; }

        public string Visual { get { return Instance.Visual; } }
        public bool CDDyn { get { return Instance.cdDyn; } }
        public bool CDStatic { get { return Instance.cdStatic; } }

        #region Constructors

        /// <summary>
        /// Creates and returns a Mob object from the given MobInstance-ID.
        /// Returns NULL when the ID is not found!
        /// </summary>
        public static VobMob Create(ushort instanceID, object scriptObject)
        {
            MobInstance inst = MobInstance.Table.Get(instanceID);
            if (inst == null)
            {
                Log.Logger.logError("Mob creation failed! Instance ID not found: " + instanceID);
                return null;
            }
            return Create(inst, scriptObject);
        }

        /// <summary>
        /// Creates and returns a Mob object from the given MobInstance-Name.
        /// Returns NULL when the name is not found!
        /// </summary>
        public static VobMob Create(object scriptObject, string instanceName)
        {
            MobInstance inst = MobInstance.Table.Get(instanceName);
            if (inst == null)
            {
                Log.Logger.logError("Mob creation failed! Instance name not found: " + instanceName);
                return null;
            }
            return Create(inst, scriptObject);
        }

        /// <summary>
        /// Creates and returns a Mob object from the given MobInstance.
        /// Returns NULL when the MobInstance is NULL!
        /// </summary>
        public static VobMob Create(MobInstance instance, object scriptObject)
        {
            if (instance != null)
            {
                VobMob mob = null;
                switch (instance.type)
                {
                    case MobType.Vob:
                        mob = new VobMob(scriptObject);
                        break;
                    case MobType.Mob:
                        mob = new Mob(scriptObject);
                        break;
                    case MobType.MobInter:
                        mob = new MobInter(scriptObject);
                        break;
                    case MobType.MobFire:
                        mob = new MobFire(scriptObject);
                        break;
                    case MobType.MobLadder:
                        mob = new MobLadder(scriptObject);
                        break;
                    case MobType.MobSwitch:
                        mob = new MobSwitch(scriptObject);
                        break;
                    case MobType.MobWheel:
                        mob = new MobWheel(scriptObject);
                        break;
                    case MobType.MobContainer:
                        mob = new MobContainer(scriptObject);
                        break;
                    case MobType.MobDoor:
                        mob = new MobDoor(scriptObject);
                        break;
                    default:
                        Log.Logger.logError("Mob creation failed! Unknown MobType!");
                        return null;
                }
                mob.Instance = instance;
                Server.Network.Server.sVobDict.Add(mob.ID, mob);
                return mob;
            }
            else
            {
                Log.Logger.logError("Mob creation failed! Instance can't be NULL!");
                return null;
            }
        }

        internal VobMob(object scriptObject) : base(scriptObject)
        {
        }

        public override void Delete()
        {
            base.Delete();
            Server.Network.Server.sVobDict.Remove(this.ID);
        }

        #endregion

        #region Networking

        internal override void WriteSpawn(IEnumerable<Client> list)
        {
            PacketWriter stream = Program.server.SetupStream(NetworkID.WorldVobSpawnMessage);
            stream.Write(ID);
            stream.Write(Instance.ID);
            stream.Write(pos);
            stream.Write(dir);
            stream.Write(physicsEnabled);

            foreach (Client client in list)
                client.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W');
        }

        #endregion
    }

    public class Mob : VobMob
    {
        public string FocusName { get { return Instance.focusName; } }

        internal Mob(object scriptObject) : base(scriptObject)
        {
        }
    }

    public class MobInter : Mob
    {
        public string OnTriggerFunc { get { return Instance.onTriggerFunc; } }
        public string OnTriggerClientFunc { get { return Instance.onTriggerClientFunc; } }
        public bool IsMulti { get { return Instance.isMulti; } }

        internal MobInter(object scriptObject) : base(scriptObject)
        {
        }
    }

    public class MobFire : MobInter
    {
        public string FireVobTreeName { get { return Instance.fireVobTreeName; } }

        internal MobFire(object scriptObject) : base(scriptObject)
        {
        }
    }

    public class MobLadder : MobInter
    {
        internal MobLadder(object scriptObject) : base(scriptObject)
        {
        }
    }

    public class MobSwitch : MobInter
    {
        internal MobSwitch(object scriptObject) : base(scriptObject)
        {
        }
    }

    public class MobWheel : MobInter
    {
        internal MobWheel(object scriptObject) : base(scriptObject)
        {
        }
    }

    public abstract class MobLockable : MobInter
    {
        public string OnTryOpenClientFunc { get { return Instance.onTryOpenClientFunc; } }

        internal MobLockable(object scriptObject) : base(scriptObject)
        {
        }
    }

    public class MobContainer : MobLockable
    {
        internal MobContainer(object scriptObject) : base(scriptObject)
        {
        }
    }

    public class MobDoor : MobLockable
    {
        internal MobDoor(object scriptObject) : base(scriptObject)
        {
        }
    }
}
