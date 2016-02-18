using Newtonsoft.Json;

namespace Pizza
{
    public class Audio//в разработке
    {
        [JsonProperty("id")]
        public object id { get; set; }
        [JsonProperty("photo")]
        public string photo { get; set; }
        [JsonProperty("name")]
        public string name { get; set; }
        [JsonProperty("name_gen")]
        public string name_gen { get; set; }
        [JsonProperty("owner_id")]
        public int? owner_id { get; set; }
        [JsonProperty("artist")]
        public string artist { get; set; }
        [JsonProperty("title")]
        public string title { get; set; }
        [JsonProperty("duration")]
        public int duration { get; set; }
        [JsonProperty("date")]
        public int date { get; set; }
        [JsonProperty("url")]
        public string url { get; set; }
        [JsonProperty("lyrics_id")]
        public int? lyrics_id { get; set; }
        [JsonProperty("genre_id")]
        public int? genre_id { get; set; }
        [JsonProperty("no_search")]
        public int? no_search { get; set; }
        [JsonProperty("album_id")]
        public int? album_id { get; set; }
    }
}