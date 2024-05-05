namespace Infrastructure.Extensions;

public class DateTimeService : IDateTimeService
{
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    public DateTime Now()
    {
        try
        {
            _semaphore.Wait();
            return DateTime.Now;
        }
        finally
        {
            _ = _semaphore.Release();
        }
    }

    public DateTime UtcNow()
    {
        try
        {
            _semaphore.Wait();
            return DateTime.UtcNow;
        }
        finally
        {
            _ = _semaphore.Release();
        }
    }

    public DateTime Today()
    {
        try
        {
            _semaphore.Wait();
            return DateTime.Today;
        }
        finally
        {
            _ = _semaphore.Release();
        }
    }

    public DateTime UtcToday()
    {
        try
        {
            _semaphore.Wait();
            return DateTime.UtcNow.Date;
        }
        finally
        {
            _ = _semaphore.Release();
        }
    }
}