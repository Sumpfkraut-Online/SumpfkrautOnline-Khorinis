using GUC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Sumpfkraut.EffectSystem
{

    // simple container to hold to total "sum" of a multitude of single changes / components
    public class TotalChange : ExtendedObject
    {

        new public static readonly string _staticName = "TotalChange (static)";



        private List<Change> components;
        public List<Change> Components { get { return components; } }

        private Change total;
        public Change Total { get { return total; } }
        public void SetTotal (Change total)
        {
            this.total = total;
            this.lastTotalUpdate = DateTime.Now;
        }

        // time of last recalculation / setting of the total change 
        private DateTime lastTotalUpdate;
        public DateTime LastTotalUpdate { get { return this.lastTotalUpdate; } }

        // time of last update (addition or removal of a Change / component)
        private DateTime lastComponentUpdate;
        public DateTime LastComponentUpdate { get { return this.lastComponentUpdate; } }



        public TotalChange ()
        {
            SetObjName("TotalChange (default)");
            this.components = new List<Change>();
            this.lastComponentUpdate = DateTime.Now;
            this.lastTotalUpdate = DateTime.Now;
        }



        public void AddChanges (Change change)
        {

        }

        public void AddChange (Change change)
        {

        }

    }

}
