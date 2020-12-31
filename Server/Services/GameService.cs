using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BlazorSignalRApp.Server
{

    public interface IGameService
    {
        public void AddPlayer(string team, string name);
        public Dictionary<string, string> GetPlayers();
    }


    public class GameService : IGameService
    {
        private readonly ILogger<GameService> _logger;
        private Dictionary<string, string> _player2TeamMap = new Dictionary<string, string>();

        public GameService(ILogger<GameService> logger)
        {
            _logger = logger;
        }


        public void AddPlayer(string team,string name)
        {
            _player2TeamMap[name] = team;
        }

        public Dictionary<string,string> GetPlayers()
        {
            return _player2TeamMap;
        }
    }
}
