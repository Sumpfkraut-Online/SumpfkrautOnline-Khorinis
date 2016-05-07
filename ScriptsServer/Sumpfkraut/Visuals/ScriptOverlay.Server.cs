using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Sumpfkraut.Visuals
{
    public partial class ScriptOverlay
    {
        string codeName;
        public string CodeName
        {
            get { return this.codeName; }
            set
            {
                if (this.IsCreated)
                    throw new Exception("CodeName can't be changed when the object is created!");

                this.codeName = value == null ? "" : value.ToUpper();
            }
        }

        public bool IsCreated { get { return this.baseOv.IsCreated; } }

        public ScriptOverlay(string codeName) : this(codeName, null)
        {
        }

        public ScriptOverlay(string codeName, string name) : this()
        {
            this.CodeName = codeName;
            this.Name = name;
        }
    }
}
