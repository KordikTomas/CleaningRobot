using Newtonsoft.Json;

namespace CleaningRobot.Json
{
    public class Position
    {
        public int X { get; set; }

        public int Y { get; set; }

        [JsonProperty("facing")]
        public string Facing { get; set; }
    }
}
