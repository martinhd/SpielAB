using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.Json;
using BlazorSignalRApp.Shared;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.SignalR;
using BlazorSignalRApp.Server.Hubs;

namespace BlazorSignalRApp.Server
{

    public interface IGameService
    {
        public void AddPlayer(string team, string name);
        public Dictionary<string, string> GetPlayers();
        public SpielTask GetNextTask();
        public SpielTask GetCurrentTask();
        public string GetNextPlayer();
        public bool ScoreCurrentTask(int score);
        bool ResetGame();
        Dictionary<string, int> GetScores();
    }


    public class GameService : IGameService
    {
        static Random rnd = new Random();

        private string _currentTeam = null;
        private string _currentPlayer = null;
        private readonly ILogger<GameService> _logger;
        private Dictionary<string, string> _player2TeamMap = new Dictionary<string, string>();
        private Dictionary<string, string> _team2lastPlayerMap = new Dictionary<string, string>();
        private Dictionary<string, int> _team2ScoreMap = new Dictionary<string, int>();
        private List<SpielTask> _tasks;
        private SpielTask _currentTask = new SpielTask();
        private readonly IHubContext<SpielTaskHub> _spielTaskHubContext;

        public GameService(ILogger<GameService> logger, IHubContext<SpielTaskHub> sth)
        {
            _logger = logger;
            _spielTaskHubContext = sth;
            try
            {
                var jsonString = File.ReadAllText("tasks.json");
                _tasks = JsonSerializer.Deserialize<List<SpielTask>>(jsonString);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while reading json config file");
                throw;
            }
        }


        public SpielTask GetNextTask()
        {
            try
            {
                var freeTasks = _tasks.Where(t => t.Player == null && t.Type == "Question").ToList();
                if (freeTasks.Count > 0 && _currentTeam != null && _currentPlayer != null)
                {
                    int r = rnd.Next(freeTasks.Count);
                    var task = freeTasks[r];
                    task.Player = _currentPlayer;
                    task.Team = _currentTeam;
                    _logger.LogInformation($"TaskHub sending new task {task.KeyWord} for player: {task.Player} in team: {task.Team}");
                    _spielTaskHubContext.Clients.All.SendAsync("ReceiveMessage", task);
                    _currentTask = task;
                    return task;
                }
                return null;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "GetNextTask");
                throw;
            }
        }

        public SpielTask GetCurrentTask()
        {
            return _currentTask;
        }

        // Switches to the next team and the next player. Starts with first team/player if at the end
        public string GetNextPlayer()
        {
            try
            {
                var teams = _player2TeamMap.Values.Distinct().ToList();
                if (teams.Count < 2) //we need two teams minimum
                    return "";

                if (_currentTeam == null)
                {
                    _currentTeam = teams.First();
                }
                else
                {
                    int pos = teams.IndexOf(_currentTeam);
                    if (pos == teams.Count - 1)
                        _currentTeam = teams[0];
                    else _currentTeam = teams[pos + 1];
                }
                var players = _player2TeamMap.Where(p => p.Value == _currentTeam).Select(p => p.Key).ToList();
                if (_currentPlayer == null)
                {
                    _currentPlayer = players.First();
                }
                else
                {
                    if (_team2lastPlayerMap.ContainsKey(_currentTeam))
                    {
                        string lastplayer = _team2lastPlayerMap[_currentTeam];
                        int pos = players.IndexOf(lastplayer);
                        if (pos == players.Count - 1)
                            _currentPlayer = players[0];
                        else _currentPlayer = players[pos + 1];
                    }
                    else _currentPlayer = players[0];
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "GetNextPlayer");
                throw;
            }
            _team2lastPlayerMap[_currentTeam] = _currentPlayer;
            _logger.LogInformation($"Set next player to: {_currentPlayer}");
            return _currentPlayer;
        }

        public void AddPlayer(string team, string name)
        {
            _player2TeamMap[name] = team;
        }

        public Dictionary<string, string> GetPlayers()
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

        public bool ScoreCurrentTask(int score)
        {
            try
            {
                if (_currentTask.Score == -1)  // no score available so far (first score wins)
                {
                    _currentTask.Score = score;
                    if (score == 1)
                        AddScore(_currentTeam);
                }
            }
            catch(Exception e)
            {
                _logger.LogError(e, "ScoreCurrentTask");
                throw;
            }
            return true;
        }

        public bool ResetGame()
        {
            try
            {
                _tasks.ForEach(t => { t.Player = null;t.Score = 0; }) ;
            }
            catch(Exception e)
            {
                _logger.LogError(e, "ResetGame");
                throw;
            }
            return true;
        }
    }
}
