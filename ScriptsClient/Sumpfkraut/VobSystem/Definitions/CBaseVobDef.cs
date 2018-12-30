using GUC.Network;
using GUC.Utilities;
using GUC.WorldObjects.Instances;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Sumpfkraut.VobSystem.Definitions
{
    public abstract class CBaseVobDef : ExtendedObject, BaseVobInstance.IScriptBaseVobInstance
    {

        /// <summary>
        /// Shared vob definition object which is wrapped by this class.
        /// </summary>
        protected BaseVobDef shared;



        public void Create ()
        {
            throw new NotImplementedException();
        }

        public void Delete ()
        {
            throw new NotImplementedException();
        }

        public byte GetVobType ()
        {
            throw new NotImplementedException();
        }

        public void OnReadProperties (PacketReader stream)
        {
            throw new NotImplementedException();
        }

        public void OnWriteProperties (PacketWriter stream)
        {
            throw new NotImplementedException();
        }
    }
}
