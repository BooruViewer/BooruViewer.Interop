using System;
using Newtonsoft.Json;

namespace BooruViewer.Interop.Dtos.Danbooru
{
    public class AutoComplete
    {
        public String Name { get; set; }
        public UInt64 PostCount { get; set; }
        [JsonProperty("category")]
        public TagType Type { get; set; }
    }
}
