using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;

namespace com.continental.TDM.HelperLibrary
{
    public static class FileHelper
    {
        private static string[] size_unit = new string[] { "", "k", "M", "G", "T", "P", "E", "Z", "Y" };

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern int GetLongPathName(string path, StringBuilder longPath, int longPathLength);

        [DllImport("Kernel32.dll", CharSet = CharSet.Unicode)]
        private static extern bool CreateHardLink(string lpFileName, string lpExistingFileName, IntPtr lpSecurityAttributes);

        [DllImport("shlwapi.dll", EntryPoint = "PathRelativePathTo")]
        private static extern bool PathRelativePathTo(StringBuilder lpszDst, string from, UInt32 attrFrom, string to, UInt32 attrTo);


        /// <summary>
        /// size representation (convert)
        /// </summary>
        /// <param name="size">size</param>
        /// <param name="iec">wether to use IEC or not</param>
        /// <returns></returns>
        public static string SizeRepresentation(double size, bool iec = false, string appendix = "")
        {
            double div = (iec ? 1024 : 1000);
            int i = 0;
            for (; size > div && i < size_unit.Length - 1; i++)
                size /= div;

            return (double.IsInfinity(size) ? "ludicrous fast" : string.Format("{0:0.00}{1}{2}B{3}", size, size_unit[i], (iec ? "i" : ""), appendix));
        }

        /// <summary>
        /// check offline attribute and whether data management toolchain archived it
        /// </summary>
        /// <param name="fname">file name to check</param>
        /// <returns>true when offline</returns>
        public static bool CheckOfflineStatus(string fname)
        {
            // test file attribute
            if (File.GetAttributes(fname).HasFlag(FileAttributes.Offline))
            {
                // read file step by step
                int cnt = 0, step = 0;
                foreach (string line in File.ReadLines(fname))
                {
                    if (cnt == 0 && line.StartsWith("COHASH:"))
                        step++;
                    else if ((cnt == 2 || cnt == 4) && line.StartsWith("*******"))
                        step++;
                    else if (cnt == 3 && line.StartsWith("This file has been archived by Data Management Toolchain."))
                        step++;
                    cnt++;
                }
                if (step == 4)
                    return false;
                else
                    return true;
            }
            return false;
        }

        /// <summary>
        /// remove directory recursively down until not empty any more
        /// </summary>
        /// <param name="dir">initial dir</param>
        /// <param name="fname">file to be removed</param>
        public static bool PurgeFileDirs(string dir, string fname = null)
        {
            bool result = true;

            try
            {
                if (fname != null)
                {
                    File.Delete(Path.Combine(dir, fname));
                }

                if (Directory.EnumerateFileSystemEntries(dir).Count() == 0)
                {
                    Directory.Delete(dir);
                    PurgeFileDirs(dir.Substring(0, dir.LastIndexOf('\\')));
                }
            }
            catch 
            { 
                //something went wrong
                result = false;
            }
            return result;
        }

        /// <summary>
        /// returns a relative path from starting to end
        /// </summary>
        /// <param name="from">starting</param>
        /// <param name="to">end</param>
        /// <returns></returns>
        public static string GetRelativePath(string from, string to)
        {
            StringBuilder builder = new StringBuilder(1024);
            bool result = PathRelativePathTo(builder, from, 0, to, 0);
            return builder.ToString();
        }

        /// <summary>
        /// convert any short form of a path to long form
        /// </summary>
        /// <param name="name">full path name</param>
        /// <returns></returns>
        public static string GetLongPath(string name)
        {
            StringBuilder longPath = new StringBuilder(300);
            if (GetLongPathName(name, longPath, longPath.Capacity) > 0)
                return longPath.ToString();
            else
                return null;
        }

        /// <summary>
        /// create a shortcut on source
        /// </summary>
        /// <param name="source">source file name</param>
        /// <param name="destination">destination file name</param>
        public static void CreateShortcut(string source, string destination)
        {
            source += ".lnk";
            if (File.Exists(source))
                File.Delete(source);

            IWshRuntimeLibrary.WshShell shell = new IWshRuntimeLibrary.WshShell();
            IWshRuntimeLibrary.IWshShortcut shortcut = (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(source);

            shortcut.TargetPath = destination;
            shortcut.Save();
        }

        /// <summary>
        /// ensures directory part of path exists,
        /// path is passed through
        /// </summary>
        /// <param name="path">path</param>
        /// <returns>path</returns>
        public static string EnsureExisting(string path)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));

            return path;
        }        
    }
}
