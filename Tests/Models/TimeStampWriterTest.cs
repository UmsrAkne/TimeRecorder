using System;
using System.Collections.Generic;
using NUnit.Framework;
using TimeRecorder.Models;

namespace Tests.Models;

[TestFixture]
public class TimeStampWriterTest
{
    [Test]
    public void テキスト出力テスト一行()
    {
        var writer = new TimeStampWriter();

        List<TimeStamp> timeStamps = new List<TimeStamp>()
        {
            new (){ DateTime = new DateTime(2022, 11, 28, 1, 1, 1), Comment = "TimeStamp1" }
        };

        Assert.AreEqual("2022/11/28 01:01:01 TimeStamp1", writer.GetTimeStampString(timeStamps));
    }

    [Test]
    public void テキスト出力テスト複数行()
    {
        var writer = new TimeStampWriter();
        writer.AttachTotalTime = false;

        List<TimeStamp> timeStamps = new List<TimeStamp>()
        {
            new (){ DateTime = new DateTime(2022, 11, 28, 1, 1, 1), Comment = "TimeStamp1" },
            new (){ DateTime = new DateTime(2022, 11, 28, 1, 1, 2), Comment = "TimeStamp2" },
            new (){ DateTime = new DateTime(2022, 11, 28, 1, 1, 3), Comment = "TimeStamp3" },
        };

        var expect = "2022/11/28 01:01:01 TimeStamp1\r\n"
                     + "2022/11/28 01:01:02 TimeStamp2\r\n"
                     + "2022/11/28 01:01:03 TimeStamp3";

        Assert.AreEqual(expect, writer.GetTimeStampString(timeStamps));
    }

    [Test]
    public void テキスト出力テスト複数行合計時間付き()
    {
        var writer = new TimeStampWriter();
        writer.AttachTotalTime = true;

        List<TimeStamp> timeStamps = new List<TimeStamp>()
        {
            new (){ DateTime = new DateTime(2022, 11, 28, 1, 1, 1), Comment = "TimeStamp1" },
            new (){ DateTime = new DateTime(2022, 11, 28, 1, 1, 2), Comment = "TimeStamp2" },
            new (){ DateTime = new DateTime(2022, 11, 28, 1, 1, 3), Comment = "TimeStamp3" },
        };

        var expect = "2022/11/28 01:01:01 TimeStamp1\r\n"
                     + "2022/11/28 01:01:02 TimeStamp2\r\n"
                     + "2022/11/28 01:01:03 TimeStamp3\r\n"
                     + "Total : 00:00:02";

        Assert.AreEqual(expect, writer.GetTimeStampString(timeStamps));
    }

    [Test]
    public void テキスト出力テストNull()
    {
        var writer = new TimeStampWriter();
        Assert.AreEqual(string.Empty, writer.GetTimeStampString(null));
    }

    [Test]
    public void テキスト出力テスト空リスト()
    {
        var writer = new TimeStampWriter();
        Assert.AreEqual(string.Empty, writer.GetTimeStampString(new List<TimeStamp>()));
    }
}