using System;
using System.Collections.Generic;
using NUnit.Framework;
using TimeRecorder.Models;

namespace Tests.Models;

public class TimeStampGetterTest
{
    private List<TimeStamp> SampleStamps =>
        new List<TimeStamp>
        {
            new TimeStamp
                { Id = 1, DateTime = new DateTime(1), Comment = "aaa" },
            new TimeStamp
                { Id = 2, DateTime = new DateTime(2), Comment = "bbb" },
            new TimeStamp
                { Id = 3, DateTime = new DateTime(3), Comment = "ccc" },
            new TimeStamp
                { Id = 4, DateTime = new DateTime(4), Comment = "ddd" },
        };

    [Test]
    public void GetPrevTest()
    {
        var db = new DatabaseMock { TimeStamps = SampleStamps };
        var timeStampGetter = new TimeStampGetter(db) { CurrentId = 4 };

        Assert.AreEqual("ccc", timeStampGetter.GetPrevTimeStamp().Comment, "3番のコメントを取得");
        Assert.AreEqual("bbb", timeStampGetter.GetPrevTimeStamp().Comment, "2番のコメントを取得");
        Assert.AreEqual("aaa", timeStampGetter.GetPrevTimeStamp().Comment, "1番のコメントを取得");

        var ts = timeStampGetter.GetPrevTimeStamp();
        Assert.NotNull(ts);

        Assert.AreEqual("", ts.Comment, "過去のコメントが存在しないので空文字");
    }

    [Test]
    public void GetNextTest()
    {
        var db = new DatabaseMock { TimeStamps = SampleStamps };
        var timeStampGetter = new TimeStampGetter(db) { CurrentId = 1 };

        Assert.AreEqual("bbb", timeStampGetter.GetNextTimeStamp().Comment, "2番のコメントを取得");
        Assert.AreEqual("ccc", timeStampGetter.GetNextTimeStamp().Comment, "3番のコメントを取得");
        Assert.AreEqual("ddd", timeStampGetter.GetNextTimeStamp().Comment, "4番のコメントを取得");

        var ts = timeStampGetter.GetNextTimeStamp();
        Assert.NotNull(ts);

        Assert.AreEqual("", ts.Comment, "次のコメントが存在しないので空文字");
    }
}