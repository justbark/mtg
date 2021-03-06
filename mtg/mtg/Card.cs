﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mtg
{
    public class Card
    {
        [JsonProperty("name")]
        public string name { get; set; }

        [JsonProperty("manaCost")]
        public string manaCost { get; set;}

        [JsonProperty("cmc")]
        public float cmc { get; set; }

        [JsonProperty("colors")]
        public List<string> colors { get; set; }

        [JsonProperty("type")]
        public string type { get; set;}

        [JsonProperty("supertypes")]
        public List<string> supertypes { get; set; }

        [JsonProperty("types")]
        public List<string> types { get; set; }

        [JsonProperty("subtypes")]
        public List<string> subtypes { get; set; }

        [JsonProperty("rarity")]
        public string rarity { get; set;}

        [JsonProperty("text")]
        public string text { get; set;}

        [JsonProperty("flavor")]
        public string flavor { get; set;}

        [JsonProperty("artist")]
        public string artist { get; set;}

        [JsonProperty("number")]
        public int number { get; set;}

        [JsonProperty("power")]
        public string power { get; set;}

        [JsonProperty("toughness")]
        public string toughness { get; set;}

        [JsonProperty("layout")]
        public string layout { get; set;}

        [JsonProperty("multiversid")]
        public int multiverseid { get; set;}

        [JsonProperty("imageName")]
        public string imageName { get; set;}

        [JsonProperty("id")]
        public string id { get; set;}
    }
}
