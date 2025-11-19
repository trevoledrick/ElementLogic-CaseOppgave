namespace WorkdayCalendar;

public interface IWorkdaySettings
{
    TimeSpan WorkdayStart { get; }
    TimeSpan WorkdayEnd { get; }
    TimeSpan WorkdayLength { get; }
}
