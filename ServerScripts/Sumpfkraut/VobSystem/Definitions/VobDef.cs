using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Server.Scripts.Sumpfkraut.VobSystem.Definitions
{
    class VobDef : ScriptObject
    {

        #region classfields

        protected int id;
        public int getId () { return this.id; }
        public void setId (int id) { this.id = id; }

        protected DateTime changeDate;
        public DateTime getChangeDate () { return this.changeDate; }
        public void setChangeDate (DateTime changeDate) { this.changeDate = changeDate; }
        public void setChangeDate (string changeDate)
        {
            DateTime dt;
            if (Sumpfkraut.Utilities.DateTimeUtil.TryStringToDateTime(changeDate, out dt))
            {
                this.changeDate = dt;
            }
        }

        protected DateTime creationDate;
        public DateTime getCreationDate () { return this.creationDate; }
        public void setCreationDate (DateTime creationDate) { this.creationDate = creationDate; }
        public void setCreationDate (string creationDate)
        {
            DateTime dt;
            if (Sumpfkraut.Utilities.DateTimeUtil.TryStringToDateTime(creationDate, out dt))
            {
                this.creationDate = dt;
            }
        }

        #endregion

        #region constructors

        public VobDef ()
            : base("VobDef (default)")
        { }

        #endregion

    }
}
