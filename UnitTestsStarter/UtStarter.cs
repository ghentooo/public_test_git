using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using com.continental.TDM.HelperLibrary;
using com.continental.TDM.Starter;

namespace com.continental.TDM.Starter.Unittests
{

    [TestClass]
    public class UtStarter
    {
        private const string ToolName = "Testtool";
        private const string TestDllName = "TestStarterLib.dll";
        private const string EntryClass = "com.continental.TDM.TestStarterLib.Testtool";
        private const string UpdatePath = ""; 
        private const int TestResult = 666;

        protected const string TEST_INI_FILE = com.continental.TDM.Starter.Constants.INI_PATH; // "IniFiles\\Starter.ini";
        
        [TestInitialize()]
        public void Initialize()
        {
            string iniPath = ReflectionHelper.DirectoryOfAssembly(Assembly.GetAssembly(GetType())) + "\\" + TEST_INI_FILE;
            FileInfo IniFile = new FileInfo(iniPath);
            if (!IniFile.Directory.Exists)
            {
                IniFile.Directory.Create();
            }

            //Write Ini
            if (!IniFile.Exists) //create simple test file
            {
                //don't catch exceptions! what shall we do then? o_0
                using (StreamWriter writer = File.CreateText(iniPath))
                {
                    //Name, <--Key
                    //DllName,
                    //EntryClassName,
                    //UpdateDir
                    writer.WriteLine("#A test file for the com.continental.TDM.Starter.Starter"); //HPC
                    writer.WriteLine(String.Format("{0},{1}, {2}, {3}", ToolName, TestDllName,EntryClass, UpdatePath));
                }
                Thread.Sleep(100);

                Assert.IsTrue(new FileInfo(iniPath).Exists);
            }
            

            //TestAssembly should exist!          
            Assert.IsTrue(new FileInfo(ReflectionHelper.DirectoryOfAssembly(Assembly.GetAssembly(GetType())) + "\\" + TestDllName).Exists);
        }

        [TestMethod]
        public void TestStarter()
        {
            //Call Starter
            int result = com.continental.TDM.Starter.Starter.Main(new string[] { ToolName, TestResult.ToString() });

            Assert.AreEqual(result, TestResult);
        }

        [TestMethod]
        public void TestTemp()
        {
          
            Assert.AreEqual(1, 1); //Fails
        }
    }
}
