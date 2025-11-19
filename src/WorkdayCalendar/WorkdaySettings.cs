namespace WorkdayCalendar;

public class WorkdaySettings : IWorkdaySettings
{
    public TimeSpan WorkdayStart { get; }
    public TimeSpan WorkdayEnd { get; }
    public TimeSpan WorkdayLength => WorkdayEnd - WorkdayStart;

    public WorkdaySettings(TimeSpan workdayStart, TimeSpan workdayEnd)
    {
        if (workdayEnd <= workdayStart)
        {
            throw new ArgumentException("Workday end time must be after start time.");
        }

        WorkdayStart = workdayStart;
        WorkdayEnd = workdayEnd;
    }
}