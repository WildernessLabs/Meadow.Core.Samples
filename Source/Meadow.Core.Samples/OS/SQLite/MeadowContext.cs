using System;
using System.IO;
using Meadow;
using Microsoft.EntityFrameworkCore;

namespace MeadowApp
{
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
}
