using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripts.Sumpfkraut.Database;
using GUC.Server.Scripts.Sumpfkraut.Utilities.Threading;

namespace GUC.Server.Scripts.Sumpfkraut.Database
{
    class DBAgent : AbstractRunnable
    {

        protected List<String> commandQueue = new List<String>();

        //public delegate void AwaitingResultsEventHandler(object sender, AwaitingResultsEventArgs e);
        //public event AwaitingResultsEventHandler AwaitingResult;
        //public class AwaitingResultsEventArgs : EventArgs
        //{
        //    private DateTime timestamp;
        //    public DateTime GetTimestamp() { return this.timestamp; }

        //    private int queueIndex;
        //    public int GetQueueIndex() { return this.queueIndex; }

        //    public AwaitingResultsEventArgs(int queueIndex)
        //    {
        //        this.timestamp = DateTime.Now;
        //        this.queueIndex = queueIndex;
        //    }
        //}

        public delegate void ReceivedResultsEventHandler (object sender, ReceivedResultsEventArgs e);
        public event ReceivedResultsEventHandler ReceivedResults;
        public class ReceivedResultsEventArgs : EventArgs
        {
            private DateTime timestamp;
            public DateTime GetTimestamp() { return this.timestamp; }

            private int queueIndex;
            public int GetQueueIndex() { return this.queueIndex; }

            private List<List<List<object>>> results;
            public List<List<List<object>>> GetResults() { return this.results; }

            public ReceivedResultsEventArgs(int queueIndex, List<List<List<object>>> results)
            {
                this.timestamp = DateTime.Now;
                this.queueIndex = queueIndex;
                this.results = results;
            }
        }

        public delegate void FinishedQueueEventHandler(object sender);
        public event FinishedQueueEventHandler FinishedQueue;


        public DBAgent(List<String> commandQueue)
            : this(commandQueue, true)
        { }

        public DBAgent(List<String> commandQueue, bool startOnCreate)
        {
            this.commandQueue = commandQueue;

            if (startOnCreate)
            {
                this.Start();
            }
        }



        public override void Run()
        {
            // iterate over commandQueue and communicating with DB while invoking the events
            for (int i = 0; i < commandQueue.Count; i++)
            {
                List<List<List<object>>> results = new List<List<List<object>>>();
                DBReader.LoadFromDB(ref results, commandQueue[i]);
                ReceivedResultsEventArgs e = new ReceivedResultsEventArgs(i, results);
                ReceivedResults.Invoke(this, e);
            }

            FinishedQueue.Invoke(this);
            this.Suspend();
        } 

    }
}
