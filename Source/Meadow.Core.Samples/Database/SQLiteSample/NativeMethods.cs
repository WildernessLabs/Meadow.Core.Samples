using System;
using System.Runtime.InteropServices;

namespace MeadowApp
{
    public static class NativeMethods
    {
        private const string LIB_NAME = "sqlite";

        [DllImport(LIB_NAME)]
        public static extern unsafe int sqlite3_open(byte* filename, out IntPtr db);

        [DllImport(LIB_NAME)]
        public static extern unsafe byte* sqlite3_errmsg(IntPtr db);
    }
}
