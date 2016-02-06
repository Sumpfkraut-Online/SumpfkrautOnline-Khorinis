using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.WorldObjects.Instances
{
    public abstract partial class BaseInstance
    {
        #region Properties

        public string CodeName { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new Instance with the given ID or searches a new one.
        /// </summary>
        /// <param name="codeName">Case insensitive!</param>
        protected BaseInstance(IScriptWorldObject scriptObject, string codeName, int id = -1) : base(scriptObject, id)
        {
            if (String.IsNullOrWhiteSpace(codeName))
                throw new ArgumentNullException("CodeName is null or white space!");

            this.CodeName = codeName.ToUpper();
        }

        #endregion
    }
}
