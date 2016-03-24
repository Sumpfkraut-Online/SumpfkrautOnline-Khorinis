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
    public class Vob : AbstractDropVob
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
        public static Vob Create(ushort instanceID)
        {
            MobInstance inst = MobInstance.Table.Get(instanceID);
            if (inst == null)
            {
                Log.Logger.logError("Mob creation failed! Instance ID not found: " + instanceID);
                return null;
            }
            return Create(inst);
        }

        /// <summary>
        /// Creates and returns a Mob object from the given MobInstance-Name.
        /// Returns NULL when the name is not found!
        /// </summary>
        public static Vob Create(string instanceName)
        {
            MobInstance inst = MobInstance.Table.Get(instanceName);
            if (inst == null)
            {
                Log.Logger.logError("Mob creation failed! Instance name not found: " + instanceName);
                return null;
            }
            return Create(inst);
        }

        /// <summary>
        /// Creates and returns a Mob object from the given MobInstance.
        /// Returns NULL when the MobInstance is NULL!
        /// </summary>
        public static Vob Create(MobInstance instance)
        {
            if (instance != null)
            {
                Vob mob = null;
                switch (instance.type)
                {
                    case MobType.Vob:
                        mob = new Vob();
                        break;
                    case MobType.Mob:
                        mob = new Mob();
                        break;
                    case MobType.MobInter:
                        mob = new MobInter();
                        break;
                    case MobType.MobFire:
                        mob = new MobFire();
                        break;
                    case MobType.MobLadder:
                        mob = new MobLadder();
                        break;
                    case MobType.MobSwitch:
                        mob = new MobSwitch();
                        break;
                    case MobType.MobWheel:
                        mob = new MobWheel();
                        break;
                    case MobType.MobContainer:
                        mob = new MobContainer();
                        break;
                    case MobType.MobDoor:
                        mob = new MobDoor();
                        break;
                    default:
                        Log.Logger.logError("Mob creation failed! Unknown MobType!");
                        return null;
                }
                mob.Instance = instance;
                return mob;
            }
            else
            {
                Log.Logger.logError("Mob creation failed! Instance can't be NULL!");
                return null;
            }
        }

        protected Vob()
        {
        }

        #endregion

        #region Networking

        internal override void WriteSpawn(IEnumerable<Client> list)
        {
            BitStream stream = Program.server.SetupStream(NetworkID.WorldVobSpawnMessage);
            stream.mWrite(ID);
            stream.mWrite(Instance.ID);
            stream.mWrite(pos);
            stream.mWrite(dir);
            stream.mWrite(physicsEnabled);

            foreach (Client client in list)
                Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W', client.guid, false);
        }

        #endregion
    }

    public class Mob : Vob
    {
        public string FocusName { get { return Instance.focusName; } }
    }

    public class MobInter : Mob
    {
        public string OnTriggerFunc { get { return Instance.onTriggerFunc; } }
        public string OnTriggerClientFunc { get { return Instance.onTriggerClientFunc; } }
        public bool IsMulti { get { return Instance.isMulti; } }
    }

    public class MobFire : MobInter
    {
        public string FireVobTreeName { get { return Instance.fireVobTreeName; } }
    }

    public class MobLadder : MobInter
    {
    }

    public class MobSwitch : MobInter
    {
    }

    public class MobWheel : MobInter
    {
    }

    public abstract class MobLockable : MobInter
    {
        public string OnTryOpenClientFunc { get { return Instance.onTryOpenClientFunc; } }
    }

    public class MobContainer : MobLockable
    {
    }

    public class MobDoor : MobLockable
    {
    }
}
