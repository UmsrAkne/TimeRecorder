using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace TimeRecorder.Models
{
    public class DatabaseContext : DbContext
    {
        private readonly string dbFileName = "records.sqlite";

        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        private DbSet<TimeStampGroup> TimeStampGroups { get; set; }

        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        private DbSet<TimeStamp> TimeStamps { get; set; }

        public void Add(TimeStampGroup group)
        {
            TimeStampGroups.Add(group);
            SaveChanges();
        }

        public void Add(TimeStamp timeStamp)
        {
            TimeStamps.Add(timeStamp);
            SaveChanges();
        }

        public List<TimeStamp> GetTimeStamps(TimeStampGroup targetGroup)
        {
            return TimeStamps.Where(t => t.GroupId == targetGroup.Id)
                .OrderBy(t => t.DateTime)
                .ToList();
        }

        public TimeStampGroup GetLatestGroup()
        {
            return TimeStampGroups.OrderByDescending(t => t.DateTime).FirstOrDefault();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!File.Exists(dbFileName))
            {
                SQLiteConnection.CreateFile(dbFileName); // ファイルが存在している場合は問答無用で上書き。
            }

            var connectionString = new SqliteConnectionStringBuilder { DataSource = dbFileName }.ToString();
            optionsBuilder.UseSqlite(new SQLiteConnection(connectionString));
        }
    }
}