using BlazorSignalRApp.Shared;
using Microsoft.AspNetCore.Mvc;

namespace BlazorSignalRApp.Server.Controllers
{
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IGameService _gameService;
        
        public GameController(IGameService gs)
        {
            _gameService = gs;
        }

        [HttpGet]
        [Route("Api/NextPlayer")]
        public string NextPlayer()
        {
            var player = _gameService.GetNextPlayer();
            return player;
        }

        [HttpGet]
        [Route("Api/NextTask")]
        public SpielTask NextTask()
        {
            return _gameService.GetNextTask();
        }
    }
}
