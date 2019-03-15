using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace com.continental.TDM.HelperLibrary
{
    /// <summary>
    /// simple data object of all the informations of a file used by the tools (e.g. copy)
    /// </summary>
    public class FileMetaData
    {
        public string SourceFilePath = string.Empty;
        public string FastSourceFilePath;
        public long SourceSize;
        public string DestinationFilePath = string.Empty;
        public string FastDestinationFilePath;
        public long DestinationSize;
        public bool DestinationExists;
        public string ProjectName; //<--Input Parameter
        public decimal ImsIssue;
        public string Asset;
        public string Family; //<--Input Parameter
        public string Location;
        public HelperLibrary.ServerShare FinalShare;
        public HelperLibrary.Constants.Direction CopyDirection;
        public DateTime RecordTime;

        //public decimal? MeasId;        
        public string MeasId;
        public decimal? DbSize;
        public decimal? Crc32;
        public decimal? ProjectId; //? pid <-- Value from DB
        public string MeasState = "dummy";
        public string Filehashstring;
        public double ReadTimeInSec;
        public string Tan;
        public string ParentId = "NULL";
        public string FamilyId; // <-- Value from DB
        public string HashState;
        public bool HashValid;

        public string SlaveCount = ""; //empty means leading measurement attribute postfix
        public string ImportDate;

        public string[] SourceParts
        {
            get { return SourceFilePath.Split(new char[] { Path.DirectorySeparatorChar }, 3, StringSplitOptions.RemoveEmptyEntries); }
        }

        public string[] DestinationParts
        {
            get { return DestinationFilePath.Split(new char[] { Path.DirectorySeparatorChar }, 4, StringSplitOptions.RemoveEmptyEntries); }
        }
    }
}
