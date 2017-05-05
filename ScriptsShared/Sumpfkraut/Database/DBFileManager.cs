using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Utilities;
using System.IO;

namespace GUC.Scripts.Sumpfkraut.Database
{

    // a simple class to overwatch the folder-structure of all database-files
    public partial class DBFileManager : ExtendedObject
    {

        new public static readonly string _staticName = "DBFileHandler (s)";



        protected static string rootDirectory = "";
        public static void SetRootDirectory (string newRoot)
        {
            string oldRoot = rootDirectory;
        }

        protected static int deleteTries = 10;
        public static int DeleteTries { get { return deleteTries; } }
        public static void SetDeleteTries (int value) { deleteTries = value; }

        protected static int deleteTimeout = 10;
        public static int DeleteTimeout { get { return deleteTimeout; } }
        public static void SetDeleteTimeout (int value) { deleteTimeout = value; }

        protected static int createTries = 10;
        public static int CreateTries { get { return createTries; } }
        public static void SetCreateTries (int value) { createTries = value; }

        protected static int createTimeout = 10;
        public static int CreateTimeout { get { return createTimeout; } }
        public static void SetCreateTimeout (int value) { createTimeout = value; }


        //public DBFileManager ()
        //    :this("DBFileManager (default)")
        //{ }

        //public DBFileManager (string objName)
        //{
        //    SetObjName(objName);
        //}



        public delegate void CreateFileEventHandler (bool success);

        public static void CreateFile (string filePath, 
            CreateFileEventHandler createFileHandler = null)
        {
            CreateFile(filePath, CreateTries, CreateTimeout, createFileHandler);
        }

        public static void CreateFile (string filePath, int tries, int tryTimeout, 
            CreateFileEventHandler createFileHandler = null)
        {
            if (File.Exists(filePath))
            {
                GUC.Utilities.Threading.Runnable deletor = new GUC.Utilities.Threading.Runnable(false, 
                    TimeSpan.Zero, true);
                deletor.OnRun += delegate (GUC.Utilities.Threading.Runnable sender) 
                {
                    LoopCreate(filePath, tries, tryTimeout, createFileHandler);
                };
                deletor.Start();
            }
        }

        protected static void LoopCreate (string filePath, int tries, int tryTimeout, 
            CreateFileEventHandler createFileHandler = null)
        {
            bool success = false;

            for (int i = 0; i < tries; i++)
            {
                if (TryCreateFile(filePath))
                {
                    success = true;
                    break;
                }

                System.Threading.Thread.Sleep(tryTimeout);
            }

            if (createFileHandler != null) { createFileHandler(success); }
        }

        public static bool TryCreateFile (string filePath)
        {
            try
            {
                File.Create(filePath);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
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
