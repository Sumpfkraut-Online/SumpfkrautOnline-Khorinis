using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Utilities;
using System.IO;

namespace GUC.Scripts.Sumpfkraut.Database
{

    // a simple class to overwatch the folder-structure of all database-files
    public class DBFileManager : ExtendedObject
    {

        new public static readonly string _staticName = "DBFileHandler (static)";

        protected static int deleteTries = 10;
        public static int DeleteTries { get { return deleteTries; } }
        public static void SetDeleteTries (int value) { deleteTries = value; }

        protected static int deleteTimeout = 10;
        public static int DeleteTimeout { get { return deleteTimeout; } }
        public static void SetDeleteTimeout (int value) { deleteTimeout = value; }

        protected static string rootDirectory = "";
        public static void SetRootDirectory (string newRoot)
        {
            string oldRoot = rootDirectory;
        }



        public DBFileManager ()
            :this("DBFileManager (default)")
        { }

        public DBFileManager (string objName)
        {
            SetObjName(objName);
        }



        public delegate void DeleteFileEventHandler (bool success);

        public static void DeleteFile (string filePath, 
            DeleteFileEventHandler deleteFileHandler = null)
        {
            DeleteFile(filePath, DeleteTries, DeleteTimeout, deleteFileHandler);
        }

        public static void DeleteFile (string filePath, int tries, int tryTimeout, 
            DeleteFileEventHandler deleteFileHandler = null)
        {
            if (File.Exists(filePath))
            {
                GUC.Utilities.Threading.Runnable deletor = new GUC.Utilities.Threading.Runnable(false, 
                    TimeSpan.Zero, true);
                deletor.OnRun += delegate (GUC.Utilities.Threading.Runnable sender) 
                {
                    LoopDelete(filePath, tries, tryTimeout, deleteFileHandler);
                };
                deletor.Start();
            }
        }

        protected static void LoopDelete (string filePath, int tries, int tryTimeout, 
            DeleteFileEventHandler deleteFileHandler = null)
        {
            bool success = false;

            for (int i = 0; i < tries; i++)
            {
                if (TryDeleteFile(filePath))
                {
                    success = true;
                    break;
                }

                System.Threading.Thread.Sleep(tryTimeout);
            }

            if (deleteFileHandler != null) { deleteFileHandler(success); }
            else { Log.Logger.Log("??????????????????????"); }
        }

        public static bool TryDeleteFile (string filePath)
        {
            try
            {
                File.Delete(filePath);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }

}
