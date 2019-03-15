using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.continental.TDM.TestStarterLib
{
    /// <summary>
    /// a real simple class to load dynamically and use for testing the Starter who retrieves it information via ini file configuration
    /// </summary>
    public class Testtool
    {
        public int Execute(string[] args)
        {
            return Convert.ToInt32(args[1]);
        }
    }
}
