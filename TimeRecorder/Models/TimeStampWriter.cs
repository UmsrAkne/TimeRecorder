using System.Collections.Generic;
using System.Text;

namespace TimeRecorder.Models;

public class TimeStampWriter
{
    private readonly string dateTimeFormatString = "yyyy/MM/dd HH:mm:ss";

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

        return builder.ToString().TrimEnd('\r', '\n');
    }
}