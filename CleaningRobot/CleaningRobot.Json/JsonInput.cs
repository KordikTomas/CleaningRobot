using Newtonsoft.Json;

namespace CleaningRobot.Json
{
    public class JsonInput
    {
        public string[,] Map { get; set; }

        public Position Start { get; set; }

        public string[] Commands { get; set; }

        public int Battery { get; set; }
    }
}
