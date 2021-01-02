using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using BlazorSignalRApp.Shared;
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
        private Dictionary<string, int> _team2ScoreMap = new Dictionary<string, int>();
        List<SpielTask> _tasks;

        public GameService(ILogger<GameService> logger)
        {
            _logger = logger;
            try
            {
                var jsonString = File.ReadAllText("tasks.json");
                _tasks = JsonSerializer.Deserialize<List<SpielTask>>(jsonString);
            }
            catch(Exception e)
            {
                _logger.LogError(e, "Error while reading json config file");
                throw;
            }
        }


        public void AddPlayer(string team,string name)
        {
            _player2TeamMap[name] = team;
        }

        public Dictionary<string,string> GetPlayers()
        {
            return _player2TeamMap;
        }

        public List<SpielTask> GetTaks()
        {
            return _tasks;
        }

        public bool AddScore(string team)
        {
            if (_team2ScoreMap.ContainsKey(team))
                _team2ScoreMap[team]++;
            else _team2ScoreMap[team] = 1;
            return true;
        }

        public Dictionary<string, int> GetScores()
        {
            return _team2ScoreMap;
        }
    }
}
