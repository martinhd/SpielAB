using System;
using System.Threading;
using BlazorSignalRApp.Server.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace BlazorSignalRApp.Server
{
    public interface ITimerService
    {
        public void Reset();
        public int GetTimer();
    }


    public class Timerservice : ITimerService
    {
        private int executionCount = 0;
        private readonly ILogger<Timerservice> _logger;
        private readonly IHubContext<TimeHub> _timeHubContext;
        private Timer _timer;

        public Timerservice(ILogger<Timerservice> logger, IHubContext<TimeHub> thc)
        {
            _logger = logger;
            _timeHubContext = thc;

            _logger.LogInformation("Timed Hosted Service running.");
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(1.0));
        }

        private void DoWork(object state)
        {
            var count = Interlocked.Increment(ref executionCount);
            _timeHubContext.Clients.All.SendAsync("ReceiveMessage", executionCount);
            //_logger.LogInformation(
            //    "Timed Hosted Service is working. Count: {Count}", count);
        }

        ~Timerservice()
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");
            _timer?.Change(Timeout.Infinite, 0);
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