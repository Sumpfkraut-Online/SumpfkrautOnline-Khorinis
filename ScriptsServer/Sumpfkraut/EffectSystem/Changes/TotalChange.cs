using GUC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.Changes
{

    // simple container to hold to total "sum" of a multitude of single changes / components
    public class TotalChange : ExtendedObject
    {

        new public static readonly string _staticName = "TotalChange (static)";



        protected List<BaseChangeInit> components;
        public List<BaseChangeInit> Components { get { return components; } }

        protected BaseChangeInit total;
        public BaseChangeInit Total { get { return total; } }
        public void SetTotal (BaseChangeInit total)
        {
            lock (totalChangeLock)
            {
                this.total = total;
                this.lastTotalUpdate = DateTime.Now;
            }
        }

        protected object totalChangeLock;

        // time of last recalculation / setting of the total change 
        protected DateTime lastTotalUpdate;
        public DateTime LastTotalUpdate { get { return this.lastTotalUpdate; } }

        // time of last update (addition or removal of a Change / component)
        protected DateTime lastComponentUpdate;
        public DateTime LastComponentUpdate { get { return this.lastComponentUpdate; } }



        public TotalChange ()
        {
            SetObjName("TotalChange (default)");
            components = new List<BaseChangeInit>();
            totalChangeLock = new object();
            lastComponentUpdate = DateTime.Now;
            lastTotalUpdate = DateTime.Now;
        }



        public void AddChange (BaseChangeInit change)
        {
            lock (totalChangeLock)
            {
                if (components.Contains(change)) { return; }
                components.Add(change);
                lastComponentUpdate = DateTime.Now;
            }
        }

        public void RemoveChange (BaseChangeInit change)
        {
            lock (totalChangeLock)
            {
                int index = components.IndexOf(change);
                if (index < 0) { return; }
                components.RemoveAt(index);
                lastComponentUpdate = DateTime.Now;
            }
        }

    }

}
