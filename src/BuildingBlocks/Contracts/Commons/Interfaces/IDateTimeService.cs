namespace Contracts.Commons.Interfaces;

public interface IDateTimeService
{
    DateTime Now();

    DateTime UtcNow();

    DateTime Today();

    DateTime UtcToday();
}