using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Animations;
using GUC.Utilities;

namespace GUC.Scripts.Sumpfkraut.Visuals
{
    public partial class ScriptAniJob : ExtendedObject, AniJob.IScriptAniJob
    {

        string codeName;
        public string CodeName
        {
            get { return this.codeName; }
            set
            {
                if (this.IsCreated)
                    throw new Exception("CodeName can't be changed when the object is already created!");

                this.codeName = value == null ? "" : value.ToUpperInvariant();
            }
        }
                
        public ScriptAniJob(string codeName) : this()
        {
            this.CodeName = codeName;
        }

        public ScriptAniJob(string codeName, string gothicName) : this(codeName)
        {
            this.AniName = gothicName;
        }

        public ScriptAniJob(string codeName, ScriptAni defaultAni) : this(codeName)
        {
            this.SetDefaultAni(defaultAni);
        }

        public ScriptAniJob(string codeName, string gothicName, ScriptAni defaultAni) : this()
        {
            this.CodeName = codeName;
            this.AniName = gothicName;
            this.SetDefaultAni(defaultAni);
        }
    }
}
