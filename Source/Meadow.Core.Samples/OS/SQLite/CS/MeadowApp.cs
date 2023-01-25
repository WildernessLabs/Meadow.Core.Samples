using Meadow;
using Meadow.Devices;
using SQLite;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SQLite_Sample
{
    public class MeadowApp : App<F7FeatherV2>
    {
        int InsertCount = 10;

        SQLiteConnection Database { get; set; }
        
        public double SensorValue { get; set; }

        public override Task Run()
        {
            // set an initial dummy sensor value
            SensorValue = 42.42;

            try
            {
                // configure the database and open a connection
                ConfigureDatabase();
                // add some dummy sensor readings
                InsertDummyData();
                // read out the data
                RetreiveData();

                // update a record
                UpdateData();

                RetreiveData();

                // retreive by primary key
                RetrieveByPrimaryKey();

                RetrieveViaLinqQuery();

                RetrieveViaTSqlQuery();

                DeleteARow();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Problem: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
            }

            return Task.CompletedTask;
        }

        void ConfigureDatabase()
        {
            // by default, SQLite runs in `Serialized` mode, which is thread safe.
            // if you need to change the threading mode, you can do it with the
            // following API
            //SQLite3.Config(SQLite3.ConfigOption.SingleThread);

            // database files should go in the `DataDirectory`
            var databasePath = Path.Combine(MeadowOS.FileSystem.DataDirectory, "MySqliteDatabase.db");
            // make the connection
            Database = new SQLiteConnection(databasePath);
            // add table(s)
            Database.CreateTable<SensorModel>();
        }

        void InsertDummyData()
        {
            for (int i = 0; i < InsertCount; i++)
            {
                Console.WriteLine($"Inserting row {i + 1}...");
                Database.Insert(new SensorModel { Timestamp = DateTime.Now, Value = SensorValue });
                Thread.Sleep(100);
                SensorValue += 1.23;
            }
        }

        void RetreiveData()
        {
            Console.WriteLine("Reading back the data...");
            var rows = Database.Table<SensorModel>();
            foreach (var r in rows)
            {
                Console.WriteLine($"Reading was {r.Value} at {r.Timestamp.ToString("HH:mm:ss")}");
            }
        }

        void UpdateData()
        {
            // pull the first record out of the table
            SensorModel reading = Database.Table<SensorModel>().Take(1).First();

            Console.WriteLine($"Found a record, ID: {reading.ID}");

            // change the value
            reading.Value = reading.Value * 2;

            // update the data
            Database.Update(reading);
        }

        void RetrieveByPrimaryKey()
        {
            SensorModel firstRow = Database.Table<SensorModel>().Take(1).First();
            var sensorReading1 = Database.Get<SensorModel>(firstRow.ID);
            Console.WriteLine($"Sensor Reading 1: {sensorReading1.Value}");
        }

        void RetrieveViaSearchPredicate()
        {
            var firstSensorReadingOver50 = Database.Get<SensorModel>(reading => reading.Value > 50);
            Console.WriteLine($"found a sensor reading over 50; ID: {firstSensorReadingOver50.ID}, value: {firstSensorReadingOver50.Value}");
        }

        void RetrieveViaLinqQuery()
        {
            Console.WriteLine("RetrieveViaLinqQuery()");
            var readings = from rows in Database.Table<SensorModel>()
                           where rows.Value > 50
                           select rows;
            Console.WriteLine($"Found {readings.Count()} readings over 50: ");
            foreach (var reading in readings)
            {
                Console.WriteLine($"ID: {reading.ID}, value: {reading.Value}");
            }
        }

        void RetrieveViaTSqlQuery()
        {
            Console.WriteLine("RetrieveViaTSqlQuery()");
            var readings = Database.Query<SensorModel>("SELECT * FROM SensorReadings WHERE value > ?", 50);
            Console.WriteLine($"Found {readings.Count()} readings over 50: ");
            foreach (var reading in readings)
            {
                Console.WriteLine($"ID: {reading.ID}, value: {reading.Value}");
            }
        }

        void DeleteARow()
        {
            // pull the first record out of the table
            SensorModel reading = Database.Table<SensorModel>().Take(1).First();
            Console.WriteLine($"First record ID: {reading.ID}");
            Database.Delete<SensorModel>(reading.ID);
            Console.WriteLine($"Deleted the record");
            // get the first record again
            reading = Database.Table<SensorModel>().Take(1).First();
            Console.WriteLine($"new first record ID: {reading.ID}");
        }
    }
}
