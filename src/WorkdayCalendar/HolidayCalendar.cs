namespace WorkdayCalendar;

public class HolidayCalendar : IHolidayCalendar
{
    private readonly HashSet<DateTime> _holidays = new();
    private readonly HashSet<(int Month, int Day)> _recurringHolidays = new();

	public void AddHoliday(DateTime date)
	{
		_holidays.Add(date.Date);
	}

	public void AddRecurringHoliday(int month, int day)
	{
		_recurringHolidays.Add((month, day));
	}

	public bool IsHoliday(DateTime date)
	{
		if (_holidays.Contains(date.Date))
        {
            return true;
        }

        var recurringKey = (date.Month, date.Day);
        
        return _recurringHolidays.Contains(recurringKey);
	}
}