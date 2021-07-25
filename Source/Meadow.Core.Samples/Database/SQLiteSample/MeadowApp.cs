using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Leds;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace MeadowApp
{
    public class SensorInfo
    {
        public DateTime Timestamp { get; set; }
        public double Value { get; set; }
    }

    public class MeadowContext : DbContext
    {
        public DbSet<SensorInfo> Readings { get; set; }

        public string DbPath { get; private set; }

        public MeadowContext()
        {
            DbPath = Path.Combine(MeadowOS.FileSystem.DataDirectory, "readings.db");
        }

        
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        { 
            options.UseSqlite($"Data Source={DbPath}");

            Console.WriteLine($"Context Configured.");
        }
    }

    public unsafe class MeadowApp : App<F7Micro, MeadowApp>
    {
        public MeadowApp()
        {
            SensorValue = 42.42;
        }

        public double SensorValue { get; set; }

        public void StoreData()
        {
            using (var db = new MeadowContext())
            {
                Console.WriteLine($"Database path: {db.DbPath}.");

                Console.WriteLine($"Ensuring DB exists...");
                db.Database.EnsureCreated();

                Console.WriteLine("Inserting a row...");
                db.Add(new SensorInfo { Timestamp = DateTime.Now, Value = SensorValue });
                db.SaveChanges();

                Console.WriteLine("Reading back the row...");
                var r = db.Readings.FirstOrDefault();
                Console.WriteLine($"Reading was {r.Value} at {r.Timestamp.ToShortTimeString()}");

            }

            SensorValue += 1;
        }

        public void PInvokeTest()
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

        public void ListFiles(string dir)
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
