using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace com.continental.TDM.HelperLibrary
{   

    public class IniFileReader
    {
        //can be adjusted by user of IniFileReader if other chars are used
        /// <summary>
        /// the delimiter in a line
        /// </summary>
        public char Delimiter = ',';
        /// <summary>
        /// the delimiter of lines themself --> \n --> new line
        /// </summary>
        public char DelimiterLine = '\n';
        /// <summary>
        /// a mark for a comment in a ini file
        /// </summary>
        public string Comment = "#";

        /// <summary>
        /// Reads a given text file into a Dictionary<string, List<string>> structure
        /// Entries in a line should be seperated by a ','
        /// First value is the key for the dictionary.
        /// </summary>
        /// <param name="filename">path to ini file</param>
        /// <param name="filterComments">if true: all lines with comment marker are filtered (optional parameter)</param>
        /// <returns></returns>
        public Dictionary<string, List<string>> ReadFileToStructure(string filename, bool filterComments = true)
        {
            Dictionary<string, List<string>> result = null;
            if (new FileInfo(filename).Exists)
            {
                string content = System.IO.File.ReadAllText(filename).Trim(); //remove EOF line from result
                result = content.Split(new char[] {DelimiterLine},StringSplitOptions.RemoveEmptyEntries)
                                      .ToDictionary(i => i.Split(Delimiter)[0].Trim(),
                                                    i => i.Split(Delimiter).Skip(1).ToList<string>());              
            }
            if (result != null && filterComments)
                result = result.Where(x => !x.Key.StartsWith(Comment)).ToDictionary(k=>k.Key, v => v.Value);

            return result;
        }

        /// <summary>
        /// Reads a given text file into a Dictionary<string, string> structure
        /// Entries in a line should be seperated by a ','
        /// First value is the key for the dictionary.
        /// Rest is inserted as value (even if there are additional ','
        /// </summary>
        /// <param name="filename">path to ini file</param>
        /// <param name="filterComments">if true: all lines with comment marker are filtered (optional parameter)</param>
        /// <returns></returns>
        public Dictionary<string, string> ReadFileToSimpleKeyValueDictionary(string filename, bool filterComments = true)
        {
            Dictionary<string, string> result = null;
            if (new FileInfo(filename).Exists)
            {
                string content = System.IO.File.ReadAllText(filename).Trim(); //remove EOF line from result
                result = content.Split(new char[] { DelimiterLine }, StringSplitOptions.RemoveEmptyEntries)
                                      .ToDictionary(i => i.Split(Delimiter)[0].Trim(),
                                                    i => String.Join(",", i.Split(Delimiter).Skip(1).ToArray()).Trim());
            }
            if (filterComments)
                result = result.Where(x => !x.Key.StartsWith(Comment)).ToDictionary(k => k.Key, v => v.Value);

            return result;
        }
    }
}
