using GUC.Server.Scripts.Sumpfkraut.Utilities.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Server.Scripts.Sumpfkraut.VobSystem
{
    abstract class VobLoader : Runnable
    {

        // 0: before loading; 1: started loading; ?: done
        protected int status = 0;

        public delegate void OnStatusChangeEventHandler (object sender, OnStatusChangeEventArgs e);
        public event OnStatusChangeEventHandler OnStatusChange;
        public class OnStatusChangeEventArgs
        {
            private DateTime timestamp;
            public DateTime GetTimestamp() { return this.timestamp; }

            public int status;

            public OnStatusChangeEventArgs (int status)
            {
                this.timestamp = DateTime.Now;
                this.status = status;
            }
        }



        public VobLoader ()
            : this(true)
        { }

        public VobLoader (bool startOnCreate)
            : base(startOnCreate, new TimeSpan(0, 0, 0), true)
        { }
      
        
        
        // override this one
        protected virtual void Load ()
        {
            OnStatusChange.Invoke(this, new OnStatusChangeEventArgs(1));
        }



        public override void Run ()
        {
            base.Run();
            Load();
        }

    }
}
