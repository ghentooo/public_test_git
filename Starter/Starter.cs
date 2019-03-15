using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Threading;

namespace com.continental.TDM.Starter
{
    /// <summary>
    /// Provides a loosly coupled mechanism to start tools. Reads needed information from Starter.ini file.
    /// </summary>
    public class Starter
    {
        public static int Main(string[] args)
        {
            int result = 0;
            Log("I am alive...");

            try
            {

                Dictionary<string, List<string>> tools = ReadFileToStructure(Constants.INI_PATH);

                if (tools != null && tools.Count > 0)
                {
                    if (tools.ContainsKey(args[Constants.PARAMETER_TOOL_NAME_POSITION]))
                    {
                        //Update(); //TODO
                        result = ExecuteJob(tools[args[Constants.PARAMETER_TOOL_NAME_POSITION]], args);
                    }
                    else
                    {
                        Log("File/Class data not provided in Starter.ini for tool...");
                        result = Constants.ERR_VCOPY_LOAD_ERROR;
                    }
                }
                else
                {
                    Log("Starter.ini not provided...");
                    result = Constants.ERR_VCOPY_LOAD_ERROR;
                }
            }
            catch (Exception ex)
            {
                Log(ex.Message);
                result = Constants.ERR_VCOPY_LOAD_ERROR;
            }

            //Console.ReadKey();
            return result;
        }

        protected static int ExecuteJob(List<string> toolData, string[] args)
        {
            //Result result = Result.Ok;
            int result = 0;

            // load assembly ...
            Assembly assembly = null;
            for (int i = 0; i < Constants.MAX_DLL_LOAD_ATTEMPTS; i++)
            {
                try
                {
                    assembly = Assembly.Load(File.ReadAllBytes(toolData[(int)Constants.ToolDataIniPosition.DllName]));
                    break;
                }
                catch
                {
                    Thread.Sleep(Constants.DEFAULT_SLEEP_TIME_IN_MS * i);
                }
            }
            if (assembly != null)
            {
                Type atype = assembly.GetType(toolData[(int)Constants.ToolDataIniPosition.EntryClassName]);
                // create an instance ...
                //ITool tool = (ITool)Activator.CreateInstance(atype);
                object tool = Activator.CreateInstance(atype);
                // create method ...
                MethodInfo ameth = atype.GetMethod(Constants.MAIN_METHOD);
                // ... and excute
                result = (int)ameth.Invoke(tool, new object[] { args });
            }
            else
            {
                Log("Unable to load assembly!");
                //result.Fails(Constants.ERR_MESSAGE_UNABLE_TO_LOAD, HelperLibrary.Constants.Error.ERR_VCOPY_LOAD_ERROR);
                result = Constants.ERR_VCOPY_LOAD_ERROR;
            }

            return result;
        }

        #region private helpers
        //stolen from helperlibrary.fileinireader
        private static Dictionary<string, List<string>> ReadFileToStructure(string filename, bool filterComments = true)
        {
            Dictionary<string, List<string>> result = null;
            if (new FileInfo(filename).Exists)
            {
                string content = System.IO.File.ReadAllText(filename).Trim(); //remove EOF line from result
                result = content.Split(new char[] { Constants.INI_DELIMITER_LINE }, StringSplitOptions.RemoveEmptyEntries)
                                      .ToDictionary(i => i.Split(Constants.INI_DELIMITER)[0].Trim(),
                                                    i => i.Split(Constants.INI_DELIMITER).Skip(1).ToList<string>());
            }
            if (result != null && filterComments)
                result = result.Where(x => !x.Key.StartsWith(Constants.INI_COMMENT)).ToDictionary(k => k.Key, v => v.Value);

            return result;
        }

        private static void Log(string message)
        {
            Console.WriteLine(string.Format("[{0}] {1}", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff"), message));
        }
        #endregion
    }
}
