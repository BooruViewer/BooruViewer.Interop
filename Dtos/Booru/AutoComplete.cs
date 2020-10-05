using System;

namespace BooruViewer.Interop.Dtos.Booru
{
    public class AutoComplete
    {
        public String Name { get; set; }
        public TagTypes Type { get; set; }
        public UInt64 Count { get; set; }
    }
}
