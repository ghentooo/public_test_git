using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.continental.TDM.HelperLibrary
{
    public static class Delegates
    {
        //public Action<string> Log;
        //public delegate bool IsInterrupted(); -->Func<bool>
        public delegate void PurgeFileDirs(string destinationFilePath);
        public delegate Result DeleteFileFromDb(FileMetaData MetaData);
        //public delegate Result CopyConcurrentlyFiles(string concurrentSourceFilePath, string concurrentDestinationFilePath, FileMetaData MeteData, string metaLocal, Dictionary<string, string> mdmShare, Action<string> log, Func<bool> isInterrupted, PurgeFileDirs purge, DeleteFileFromDb deleteFromDb);
        public delegate Result CopyConcurrentlyFiles(string concurrentSourceFilePath, string concurrentDestinationFilePath, string versionNumber, List<string> otherFilePaths, FileMetaData MetaData, Action<string> log, Func<bool> isInterrupted, Delegates.PurgeFileDirs purge, Delegates.DeleteFileFromDb deleteFromDb);
    }
}
