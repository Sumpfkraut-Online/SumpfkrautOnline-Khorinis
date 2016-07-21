using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Utilities;
using System.IO;

namespace GUC.Utilities.FileSystem
{

    public class FileSystemManager : Threading.AbstractRunnable
    {

        new public static readonly string _staticName = "FileSystemManager (static)";

        public static List<FileSystemManager> managers = new List<FileSystemManager>();



        protected List<FileSystemProtocol> protocolQueue;

        public delegate void CreateFileEventHandler (bool success);
        public delegate void DeleteFileEventHandler (bool success);
        public delegate void MoveFileEventHandler (bool success);

        public delegate void CreateFolderEventHandler (bool success);
        public delegate void DeleteFolderEventHandler (bool success);
        public delegate void MoveFolderEventHandler (bool success);

        protected int createTries = 10;
        public int CreateTries { get { return createTries; } }
        public void SetCreateTries (int value) { createTries = value; }

        protected int createTimeout = 10;
        public int CreateTimeout { get { return createTimeout; } }
        public void SetCreateTimeout (int value) { createTimeout = value; }

        protected int deleteTries = 10;
        public int DeleteTries { get { return deleteTries; } }
        public void SetDeleteTries (int value) { deleteTries = value; }

        protected int deleteTimeout = 10;
        public int DeleteTimeout { get { return deleteTimeout; } }
        public void SetDeleteTimeout (int value) { deleteTimeout = value; }



        public FileSystemManager (bool startOnCreate, TimeSpan timeout, bool runOnce)
            : base(startOnCreate, timeout, runOnce)
        {
            SetObjName("FileSystemManager (default)");
            protocolQueue = new List<FileSystemProtocol>();
            managers.Add(this);
        }



        // stop thread as soon as possible and remove FileSystemManager from static list
        public void Destroy ()
        {
            Suspend();
            managers.Remove(this);
        }

        

        public void CreateFile (string filePath, 
            CreateFileEventHandler createFileHandler = null)
        {
            CreateFile(filePath, CreateTries, CreateTimeout, createFileHandler);
        }

        public void CreateFile (string filePath, int tries, int tryTimeout, 
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

        protected void LoopCreate (string filePath, int tries, int tryTimeout, 
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

        public bool TryCreateFile (string filePath)
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

        

        public void DeleteFile (string filePath, 
            DeleteFileEventHandler deleteFileHandler = null)
        {
            DeleteFile(filePath, DeleteTries, DeleteTimeout, deleteFileHandler);
        }

        public void DeleteFile (string filePath, int tries, int tryTimeout, 
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

        protected void LoopDelete (string filePath, int tries, int tryTimeout, 
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

        public bool TryDeleteFile (string filePath)
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
