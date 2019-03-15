using System;
using System.IO;
using System.Reflection;

namespace com.continental.TDM.HelperLibrary
{
    public static class ReflectionHelper
    {
        public static string CodeBase { get { return Assembly.GetCallingAssembly().CodeBase; }}        
        /// <summary>
        /// Returns the path of the library that calls this method
        /// </summary>
        public static string AssemblyDirectoryPath { get { return DirectoryOfAssembly(Assembly.GetCallingAssembly()); }}
        
        public static string CodeBaseExecutable { get { return Assembly.GetEntryAssembly().CodeBase; } }
        /// <summary>
        /// Returns the path of the executable
        /// </summary>
        public static string ExecutorAssemblyDirectoryPath { get { return DirectoryOfAssembly(Assembly.GetEntryAssembly()); } }

        /// <summary>
        /// returns the Directory of the provided assembly
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static string DirectoryOfAssembly(Assembly assembly)
        {
            UriBuilder uri = new UriBuilder(assembly.CodeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            return Path.GetDirectoryName(path);
        }        
    }    
}
