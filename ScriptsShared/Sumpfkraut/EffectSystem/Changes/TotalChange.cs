using GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers;
using GUC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.Changes
{

    /// <summary>
    /// Simple container to hold to total "sum" of a multitude of single changes / components.
    /// </summary>
    public class TotalChange : ExtendedObject
    {

        /// <summary>
        /// Dummy object used to prevent multi-threaded access to the object for data integrity.
        /// </summary>
        protected object totalChangeLock;

        /// <summary>
        /// Used to calculate a total / final change after processing all single input changes.
        /// </summary>
        protected BaseEffectHandler.CalculateTotalChange calcFunction;
        /// <summary>
        /// Get the function used to calculate a total / final change out of all single input changes.
        /// </summary>
        public BaseEffectHandler.CalculateTotalChange GetCalcFunction ()
        {
            lock (totalChangeLock) { return calcFunction; }
        }
        /// <summary>
        /// Set the function used to calculate a total / final change out of all single input changes.
        /// </summary>
        public void SetCalcFunction (BaseEffectHandler.CalculateTotalChange val)
        {
            lock (totalChangeLock) { calcFunction = val; }
        }

        /// <summary>
        /// Used to apply the TotalChange to the running program (likly vobs).
        /// </summary>
        protected BaseEffectHandler.ApplyTotalChange applyFunction;
        /// <summary>
        /// Get the function used to apply the TotalChange to the running program (likly vobs).
        /// </summary>
        public BaseEffectHandler.ApplyTotalChange GetApplyFunction ()
        {
            lock (totalChangeLock) { return applyFunction; }
        }
        /// <summary>
        /// Set the function used to apply the TotalChange to the running program (likly vobs).
        /// </summary>
        public void SetApplyFunction (BaseEffectHandler.ApplyTotalChange val)
        {
            lock (totalChangeLock) { applyFunction = val; }
        }

        /// <summary>
        /// All changes which are evaluated when calculating a final, total value out of them.
        /// </summary>
        protected List<Change> components;
        /// <summary>
        /// All changes which are evaluated when calculating a final, total value out of them.
        /// </summary>
        public List<Change> Components { get { return components; } }

        /// <summary>
        /// Subscription dates of effects to evaluate their chronological order.
        /// </summary>
        protected List<DateTime> effectSubDates;
        /// <summary>
        /// Subscription dates of changes to evaluate their chronological order.
        /// </summary>
        protected List<DateTime> changeSubDates;


        /// <summary>
        /// The final, toal Change after evaluating all single input Changes.
        /// </summary>
        protected Change total;
        /// <summary>
        /// Get the final, toal Change after evaluating all single input Changes.
        /// </summary>
        public Change GetTotal ()
        {
            lock (totalChangeLock) { return total; }
        }
        /// <summary>
        /// Set the final, toal Change after evaluating all single input Changes.
        /// This will also set a timestamp of the last update process.
        /// </summary>
        public void SetTotal (Change total)
        {
            lock (totalChangeLock)
            {
                this.total = total;
                this.lastTotalUpdate = DateTime.Now;
            }
        }

        /// <summary>
        /// Time of last recalculation / setting of the total change.
        /// </summary>
        protected DateTime lastTotalUpdate;
        /// <summary>
        /// Time of last recalculation / setting of the total change.
        /// </summary>
        public DateTime LastTotalUpdate { get { return this.lastTotalUpdate; } }

        /// <summary>
        /// Time of last update (addition or removal of a Change / component).
        /// </summary>
        protected DateTime lastComponentUpdate;
        /// <summary>
        /// Time of last recalculation / setting of the total change.
        /// </summary>
        public DateTime LastComponentUpdate { get { return this.lastComponentUpdate; } }


        /// <summary>
        /// Creates a convenient container to store multiple Changes as well as their
        /// summarized, total Change.
        /// </summary>
        /// <param name="calcFunction"></param>
        /// <param name="applyFunction"></param>
        public TotalChange (BaseEffectHandler.CalculateTotalChange calcFunction, 
            BaseEffectHandler.ApplyTotalChange applyFunction)
            : this()
        {
            this.calcFunction = calcFunction;
            this.applyFunction = applyFunction;
        }

        /// <summary>
        /// Creates a convenient container to store multiple Changes as well as their
        /// summarized, total Change.
        /// </summary>
        public TotalChange ()
        {
            totalChangeLock = new object();
            components = new List<Change>();
            effectSubDates = new List<DateTime>();
            changeSubDates = new List<DateTime>();
            lastComponentUpdate = DateTime.Now;
            lastTotalUpdate = DateTime.Now;
        }



        /// <summary>
        /// Add a Change to the collection, also storing timestamps to allow evaluating order.
        /// </summary>
        /// <param name="change"></param>
        /// <param name="effectSubDate"></param>
        /// <param name="changeSubDate"></param>
        public void AddChange (Change change, DateTime effectSubDate, DateTime changeSubDate)
        {
            lock (totalChangeLock)
            {
                // search for the chronological place / index 
                var index = 0;
                for (int i = 0; i < effectSubDates.Count; i++)
                {
                    index = i;
                    // first compare subscription dates of the surrounding Effects
                    if (effectSubDate < effectSubDates[i]) { break; }
                    else if (effectSubDate == effectSubDates[i])
                    {
                        // then compare subscription dates of the Changes themselves
                        if (changeSubDate < changeSubDates[i]) { break; }
                    }
                }
                components.Insert(index, change);
                effectSubDates.Insert(index, effectSubDate);
                changeSubDates.Insert(index, changeSubDate);
                lastComponentUpdate = DateTime.Now;
            }
        }

        /// <summary>
        /// Remove a Change from the collection.
        /// </summary>
        /// <param name="change"></param>
        public void RemoveChange (Change change)
        {
            lock (totalChangeLock)
            {
                int index = components.IndexOf(change);
                if (index < 0) { return; }
                components.RemoveAt(index);
                effectSubDates.RemoveAt(index);
                changeSubDates.RemoveAt(index);
                lastComponentUpdate = DateTime.Now;
            }
        }

    }

}
