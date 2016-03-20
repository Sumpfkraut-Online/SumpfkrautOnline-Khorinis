using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Animations;

namespace GUC.Scripts.Sumpfkraut.Visuals
{
    public partial class ScriptAniJob : ScriptObject, AniJob.IScriptAniJob
    {
        string codeName;
        public string CodeName
        {
            get { return this.codeName; }
            set
            {
                if (this.IsCreated)
                    throw new Exception("CodeName can't be changed while the object is created!");

                this.codeName = value == null ? "" : value.ToUpper();
            }
        }

        public ScriptAniJob(string codeName) : this()
        {
            this.CodeName = codeName;
        }

        public ScriptAniJob(string codeName, ScriptAni defaultAni) : this(codeName)
        {
            this.SetDefaultAni(defaultAni);
        }
    }
}
