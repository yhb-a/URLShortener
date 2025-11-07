
using Microsoft.Extensions.Hosting;
using URLShortener.Repository;

namespace URLShortener
{
    // This class implements an IHostedService, which means it’s a background service that runs automatically when the application starts up and stops when the host shuts down.
    public class CounterInitializerService : IHostedService 
    {
        private readonly IGlobalCounter globalCounter;
        private readonly IServiceScopeFactory scopeFactory;

        public CounterInitializerService(IServiceScopeFactory serviceScopeFactory, IGlobalCounter globalCounter)
        {
            this.scopeFactory = serviceScopeFactory;
            this.globalCounter = globalCounter;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            // It lives only for a short time — for one “scope” — and then it’s destroyed (disposed).
            using (var scope = scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<IURLRepository>();
                var latestId = await repo.GetLatestId();
                globalCounter.SetCounter(latestId);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
