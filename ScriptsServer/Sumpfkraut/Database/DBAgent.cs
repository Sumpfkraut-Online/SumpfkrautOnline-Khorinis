using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace GUC.Scripts.Sumpfkraut.Database
{
    public class DBAgent : GUC.Utilities.Threading.AbstractRunnable
    {

        new public static readonly string _staticName = "DBAgent (static)";

        protected string dataSource = null;
        protected List<String> commandQueue = new List<String>();



        public delegate void ReceivedResultsEventHandler (GUC.Utilities.Threading.AbstractRunnable sender, 
            ReceivedResultsEventArgs e);
        public event ReceivedResultsEventHandler ReceivedResults;
        public class ReceivedResultsEventArgs : EventArgs
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
        public class FinishedQueueEventHandlerArgs
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



        public DBAgent(List<String> commandQueue)
            : this(commandQueue, true)
        { }

        public DBAgent(List<String> commandQueue, bool startOnCreate)
            : base(false, new TimeSpan(0, 0, 0), true)
        {
            this.commandQueue = commandQueue;

            if (startOnCreate)
            {
                this.Start();
            }
        }



        public override void Run()
        {
            base.Run();

            List<List<List<object>>> sqlResults = new List<List<List<object>>>();
            DateTime queueStartTime, queueEndTime, queryStartTime, queryEndTime;

            // iterate over commandQueue and communicating with DB while invoking the events
            queueStartTime = DateTime.Now;
            for (int i = 0; i < commandQueue.Count; i++)
            {
                queryStartTime = DateTime.Now;
                DBReader.LoadFromDB(ref sqlResults, commandQueue[i]);
                queryEndTime = DateTime.Now;

                ReceivedResultsEventArgs rse = new ReceivedResultsEventArgs(dataSource, commandQueue[i], 
                    queryStartTime, queryEndTime, sqlResults);
                ReceivedResults.Invoke(this, rse);

                if (waitHandle != null)
                {
                    waitHandle.Set();
                }
            }
            queueEndTime = DateTime.Now;

            // send finishing event message to all listeners
            FinishedQueueEventHandlerArgs fqe = new FinishedQueueEventHandlerArgs(dataSource, commandQueue, 
                queueStartTime, queueEndTime, sqlResults);
            FinishedQueue.Invoke(this, fqe);
            //this.Suspend();
        } 

    }
}
