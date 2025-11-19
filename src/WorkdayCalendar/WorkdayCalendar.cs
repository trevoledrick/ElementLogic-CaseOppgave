namespace WorkdayCalendar;

public class WorkdayCalendar : IWorkdayCalculator
{
    private readonly IWorkdaySettings _workdaySettings;
    private readonly IHolidayCalendar _holidayCalendar;

    public WorkdayCalendar(IWorkdaySettings workdaySettings, IHolidayCalendar holidayCalendar)
    {
        _workdaySettings = workdaySettings;
        _holidayCalendar = holidayCalendar;
    }

    public DateTime AddWorkdays(DateTime start, double workdays)
    {
        throw new NotImplementedException();
    }

    private bool IsWeekend(DateTime date)
    {
        return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
    }

    private bool IsWorkday(DateTime date)
    {
        return !IsWeekend(date) && !_holidayCalendar.IsHoliday(date);
    }
    
    private DateTime MoveToNextWorkday(DateTime date)
    {
        var result = date;
        while (!IsWorkday(result))
        {
            result = result.AddDays(1);
        }

        return result;
    }

    private DateTime MoveToPreviousWorkday(DateTime date)
    {
        var result = date;
        while (!IsWorkday(result))
        {
            result = result.AddDays(-1);
        }

        return result;
    }

    private DateTime NormalizeForward(DateTime dateTime)
    {
        var date = dateTime.Date;
        var time = dateTime.TimeOfDay;

        if (!IsWorkday(date))
        {
            var nextWorkDay = MoveToNextWorkday(date);
            return nextWorkDay.Date + _workdaySettings.WorkdayStart;
        }

        if (time < _workdaySettings.WorkdayStart)
        {
            return date + _workdaySettings.WorkdayStart;
        }

        if (time > _workdaySettings.WorkdayEnd)
        {
            var nextWorkDay = MoveToNextWorkday(date.AddDays(1));
            return nextWorkDay.Date + _workdaySettings.WorkdayStart;
        }

        return dateTime;
    }

    private DateTime NormalizeBackward(DateTime dateTime)
    {
        var date = dateTime.Date;
        var time = dateTime.TimeOfDay;

        if (!IsWorkday(date))
        {
            var previousWorkDay = MoveToPreviousWorkday(date);
            return previousWorkDay.Date + _workdaySettings.WorkdayEnd;
        }

        if (time > _workdaySettings.WorkdayEnd)
        {
            return date + _workdaySettings.WorkdayEnd;
        }

        if (time < _workdaySettings.WorkdayStart)
        {
            var previousWorkDay = MoveToPreviousWorkday(date.AddDays(-1));
            return previousWorkDay.Date + _workdaySettings.WorkdayEnd;
        }

        return dateTime;
    }
}