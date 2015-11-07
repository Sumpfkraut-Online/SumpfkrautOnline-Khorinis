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
    public class Mob : AbstractDropVob
    {
        public MobInstance Instance { get; protected set; }
        public MobType Type { get { return Instance.type; } }

        #region Constructors

        /// <summary>
        /// Creates and returns a Mob object from the given MobInstance-ID.
        /// Returns NULL when the ID is not found!
        /// </summary>
        public static Mob Create(ushort instanceID)
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
        public static Mob Create(string instanceName)
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
        public static Mob Create(MobInstance instance)
        {
            if (instance != null)
            {
                Mob mob = new Mob();
                mob.Instance = instance;
                return mob;
            }
            else
            {
                Log.Logger.logError("Mob creation failed! Instance can't be NULL!");
                return null;
            }
        }

        protected Mob()
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
}
