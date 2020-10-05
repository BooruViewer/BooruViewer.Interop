using System;

namespace BooruViewer.Interop.Dtos.Booru
{
    public class Tag
    {
        public String Name { get; set; }
        public TagTypes Type { get; set; }

        public Tag(String name, TagTypes type)
        {
            this.Name = name;
            this.Type = type;
        }
    }
}
