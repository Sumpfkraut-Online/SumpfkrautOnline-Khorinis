using GUC.Server.Scripts.Sumpfkraut.Database.DBQuerying;
using GUC.Server.Scripts.Sumpfkraut.Utilities.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Server.Scripts.Sumpfkraut.Database
{
    public class DBQueryHandler : Runnable
    {

        new public static readonly String _staticName = "DBQueryHandler (static)";
        new protected String _objName = "DBQueryHandler (default)";

        protected String sqLiteDataSource;
        public String GetSqLiteDataSource () { return this.sqLiteDataSource; }

        protected List<DBQuery> queryQueue = new List<DBQuery>();
        protected object queryQueueLock = new object();
        protected bool isPaused = false;



        public DBQueryHandler (String sqLiteDataSource)
            : this(true, sqLiteDataSource)
        { }

        public DBQueryHandler (bool startOnCreate, String sqLiteDataSource)
            : base(startOnCreate, TimeSpan.Zero, false)
        {
            this.sqLiteDataSource = sqLiteDataSource;
        }



        public void Add (DBQuery query)
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

        public bool RemoveQuery (DBQuery query)
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
                DBQuery nextQuery = queryQueue[0];
                List<List<List<object>>> results = new List<List<List<object>>>();

                switch (nextQuery.GetDBReaderMode())
                {
                    case DBReaderMode.loadData:
                        DBReader.LoadFromDB(ref results, nextQuery.GetSqlCommand());
                        nextQuery.ReturnResults(results);
                        break;
                    case DBReaderMode.saveData:
                        DBReader.SaveToDB(nextQuery.GetSqlCommand(), this.sqLiteDataSource);
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
