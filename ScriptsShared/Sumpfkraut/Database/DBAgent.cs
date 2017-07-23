using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace GUC.Scripts.Sumpfkraut.Database
{
    public partial class DBAgent : GUC.Utilities.Threading.AbstractRunnable
    {

        protected bool useAsyncMode;
        bool running;

        protected string dataSource = null;
        public string DataSource { get { return dataSource; } }

        protected List<string> commandQueue = new List<string>();



        public delegate void ReceivedResultsEventHandler (GUC.Utilities.Threading.AbstractRunnable sender, 
            ReceivedResultsEventArgs e);
        public event ReceivedResultsEventHandler ReceivedResults;
        public partial class ReceivedResultsEventArgs : EventArgs
        {
            protected string dataSource;
            public string DataSource { get { return dataSource; } }

            protected string sqlCommand;
            public string SQLCommand { get { return sqlCommand; } }

            protected DateTime startTime;
            public DateTime StartTime { get { return startTime; } }

            protected DateTime endTime;
            public DateTime EndTime { get { return endTime; } }

            protected List<List<List<object>>> sqlResults;
            public List<List<List<object>>> GetSQLResults() { return sqlResults; }

            public ReceivedResultsEventArgs (string dataSource, string sqlCommand, 
                DateTime startTime, DateTime endTime, List<List<List<object>>> sqlResults)
            {
                this.dataSource = dataSource;
                this.sqlCommand = sqlCommand;
                this.startTime = startTime;
                this.endTime = endTime;
                this.sqlResults = sqlResults;
            }
        }

        public delegate void FinishedQueueEventHandler(GUC.Utilities.Threading.AbstractRunnable sender,
            FinishedQueueEventHandlerArgs e);
        public event FinishedQueueEventHandler FinishedQueue;
        public partial class FinishedQueueEventHandlerArgs
        {
            protected string dataSource;
            public string DataSource { get { return dataSource; } }

            protected DateTime startTime;
            public DateTime StartTime { get { return startTime; } }

            protected DateTime endTime;
            public DateTime EndTime { get { return endTime; } }

            protected List<string> commandQueue;
            public List<string> CommandQueue { get { return commandQueue; } }

            protected List<List<List<object>>> sqlResults;
            public List<List<List<object>>> GetSQLResults() { return sqlResults; }

            public FinishedQueueEventHandlerArgs (string dataSource, List<string> commandQueue, 
                DateTime startTime, DateTime endTime, List<List<List<object>>> sqlResults)
            {
                this.dataSource = dataSource;
                this.commandQueue = commandQueue;
                this.startTime = startTime;
                this.endTime = endTime;
                this.sqlResults = sqlResults;
            }
        }

        // used to alarm another thread when a queue is finished
        // in case the alarmed thread is waiting to continue afterwards
        public AutoResetEvent waitHandle;



        public DBAgent(string dataSource, List<string> commandQueue, bool useAsyncMode = true)
            : this(dataSource, commandQueue, true, useAsyncMode)
        { }

        public DBAgent(string dataSource, List<string> commandQueue, bool startOnCreate, bool useAsyncMode = true)
            : base(false, new TimeSpan(0, 0, 0), true)
        {
            this.dataSource = dataSource;
            this.commandQueue = commandQueue;
            this.useAsyncMode = useAsyncMode;
            this.running = false;
            this.waitHandle = new AutoResetEvent(false);
            
            if (startOnCreate)
            {
                Start();
            }
        }



        public override void Abort ()
        {
            running = false;
            if (useAsyncMode) { base.Abort(); }
            if (printStateControls) { Print("Aborting run..."); }
        }

        public override void Reset ()
        {
            if (useAsyncMode) { base.Reset(); }
            Suspend();
            Resume();
        }

        public override void Resume ()
        {
            if (useAsyncMode) { base.Resume(); }
            if (printStateControls) { Print("Resuming run..."); }
            if (!running) { Start(); }
        }

        public override void Start ()
        {
            running = true;
            if (useAsyncMode) { base.Start(); }
            if (printStateControls) { Print("Starting run..."); }
            Run();
        }

        public override void Suspend ()
        {
            running = false;
            if (useAsyncMode) { base.Suspend(); }
            if (printStateControls) { Print("Suspending run..."); }
        }

        public override void Run ()
        {
            List<List<List<object>>> sqlResults = new List<List<List<object>>>();
            DateTime queueStartTime, queueEndTime, queryStartTime, queryEndTime;

            // iterate over commandQueue and communicating with DB while invoking the events
            queueStartTime = DateTime.Now;
            for (int i = 0; i < commandQueue.Count; i++)
            {
                if (running == false)
                {
                    // if aborted or suspended, exit even in midst of commandQueue
                    if (waitHandle != null) { waitHandle.Set(); }
                    return;
                }
                queryStartTime = DateTime.Now;
                DBReader.LoadFromDB(ref sqlResults, commandQueue[i], DataSource);
                queryEndTime = DateTime.Now;

                ReceivedResultsEventArgs rse = new ReceivedResultsEventArgs(DataSource, commandQueue[i], 
                    queryStartTime, queryEndTime, sqlResults);

                if (ReceivedResults != null) { ReceivedResults.Invoke(this, rse); }
            }
            queueEndTime = DateTime.Now;

            // send finishing event message to all listeners
            FinishedQueueEventHandlerArgs fqe = new FinishedQueueEventHandlerArgs(DataSource, commandQueue, 
                queueStartTime, queueEndTime, sqlResults);

            if (FinishedQueue != null) { FinishedQueue.Invoke(this, fqe); }
            if (waitHandle != null) { waitHandle.Set(); }
        } 

    }
}
