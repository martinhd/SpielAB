using BlazorSignalRApp.Shared;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

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

        [HttpGet]
        [Route("Api/CurrentTask")]
        public SpielTask CurrentTask()
        {
            return _gameService.GetCurrentTask();
        }

        [HttpGet]
        [Route("Api/GetScores")]
        public Dictionary<string,int> GetScores()
        {
            return _gameService.GetScores();
        }

        [HttpGet]
        [Route("Api/ScoreTask")]
        public bool ScoreCurrentTask(int score)
        {
            return _gameService.ScoreCurrentTask(score);
        }

        [HttpGet]
        [Route("Api/ResetGame")]
        public bool ResetGame()
        {
            return _gameService.ResetGame();
        }
    }
}
