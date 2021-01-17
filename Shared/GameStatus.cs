using System.Collections.Generic;

namespace BlazorSignalRApp.Shared
{
    public class GameStatus
    {
        public string CurrentPlayer { get; set; }
        public string CurrentTeam { get; set; }
        public Dictionary<string,string> Players2Team { get; set; }
        public Dictionary<string, int> Teams2Score { get; set; }
    }
}
