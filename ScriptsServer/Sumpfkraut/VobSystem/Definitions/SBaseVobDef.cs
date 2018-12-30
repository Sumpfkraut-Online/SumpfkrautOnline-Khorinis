using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.Utilities;
using GUC.WorldObjects.Instances;

namespace GUC.Scripts.Sumpfkraut.VobSystem.Definitions
{
    /// <summary>
    /// Serverside wrapper class for BaseVobDef.
    /// </summary>
    public abstract class SBaseVobDef : ExtendedObject, BaseVobInstance.IScriptBaseVobInstance
    {
        
        /// <summary>
        /// Access all existing and registered vob definitions by their name.
        /// </summary>
        private static Dictionary<string, SBaseVobDef> nameToVobDef = 
            new Dictionary<string, SBaseVobDef>(StringComparer.OrdinalIgnoreCase);



        /// <summary>
        /// Shared vob definition object which is wrapped by this class.
        /// </summary>
        BaseVobDef shared;

        /// <summary>
        /// Unique name to refer to the vob definition.
        /// </summary>
        protected string codeName;
        public string CodeName
        {
            get { return codeName; }
            //set
            //{
                //if (shared.IsCreated)
                //{
                //    throw new NotSupportedException(
                //        "Can't change CodeName when the Definition is already added to the static collection!");
                //}
                //if (string.IsNullOrWhiteSpace(value))
                //{
                //    throw new ArgumentException("CodeName is null or white space!");
                //}
                //codeName = value;
            //}
        }



        public SBaseVobDef (string codeName)
            : base()
        {
            //shared = new BaseVobDef();
            this.codeName = codeName;
        }



        /// <summary>
        /// Attempt to retrieve an existing vob definition from the static cache.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="codeName"></param>
        /// <param name="vobDef"></param>
        /// <returns></returns>
        public static bool TryGetVobDef<T> (string codeName, out T vobDef)
            where T : SBaseVobDef
        {
            SBaseVobDef value;
            bool success = nameToVobDef.TryGetValue(codeName, out value);
            vobDef = value is T ? (T)value : null;
            return success;
        }

        /// <summary>
        /// Registers the vob definition in the static cache.
        /// </summary>
        public void Create ()
        {
            nameToVobDef.Add(CodeName, this);
        }

        /// <summary>
        /// Remove the vob definition from the static cache.
        /// </summary>
        public void Delete ()
        {
            nameToVobDef.Remove(CodeName);
        }

        /// <summary>
        /// Get shorthand byte representation of VobType.
        /// </summary>
        /// <returns></returns>
        public byte GetVobType ()
        {
            throw new NotImplementedException();
        }

        public virtual void OnReadProperties (PacketReader stream) { }

        public virtual void OnWriteProperties (PacketWriter stream) { }
    }
}
