using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mtg
{
    class Card
    {
        [JsonProperty("name")]
        public string name { get; set; }
        [JsonProperty("manaCost")]
        public string manaCost { get; set;}
        [JsonProperty("cmc")]
        public float cmc { get; set; }
        /*public string colors;
        public string type;
        public string supertypes;
        public string types;
        public string subtypes;
        public string rarity;
        public string text;
        public string flavor;
        public string artist;
        public int number;
        public int power;
        public int toughness;
        public string layout;
        public int multiverseid;
        public string imageName;
        public string id;*/
    }
}
