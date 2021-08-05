using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Meadow;
using Meadow.Devices;
using SQLite;

namespace MeadowApp
{
    public unsafe class MeadowApp : App<F7Micro, MeadowApp>
    {
        private int InsertCount = 10;

        public MeadowApp()
        {
            SensorValue = 42.42;
        }

        public double SensorValue { get; set; }

        public void SQLiteNetTest()
        {
            var databasePath = Path.Combine(MeadowOS.FileSystem.DataDirectory, "sensor_data.db");
            var db = new SQLite.SQLiteConnection(databasePath);
            db.CreateTable<SensorInfo>();

            for (int i = 0; i < InsertCount; i++)
            {
                Console.WriteLine($"Inserting row {i + 1}...");
                db.Insert(new SensorInfo { Timestamp = DateTime.Now, Value = SensorValue });
                Thread.Sleep(1000);
                SensorValue += 1.23;
            }

            Console.WriteLine("Reading back the data...");
            var rows = db.Table<SensorInfo>();
            foreach (var r in rows)
            {
                Console.WriteLine($"Reading was {r.Value} at {r.Timestamp.ToString("HH:mm:ss")}");
            }
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
