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
        //==== internals
        SQLiteConnection Database { get; set; }
        int InsertCount = 10;
        public double SensorValue { get; set; }

        public MeadowApp()
        {
            // set an initial dummy sensor value
            SensorValue = 42.42;

            try {
                // configure the database and open a connection
                ConfigureDatabase();
                // add some dummy sensor readings
                InsertDummyData();
                // read out the data
                RetreiveData();

            } catch (Exception ex) {
                Console.WriteLine($"FAILURE: {ex.Message}");
                if (ex.InnerException != null) {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
            }
        }

        protected void ConfigureDatabase()
        {
            // database files should go in the `DataDirectory`
            var databasePath = Path.Combine(MeadowOS.FileSystem.DataDirectory, "sensor_data.db");
            // make the connection
            Database = new SQLite.SQLiteConnection(databasePath);
            // add table(s)
            Database.CreateTable<SensorModel>();
        }

        protected void InsertDummyData()
        {
            for (int i = 0; i < InsertCount; i++) {
                Console.WriteLine($"Inserting row {i + 1}...");
                Database.Insert(new SensorModel { Timestamp = DateTime.Now, Value = SensorValue });
                Thread.Sleep(1000);
                SensorValue += 1.23;
            }
        }

        protected void RetreiveData()
        {
            Console.WriteLine("Reading back the data...");
            var rows = Database.Table<SensorModel>();
            foreach (var r in rows) {
                Console.WriteLine($"Reading was {r.Value} at {r.Timestamp.ToString("HH:mm:ss")}");
            }
        }


        //public void ListFiles(string dir)
        //{
        //    Console.WriteLine($"Files in {dir}:");

        //    var files = Directory.GetFiles(dir);
        //    if (files == null || files.Length == 0)
        //    {
        //        Console.WriteLine("  [ EMPTY DIRECTORY ]");
        //    }
        //    else
        //    {
        //        foreach (var f in Directory.GetFiles(dir))
        //        {
        //            Console.WriteLine($"  {f}");
        //        }
        //    }
        //}
    }
}
