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

        public static List<FileSystemManager> managerList = new List<FileSystemManager>();

        public static readonly int StandardCreateTries = 10;
        public static readonly int StandardCreateTimeout = 10; // [ms]

        public static readonly int StandardDeleteTries = 10;
        public static readonly int StandardDeleteTimeout = 10; // [ms]

        public static readonly int StandardMoveTries = 10;
        public static readonly int StandardMoveTimeout = 10; // [ms]



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

        protected int moveTries;
        public int MoveTries { get { return moveTries; } }
        public void SetMoveTries (int value) { moveTries = value; }

        protected int moveTimeout;
        public int MoveTimeout { get { return moveTimeout; } }
        public void SetMoveTimeout (int value) { moveTimeout = value; }

        public readonly Dictionary<FileSystemManipulation, FileSystemManipulationHandler> ManipulationToHandler = 
            new Dictionary<FileSystemManipulation, FileSystemManipulationHandler>()
        {
            { FileSystemManipulation.Create, TryCreate},
            { FileSystemManipulation.CreateFile, TryCreateFile},
            { FileSystemManipulation.CreateFolder, TryCreateFolder},

            { FileSystemManipulation.Delete, TryDelete},
            { FileSystemManipulation.DeleteFile, TryDeleteFile},
            { FileSystemManipulation.DeleteFolder, TryDeleteFolder},

            { FileSystemManipulation.Move, TryMove},
            { FileSystemManipulation.MoveFile, TryMoveFile},
            { FileSystemManipulation.MoveFolder, TryMoveFolder},
        };



        public FileSystemManager (string root, bool startOnCreate, TimeSpan timeout, bool runOnce)
            : base(startOnCreate, timeout, runOnce)
        {
            this.root = root;
            this.protocolQueue = new List<FileSystemProtocol>();
            this.protocolQueueLock = new object();

            this.createTries = StandardCreateTries;
            this.createTimeout = StandardCreateTimeout;

            this.deleteTries = StandardDeleteTries;
            this.deleteTimeout = StandardDeleteTimeout;

            this.moveTries = StandardMoveTries;
            this.moveTimeout = StandardMoveTimeout;

            managerList.Add(this);
        }



        // adds a new protocol at the timestamp-position it belongs in the queue
        public void AddProtocol (ref FileSystemProtocol protocol)
        {
            int index = 0;
            lock (protocolQueueLock)
            {
                for (int i = protocolQueue.Count; i > 0; i--)
                {
                    if (protocol.Timestamp > protocolQueue[i].Timestamp)
                    {
                        // insert new protocol after the other
                        index = i;
                        //protocolQueue.Insert(i + 1, protocol);
                    }
                }

                protocolQueue.Insert(index, protocol);
                this.Resume();
            }
        }

        public string GetRealPath (string path)
        {
            return root + path;
        }



        public void MoveFile (string path, int maxTries = -1, 
            List<object> options = null, FileSystemProtocol.ProtocollApplicationEventHandler handler = null)
        {
            MoveFile(path, DateTime.Now, maxTries, options, handler);
        }

        public void MoveFile (string path, DateTime executionTime, int maxTries = -1, 
            List<object> options = null, FileSystemProtocol.ProtocollApplicationEventHandler handler = null)
        {
            if (maxTries < 0) { maxTries = moveTries; }

            FileSystemProtocol protocol = new FileSystemProtocol(FileSystemManipulation.MoveFile,
                    GetRealPath(path), executionTime, maxTries, options, handler);

            AddProtocol(ref protocol);
        }

        public void MoveFolder (string path, int maxTries = -1, 
            List<object> options = null, FileSystemProtocol.ProtocollApplicationEventHandler handler = null)
        {
            MoveFolder(path, DateTime.Now, maxTries, options, handler);
        }

        public void MoveFolder (string path, DateTime executionTime, int maxTries = -1, 
            List<object> options = null, 
            FileSystemProtocol.ProtocollApplicationEventHandler handler = null)
        {
            if (maxTries < 0) { maxTries = moveTries; }

            FileSystemProtocol protocol = new FileSystemProtocol(FileSystemManipulation.MoveFolder,
                    GetRealPath(path), executionTime, maxTries, options, handler);

            AddProtocol(ref protocol);
        }

        public static bool TryMove (ref FileSystemProtocol protocol)
        {
            if (File.Exists(protocol.Path)) { return TryMoveFile(ref protocol); }
            else                            { return TryMoveFolder(ref protocol); }
        }

        public static bool TryMoveFile (ref FileSystemProtocol protocol)
        {
            try
            {
                File.Move(protocol.Path, protocol.Options[0].ToString());
                return true;
            }
            catch (Exception ex) { return false; }
        }

        public static bool TryMoveFolder (ref FileSystemProtocol protocol)
        {
            try
            {
                Directory.Move(protocol.Path, protocol.Options[0].ToString());
                return true;
            }
            catch (Exception ex) { return false; }
        }



        public void CreateFile (string path, int maxTries = -1, 
            List<object> options = null, FileSystemProtocol.ProtocollApplicationEventHandler handler = null)
        {
            CreateFile(path, DateTime.Now, maxTries, options, handler);
        }

        public void CreateFile (string path, DateTime executionTime, int maxTries = -1, 
            List<object> options = null, FileSystemProtocol.ProtocollApplicationEventHandler handler = null)
        {
            if (maxTries < 0) { maxTries = deleteTries; }

            FileSystemProtocol protocol = new FileSystemProtocol(FileSystemManipulation.DeleteFile,
                    GetRealPath(path), executionTime, maxTries, options, handler);

            AddProtocol(ref protocol);
        }

        public void CreateFolder(string path, int maxTries = -1, 
            List<object> options = null, FileSystemProtocol.ProtocollApplicationEventHandler handler = null)
        {
            CreateFolder(path, DateTime.Now, maxTries, options, handler);
        }

        public void CreateFolder (string path, DateTime executionTime, int maxTries = -1, 
            List<object> options = null, 
            FileSystemProtocol.ProtocollApplicationEventHandler handler = null)
        {
            if (maxTries < 0) { maxTries = deleteTries; }

            FileSystemProtocol protocol = new FileSystemProtocol(FileSystemManipulation.DeleteFolder,
                    GetRealPath(path), executionTime, maxTries, options, handler);

            AddProtocol(ref protocol);
        }

        public static bool TryCreate (ref FileSystemProtocol protocol)
        {
            if (File.Exists(protocol.Path)) { return TryCreateFile(ref protocol); }
            else                            { return TryCreateFolder(ref protocol); }
        }

        public static bool TryCreateFile (ref FileSystemProtocol protocol)
        {
            try
            {
                File.Create(protocol.Path);
                return true;
            }
            catch (Exception ex) { return false; }
        }

        public static bool TryCreateFolder (ref FileSystemProtocol protocol)
        {
            try
            {
                Directory.CreateDirectory(protocol.Path);
                return true;
            }
            catch (Exception ex) { return false; }
        }



        public void DeleteFile (string path, int maxTries = -1, 
            List<object> options = null, FileSystemProtocol.ProtocollApplicationEventHandler handler = null)
        {
            DeleteFile(path, DateTime.Now, maxTries, options, handler);
        }

        public void DeleteFile (string path, DateTime executionTime, int maxTries = -1, 
            List<object> options = null, FileSystemProtocol.ProtocollApplicationEventHandler handler = null)
        {
            if (maxTries < 0) { maxTries = deleteTries; }

            FileSystemProtocol protocol = new FileSystemProtocol(FileSystemManipulation.DeleteFile,
                    GetRealPath(path), executionTime, maxTries, options, handler);

            AddProtocol(ref protocol);
        }

        public void DeleteFolder(string path, int maxTries = -1, 
            List<object> options = null, FileSystemProtocol.ProtocollApplicationEventHandler handler = null)
        {
            DeleteFolder(path, DateTime.Now, maxTries, options, handler);
        }

        public void DeleteFolder (string path, DateTime executionTime, int maxTries = -1, 
            List<object> options = null, 
            FileSystemProtocol.ProtocollApplicationEventHandler handler = null)
        {
            if (maxTries < 0) { maxTries = deleteTries; }

            FileSystemProtocol protocol = new FileSystemProtocol(FileSystemManipulation.DeleteFolder,
                    GetRealPath(path), executionTime, maxTries, options, handler);

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
            catch (Exception ex) { return false; }
        }

        public static bool TryDeleteFolder (ref FileSystemProtocol protocol)
        {
            try
            {
                Directory.Delete(protocol.Path);
                return true;
            }
            catch (Exception ex) { return false; }
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
                if (protocolQueue.Count < 1) { this.Suspend(); }

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
                            continue;
                        }

                        if (protocolQueue[i].Tries >= protocolQueue[i].MaxTries)
                        {
                            // final fail at manipulating the file or folder
                            protocolQueue.RemoveAt(i);
                            i--;
                            protocol.InvokeProtocollApplication(ProtocolStatus.FinalFail);
                            continue;
                        }

                        // still tries left to succeed in manipulating the file or folder
                        protocol.InvokeProtocollApplication(ProtocolStatus.Fail);
                        // assign new iterated value to protocolQueue
                        protocolQueue[i] = protocol;
                    }
                }
            }
        }

    }

}
