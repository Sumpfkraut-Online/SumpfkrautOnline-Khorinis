using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Utilities;
using System.IO;
using GUC.Utilities.FileSystem.Enumeration;

namespace GUC.Utilities.FileSystem
{

    public class FileSystemManager : Threading.AbstractRunnable
    {

        new public static readonly string _staticName = "FileSystemManager (static)";

        public static List<FileSystemManager> managerList = new List<FileSystemManager>();

        public static readonly int StandardCreateTries = 10;
        public static readonly int StandardCreateTimeout = 10; // [ms]

        public static readonly int StandardDeleteTries = 10;
        public static readonly int StandardDeleteTimeout = 10; // [ms]



        protected string root;
        public string Root { get { return this.root; } }
        public void SetRoot (string root) { this.root = root; }

        protected List<FileSystemProtocol> protocolQueue;

        protected object protocolQueueLock;

        public delegate bool FileSystemManipulationHandler (ref FileSystemProtocol protocol);


        protected int createTries;
        public int CreateTries { get { return createTries; } }
        public void SetCreateTries (int value) { createTries = value; }

        protected int createTimeout;
        public int CreateTimeout { get { return createTimeout; } }
        public void SetCreateTimeout (int value) { createTimeout = value; }

        protected int deleteTries;
        public int DeleteTries { get { return deleteTries; } }
        public void SetDeleteTries (int value) { deleteTries = value; }

        protected int deleteTimeout;
        public int DeleteTimeout { get { return deleteTimeout; } }
        public void SetDeleteTimeout (int value) { deleteTimeout = value; }

        public readonly Dictionary<FileSystemManipulation, FileSystemManipulationHandler> ManipulationToHandler = 
            new Dictionary<FileSystemManipulation, FileSystemManipulationHandler>()
        {
            //{ FileSystemManipulation.Create, TryCreate},
            //{ FileSystemManipulation.CreateFile, TryCreateFile},
            //{ FileSystemManipulation.CreateFolder, TryCreateFolder},

            //{ FileSystemManipulation.Delete, TryDelete},
            //{ FileSystemManipulation.DeleteFile, TryDeleteFile},
            //{ FileSystemManipulation.DeleteFolder, TryDeleteFolder},

            //{ FileSystemManipulation.Move, TryMove},
            //{ FileSystemManipulation.MoveFile, TryMoveFile},
            //{ FileSystemManipulation.MoveFolder, TryMoveFolder},
        };



        public FileSystemManager (string root, bool startOnCreate, TimeSpan timeout, bool runOnce)
            : base(startOnCreate, timeout, runOnce)
        {
            SetObjName("FileSystemManager (default)");
            this.root = root;
            this.protocolQueue = new List<FileSystemProtocol>();

            this.createTries = StandardCreateTries;
            this.createTimeout = StandardCreateTimeout;

            this.deleteTries = StandardDeleteTries;
            this.deleteTimeout = StandardDeleteTimeout;

            managerList.Add(this);
        }



        // adds a new protocol at the timestamp-position it belongs in the queue
        public void AddProtocol (ref FileSystemProtocol protocol)
        {
            lock (protocolQueueLock)
            {
                for (int i = protocolQueue.Count - 1; i > -1; i--)
                {
                    if (protocol.Timestamp > protocolQueue[i].Timestamp)
                    {
                        // insert new protocol after the other
                        protocolQueue.Insert(i + 1, protocol);
                    }
                }
            }
        }



        public void DeleteFile (string path, DateTime executionTime, int maxTries = -1, 
            List<object> options = null, 
            FileSystemProtocol.ProtocollApplicationEventHandler handler = null)
        {
            if (maxTries < 0) { maxTries = deleteTries; }

            FileSystemProtocol protocol = new FileSystemProtocol(FileSystemManipulation.DeleteFile,
                    path, executionTime, maxTries, options, handler);

            AddProtocol(ref protocol);
        }

        public void DeleteFolder (string path, DateTime executionTime, int maxTries = -1, 
            List<object> options = null, 
            FileSystemProtocol.ProtocollApplicationEventHandler handler = null)
        {
            if (maxTries < 0) { maxTries = deleteTries; }

            FileSystemProtocol protocol = new FileSystemProtocol(FileSystemManipulation.DeleteFolder,
                    path, executionTime, maxTries, options, handler);

            AddProtocol(ref protocol);
        }

        public static bool TryDelete (ref FileSystemProtocol protocol)
        {
            if (File.Exists(protocol.Path)) { return TryDeleteFile(ref protocol); }
            else                            { return TryDeleteFolder(ref protocol); }
        }

        public static bool TryDeleteFile (ref FileSystemProtocol protocol)
        {
            try
            {
                File.Delete(protocol.Path);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool TryDeleteFolder (ref FileSystemProtocol protocol)
        {
            try
            {
                File.Delete(protocol.Path);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        
        
        // destroy the FileSystemManager
        // - stop thread as soon as possible and remove FileSystemManager from static list
        // ! will not be garbage collected if still referenced somewhere else !
        public void Destroy ()
        {
            Suspend();
            managerList.Remove(this);
        }

        public override void Run ()
        {
            lock (protocolQueueLock)
            {
                DateTime now = DateTime.Now;
                for (int i = 0; i < protocolQueue.Count; i++)
                {
                    // no more present and past-due manipulations ahead in the list
                    if (protocolQueue[i].Timestamp > now) { break; }

                    // manipulate the valid protocol after all those checks
                    FileSystemProtocol protocol = protocolQueue[i];
                    FileSystemManipulationHandler manipulate;
                    if (ManipulationToHandler.TryGetValue(protocolQueue[i].Manipulation, out manipulate))
                    {
                        protocol.Tries++;

                        if (manipulate(ref protocol))
                        {
                            // successful manipulation
                            protocolQueue.RemoveAt(i);
                            i--;
                            protocol.InvokeProtocollApplication(ProtocolStatus.FinalSuccess);
                        }

                        if (protocolQueue[i].Tries >= protocolQueue[i].MaxTries)
                        {
                            // final fail at manipulating the file or folder
                            protocolQueue.RemoveAt(i);
                            i--;
                            protocol.InvokeProtocollApplication(ProtocolStatus.FinalFail);
                        }

                        // still tries left to succeed in manipulating the file or folder
                        protocol.InvokeProtocollApplication(ProtocolStatus.Fail);
                    }
                }
            }
        }

    }

}
