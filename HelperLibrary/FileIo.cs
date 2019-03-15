using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace com.continental.TDM.HelperLibrary
{
    public class FileIo : IDisposable
    {
        // Constants required to handle file I/O:
        private const uint GENERIC_READ = 0x80000000;
        private const uint GENERIC_WRITE = 0x40000000;
        private const uint FILE_SHARE_READ = 0x00000001;

        private const uint CREATE_NEW = 1;
        private const uint CREATE_ALWAYS = 2;
        private const uint OPEN_EXISTING = 3;
        private const uint OPEN_ALWAYS = 4;
        private const uint TRUNCATE_EXISTING = 5;

        private const uint FILE_BEGIN = 0;
        private const uint FILE_CURRENT = 1;
        private const uint FILE_END = 2;

        private IntPtr _fhdl = IntPtr.Zero;

        public enum FileMode { READ, WRITE }
        private FileMode _mode;

        public enum FileIoFlags : uint
        {
            BACKUP_SEMANTICS = 0x02000000,
            DELETE_ON_CLOSE = 0x04000000,
            NO_BUFFERING = 0x20000000,
            OPEN_NO_RECALL = 0x00100000,
            OPEN_REPARSE_POINT = 0x00200000,
            OVERLAPPED = 0x40000000,
            POSIX_SEMANTICS = 0x0100000,
            RANDOM_ACCESS = 0x10000000,
            SESSION_AWARE = 0x00800000,
            SEQUENTIAL_SCAN = 0x08000000,
            WRITE_THROUGH = 0x80000000,
        }

        #region dll-imports
        // Define the Windows system functions that are called by this class via COM Interop:
        [DllImport("kernel32", SetLastError = true)]
        private static extern unsafe IntPtr CreateFile
        (
             string FileName,           // file name
             uint DesiredAccess,        // access mode
             uint ShareMode,            // share mode
             UIntPtr SecurityAttributes,   // Security Attributes
             uint CreationDisposition,  // how to create
             uint FlagsAndAttributes,   // file attributes
             IntPtr hTemplateFile          // handle to template file
        );

        [DllImport("kernel32", SetLastError = true)]
        private static extern unsafe bool ReadFile
        (
             IntPtr hFile,              // handle to file
             void* pBuffer,             // data buffer
             int NumberOfBytesToRead,   // number of bytes to read
             int* pNumberOfBytesRead,   // number of bytes read
             int Overlapped             // overlapped buffer which is used for async I/O.  Not used here.
        );

        [DllImport("kernel32", SetLastError = true)]
        private static extern unsafe bool WriteFile
        (
            IntPtr handle,              // handle to file
            void* pBuffer,              // data buffer
            int NumberOfBytesToWrite,   // Number of bytes to write.
            int* pNumberOfBytesWritten, // Number of bytes that were written..
            int Overlapped              // Overlapped buffer which is used for async I/O.  Not used here.
        );

        [DllImport("kernel32", SetLastError = true)]
        private static extern unsafe bool CloseHandle
        (
             IntPtr hObject             // handle to object
        );
        #endregion

        /// <summary>
        /// construct an IO File, opening it for read or write access
        /// </summary>
        /// <param name="fname">name of file</param>
        /// <param name="mode">read or write mode</param>
        /// <param name="flags">additional flags if needed</param>
        public FileIo(string fname, FileMode mode, FileIoFlags flags = 0)
        {
            if (mode == FileMode.READ)
                _fhdl = CreateFile(fname, GENERIC_READ, FILE_SHARE_READ, UIntPtr.Zero, OPEN_EXISTING, (uint)flags, IntPtr.Zero);
            else if (mode == FileMode.WRITE)
                _fhdl = CreateFile(fname, GENERIC_WRITE, 0, UIntPtr.Zero, OPEN_ALWAYS | TRUNCATE_EXISTING, (uint)flags, IntPtr.Zero);

            _mode = mode;
            
            if (_fhdl == IntPtr.Zero)
                throw new ApplicationException(string.Format("unable to open {0}: {1}", fname, new Win32Exception().Message));
        }

        /// <summary>
        /// read some bytes into provided buffer
        /// </summary>
        /// <param name="buf">byte buffer to read to</param>
        /// <param name="count">default=0, read up to sizeof(buf)</param>
        /// <returns></returns>
        public unsafe int Read(byte[] buf, int count=0)
        {
            // This function reads in a file up to BytesToRead using the Windows API function ReadFile.
            // The return value is the number of bytes read.
            int n = 0;
            fixed (byte* p = buf) {
                if (!ReadFile(_fhdl, p, (count == 0 ? buf.Count() : count), &n, 0))
                    throw new ApplicationException(string.Format("read error: {1}", new Win32Exception().Message));
            }
            return n;
        }

        /// <summary>
        /// write some bytes out to file
        /// </summary>
        /// <param name="buf">byte buffer to write from</param>
        /// <param name="count">amount of bytes from buffer</param>
        /// <returns></returns>
        public unsafe int Write(byte[] buf, int count=0)
        {
            // Writes out the file in one swoop using the Windows WriteFile function.
            int n = 0;
            fixed (byte* p = buf) {
                if (!WriteFile(_fhdl, p, (count == 0 ? buf.Count() : count), &n, 0))
                throw new ApplicationException(string.Format("write error: {1}", new Win32Exception().Message));
            }
            return n;
        }

        /// <summary>
        /// close down
        /// </summary>
        public void Close()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// dispose me
        /// </summary>
        public void Dispose()
        {
            this.Close();
        }

        /// <summary>
        /// dispose me real
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (_fhdl != IntPtr.Zero && disposing) {
                // abort the threads if still running
                //If read and write are using the same handle, require both to be closed at once
                if (!CloseHandle(_fhdl))
                    throw new Exception("file couldn't be closed!");

                _fhdl = IntPtr.Zero;
            }
        }
    }
}
