using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
//using HashLib;
using System.Security.Cryptography;
using Force.Crc32;

namespace com.continental.TDM.HelperLibrary
{
    public static class IdHelper
    {
        /// <summary>
        /// generate a tan number (adaptation of Tom's python code)
        /// </summary>
        /// <returns>TAN</returns>
        public static string GenerateTan()
        {
            DateTime now = DateTime.UtcNow;
            Random rand = new Random();
            char[] rndpart = string.Format("{0:0}", Math.Round((now - Process.GetCurrentProcess().StartTime.ToUniversalTime()).TotalSeconds + (now - Process.GetCurrentProcess().StartTime.ToUniversalTime()).TotalMilliseconds) * 10000000 + (rand.NextDouble() * (rand.NextDouble() * 1000 + 10) + (rand.NextDouble() * 10 - 5))).ToCharArray();
            Array.Reverse(rndpart);
            // user id 00683 is static inside DB
            return string.Format("{0:yyyyMMddHHmmss}{1,8}00683{2}000000000000", now, Environment.MachineName, (new String(rndpart))).Substring(0, 32);
        }

        /// <summary>
        /// transforms an hash algo hash code to string
        /// </summary>
        /// <param name="algo">algo</param>
        /// <returns>hex hash code from algo</returns>
        public static string HashString(HashAlgorithm algo)
        {
            StringBuilder sBuilder = new StringBuilder();
            foreach (byte b in algo.Hash)
                sBuilder.Append(b.ToString("x2"));
            return sBuilder.ToString();
        }

        /// <summary>
        /// calculate and return CRC-32 from given text
        /// </summary>
        /// <param name="text">input text</param>
        /// <returns>CRC-32 uint</returns>
        public static uint Crc32(string text)
        {
            Crc32Algorithm crc = new Crc32Algorithm();
            byte[] buffer = Encoding.ASCII.GetBytes(text);
            return BitConverter.ToUInt32(crc.ComputeHash(buffer).Reverse().ToArray(), 0);
        }
    }
}
