using System.Collections.Generic;

namespace BlazorSignalRApp.Shared
{
    public class SpielTask
    {
        /// <summary>
        /// Question,Pantomime,Drawing
        /// </summary>
        public string Type { get; set; }
        public string KeyWord { get; set; }
        public string Team { get; set; }
        public string Player { get; set; }
        public List<string> Taboos { get; set; }  = new List<string>();
    }
}
