namespace Utils.Timer;

public interface ITimer
{
    event Action<DateTime> OnTime;
    Task StartAsync();

    Task StopAsync();
}
