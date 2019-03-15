using System;

namespace com.continental.TDM.HelperLibrary
{
    public sealed class Constants
    {
        public enum Direction 
        { 
            server, 
            legacy, 
            disk, 
            none 
        };

        public static Direction GetDirectionByName(string direction)
        {
            return (Direction)Enum.Parse(typeof(Direction), direction);
        }

        /// <summary>
        /// All Constants for errors, fell free to add (new numbers!)
        /// </summary>
        public enum Error 
        { //order the values 0, int.MIN .... int.MAX
            ERR_NO_ERROR = 0,
            ERR_VCOPY_INVALID_COMMANDLINE_ARGUMENTS = -3998,
            ERR_VCOPY_DST_ALREADY_EXISTS = -3994,
            ERR_VCOPY_SRC_FILE_ILLEGAL_NAME = -3992,
            ERR_VCOPY_SRC_FILE_NOT_AVAILABLE = -3991,            
            ERR_VCOPY_LOAD_ERROR = -3902,
            ERR_VCOPY_UNSPECIFIED_ERROR = -3901,
            ERR_VCOPY_ABORTED = -3900,
            ERR_VCOPY_ACCESS_DENIED = -3989,
            ERR_VCOPY_ERROR_DURING_COPY = -3988,
            ERR_VCOPY_SOURCE_FILE_OFFLINE = -3987,
            ERR_VCOPY_SOURCE_FILE_SIZE = -3986,
            ERR_VCOPY_DST_PATH_TOO_LONG = -3984,
            ERR_VCOPY_DST_HOST_NOT_EXIST = -3983,
            ERR_VCOPY_UNKNOWN_LOCATION = -3982,
            ERR_VCOPY_MKS_FAULT = -3971,
            ERR_VCOPY_DB_ERROR = -3970,
            ERR_HASH_DB_LOOKUP = -3966,
            ERR_HASH_ERROR  = -3965,
            ERR_CHKDSK_REQUIRED = -3961,
            ERR_CHKDSK_ERROR = -3960,
            ERR_APP_ERR_NETWORK_UNAVAILABLE = -360,

            ERR_FOR_TESTING = 666, //can be changed
            ERR_WHILE_WIRING_STARTUP = 998, //can be changed
            ERR_UNSPECIFIC_ERROR = 999, //can be changed
            ERR_CLASS_TO_EXECUTE_NOT_FOUND = 10000, //can be changed
            ERR_CLASS_TO_EXECUTE_NOT_PROVIDED = 10001, //can be changed
            ERR_INI_FILE_NOT_FOUND = 10002, //can be changed
            ERR_INI_FILE_UNEXPECTED_FORMAT = 10003, //can be changed
            ERR_WHILE_INTERRUPTION_OCCURED = 10004 //can be changed
        }
    }
}

