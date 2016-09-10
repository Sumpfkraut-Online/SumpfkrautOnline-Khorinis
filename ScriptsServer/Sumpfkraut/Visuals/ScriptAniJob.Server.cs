using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Animations;

namespace GUC.Scripts.Sumpfkraut.Visuals
{
    public partial class ScriptAniJob : ScriptObject, AniJob.IScriptAniJob
    {
        int comboNum = 0;
        public int ComboNum
        {
            get { return this.comboNum; }
            set
            {
                if (this.ModelDef != null && this.ModelDef.IsCreated)
                    throw new NotSupportedException("Can't change value when the AniJob's ModelDef is already created!");
                if (comboNum < 0)
                    throw new ArgumentOutOfRangeException("ComboNum needs to be greater than or equal zero!");
                this.comboNum = value;
            }
        }

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
    }
}
