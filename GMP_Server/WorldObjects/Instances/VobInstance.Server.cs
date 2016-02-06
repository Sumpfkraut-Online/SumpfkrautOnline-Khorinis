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
    public partial class VobInstance
    {
        #region Constructors

        /// <summary>
        /// Creates a new Instance with the given ID or searches a new one.
        /// </summary>
        /// <param name="codeName">Case insensitive!</param>
        public VobInstance(IScriptWorldObject scriptObject, string codeName, int id = -1) : base(scriptObject, codeName, id)
        {
        }

        #endregion
    }
}
