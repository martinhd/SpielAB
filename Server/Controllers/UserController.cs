using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorSignalRApp.Server.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IGameService _gameService;
        public UserController(IGameService gs)
        {
            _gameService = gs;
        }

        [HttpGet]
        [Route("CreateUser/{Team}/{Player}")]
        public void CreateUser(string Team,string Player)
        {
            _gameService.AddPlayer(Team, Player);
        }


        [HttpGet]
        [Route("GetUsers")]
        public Dictionary<string,string> GetUsers()
        {
            return _gameService.GetPlayers();
        }

    }
}
