using URLShortener.Repository;

namespace URLShortener.Service
{
    // This class implements an IHostedService, which means it’s a background service that runs automatically when the application starts up and stops when the host shuts down.
    public class CounterInitializerService(
        IServiceScopeFactory serviceScopeFactory,
        IGlobalCounterService globalCounter)
        : IHostedService
    {
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            // It lives only for a short time — for one “scope” — and then it’s destroyed (disposed).
            using var scope = serviceScopeFactory.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<IUrlRepository>();
                
            var latestId = await repo.GetLatestIdAsync();
            globalCounter.SetCounter(latestId);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
