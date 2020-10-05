using System;

namespace BooruViewer.Interop.Dtos.Booru.Posts
{
    public class Size
    {
        public UInt64 Width { get; set; }
        public UInt64 Height { get; set; }

        public Size(UInt64 width, UInt64 height)
        {
            this.Width = width;
            this.Height = height;
        }
    }
}
