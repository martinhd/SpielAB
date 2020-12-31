using System;
using System.Threading;
using System.Threading.Tasks;
using BlazorSignalRApp.Server.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BlazorSignalRApp.Server
{
    public class TimedHostedService : BackgroundService
    {
        private int executionCount = 0;
        private readonly ILogger<TimedHostedService> _logger;
        private readonly IHubContext<TimeHub> _timeHubContext;
        private Timer _timer;

        public TimedHostedService(ILogger<TimedHostedService> logger, IHubContext<TimeHub> thc)
        {
            _logger = logger;
            _timeHubContext = thc;
        }

        public override async Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service running.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(1.0));

            await base.StartAsync(stoppingToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await _timeHubContext.Clients.All.SendAsync("ReceiveMessage", executionCount);
                await Task.Delay(1000, stoppingToken);
            }
        }

        private void DoWork(object state)
        {
            var count = Interlocked.Increment(ref executionCount);

            _logger.LogInformation(
                "Timed Hosted Service is working. Count: {Count}", count);
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            await base.StopAsync(stoppingToken);
        }

        public void Reset()
        {
            _logger.LogInformation("Timed Hosted Service Reset to 0.");
            executionCount = 0;
        }

        public int GetTimer()
        {
            return executionCount;
        }
    }
}