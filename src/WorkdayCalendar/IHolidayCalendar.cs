namespace WorkdayCalendar;

public interface IHolidayCalendar
{
    void AddHoliday(DateTime date);
    void AddRecurringHoliday(int month, int day);
    bool IsHoliday(DateTime date);
}
