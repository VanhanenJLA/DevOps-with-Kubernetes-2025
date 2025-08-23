namespace LogOutput;

public class PrintJob : BackgroundService
{
    private readonly ILogger<PrintJob> _logger;
    private readonly Guid _random = Guid.NewGuid();

    public PrintJob(ILogger<PrintJob> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var timer = new PeriodicTimer(TimeSpan.FromSeconds(5));
        try
        {
            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                _logger.LogInformation("{}", _random);
            }
        }
        catch (OperationCanceledException)
        {
            /* shutting down */
        }
        finally
        {
            timer.Dispose();
        }
    }
}