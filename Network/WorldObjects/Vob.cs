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
    public partial class Vob : VobObj<VobInstance>
    {
        public partial interface IScriptVob : IScriptVobObj
        {
        }

        public override VobTypes VobType { get { return Instance.VobType; } }

        public string Visual { get { return Instance.Visual; } }
        public bool CDDyn { get { return Instance.CDDyn; } }
        public bool CDStatic { get { return Instance.CDStatic; } }

        public Vob(VobInstance instance, IScriptVob scriptObj) : base(scriptObj)
        {
            if (instance == null)
                throw new ArgumentNullException("VobInstance can't be null!");

            this.Instance = instance;
        }

        #region Creation

        public override void Create()
        {
            base.Create();
            Vob.AllVobs.Add(this);
        }
        
        public override void Delete()
        {
            this.Despawn();
            base.Delete();
            Vob.AllVobs.Remove(this);
        }

        #endregion




    }
}
