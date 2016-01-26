using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.Network;
using GUC.Log;
using GUC.WorldObjects.Collections;

namespace GUC.WorldObjects.Instances
{
    public partial class VobInstance : WorldObject, IVobObj<ushort>
    {
        public string InstanceName { get; protected set; }

        public VobInstance(string instanceName, IScriptVobInstance scriptObj) : this(0, instanceName, scriptObj)
        {
        }

        public VobInstance(ushort id, string instanceName, IScriptVobInstance scriptObj) : base(scriptObj)
        {
            this.ID = id;
            this.InstanceName = instanceName.ToUpper();
            this.VobType = VobInstance.sVobType;
        }

        static ushort idCount = 1;
        partial void pCreate()
        {
            if (ID == 0) //seek a new ID for this instance
            {
                for (int i = 0; i < ushort.MaxValue; i++)
                {
                    if (idCount != 0)
                    {
                        if (AllInstances.Get(idCount) == null)
                        {
                            ID = idCount++;
                            break;
                        }
                    }
                    idCount++;
                }

                if (ID == 0) //no free id found
                {
                    throw new Exception(String.Format("{0} creation failed: Maximum reached: {1}", this.GetType(), ushort.MaxValue));
                }
            }
        }
    }
}
