using GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers;
using GUC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.Changes
{

    // simple container to hold to total "sum" of a multitude of single changes / components
    public partial class TotalChange : ExtendedObject
    {

        new public static readonly string _staticName = "TotalChange (static)";



        protected object totalChangeLock;

        protected BaseEffectHandler.CalculateTotalChange calcFunction;
        public BaseEffectHandler.CalculateTotalChange GetCalcFunction ()
        {
            lock (totalChangeLock) { return calcFunction; }
        }
        public void SetCalcFunction (BaseEffectHandler.CalculateTotalChange val)
        {
            lock (totalChangeLock) { calcFunction = val; }
        }

        protected BaseEffectHandler.ApplyTotalChange applyFunction;
        public BaseEffectHandler.ApplyTotalChange GetApplyFunction ()
        {
            lock (totalChangeLock) { return applyFunction; }
        }
        public void SetApplyFunction (BaseEffectHandler.ApplyTotalChange val)
        {
            lock (totalChangeLock) { applyFunction = val; }
        }

        protected List<Change> components;
        public List<Change> Components { get { return components; } }

        protected List<DateTime> subscriptionDates;

        protected Change total;
        public Change GetTotal ()
        {
            lock (totalChangeLock) { return total; }
        }
        // always set the value with this method to also create a timestamp
        public void SetTotal (Change total)
        {
            lock (totalChangeLock)
            {
                this.total = total;
                this.lastTotalUpdate = DateTime.Now;
            }
        }

        // time of last recalculation / setting of the total change 
        protected DateTime lastTotalUpdate;
        public DateTime LastTotalUpdate { get { return this.lastTotalUpdate; } }

        // time of last update (addition or removal of a Change / component)
        protected DateTime lastComponentUpdate;
        public DateTime LastComponentUpdate { get { return this.lastComponentUpdate; } }


        public TotalChange (BaseEffectHandler.CalculateTotalChange calcFunction, 
            BaseEffectHandler.ApplyTotalChange applyFunction)
            : this()
        {
            this.calcFunction = calcFunction;
            this.applyFunction = applyFunction;
        }

        public TotalChange ()
        {
            SetObjName("TotalChange (default)");
            totalChangeLock = new object();
            components = new List<Change>();
            subscriptionDates = new List<DateTime>();
            lastComponentUpdate = DateTime.Now;
            lastTotalUpdate = DateTime.Now;
        }



        public void AddChange (Change change, DateTime subDate)
        {
            lock (totalChangeLock)
            {
                var index = 0;
                for (int i = subscriptionDates.Count - 1; i > -1; i--)
                {
                    if (subscriptionDates[i] < subDate) { index = i; }
                }
                components.Insert(index, change);
                lastComponentUpdate = DateTime.Now;
            }
        }

        //public void AddChange (Change change)
        //{
        //    lock (totalChangeLock)
        //    {
        //        if (components.Contains(change)) { return; }
        //        components.Add(change);
        //        lastComponentUpdate = DateTime.Now;
        //    }
        //}

        public void RemoveChange (Change change)
        {
            lock (totalChangeLock)
            {
                int index = components.IndexOf(change);
                if (index < 0) { return; }
                components.RemoveAt(index);
                subscriptionDates.RemoveAt(index);
                lastComponentUpdate = DateTime.Now;
            }
        }

    }

}
