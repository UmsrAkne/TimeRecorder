namespace TimeRecorder.Models;

public class TimeStampGetter
{
    public TimeStampGetter(IDataSource dataSource)
    {
        DataSource = dataSource;
    }

    public int CurrentId { get; set; }

    private IDataSource DataSource { get; }

    public TimeStamp GetPrevTimeStamp()
    {
        if (CurrentId <= 1)
        {
            return new TimeStamp();
        }

        CurrentId--;
        return DataSource.GetTimeStamp(CurrentId);
    }

    public TimeStamp GetNextTimeStamp()
    {
        if (CurrentId >= DataSource.Count)
        {
            return new TimeStamp();
        }

        CurrentId++;
        return DataSource.GetTimeStamp(CurrentId);
    }
}