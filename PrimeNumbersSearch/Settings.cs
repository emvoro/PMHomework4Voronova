using System;
using System.Text.Json.Serialization;

namespace PrimeNumbersSearch
{
    public class Settings
    {
        [JsonPropertyName("primesFrom")]
        public int PrimesFrom { get; set; }

        [JsonPropertyName("primesTo")]
        public int PrimesTo { get; set; }
    }
}
