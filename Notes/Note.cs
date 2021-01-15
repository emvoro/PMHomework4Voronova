using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Notes
{
    public class Note : INote
    {
        [JsonProperty("id")]
        public int Id { get; }

        [JsonProperty("header")]
        public string Header { get; }

        [JsonProperty("text")]
        public string Text { get; }

        [JsonProperty("createdOn")]
        public DateTime CreatedOn { get; }

        public Note(int id, string header, string text, DateTime createdOn)
        {
            Id = id;
            Header = header;
            Text = text;
            CreatedOn = createdOn;
        }
    }
}
