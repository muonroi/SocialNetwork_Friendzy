﻿using Newtonsoft.Json;

namespace ExternalAPI.DTOs
{
    public class CategoryData
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        [JsonProperty("description")]
        public string Description { get; set; } = string.Empty;

        [JsonProperty("imageUrl")]
        public string ImageUrl { get; set; } = string.Empty;
    }
}