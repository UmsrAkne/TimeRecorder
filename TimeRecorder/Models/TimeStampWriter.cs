using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TimeRecorder.Models;

public class TimeStampWriter
{
    private readonly string dateTimeFormatString = "yyyy/MM/dd HH:mm:ss";

    public bool AttachTotalTime { get; set; } = true;

    public string GetTimeStampString(List<TimeStamp> timeStamps)
    {
        if (timeStamps == null || timeStamps.Count == 0)
        {
            return string.Empty;
        }

        var builder = new StringBuilder();

        timeStamps.ForEach(ts =>
        {
            builder.AppendLine($"{ts.DateTime.ToString(dateTimeFormatString)} {ts.Comment}");
        });

        if (AttachTotalTime && timeStamps.Count >= 2)
        {
            builder.AppendLine($"Total : {GetTotalTime(timeStamps)}");
        }

        return builder.ToString().TrimEnd('\r', '\n');
    }

    public TimeSpan GetTotalTime(List<TimeStamp> timeStamps)
    {
        if (timeStamps.Count < 2)
        {
            return TimeSpan.Zero;
        }

        // 最初と最後の DateTime の差を出すので要素数 2 以上は必須
        var sortedList = timeStamps.OrderBy(t => t.DateTime).ToList();
        return sortedList.LastOrDefault()!.DateTime - sortedList.FirstOrDefault()!.DateTime;
    }
}