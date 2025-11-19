namespace WorkdayCalendar;

public interface IWorkdayCalculator
{
    DateTime AddWorkdays(DateTime start, double workdays);
}
