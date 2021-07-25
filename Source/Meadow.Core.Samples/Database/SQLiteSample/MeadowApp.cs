using System;
using System.IO;
using System.Text;
using System.Threading;
using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Leds;

namespace MeadowApp
{
    public unsafe class MeadowApp : App<F7Micro, MeadowApp>
    {
        public MeadowApp()
        {
        }

        public void Run()
        { 
            ListFiles(MeadowOS.FileSystem.DataDirectory);

            try
            {
                var file = Encoding.ASCII.GetBytes(Path.Combine(MeadowOS.FileSystem.DataDirectory, "test2.db"));

                fixed (byte* pName = file)
                {
                    Console.WriteLine($"Opening DB at {file}...");
                    var result = NativeMethods.sqlite3_open_v2(pName, out IntPtr pDB, NativeMethods.SQLITE_OPEN_READWRITE | NativeMethods.SQLITE_OPEN_CREATE, (byte*)null);
                    Console.WriteLine($"result={result}  DB={pDB}");
                    if(result != 0)
                    {
                        var pMsg = NativeMethods.sqlite3_errmsg(pDB);
                        Console.WriteLine($"pMsg={(int)pMsg}");
                        var p = pMsg;
                        var count = 0;
                        while(*p != 0)
                        {
                            count++;
                            p++;
                        }
                        var message = Encoding.ASCII.GetString(pMsg, count);

                        Console.WriteLine($"errmsg={message}");
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"EXCEPTION: {ex.Message}");
            }

            ListFiles(MeadowOS.FileSystem.DataDirectory);
        }

        private void ListFiles(string dir)
        {
            Console.WriteLine($"Files in {dir}:");

            var files = Directory.GetFiles(dir);
            if (files == null || files.Length == 0)
            {
                Console.WriteLine("  [ EMPTY DIRECTORY ]");
            }
            else
            {
                foreach (var f in Directory.GetFiles(dir))
                {
                    Console.WriteLine($"  {f}");
                }
            }
        }
    }
}
