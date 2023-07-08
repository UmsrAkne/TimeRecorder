using System.Collections.Generic;

namespace TimeRecorder.Models;

public interface IDataSource
{
    int Count { get; }

    void Add(TimeStamp timeStamp);

    void Add(TimeStampGroup group);

    /// <summary>
    ///     新しいグループを追加し、そのグループの id をパラメーターの GroupId に割り当てます
    /// </summary>
    /// <param name="ts">新しいグループに追加するタイムスタンプ</param>
    void AddNewGroup(TimeStamp ts);

    List<TimeStamp> GetTimeStamps(TimeStampGroup targetGroup);

    TimeStamp GetTimeStamp(int id);

    List<TimeStampGroup> GetGroups();

    TimeStampGroup GetLatestGroup();
}