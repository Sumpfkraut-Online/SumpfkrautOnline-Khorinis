using GUC.Scripts.Sumpfkraut.Database.DBQuerying;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Sumpfkraut.Database
{
    public class DBQueryHandler : GUC.Utilities.Threading.AbstractRunnable
    {

        new public static readonly String _staticName = "DBQueryHandler (static)";

        protected String dataSource;
        public String GetDataSource () { return this.dataSource; }

        protected List<AbstractDBQuery> queryQueue = new List<AbstractDBQuery>();
        protected object queryQueueLock = new object();
        protected bool isPaused = false;



        public DBQueryHandler (String dataSource)
            : this(true, dataSource)
        { }

        public DBQueryHandler (bool startOnCreate, String dataSource)
            : base(startOnCreate, TimeSpan.Zero, false)
        {
            SetObjName("DBQueryHandler (default)");
            this.dataSource = dataSource;
        }



        public void Add (AbstractDBQuery query)
        {
            lock (queryQueueLock)
            {
                queryQueue.Add(query);

                // if paused continue running to handle the new queue-entry
                if (isPaused)
                {
                    Resume();
                }
            }
        }

        public bool RemoveQuery (AbstractDBQuery query)
        {
            lock (queryQueueLock)
            {
                return queryQueue.Remove(query);
            }
        }



        public override void Resume ()
        {
            isPaused = false;
            base.Resume();
        }

        public override void Suspend ()
        {
            isPaused = true;
            base.Suspend();
        }

        public override void Abort ()
        {
            isPaused = true;
            base.Abort();
        }



        public override void Run ()
        {
            base.Run();

            lock (queryQueueLock)
            {
                if (queryQueue.Count < 1)
                {
                    Suspend();
                    return;
                }

                // grab next query of the queue and call the database
                AbstractDBQuery nextQuery = queryQueue[0];
                List<List<List<object>>> results = new List<List<List<object>>>();

                switch (nextQuery.GetDBReaderMode())
                {
                    case DBReaderMode.loadData:
                        DBReader.LoadFromDB(ref results, nextQuery.GetSqlCommand());
                        nextQuery.ReturnResults(results);
                        break;
                    case DBReaderMode.saveData:
                        DBReader.SaveToDB(nextQuery.GetSqlCommand(), this.dataSource);
                        nextQuery.ReturnResults(null);
                        break;
                    default:
                        MakeLogWarning(String.Format("Cannot contact database using invalid"
                            + " dbReaderMode {0}!", nextQuery.GetDBReaderMode()));
                        break;
                }

                queryQueue.RemoveAt(0);    
            }
        }

    }
}
