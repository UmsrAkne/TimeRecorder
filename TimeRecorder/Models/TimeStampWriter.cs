using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace TimeRecorder.Models;

public class TimeStampWriter
{
    private readonly string dateTimeFormatString = "yyyy/MM/dd HH:mm:ss";

    public string GetTimeStampString([NotNull] List<TimeStamp> timeStamps)
    {
        return string.Empty;
    }
}