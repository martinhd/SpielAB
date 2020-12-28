using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BlazorSignalRApp.Shared;

namespace BlazorSignalRApp.Server.Controllers
{
    [ApiController]
    public class TimerController : ControllerBase
    {
        private readonly ILogger<TimerController> _logger;
        private readonly TimedHostedService _timer;

        public TimerController(ILogger<TimerController> logger,TimedHostedService timer)
        {
            _logger = logger;
            _timer = timer;
        }

        [HttpGet]
        [Route("Timer")]
        public int Get()
        {
            return 42;
        }

        [HttpGet]
        [Route("Timer/Reset")]
        public void Reset()
        {
            _timer.Reset();
        }
    }
}
