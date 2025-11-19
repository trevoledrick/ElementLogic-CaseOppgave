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
        if (workdays == 0)
        {
            return NormalizeForward(start);
        }

        int direction = workdays > 0 ? 1 : -1;
        double absoluteWorkDays = Math.Abs(workdays);

        int wholeDays = (int)Math.Abs(workdays);
        double fractionalPart = absoluteWorkDays - wholeDays;

        DateTime current = direction > 0 ? NormalizeForward(start) : NormalizeBackward(start);

        for (int i = 0; i < wholeDays; i++)
        {
            if (direction > 0)
            {
                current = MoveToNextWorkday(current.AddDays(1));
            }
            else
            {
                current = MoveToPreviousWorkday(current.AddDays(-1));
            }
        }

        if (fractionalPart == 0)
        {
            return current;
        }
        
        // Fraksjonsdelen (0.5, 0.25 osv.) implementerer vi i morgen den 20.11.2025
        throw new NotImplementedException("Fractional workday handling not implemented yet."); 
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