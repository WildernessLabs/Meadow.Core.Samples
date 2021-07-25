using System;
using System.Runtime.InteropServices;

namespace MeadowApp
{
    public static class NativeMethods
    {
        public const int SQLITE_OPEN_READONLY = 0x00000001;  /* Ok for sqlite3_open_v2() */
        public const int SQLITE_OPEN_READWRITE = 0x00000002;  /* Ok for sqlite3_open_v2() */
        public const int SQLITE_OPEN_CREATE = 0x00000004;  /* Ok for sqlite3_open_v2() */

        private const string LIB_NAME = "sqlite3";

        [DllImport(LIB_NAME)]
        public static extern unsafe int sqlite3_open(byte* filename, out IntPtr db);

        [DllImport(LIB_NAME)]
        public static extern unsafe int sqlite3_open_v2(byte* filename, out IntPtr db, int flags, byte* vfs);

        [DllImport(LIB_NAME)]
        public static extern unsafe byte* sqlite3_errmsg(IntPtr db);

        [DllImport(LIB_NAME)]
        public static extern unsafe IntPtr sqlite3_vfs_find(byte* name);

        [DllImport(LIB_NAME)]
        public static extern unsafe IntPtr sqlite3_demovfs();

        [DllImport(LIB_NAME)]
        public static extern int sqlite3_vfs_register();

        [DllImport(LIB_NAME)]
        public static extern int sqlite3_vfs_register(IntPtr pVfs, int makeDflt);

    }
}
