// ReSharper disable All

using Newtonsoft.Json;

namespace Pizza
{
    public class DeserializedSerachResult
    {
        [JsonProperty("count")]
        public int count { get; set; }
        [JsonProperty("aid")]
        public int aid { get; set; }
        [JsonProperty("owner_id")]
        public int owner_id { get; set; }
        [JsonProperty("artist")]
        public string artist { get; set; }
        [JsonProperty("title")]
        public string title { get; set; }
        [JsonProperty("duration")]
        public int duration { get; set; }
        [JsonProperty("url")]
        public string url { get; set; }
        [JsonProperty("lyrics_id")]
        public string lyrics_id { get; set; }
        [JsonProperty("genre")]
        public int genre { get; set; }
    }
}