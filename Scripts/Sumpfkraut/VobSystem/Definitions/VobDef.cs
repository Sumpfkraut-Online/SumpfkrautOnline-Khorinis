using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Instances;
using GUC.Network;
using GUC.Enumeration;

namespace GUC.Scripts.Sumpfkraut.VobSystem.Definitions
{
    public partial class VobDef : ScriptObject, VobInstance.IScriptVobInstance
    {
        #region Static accessors to ALL VobDefs

        readonly static WorldObjects.Collections.InstanceCollection allInstances = VobInstance.AllInstances;

        /// <summary>
        /// Gets ANY Definition from the server. Default is null.
        /// </summary>
        public static VobDef GetDef(ushort id)
        {
            return (VobDef)allInstances.Get(id).ScriptObj;
        }

        /// <summary>
        /// Gets ALL Definitions on the server.
        /// </summary>
        public static IEnumerable<VobDef> GetAllDefs()
        {
            return allInstances.GetAll().Select(d => (VobDef)d.ScriptObj);
        }

        /// <summary>
        /// Gets the number of ALL Definitions on the server.
        /// </summary>
        public static int GetAllDefsCount()
        {
            return allInstances.GetCount();
        }

        #endregion

        #region Static accessors to VobDefs (static vobs)

        readonly static WorldObjects.Collections.InstanceDictionary staticVobInstances = VobInstance.AllInstances.GetDict(VobTypes.Vob);

        /// <summary>
        /// Gets a 'VobDef' from the server. Default is null.
        /// </summary>
        public static VobDef GetVobDef(ushort id)
        {
            return (VobDef)staticVobInstances.Get(id).ScriptObj;
        }

        /// <summary>
        /// Gets all 'VobDefs' on the server.
        /// </summary>
        public static IEnumerable<VobDef> GetAllVobDefs()
        {
            return staticVobInstances.GetAll().Select(d => (VobDef)d.ScriptObj);
        }

        /// <summary>
        /// Gets the number of all 'VobDefs' on the server.
        /// </summary>
        public static int GetVobDefCount()
        {
            return staticVobInstances.GetCount();
        }

        #endregion

        new public static readonly String _staticName = "VobDef (static)";

        #region Properties

        protected VobInstance baseDef;
        public ushort ID { get { return baseDef.ID; } }
        public VobTypes VobType { get { return baseDef.VobType; } }

        public string Visual { get { return baseDef.Visual; } set { baseDef.Visual = value; } }
        public bool CDDyn { get { return baseDef.CDDyn; } set { baseDef.CDDyn = value; } }
        public bool CDStatic { get { return baseDef.CDStatic; } set { baseDef.CDStatic = value; } }

        #endregion

        #region Constructors

        protected VobDef()
        {
            SetObjName("VobDef (default)");
        }

        #endregion

        #region Create & Delete

        public virtual void Create()
        {
            baseDef.Create();
        }

        public virtual void Delete()
        {
            baseDef.Delete();
        }

        #endregion

        public virtual void OnWriteProperties(PacketWriter stream) { }
        public virtual void OnReadProperties(PacketReader stream) { }
    }
}
