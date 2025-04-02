namespace Invensys.ExternalApi.Sage300;

public class RateLimitingHandler : DelegatingHandler
{
    private readonly SemaphoreSlim _semaphore;
    private readonly int _delayBetweenRequestsMilliseconds;
    private int _requestCount;
    private readonly Timer _timer;

    public RateLimitingHandler(int maxConcurrentRequests, int delayBetweenRequestsMilliseconds)
    {
        _semaphore = new SemaphoreSlim(maxConcurrentRequests);
        _delayBetweenRequestsMilliseconds = delayBetweenRequestsMilliseconds;
        _requestCount = 0;

        // Timer to log the request count every minute
        _timer = new Timer(LogRequestCount, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        await _semaphore.WaitAsync(cancellationToken);

        try
        {
            Interlocked.Increment(ref _requestCount); // Increment request count
            var startTime = DateTime.UtcNow;
            var response = await base.SendAsync(request, cancellationToken);

            var elapsedTime = (int)(DateTime.UtcNow - startTime).TotalMilliseconds;

            // Calculate remaining delay time
            var remainingDelay = _delayBetweenRequestsMilliseconds - elapsedTime;
            if (remainingDelay > 0)
                await Task.Delay(remainingDelay, cancellationToken);

            return response;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    private void LogRequestCount(object? state)
    {
        Console.WriteLine($"Requests made in the last minute: {_requestCount}");
        Interlocked.Exchange(ref _requestCount, 0); // Reset count for the next minute
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        _timer?.Dispose();
        _semaphore?.Dispose();
    }
}

