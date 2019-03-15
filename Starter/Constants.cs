using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.continental.TDM.Starter
{
    public class Constants
    {
        public const string INI_PATH = "IniFiles\\Starter.ini";
        public const int PARAMETER_TOOL_NAME_POSITION = 0;

        public enum ToolDataIniPosition
        {
            //Name, <--Key
            DllName,
            EntryClassName,
            UpdateDir
        }

        public const string MAIN_METHOD = "Execute";

        //can be adjusted by user of IniFileReader if other chars are used
        /// <summary>
        /// the delimiter in a line
        /// </summary>
        public static char INI_DELIMITER = ',';
        /// <summary>
        /// the delimiter of lines themself --> \n --> new line
        /// </summary>
        public static char INI_DELIMITER_LINE = '\n';
        /// <summary>
        /// a mark for a comment in a ini file
        /// </summary>
        public static string INI_COMMENT = "#";

        public const int MAX_DLL_LOAD_ATTEMPTS = 18;
        public const int DEFAULT_SLEEP_TIME_IN_MS = 222;

        //all the other ERROR_CODES ARE IN HelperLibrary.Constants.Error
        public const int ERR_VCOPY_LOAD_ERROR = -3902;
    }
}
