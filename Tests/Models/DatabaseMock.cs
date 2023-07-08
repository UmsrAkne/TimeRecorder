using System;
using System.Collections.Generic;
using System.Linq;
using TimeRecorder.Models;

namespace Tests.Models;

public class DatabaseMock : IDataSource
{
    public List<TimeStamp> TimeStamps { get; set; }

    public int Count => TimeStamps.Count;

    public void Add(TimeStamp timeStamp)
    {
        throw new NotImplementedException();
    }

    public void Add(TimeStampGroup group)
    {
        throw new NotImplementedException();
    }

    public void AddNewGroup(TimeStamp ts)
    {
        throw new NotImplementedException();
    }

    public List<TimeStamp> GetTimeStamps(TimeStampGroup targetGroup)
    {
        throw new NotImplementedException();
    }

    public TimeStamp GetTimeStamp(int id)
    {
        return TimeStamps.FirstOrDefault(t => t.Id == id);
    }

    public List<TimeStampGroup> GetGroups()
    {
        throw new NotImplementedException();
    }

    public TimeStampGroup GetLatestGroup()
    {
        throw new NotImplementedException();
    }
}