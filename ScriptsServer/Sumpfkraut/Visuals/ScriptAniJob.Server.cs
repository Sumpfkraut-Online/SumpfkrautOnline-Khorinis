using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Animations;

namespace GUC.Scripts.Sumpfkraut.Visuals
{
    public partial class ScriptAniJob : ScriptObject, AniJob.IScriptAniJob
    {
        public int AttackBonus = 0;

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

        public ScriptAniJob(string codeName, AniType type = AniType.Normal) : this()
        {
            this.CodeName = codeName;
            this.Type = type;
        }

        public ScriptAniJob(string codeName, ScriptAni defaultAni, AniType type = AniType.Normal) : this(codeName)
        {
            this.SetDefaultAni(defaultAni);
            this.Type = type;
        }
    }
}
