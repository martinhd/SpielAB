using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BlazorSignalRApp.Shared;
using Microsoft.Extensions.Hosting;

namespace BlazorSignalRApp.Server.Controllers
{
    [ApiController]
    public class TimerController : ControllerBase
    {
        private readonly ILogger<TimerController> _logger;
        private readonly ITimerService _timer;

        public TimerController(ILogger<TimerController> logger, ITimerService timer)
        {
            _logger = logger;
            _timer  = timer;
        }

        [HttpGet]
        [Route("Api/Timer")]
        public int Get()
        {
            return _timer.GetTimer();
        }

        [HttpGet]
        [Route("Api/Timer/Reset")]
        public bool Reset()
        {
            _timer.Reset();
            return true;
        }
    }
}
