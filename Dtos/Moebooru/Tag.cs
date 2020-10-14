using System;

namespace BooruViewer.Interop.Dtos.Moebooru
{
    public class Tag
    {
        public Int32 Id { get; set; }
        public String Name { get; set; }
        public Int32 Count { get; set; }
        public Int32 Type { get; set; }
        public Boolean Ambigous { get; set; }
    }
}
