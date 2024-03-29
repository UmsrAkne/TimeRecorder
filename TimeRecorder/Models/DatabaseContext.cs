﻿using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using TimeRecorder.Models.Converters;

namespace TimeRecorder.Models;

public class DatabaseContext : DbContext, IDataSource
{
    private readonly string dbFileName = "records.sqlite";

    public int Count => TimeStamps.Count();

    // ReSharper disable once UnusedAutoPropertyAccessor.Local
    private DbSet<TimeStampGroup> TimeStampGroups { get; set; }

    // ReSharper disable once UnusedAutoPropertyAccessor.Local
    private DbSet<TimeStamp> TimeStamps { get; set; }

    public void Add(TimeStamp timeStamp)
    {
        if (timeStamp.BaseId != 0)
        {
            var baseTimeStamp = TimeStamps.FirstOrDefault(t => t.Id == timeStamp.BaseId);
            if (baseTimeStamp != null)
            {
                baseTimeStamp.IsLatest = false;
            }
        }

        TimeStamps.Add(timeStamp);
        SaveChanges();
    }

    /// <summary>
    ///     新しいグループを追加し、そのグループの id をパラメーターの GroupId に割り当てます
    /// </summary>
    /// <param name="ts">新しいグループに追加するタイムスタンプ</param>
    public void AddNewGroup(TimeStamp ts)
    {
        var group = new TimeStampGroup
            { DateTime = DateTime.Now };

        Add(group);

        ts.GroupId = group.Id;
        Add(ts);

        SaveChanges();
    }

    public List<TimeStamp> GetTimeStamps(TimeStampGroup targetGroup)
    {
        return TimeStamps.Where(t => t.GroupId == targetGroup.Id && t.IsLatest)
            .OrderBy(t => t.DateTime)
            .ToList();
    }

    public List<TimeStampGroup> GetGroups()
    {
        return TimeStampGroups.OrderBy(tsg => tsg.DateTime).ToList();
    }

    public TimeStampGroup GetLatestGroup()
    {
        return TimeStampGroups.OrderByDescending(t => t.DateTime).FirstOrDefault();
    }

    public void Add(TimeStampGroup group)
    {
        TimeStampGroups.Add(group);
        SaveChanges();
    }

    public TimeStamp GetTimeStamp(int id)
    {
        return TimeStamps.FirstOrDefault(t => t.Id == id);
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