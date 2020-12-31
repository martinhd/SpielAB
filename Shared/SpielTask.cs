using System.Collections.Generic;

namespace BlazorSignalRApp.Shared
{
    public enum TaskType { Question,Pantomime,Drawing}
    public class SpielTask
    {
        public TaskType Type;
        public string KeyWord;
        public string Team;
        public string Player;
        public List<string> taboos = new List<string>();
    }
}
