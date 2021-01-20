using System.Collections.Generic;
using CleaningRobot.Core;
using Newtonsoft.Json;

namespace CleaningRobot.Json
{
    public class JsonResult
    {
        [JsonProperty("visited")]
        public IEnumerable<Point> Visited { get; set; }

        [JsonProperty("cleaned")]
        public IEnumerable<Point> Cleaned { get; set; }

        [JsonProperty("final")]
        public Position Final { get; set; }

        [JsonProperty("battery")]
        public int Battery { get; set; }
    }
}
